#include "stdafx.h"
#include "$serviceimplclassname$.h"
#include <memory>

using namespace $servicenamespace$;

// Defined in generated code
void InitUplift(std::shared_ptr<$serviceinterfacename$> serviceInstance);

BOOL APIENTRY DllMain(HINSTANCE hInst, DWORD  reason, LPVOID pReserved)
{
	BOOL ret = TRUE;

	switch (reason)
	{
		case DLL_PROCESS_ATTACH:
		{
			DisableThreadLibraryCalls(hInst);

			// Initialise Uplift
			InitUplift(std::make_shared<$serviceimplclassname$>());
		}
		break;

		case DLL_PROCESS_DETACH:
			break;

		default:
			break;
	}

	return ret;
}
