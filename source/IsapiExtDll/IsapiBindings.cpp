#include "stdafx.h"
#include "IsapiBindings.h"
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

			string httpMethod(pECB->lpszMethod);
			if (httpMethod == "POST" && pECB->cbAvailable > 0) // there must also be some data! TODO - throw a handled exception if not
			{
				vector<char> dataBuf(pECB->cbAvailable);

				DWORD dwTotalWritten = 0;
				// pECB->cbAvailable: The available number of bytes (out of a total of cbTotalBytes)
				// in the buffer pointed to by lpbData. If cbTotalBytes is the same as cbAvailable,
				// the lpbData variable will point to a buffer that contains all the data as sent by
				// the client.Otherwise, cbTotalBytes will contain the total number of bytes of data received
				DWORD dwTotalRead = pECB->cbAvailable;

				// Copy first chunk of the data into the buffer
				memcpy_s(&dataBuf[0], dataBuf.size(), pECB->lpbData, dwTotalRead);

				// Loop to read in the rest of the data from the client
				while (dwTotalRead < pECB->cbTotalBytes)
				{
					DWORD dwBytesRead = pECB->cbTotalBytes - dwTotalRead; // bytes available in buf

					// pECB->cbTotalBytes: The total number of bytes to be received from the client.
					// This is equivalent to the CGI variable CONTENT_LENGTH
					if (!pECB->ReadClient(pECB->ConnID, &dataBuf[dwTotalRead], &dwBytesRead))
						return HSE_STATUS_ERROR; // TODO - throw an exception to handle instead of returning inline

					dwTotalRead += dwBytesRead;
				}

				// ok we've read all the bytes from the client, handle the soap message body
				// (assuming it is a soap request - core will error out if not)
				auto&& result = m_pRequestHandler->HandleSoapRequest(dataBuf);

				hseStatus = SendResponse(pECB, result);
			}
			else if (httpMethod == "GET")
			{
				if (string(pECB->lpszQueryString) == "WSDL")
				{
					// return WSDL definition
					auto wsdl = m_pRequestHandler->GetWebServiceDefinition();
					vector<char> data(wsdl.begin(), wsdl.end());
					hseStatus = SendResponse(pECB, data);
				}
				else
				{
					// return service status page. could possibly get info for this as a struct
					// from service interface, rather than expecting core to build a web page?
					auto statusHtml = m_pRequestHandler->GetStatusPage();
					vector<char> data(statusHtml.begin(), statusHtml.end());
					hseStatus = SendResponse(pECB, data);
				}
			}
			else
			{
				hseStatus = HSE_STATUS_ERROR;
			}

			return hseStatus;
		}

		// TO DO - make this function take different status code enums and content types as input
		DWORD CIsapiBindings::SendResponse(EXTENSION_CONTROL_BLOCK* pECB, const std::vector<char>& responseData)
		{
			HSE_SEND_HEADER_EX_INFO SendHeaderExInfo;
			SendHeaderExInfo.pszStatus = "200 OK";
			SendHeaderExInfo.pszHeader = "Content-Type: text/html\r\n\r\n";
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
		"Uplift web service", _TRUNCATE); //to do - get this string from core
	return TRUE;
}

DWORD WINAPI HttpExtensionProc(EXTENSION_CONTROL_BLOCK *pECB)
{
	return HttpLayer.OnIsapiCall(pECB);
}

BOOL WINAPI TerminateExtension(DWORD dwFlags) {
	return TRUE;
}