using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample
{
    [TestClass]
    public class Usage2
    {
        private class Base
        {
            private string _private = "base";

            protected string Property { get { return "property"; } }
        }

        private class Derived : Base
        {
            private string _private = "derived";

            new public string Property() // this hides 'Base.Property'
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
        public void CanSpecifyTypeForSeaching()
        {
            var derived = new Derived();

            Assert.AreEqual("base", derived.GetPrivate("_private", typeof(Base)));
        }

        [TestMethod]
        public void MethodIsIgnored()
        {
            var derived = new Derived();

            Assert.AreEqual("property", derived.GetPrivate("Property"));
        }
    }
}
