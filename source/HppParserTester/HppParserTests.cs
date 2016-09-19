using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uplift.HppParser;
using TypeAndName = System.Collections.Generic.Dictionary<string, string>;

namespace HppParserTester
{
    [TestClass]
    public class HppParserTests
    {
        [TestMethod]
        public void HppParserTests_Function_ToString_NoArgsNoReturn_CreatesGoodOutput()
        {
            var function = new Function("Func1");
            const string expeectedReturn = "void Func1();";
            Assert.AreEqual(expeectedReturn, function.ToString());
        }

        [TestMethod]
        public void HppParserTests_Function_ToString_OneArgNoReturn_CreatesGoodOutput()
        {
            var function = new Function("Func1", new TypeAndName { ["Arg1"] = "arg1" }); 
            const string expeectedReturn = "void Func1(Arg1 arg1);";
            Assert.AreEqual(expeectedReturn, function.ToString());
        }

        [TestMethod]
        public void HppParserTests_Function_ToString_TwoArgNoReturn_CreatesGoodOutput()
        {
            var function = new Function("Func1", new TypeAndName { ["Arg1"] = "arg1", ["int"] = "numberOfTimes"});
            const string expeectedReturn = "void Func1(Arg1 arg1, int numberOfTimes);";
            Assert.AreEqual(expeectedReturn, function.ToString());
        }

        [TestMethod]
        public void HppParserTests_Function_ToString_TwoArgWithReturn_CreatesGoodOutput()
        {
            var function = new Function("Func1", new TypeAndName { ["Arg1"] = "arg1", ["int"] = "numberOfTimes" }, "int");
            const string expeectedReturn = "int Func1(Arg1 arg1, int numberOfTimes);";
            Assert.AreEqual(expeectedReturn, function.ToString());
        }

        [TestMethod]
        public void HppParserTests_Parse()
        {
            var res = HppParser.ParseFile(@"C:\Users\edwar\OneDrive\Documents\Work\Uplift\source\IsapiExtDll\ITestService.h");
        }

        

    }
}
