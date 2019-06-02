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
            // field
            private string _private = "original";
            private readonly string _privateReadOnly = "original";
            private static string _privateStatic = "original";
            private dynamic _dynamic = "original";

            // property
            private string Private { get; set; } = "original";
            private static string PrivateStatic { get; set; } = "original";
            private string PrivateGetterOnly { get; } = "original";
            private static string PrivateGetterOnlyStatic { get; } = "original";

            // accessibility
            protected string Protected { get; private set; } = "original";
            internal string Internal { get; private set; } = "original";
            protected internal string ProtectedInternal { get; private set; } = "original";
            public string Public { get; private set; } = "original";

            // virtual
            protected virtual string ProtectedVirtual { get; private set; } = "original";

            // other
            private IEnumerable<int> _interface = new int[] { };
        }

        private class Derived : Base
        {
        }

        [Fact]
        public void Fields()
        {
            var derived = new Derived();

            derived.SetPrivate("_private", "changed");
            derived.SetPrivate("_privateReadOnly", "changed");
            derived.SetPrivate("_privateStatic", "changed");
            derived.SetPrivate("_dynamic", new List<int>() { 1, 2, 3 });

            Assert.Equal("changed", derived.GetPrivate("_private"));
            Assert.Equal("changed", derived.GetPrivate("_privateReadOnly"));
            Assert.Equal("changed", derived.GetPrivate("_privateStatic"));
            Assert.Equal(3, derived.GetPrivate<dynamic>("_dynamic").Count);
        }

        [Fact]
        public void Properties()
        {
            var derived = new Derived();

            derived.SetPrivate("Private", "changed");
            derived.SetPrivate("PrivateStatic", "changed");
            derived.SetPrivate("PrivateGetterOnly", "changed");
            derived.SetPrivate("PrivateGetterOnlyStatic", "changed");

            Assert.Equal("changed", derived.GetPrivate("Private"));
            Assert.Equal("changed", derived.GetPrivate("PrivateStatic"));
            Assert.Equal("changed", derived.GetPrivate("PrivateGetterOnly"));
            Assert.Equal("changed", derived.GetPrivate("PrivateGetterOnlyStatic"));
        }

        [Fact]
        public void Accessibilities()
        {
            var derived = new Derived();

            derived.SetPrivate("Protected", "changed");
            derived.SetPrivate("Internal", "changed");
            derived.SetPrivate("ProtectedInternal", "changed");
            derived.SetPrivate("Public", "changed");

            Assert.Equal("changed", derived.GetPrivate("Protected"));
            Assert.Equal("changed", derived.GetPrivate("Internal"));
            Assert.Equal("changed", derived.GetPrivate("ProtectedInternal"));
            Assert.Equal("changed", derived.GetPrivate("Public"));
        }

        [Fact]
        public void Virtual()
        {
            var derived = new Derived();

            derived.SetPrivate("ProtectedVirtual", "changed");

            Assert.Equal("changed", derived.GetPrivate("ProtectedVirtual"));
        }

        [Fact]
        public void GetPrivate_Generics()
        {
            var derived = new Derived();

            derived.SetPrivate("_interface", new List<int>() { 1, 2, 3 });

            Assert.Equal(3, derived.GetPrivate<IEnumerable<int>>("_interface").Count());
        }

        [Fact]
        public void GetPrivate_SpecifyingWrongNameThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => new Derived().GetPrivate("_"));
        }

        [Fact]
        public void SetPrivate_SpecifyingWrongNameThrowsException()
        {
            Assert.Throws<ArgumentException>(
                () => new Derived().SetPrivate("_", ""));
        }

        [Fact]
        public void CanGetAndSetStaticMembersByType()
        {
            typeof(Base).SetPrivate("_privateStatic", "changed2");
            typeof(Base).SetPrivate("PrivateGetterOnlyStatic", "changed2");

            Assert.Equal("changed2", typeof(Base).GetPrivate("_privateStatic"));
            Assert.Equal("changed2", typeof(Base).GetPrivate("PrivateGetterOnlyStatic"));
        }

        [Fact]
        public void BaseTypeIsNotSearchedForStaticMembersByType()
        {
            Assert.Throws<ArgumentException>(
                () => typeof(Derived).SetPrivate("_privateStatic", ""));
        }
    }
}
