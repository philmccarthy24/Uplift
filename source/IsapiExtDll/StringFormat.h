#pragma once

namespace Uplift
{
	namespace Utility
	{
		typedef std::pair<std::string, std::string> kwarg;

		class StringFormat
		{
		public:
			StringFormat(const std::string& templateStr);
			virtual ~StringFormat();

			StringFormat& operator %(const std::string& positional);
			StringFormat& operator %(const kwarg& arg);

			const std::string& str();

		private:
			std::string m_strResult;
			int m_nPos;
		};
	}
}


