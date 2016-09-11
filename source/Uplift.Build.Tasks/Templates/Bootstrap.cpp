#include "stdafx.h"
#include "UpliftCore.h"
#include "IsapiBindings.h"

// Global request processing object
Uplift::Core::CUpliftCore UpliftCore;

// The Isapi layer
extern Uplift::Isapi::CIsapiBindings HttpLayer;

void InitUplift(std::shared_ptr<org::tempuri::ITestService> serviceInstance)
{
	// register the service implementation with the core
	UpliftCore.RegisterService(serviceInstance);

	// register the core as a request handler with the Isapi layer
	HttpLayer.RegisterRequestHandler(&UpliftCore);
}
