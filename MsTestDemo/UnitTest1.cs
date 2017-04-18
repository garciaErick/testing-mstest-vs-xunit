using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankingSystem;

namespace MsTestDemo
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void TestMethod1()   
        {
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(2, 1 + 1);
        }

        [TestMethod]
        public void TestMethod3()
        {
            String x = "hello";
        }
    }
}
