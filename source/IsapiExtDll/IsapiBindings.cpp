#include "stdafx.h"
#include "IsapiBindings.h"
#include "Errors.h"
#include <vector>

// Global object that handles ISAPI interface
Uplift::Isapi::CIsapiBindings HttpLayer;

using namespace std;

namespace Uplift
{
	namespace Isapi
	{

		CIsapiBindings::CIsapiBindings()
		{
		}


		CIsapiBindings::~CIsapiBindings()
		{
		}

		void CIsapiBindings::RegisterRequestHandler(Core::IRequestHandler* pRequestHandler)
		{
			m_pRequestHandler = pRequestHandler;
		}

		// Call Core handlers via IRequestHandler interface (which seperates the architectural layers - 
		// core should have no knowledge of isapi, nor strictly of http - should be transport independent).
		//
		// TO DO - This reads from web server SYNCHRONOUSLY which is NOT RECOMMENDED - for prototype this is ok (and
		// simplifies the code), but for final version we should be using the ISAPI asynchronous I/O feature.
		// See microsoft and various "code project" site samples for how to achieve this. Looks like a pain in the ass.
		DWORD CIsapiBindings::OnIsapiCall(EXTENSION_CONTROL_BLOCK* pECB)
		{
			DWORD hseStatus = HSE_STATUS_SUCCESS;

			try
			{
				// response buffer and type of data being returned
				std::vector<char> responseBuf;
				ContentType responseContent = HTML;

				// check that the http request is being done on the service root uri.
				// we don't support any other paths
				if (pECB->lpszPathInfo != nullptr && strlen(pECB->lpszPathInfo) > 0)
					throw NotFoundException("The requested resource does not exist");

				string httpMethod(pECB->lpszMethod);
				if (httpMethod == "POST")
				{
					vector<char> dataBuf(pECB->cbTotalBytes);

					DWORD dwTotalWritten = 0;
					// pECB->cbAvailable: The available number of bytes (out of a total of cbTotalBytes)
					// in the buffer pointed to by lpbData. If cbTotalBytes is the same as cbAvailable,
					// the lpbData variable will point to a buffer that contains all the data as sent by
					// the client. Otherwise, cbTotalBytes will contain the total number of bytes of data received
					DWORD dwTotalRead = pECB->cbAvailable;

					if (pECB->cbAvailable > 0)
						// Copy first chunk of the data into the buffer
						memcpy_s(&dataBuf[0], dataBuf.size(), pECB->lpbData, dwTotalRead);

					// Loop to read in the rest of the data from the client
					while (dwTotalRead < pECB->cbTotalBytes)
					{
						DWORD dwBytesRead = pECB->cbTotalBytes - dwTotalRead; // bytes available in buf

						// pECB->cbTotalBytes: The total number of bytes to be received from the client.
						// This is equivalent to the CGI variable CONTENT_LENGTH
						if (!pECB->ReadClient(pECB->ConnID, &dataBuf[dwTotalRead], &dwBytesRead))
							throw BadRequestException("Couldn't read data from client");

						dwTotalRead += dwBytesRead;
					}

					// we've read all the bytes from the client, next check that the content
					// type is text/xml (required for SOAP 1.1 - note SOAP 1.2 with media type application/soap+xml
					// is currently not supported)
					std::string contentType(pECB->lpszContentType);
					if (contentType.find("text/xml") == std::string::npos)
						throw InvalidMediaTypeException("Expected SOAP 1.1 message with text/xml content type");
					
					//handle the soap message body
					responseBuf = m_pRequestHandler->HandleSoapRequest(dataBuf);
					responseContent = XML;
					
				}
				else if (httpMethod == "GET")
				{
					// get the root Uri, so that it can be spliced into templates
					auto&& rootUrl = GetRootUrl(pECB);

					if (string(pECB->lpszQueryString) == "WSDL")
					{
						// return WSDL definition
						auto&& wsdl = m_pRequestHandler->GetWebServiceDefinition(rootUrl);
						responseBuf.insert(responseBuf.end(), wsdl.begin(), wsdl.end());
						responseContent = XML;
					}
					else
					{
						// any other get returns the service status page.
						auto statusHtml = m_pRequestHandler->GetStatusPage(rootUrl);
						responseBuf.insert(responseBuf.end(), statusHtml.begin(), statusHtml.end());
						responseContent = HTML;
					}
				}
				else throw MethodNotAllowedException("Only GET and POST are supported.");

				hseStatus = SendResponse(pECB, responseContent, responseBuf);
			}
			catch (const BadRequestException& br)
			{
				hseStatus = SendError(pECB, BadRequest, br.what());
			}
			catch (const NotFoundException& nf)
			{
				hseStatus = SendError(pECB, NotFound, nf.what());
			}
			catch (const MethodNotAllowedException& mna)
			{
				hseStatus = SendError(pECB, MethodNotAllowed, mna.what());
			}
			catch (const InvalidMediaTypeException& imt)
			{
				hseStatus = SendError(pECB, InvalidMediaType, imt.what());
			}
			catch (const std::exception& e) // this will catch InternalServerErrorException too
			{
				hseStatus = SendError(pECB, InternalServerError, e.what());
			}
			catch (...)
			{
				hseStatus = SendError(pECB, InternalServerError, "An unknown error occurred");
			}

			return hseStatus;
		}

		std::string CIsapiBindings::GetRootUrl(EXTENSION_CONTROL_BLOCK* pECB)
		{
			std::string rootUrl;

			std::vector<char> buf;
			DWORD bufSz = MAX_PATH;
			buf.resize(MAX_PATH);
			if (!pECB->GetServerVariable(pECB->ConnID, "HTTPS", &buf[0], &bufSz))
				throw InternalServerErrorException("Cannot retrieve root URL");
			if (string("ON") == buf.data())
				rootUrl = "https://";
			else
				rootUrl = "http://";
			buf.clear();
			buf.resize(MAX_PATH);
			bufSz = MAX_PATH;
			if (!pECB->GetServerVariable(pECB->ConnID, "SERVER_NAME", &buf[0], &bufSz))
				throw InternalServerErrorException("Cannot retrieve root URL");
			rootUrl += buf.data();
			buf.clear();
			buf.resize(MAX_PATH);
			bufSz = MAX_PATH;
			if (!pECB->GetServerVariable(pECB->ConnID, "SERVER_PORT", &buf[0], &bufSz))
				throw InternalServerErrorException("Cannot retrieve root URL");
			rootUrl += string(":") + buf.data();
			buf.clear();
			buf.resize(MAX_PATH);
			bufSz = MAX_PATH;
			if (!pECB->GetServerVariable(pECB->ConnID, "URL", &buf[0], &bufSz))
				throw InternalServerErrorException("Cannot retrieve root URL");
			rootUrl += buf.data();
			return rootUrl;
		}

		DWORD CIsapiBindings::SendError(EXTENSION_CONTROL_BLOCK* pECB, HttpStatus responseCode, const std::string& errorMsg)
		{
			HSE_SEND_HEADER_EX_INFO SendHeaderExInfo;
			switch (responseCode)
			{
			case BadRequest:
				SendHeaderExInfo.pszStatus = "400 Bad Request";
				break;
			case NotFound:
				SendHeaderExInfo.pszStatus = "404 Not Found";
				break;
			case MethodNotAllowed:
				SendHeaderExInfo.pszStatus = "405 Method Not Allowed";
				break;
			case InvalidMediaType:
				SendHeaderExInfo.pszStatus = "415 Invalid Media Type";
				break;
			case InternalServerError:
				SendHeaderExInfo.pszStatus = "500 Internal Server Error";
				break;
			};
			SendHeaderExInfo.pszHeader = "Content-Type: text/xml; charset=utf-8\r\n\r\n";

			SendHeaderExInfo.cchStatus = strlen(SendHeaderExInfo.pszStatus);
			SendHeaderExInfo.cchHeader = strlen(SendHeaderExInfo.pszHeader);
			SendHeaderExInfo.fKeepConn = FALSE;

			if (!pECB->ServerSupportFunction(pECB->ConnID, HSE_REQ_SEND_RESPONSE_HEADER_EX, &SendHeaderExInfo, NULL, NULL))
				return HSE_STATUS_ERROR;

			string errContent = string("<ErrorMessage>") + errorMsg + "</ErrorMessage>";
			vector<char> responseData(errContent.begin(), errContent.end());
			DWORD dwBytesWritten = responseData.size();
			if (!pECB->WriteClient(pECB->ConnID, (LPVOID)responseData.data(), &dwBytesWritten, 0))
				return HSE_STATUS_ERROR;

			return HSE_STATUS_SUCCESS;
		}
		
		DWORD CIsapiBindings::SendResponse(EXTENSION_CONTROL_BLOCK* pECB, ContentType sendAsType, const std::vector<char>& responseData)
		{
			HSE_SEND_HEADER_EX_INFO SendHeaderExInfo;
			SendHeaderExInfo.pszStatus = "200 OK";

			switch (sendAsType)
			{
			case HTML:
				SendHeaderExInfo.pszHeader = "Content-Type: text/html; charset=utf-8\r\n\r\n";
				break;
			case XML:
				SendHeaderExInfo.pszHeader = "Content-Type: text/xml; charset=utf-8\r\n\r\n";
				break;
			}
			
			SendHeaderExInfo.cchStatus = strlen(SendHeaderExInfo.pszStatus);
			SendHeaderExInfo.cchHeader = strlen(SendHeaderExInfo.pszHeader);
			SendHeaderExInfo.fKeepConn = FALSE;

			if (!pECB->ServerSupportFunction(pECB->ConnID, HSE_REQ_SEND_RESPONSE_HEADER_EX, &SendHeaderExInfo, NULL, NULL))
				return HSE_STATUS_ERROR;

			DWORD dwBytesWritten = responseData.size();
			if (!pECB->WriteClient(pECB->ConnID, (LPVOID)responseData.data(), &dwBytesWritten, 0))
				return HSE_STATUS_ERROR;

			return HSE_STATUS_SUCCESS;
		}
	}
}

BOOL WINAPI GetExtensionVersion(HSE_VERSION_INFO *pVer)
{
	pVer->dwExtensionVersion = HSE_VERSION;
	strncpy_s(pVer->lpszExtensionDesc,
		HSE_MAX_EXT_DLL_NAME_LEN,
		"TestService SOAP web service", _TRUNCATE); //to do - get this string from core
	return TRUE;
}

DWORD WINAPI HttpExtensionProc(EXTENSION_CONTROL_BLOCK *pECB)
{
	return HttpLayer.OnIsapiCall(pECB);
}

BOOL WINAPI TerminateExtension(DWORD dwFlags) {
	return TRUE;
}