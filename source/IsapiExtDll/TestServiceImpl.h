#pragma once

#include "ITestService.h"

namespace org
{
	namespace tempuri
	{
		class CTestServiceImpl : public ITestService
		{
		public:
			CTestServiceImpl();
			virtual ~CTestServiceImpl();

			virtual std::string GetData() override;
			virtual CompositeType GetDataUsingDataContract(CompositeType composite) override;
			virtual void SetData(const std::vector<char>& binaryData, double importantNumber) override;
			virtual long long PerformOperation(const std::vector<double> things, int widgetValue) override;
			virtual void ThrowException() override;
		};
	}
}


