using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample
{
    public class Base
    {
        private string _private = "private member";
        private static string _privateStatic = "private static member";
        protected string Protected { get; private set; }
        protected static string ProtectedStatic { get; private set; }
        protected virtual string ProtectedVirtual { get; private set; }
        public string Public { get; private set; }
        public static string PublicStatic { get; private set; }
        public virtual string PublicVirtual { get; private set; }

        public Base()
        {
            Protected = "protected member";
            ProtectedStatic = "protected static member";
            ProtectedVirtual = "protected virtual member";
            Public = "public member";
            PublicStatic = "public static member";
            PublicVirtual = "public virtual member";
        }
    }

    public class Derived : Base
    {
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanGetMembers()
        {
            var derived = new Derived();

            Assert.IsNotNull(derived.GetPrivate("_private"));
            Assert.IsNotNull(derived.GetPrivate("_privateStatic"));
            Assert.IsNotNull(derived.GetPrivate("Protected"));
            Assert.IsNotNull(derived.GetPrivate("ProtectedStatic"));
            Assert.IsNotNull(derived.GetPrivate("ProtectedVirtual"));
            Assert.IsNotNull(derived.GetPrivate("Public"));
            Assert.IsNotNull(derived.GetPrivate("PublicStatic"));
            Assert.IsNotNull(derived.GetPrivate("PublicVirtual"));
        }

        [TestMethod]
        public void CanSetMembers()
        {
            var derived = new Derived();

            derived.SetPrivate("_private", "changed");
            derived.SetPrivate("_privateStatic", "changed");
            derived.SetPrivate("Protected", "changed");
            derived.SetPrivate("ProtectedVirtual", "changed");
            derived.SetPrivate("ProtectedStatic", "changed");
            derived.SetPrivate("Public", "changed");
            derived.SetPrivate("PublicStatic", "changed");
            derived.SetPrivate("PublicVirtual", "changed");

            Assert.AreEqual("changed", derived.GetPrivate("_private"));
            Assert.AreEqual("changed", derived.GetPrivate("_privateStatic"));
            Assert.AreEqual("changed", derived.GetPrivate("Protected"));
            Assert.AreEqual("changed", derived.GetPrivate("ProtectedVirtual"));
            Assert.AreEqual("changed", derived.GetPrivate("ProtectedStatic"));
            Assert.AreEqual("changed", derived.GetPrivate("Public"));
            Assert.AreEqual("changed", derived.GetPrivate("PublicStatic"));
            Assert.AreEqual("changed", derived.GetPrivate("PublicVirtual"));
        }
    }
}
