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

        [Fact]
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
            Assert.Equal("changed", derived.GetPrivate("_private"));
            Assert.Equal("changed", derived.GetPrivate("_privateReadOnly"));
            Assert.Equal("changed", derived.GetPrivate("Private"));
            Assert.Equal("changed", derived.GetPrivate("GetterOnly"));
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
            typeof(Base).SetPrivate("GetterOnlyStatic", "changed2");
            Assert.Equal("changed2", typeof(Base).GetPrivate("_privateStatic"));
            Assert.Equal("changed2", typeof(Base).GetPrivate("GetterOnlyStatic"));
        }

        [Fact]
        public void BaseTypeIsNotSearchedForStaticMembersByType()
        {
            Assert.Throws<ArgumentException>(() => typeof(Derived).SetPrivate("_privateStatic", "changed3"));
        }
    }
}
