using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TestService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : ITestService
    {
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string GetData()
        {
            return "Here is a nice string";
        }

        public void SetData(byte[] binaryData, double importantNumber)
        {
        }

        public long PerformOperation(List<double> things, int widgetValue)
        {
            return DateTime.Now.ToBinary();
        }

        public void ThrowException()
        {
            throw new Exception("Danger Will Robinson!");
        }
    }
}
