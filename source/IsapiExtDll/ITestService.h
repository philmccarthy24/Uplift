#pragma once

#include <string>
#include <vector>

namespace org
{
	namespace tempuri
	{
		struct CompositeType
		{
			bool BoolValue;
			std::string StringValue;
		};
		
		// <% WebService %>
		class ITestService
		{
			// <% ServiceVersion = 1.0 %>
		public:

			virtual ~ITestService() {}

			virtual std::string GetData() = 0;
			virtual CompositeType GetDataUsingDataContract(CompositeType composite) = 0;
			virtual void SetData(const std::vector<char>& binaryData, double importantNumber) = 0;
			virtual long long PerformOperation(const std::vector<double> things, int widgetValue) = 0;
			virtual void ThrowException() = 0;
		};
	}
}
