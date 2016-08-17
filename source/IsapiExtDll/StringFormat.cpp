#include "stdafx.h"
#include "StringFormat.h"
#include <regex>

using namespace std;

namespace Uplift
{
	namespace Utility
	{
		StringFormat::StringFormat(const std::string& templateStr) :
			m_strResult(templateStr),
			m_nPos(0)
		{
		}


		StringFormat::~StringFormat()
		{
		}

		StringFormat& StringFormat::operator %(const std::string& positional)
		{
			regex formatKey("\\{" + std::to_string(m_nPos++) + "\\}");
			m_strResult = regex_replace(m_strResult, formatKey, positional);
			return *this;
		}

		StringFormat& StringFormat::operator %(const kwarg& arg)
		{
			regex formatKey("\\{" + arg.first + "\\}");
			m_strResult = regex_replace(m_strResult, formatKey, arg.second);
			return *this;
		}

		const std::string& StringFormat::str()
		{
			return m_strResult;
		}
	}
}

