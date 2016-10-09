using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample
{
    [TestClass]
    public class UnitTest2
    {
        private class Base
        {
            private string _private = "base";

            private string Property { get { return "property"; } }
        }

        private class Derived : Base
        {
            private string _private = "derived";

            public string Property()
            {
                return "method";
            }
        }

        [TestMethod]
        public void RealTypeIsPriority()
        {
            var derivedAsBase = new Derived() as Base;

            Assert.AreEqual("derived", derivedAsBase.GetPrivate("_private"));
        }

        [TestMethod]
        public void MethodIsIgnored()
        {
            var derived = new Derived();

            Assert.AreEqual("property", derived.GetPrivate("Property"));
        }
    }
}
