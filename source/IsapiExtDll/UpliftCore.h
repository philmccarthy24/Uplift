#pragma once
#include "IRequestHandler.h"
#include <memory>
#include <vector>
#include <string>

namespace Uplift
{
	namespace Core
	{

		class IMyService; // forward class def for service interface

		class CUpliftCore : public IRequestHandler
		{
		public:
			CUpliftCore();
			virtual ~CUpliftCore();

			void RegisterService(std::shared_ptr<IMyService> pService);

			virtual std::vector<char> HandleSoapRequest(const std::vector<char>& rawSoapPacket) override;
			virtual std::string GetWebServiceDefinition() override;
			virtual std::string GetStatusPage() override;

		private:
			std::shared_ptr<IMyService> m_pService;

			// map of soap ports (service functions) to deserialisers required to go from xml -> c++ params
			//std::map<string, IXmlSerialiser> m_soapInMessageDeserialisers
			// map of soap ports (service functions) to serialisers required to go from c++ return -> xml
			//std::map<string, IXmlSerialiser> m_soapOutMessageSerialisers
		};

	}
}