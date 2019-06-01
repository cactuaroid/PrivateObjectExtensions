using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample
{
    [TestFixture]
    public class Usage1
    {
        private class Base
        {
            // accessibility
            private string _private = "original";
            private readonly string _privateReadOnly = "original";
            private string Private { get; set; } = "original";
            private string GetterOnly { get; } = "original";
            protected string Protected { get; private set; } = "original";
            internal string Internal { get; private set; } = "original";
            protected internal string ProtectedInternal { get; private set; } = "original";
            public string Public { get; private set; } = "original";

            // static
            private static string _privateStatic = "original";
            private static string GetterOnlyStatic { get; } = "original";

            // virtual
            protected virtual string ProtectedVirtual { get; private set; } = "original";

            // type
            private int _struct = 0;
            private IEnumerable<int> _interface = new int[] { };
            private int[] _array = new int[] { };
            private dynamic _dynamic = "original";
        }

        private class Derived : Base
        {
        }

        [Test]
        public void CanGetAndSetMembers()
        {
            var derived = new Derived();

            derived.SetPrivate("_private", "changed");
            derived.SetPrivate("_privateReadOnly", "changed");
            derived.SetPrivate("Private", "changed");
            derived.SetPrivate("GetterOnly", "changed");
            derived.SetPrivate("Protected", "changed");
            derived.SetPrivate("Internal", "changed");
            derived.SetPrivate("ProtectedInternal", "changed");
            derived.SetPrivate("Public", "changed");
            Assert.AreEqual("changed", derived.GetPrivate("_private"));
            Assert.AreEqual("changed", derived.GetPrivate("_privateReadOnly"));
            Assert.AreEqual("changed", derived.GetPrivate("Private"));
            Assert.AreEqual("changed", derived.GetPrivate("GetterOnly"));
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

        [Test]
        public void CanGetAndSetStaticMembersByType()
        {
            typeof(Base).SetPrivate("_privateStatic", "changed2");
            typeof(Base).SetPrivate("GetterOnlyStatic", "changed2");
            Assert.AreEqual("changed2", typeof(Base).GetPrivate("_privateStatic"));
            Assert.AreEqual("changed2", typeof(Base).GetPrivate("GetterOnlyStatic"));
        }

        [Test]
        public void BaseTypeIsNotSearchedForStaticMembersByType()
        {
            try
            {
                typeof(Derived).SetPrivate("_privateStatic", "changed3");
            }
            catch(ArgumentException)
            {
                return;
            }

            Assert.Fail();
        }
    }
}
