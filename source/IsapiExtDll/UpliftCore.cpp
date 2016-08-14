#include "stdafx.h"
#include "UpliftCore.h"
#include "IsapiBindings.h"

using namespace Uplift::Isapi;
using namespace std;

namespace Uplift
{
	namespace Core
	{
		CUpliftCore::CUpliftCore()
		{
		}

		CUpliftCore::~CUpliftCore()
		{
		}

		void CUpliftCore::RegisterService(std::shared_ptr<IMyService> pService)
		{
			m_pService = pService;
		}

		std::vector<char> CUpliftCore::HandleSoapRequest(const std::vector<char>& rawSoapPacket)
		{
			return vector<char>();
		}

		string CUpliftCore::GetWebServiceDefinition()
		{
			return "";
		}

		string CUpliftCore::GetStatusPage()
		{
			return "<HTML>\r\n"
				"<HEAD>\r\n"
				"<title>Hello from ISAPI</title>\r\n"
				"</HEAD>\r\n"
				"<BODY>\r\n"
				"<h1>Hello from an ISAPI Extension</h1>\r\n"
				"</BODY>\r\n"
				"</HTML>\r\n";
		}

		std::unique_ptr<CIsapiBindings> m_isapiBindings;
	}
}


