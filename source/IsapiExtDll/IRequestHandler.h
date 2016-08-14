#pragma once
#include <vector>
#include <string>

namespace Uplift 
{
	namespace Core
	{
		class IRequestHandler
		{
		public:
			virtual ~IRequestHandler() {}

			virtual std::vector<char> HandleSoapRequest(const std::vector<char>& rawSoapPacket) = 0;
			virtual std::string GetWebServiceDefinition() = 0;
			virtual std::string GetStatusPage() = 0; // return a simple service status page
		};
	}
}
	
