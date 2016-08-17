#include "stdafx.h"
#include "TestServiceImpl.h"

namespace org
{
	namespace tempuri
	{
		CTestServiceImpl::CTestServiceImpl()
		{
		}

		CTestServiceImpl::~CTestServiceImpl()
		{
		}

		std::string CTestServiceImpl::GetData()
		{
			return "hello world";
		}

		CompositeType CTestServiceImpl::GetDataUsingDataContract(CompositeType composite)
		{
			return composite;
		}
			
		void CTestServiceImpl::SetData(const std::vector<char>& binaryData, double importantNumber)
		{
		}
		
		long long CTestServiceImpl::PerformOperation(const std::vector<double> things, int widgetValue)
		{
			return 49383;
		}
		
		void CTestServiceImpl::ThrowException()
		{
			throw std::exception("something bad happened");
		}
	}
}

