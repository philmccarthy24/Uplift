#pragma once
#include "IRequestHandler.h"
#include <memory>
#include <vector>
#include <map>
#include <string>

//<for_generation>
// forward class def for service interface
namespace org
{
	namespace tempuri
	{
		class ITestService; 
	}
}
//</for_generation>

namespace Uplift
{
	namespace Core
	{

		class CUpliftCore : public IRequestHandler
		{
		public:
			CUpliftCore();
			virtual ~CUpliftCore();

			//<for_generation>
			void RegisterService(std::shared_ptr<org::tempuri::ITestService> pService);
			//</for_generation>

			virtual std::vector<char> HandleSoapRequest(const std::vector<char>& rawSoapPacket) override;
			virtual std::string GetWebServiceDefinition(const std::string& rootRequestUri) override;
			virtual std::string GetStatusPage(const std::string& rootRequestUri) override;

		private:
			std::string GetEmbeddedStringDataResource(int ResId);

			std::string m_strServiceName;
			std::string m_strServiceVersion;
			std::string m_strServiceStartTimestamp;

			//<for_generation>
			std::shared_ptr<org::tempuri::ITestService> m_pService;
			//</for_generation>

			// map of soap operation names to (de)serialisers required to go from xml -> c++ params and back
			//std::map<string, IXmlSerialiser> m_soapInMessageDeserialisers
		};

	}
}