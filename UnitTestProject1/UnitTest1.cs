using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PM_02___Vorobev_31P;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            crit Cp = new crit();
            Cp.solution();
            Assert.AreEqual(29, Cp.max);
        }

        [TestMethod]
        public void TestMethod4()
        {
            crit Cp = new crit();
            Cp.solution();
            Assert.IsNotNull(Cp.str);
        }

        [TestMethod]
        public void TestMethod2()
        {
            crit Cp = new crit();
            Cp.solution();
            Assert.IsInstanceOfType(Cp.str, typeof(string));
        }

        [TestMethod]
        public void TestMethod3()
        {
            crit Cp = new crit();
            Cp.solution();
            Assert.IsTrue(Cp.max == 29);
        }

    }
}
