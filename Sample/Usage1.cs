using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sample
{
    [TestClass]
    public class Usage1
    {
        private class Base
        {
            private string _private = "private member";
            private static string _privateStatic = "private static member";
            protected string Protected { get; private set; }
            protected static string ProtectedStatic { get; private set; }
            protected virtual string ProtectedVirtual { get; private set; }
            public string Public { get; private set; }
            public static string PublicStatic { get; private set; }
            public virtual string PublicVirtual { get; private set; }

            private int _struct = 0;
            private IEnumerable<int> _interface = new int[] { };
            private int[] _array = new int[] { };

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

        private class Derived : Base
        {
        }

        [TestMethod]
        public void CanGetAndSetMembers()
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

            derived.SetPrivate("_struct", 123);
            derived.SetPrivate("_interface", new List<int>() { 1, 2, 3 });
            derived.SetPrivate("_array", new int[] { 1, 2, 3 });

            Assert.AreEqual("changed", derived.GetPrivate("_private"));
            Assert.AreEqual("changed", derived.GetPrivate("_privateStatic"));
            Assert.AreEqual("changed", derived.GetPrivate("Protected"));
            Assert.AreEqual("changed", derived.GetPrivate("ProtectedVirtual"));
            Assert.AreEqual("changed", derived.GetPrivate("ProtectedStatic"));
            Assert.AreEqual("changed", derived.GetPrivate("Public"));
            Assert.AreEqual("changed", derived.GetPrivate("PublicStatic"));
            Assert.AreEqual("changed", derived.GetPrivate("PublicVirtual"));

            Assert.AreEqual(123, derived.GetPrivate("_struct"));
            Assert.AreEqual(3, derived.GetPrivate<IEnumerable<int>>("_interface").Count());
            Assert.AreEqual(3, derived.GetPrivate<int[]>("_array").Length);
        }
    }
}
