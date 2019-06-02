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
            private object _object = null;

            protected string Property { get { return "property"; } }
        }

        private class Derived : Base
        {
            private int _private = 123;

            new public string Property() // this hides 'Base.Property'
            {
                return "method";
            }
        }

        [TestMethod]
        public void RealTypeIsPriority()
        {
            var derivedAsBase = new Derived() as Base;

            Assert.AreEqual(123, derivedAsBase.GetPrivate("_private"));
        }

        [TestMethod]
        public void CanSpecifyObjectTypeForSeaching()
        {
            var derived = new Derived();

            Assert.AreEqual("base", derived.GetPrivate("_private", typeof(Base)));
        }

        [TestMethod]
        public void CanSpecifyValueTypeForSeaching()
        {
            var derived = new Derived();

            Assert.AreEqual("base", derived.GetPrivate<string>("_private"));
            Assert.AreEqual(123, derived.GetPrivate<int>("_private"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GettingValueTypeNotAllowsBaseType()
        {
            new Derived().GetPrivate<object>("_private");
        }

        [TestMethod]
        public void SettingValueTypeAllowsDerivedType()
        {
            var derived = new Derived();

            derived.SetPrivate("_object", "string");

            Assert.AreEqual("string", derived.GetPrivate("_object"));
        }

        [TestMethod]
        public void MethodIsIgnored()
        {
            var derived = new Derived();

            Assert.AreEqual("property", derived.GetPrivate("Property"));
        }
    }
}
