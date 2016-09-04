#include "stdafx.h"
#include "UpliftCore.h"
#include "IsapiBindings.h"
#include "ITestService.h"
#include "Errors.h"
#include <ctime>
#include <sstream>
#include <iomanip>
#include <regex>
#include "StringFormat.h"
#include "Base64.h"
#include "pugixml.cpp"

using namespace Uplift::Isapi;
using namespace Uplift::Utility;
using namespace std;
using namespace pugi;

// handle to the current module for resource loading
EXTERN_C IMAGE_DOS_HEADER __ImageBase;
#define HINST_THISCOMPONENT ((HINSTANCE)&__ImageBase)

namespace Uplift
{
	namespace Core
	{
		CUpliftCore::CUpliftCore()
		{
			// get startup time
			auto t = time(nullptr);
			tm tm = {};
			localtime_s(&tm, &t);

			std::ostringstream oss;
			//in format August 26, 2016 16:03:00
			oss << std::put_time(&tm, "%B %d, %Y %H:%M:%S");

			//<generated>
			m_strServiceName = "ITestService";	// this is retrieved from the name of the service interface
			m_strServiceVersion = "0.1.0";	// this is retrieved from the "ServiceVersion" code generator param in the service interface definition
			//</generated>
			m_strServiceStartTimestamp = oss.str();
		}

		CUpliftCore::~CUpliftCore()
		{
		}

		void CUpliftCore::RegisterService(shared_ptr<org::tempuri::ITestService> pService)
		{
			m_pService = pService;
		}

		// Decided to use PugiXML after some XML parser research, looking at Pull Parsers vs DOM Parsers and speed
		// of various implementations.
		// PugiXML is fast enough, nice to use, well written, actively maintained, and soap packets are typically small, 
		// therefore for now I think this is a good choice for the soap (de)serialisers.
		// May have to look at other options in future when dealing with MTOM.
		std::vector<char> CUpliftCore::HandleSoapRequest(const std::vector<char>& rawSoapPacket)
		{
			string replyEnvelopeTemplate = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Header /><s:Body>{0}</s:Body></s:Envelope>";
			string faultBodyTemplate = "<s:Fault><faultcode>s:{0}</faultcode><faultstring>{1}</faultstring></s:Fault>";
			string reply = "";

			try
			{
				xml_document doc;
				if (!doc.load_buffer(rawSoapPacket.data(), rawSoapPacket.size()))
					throw SoapProcessingException("Soap packet was malformed");

				auto&& node = doc.select_single_node("//*[local-name()='Body']"); // node called "Body" ignoring namespace
				if (!node)
					throw SoapProcessingException("No Body node found in Soap packet");

				auto&& reqMsgNode = node.node().first_child();
				if (!reqMsgNode)
					throw SoapProcessingException("No Request Message found in Soap packet");

				string reqOp = reqMsgNode.name();

				if (reqOp == "GetData")
				{
					string result = m_pService->GetData();
					auto replyBody = (StringFormat("<GetDataResponse xmlns=\"http://tempuri.org/\"><GetDataResult>{0}</GetDataResult></GetDataResponse>") % result).str();
					reply = (StringFormat(replyEnvelopeTemplate) % replyBody).str();
				}
				else if (reqOp == "GetDataUsingDataContract")
				{
					// declare variables to deserialise
					org::tempuri::CompositeType composite;

					// deserialise "composite"
					auto&& compositeNode = reqMsgNode.child("composite");
					if (!compositeNode)
						throw SoapProcessingException("Expected composite parameter");

					for (auto&& structMemberNode : compositeNode.children())
					{
						string nodeName = structMemberNode.name();
						string localName = nodeName.substr(nodeName.find_first_of(':') + 1);
						if (localName == "BoolValue")
							composite.BoolValue = structMemberNode.text().as_bool();
						else if (localName == "StringValue")
							composite.StringValue = structMemberNode.text().as_string();
						else throw SoapProcessingException("Encountered unexpected node while processing composite composite parameter");
					}

					// make the call to the service function
					org::tempuri::CompositeType result = m_pService->GetDataUsingDataContract(composite);

					// format the reply
					auto replyBody = (StringFormat("<GetDataUsingDataContractResponse xmlns=\"http://tempuri.org/\"><GetDataUsingDataContractResult xmlns:a=\"http://schemas.datacontract.org/2004/07/TestService\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\">"
						"<a:BoolValue>{0}</a:BoolValue><a:StringValue>{1}</a:StringValue></GetDataUsingDataContractResult></GetDataUsingDataContractResponse>") % std::to_string(result.BoolValue) % result.StringValue).str();
					reply = (StringFormat(replyEnvelopeTemplate) % replyBody).str();
						
				}
				else if (reqOp == "PerformOperation")
				{
					// declare variables to deserialise
					vector<double> things;
					int widgetValue;

					// deserialise "things"
					auto&& thingsNode = reqMsgNode.child("things");
					if (!thingsNode)
						throw SoapProcessingException("Expected things parameter");
					for (auto&& listItem : thingsNode.children())
					{
						if (string("double") != listItem.name())
							throw SoapProcessingException("Expected list of double items");
						things.push_back(listItem.text().as_double());
					}

					// deserialise "widgetValue"
					auto&& widgetValueNode = reqMsgNode.child("widgetValue");
					if (!widgetValueNode)
						throw SoapProcessingException("Expected widgetValue parameter");
					widgetValue = widgetValueNode.text().as_int();

					// make the call to the service function
					long long result = m_pService->PerformOperation(things, widgetValue);

					// format the reply
					auto resultAsStr = std::to_string(result); // this is a C++11 feature - makes conversions a bit easier
					auto replyBody = (StringFormat("<PerformOperationResponse xmlns=\"http://tempuri.org/\"><PerformOperationResult>{0}</PerformOperationResult></PerformOperationResponse>") % resultAsStr).str();
					reply = (StringFormat(replyEnvelopeTemplate) % replyBody).str();
				}
				else if (reqOp == "SetData")
				{
					// declare variables to deserialise
					// [binaryData assigned from rval for efficiency]
					double importantNumber;

					// deserialise "binaryData"
					auto&& binaryDataNode = reqMsgNode.child("binaryData");
					if (!binaryDataNode)
						throw SoapProcessingException("Expected binaryData parameter");
					auto&& binaryData = Utility::base64_decode(binaryDataNode.text().as_string());

					// deserialise "importantNumber"
					auto&& importantNumberNode = reqMsgNode.child("importantNumber");
					if (!importantNumberNode)
						throw SoapProcessingException("Expected importantNumber parameter");
					importantNumber = importantNumberNode.text().as_double();

					// make the call to the service function
					m_pService->SetData(binaryData, importantNumber);

					// format the (empty return value) reply
					reply = (StringFormat(replyEnvelopeTemplate) % "<SetDataResponse xmlns=\"http://tempuri.org/\" />").str();
				}
				else if (reqOp == "ThrowException")
				{
					// no parameters and no return type
					m_pService->ThrowException();

					// format the (empty return value) reply - NOTE this won't be called as the demo service function will throw an exception
					reply = (StringFormat(replyEnvelopeTemplate) % "<ThrowExceptionResponse xmlns=\"http://tempuri.org/\" />").str();
				}
				else throw SoapProcessingException("An unrecognised operation was requested");
			}
			catch (const SoapProcessingException& spe)
			{
				// this is a fault caused by the client
				string faultBody = (StringFormat(faultBodyTemplate) % "Client" % spe.what()).str();
				reply = (StringFormat(replyEnvelopeTemplate) % faultBody).str();
			}
			catch (const std::exception& e)
			{
				// this is a fault caused by the service
				string faultBody = (StringFormat(faultBodyTemplate) % "Server" % e.what()).str();
				reply = (StringFormat(replyEnvelopeTemplate) % faultBody).str();
			}
			catch (...)
			{
				// this is a fault caused by the service
				string faultBody = (StringFormat(faultBodyTemplate) % "Server" % "An unknown error occurred in the service").str();
				reply = (StringFormat(replyEnvelopeTemplate) % faultBody).str();
			}
			
			return vector<char>(reply.begin(), reply.end());
		}

		std::string CUpliftCore::GetWebServiceDefinition(const std::string& rootRequestUri)
		{
			auto&& templateWsdl = GetEmbeddedStringDataResource(IDR_WSDL_FILE);

			return (StringFormat(templateWsdl) % kwarg{ "ServiceRoot", rootRequestUri }).str();
		}

		string CUpliftCore::GetStatusPage(const std::string& rootRequestUri)
		{
			auto&& templatePage = GetEmbeddedStringDataResource(IDR_CPHTML_FILE);

			return (StringFormat(templatePage) % kwarg{ "ServiceWsdlUri", rootRequestUri + "?WSDL" } %
				kwarg{ "ServiceName", m_strServiceName } % kwarg{ "ServiceVersion", m_strServiceVersion } %
				kwarg{ "ServiceStartTimestamp", m_strServiceStartTimestamp }).str();
		}

		std::string CUpliftCore::GetEmbeddedStringDataResource(int ResId)
		{
			HRSRC hRes = FindResource(HINST_THISCOMPONENT, MAKEINTRESOURCE(ResId), RT_RCDATA);
			HGLOBAL hData = LoadResource(HINST_THISCOMPONENT, hRes);
			auto resSz = SizeofResource(HINST_THISCOMPONENT, hRes);
			auto pData = static_cast<const char*>(LockResource(hData));
			return string(pData, pData + resSz);
		}
	}
}


