using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sample
{
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

        [Fact]
        public void CanGetAndSetMembers()
        {
            var derived = new Derived();

            derived.SetPrivate("_private", "changed");
            derived.SetPrivate("Protected", "changed");
            derived.SetPrivate("Internal", "changed");
            derived.SetPrivate("ProtectedInternal", "changed");
            derived.SetPrivate("Public", "changed");
            Assert.Equal("changed", derived.GetPrivate("_private"));
            Assert.Equal("changed", derived.GetPrivate("Protected"));
            Assert.Equal("changed", derived.GetPrivate("Internal"));
            Assert.Equal("changed", derived.GetPrivate("ProtectedInternal"));
            Assert.Equal("changed", derived.GetPrivate("Public"));

            derived.SetPrivate("_privateStatic", "changed");
            Assert.Equal("changed", derived.GetPrivate("_privateStatic"));

            derived.SetPrivate("ProtectedVirtual", "changed");
            Assert.Equal("changed", derived.GetPrivate("ProtectedVirtual"));

            derived.SetPrivate("_struct", 123);
            derived.SetPrivate("_interface", new List<int>() { 1, 2, 3 });
            derived.SetPrivate("_array", new int[] { 1, 2, 3 });
            derived.SetPrivate("_dynamic", new List<int>() { 1, 2, 3 });
            Assert.Equal(123, derived.GetPrivate("_struct"));
            Assert.Equal(3, derived.GetPrivate<IEnumerable<int>>("_interface").Count());
            Assert.Equal(3, derived.GetPrivate<int[]>("_array").Length);
            Assert.Equal(3, derived.GetPrivate<dynamic>("_dynamic").Count);
        }

        [Fact]
        public void CanGetAndSetStaticMembersByType()
        {
            typeof(Base).SetPrivate("_privateStatic", "changed2");
            Assert.Equal("changed2", typeof(Base).GetPrivate("_privateStatic"));
        }

        [Fact]
        public void BaseTypeIsNotSearchedForStaticMembersByType()
        {
            Assert.Throws<ArgumentException>(() => typeof(Derived).SetPrivate("_privateStatic", "changed3"));
        }
    }
}
