#pragma once

#include "httpext.h"
#include "IRequestHandler.h"

namespace Uplift
{
	namespace Isapi
	{

		class CIsapiBindings
		{
		public:
			CIsapiBindings();
			virtual ~CIsapiBindings();

			void RegisterRequestHandler(Core::IRequestHandler* pRequestHandler);

			DWORD OnIsapiCall(EXTENSION_CONTROL_BLOCK* pECB);

		private:
			DWORD SendResponse(EXTENSION_CONTROL_BLOCK* pECB, const std::vector<char>& responseData);

			Core::IRequestHandler* m_pRequestHandler;
		};
	}
}