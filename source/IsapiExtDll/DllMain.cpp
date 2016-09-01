#include "stdafx.h"
#include "UpliftCore.h"
#include "IsapiBindings.h"
#include "TestServiceImpl.h"

// Global request processing object
Uplift::Core::CUpliftCore UpliftCore;

// The Isapi layer
extern Uplift::Isapi::CIsapiBindings HttpLayer;

BOOL APIENTRY DllMain(HINSTANCE hInst, DWORD  reason, LPVOID pReserved)
{
	BOOL ret = TRUE;

	switch (reason)
	{
		case DLL_PROCESS_ATTACH:
		{
			DisableThreadLibraryCalls(hInst);

			// register the service implementation with the core
			UpliftCore.RegisterService(std::make_shared<org::tempuri::CTestServiceImpl>());

			// register the core as a request handler with the Isapi layer
			HttpLayer.RegisterRequestHandler(&UpliftCore);
		}
		break;

		case DLL_PROCESS_DETACH:
			// could cleanup UpliftCore here?
			break;

		default:
			break;
	}

	return ret;
}
