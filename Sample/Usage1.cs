using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Sample
{
    [TestClass]
    public class Usage1
    {
        private class Base
        {
            // accessibility
            private string _private = "private";
            protected string Protected { get; private set; }
            internal string Internal { get; private set; }
            protected internal string ProtectedInternal { get; private set; }
            public string Public { get; private set; }

            // static
            private static string _privateStatic = "private static";

            // virtual
            protected virtual string ProtectedVirtual { get; private set; }

            // type
            private int _struct = 0;
            private IEnumerable<int> _interface = new int[] { };
            private int[] _array = new int[] { };
            private dynamic _dynamic = "dynamic member";

            public Base()
            {
                Protected = "protected";
                Internal = "internal";
                ProtectedInternal = "protected internal";
                Public = "public";
                ProtectedVirtual = "protected virtual";
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
            derived.SetPrivate("Protected", "changed");
            derived.SetPrivate("Internal", "changed");
            derived.SetPrivate("ProtectedInternal", "changed");
            derived.SetPrivate("Public", "changed");
            Assert.AreEqual("changed", derived.GetPrivate("_private"));
            Assert.AreEqual("changed", derived.GetPrivate("Protected"));
            Assert.AreEqual("changed", derived.GetPrivate("Internal"));
            Assert.AreEqual("changed", derived.GetPrivate("ProtectedInternal"));
            Assert.AreEqual("changed", derived.GetPrivate("Public"));

            derived.SetPrivate("_privateStatic", "changed");
            Assert.AreEqual("changed", derived.GetPrivate("_privateStatic"));

            derived.SetPrivate("ProtectedVirtual", "changed");
            Assert.AreEqual("changed", derived.GetPrivate("ProtectedVirtual"));

            derived.SetPrivate("_struct", 123);
            derived.SetPrivate("_interface", new List<int>() { 1, 2, 3 });
            derived.SetPrivate("_array", new int[] { 1, 2, 3 });
            derived.SetPrivate("_dynamic", new List<int>() { 1, 2, 3 });
            Assert.AreEqual(123, derived.GetPrivate("_struct"));
            Assert.AreEqual(3, derived.GetPrivate<IEnumerable<int>>("_interface").Count());
            Assert.AreEqual(3, derived.GetPrivate<int[]>("_array").Length);
            Assert.AreEqual(3, derived.GetPrivate<dynamic>("_dynamic").Count);
        }
    }
}
