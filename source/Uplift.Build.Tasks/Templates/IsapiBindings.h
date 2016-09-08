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
			enum ContentType
			{
				HTML,
				XML
			};

			enum HttpStatus
			{
				Ok,
				BadRequest,
				InternalServerError,
				MethodNotAllowed,
				InvalidMediaType,
				NotFound
			}; // there are, of course, many others, but these are the simple ones we support

			DWORD SendResponse(EXTENSION_CONTROL_BLOCK* pECB, ContentType sendAsType, const std::vector<char>& responseData);
			DWORD SendError(EXTENSION_CONTROL_BLOCK* pECB, HttpStatus responseCode, const std::string& errorMsg);
			std::string GetRootUrl(EXTENSION_CONTROL_BLOCK* pECB);

			Core::IRequestHandler* m_pRequestHandler;
		};
	}
}