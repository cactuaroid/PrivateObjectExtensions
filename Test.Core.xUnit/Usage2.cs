using System;
using Xunit;

namespace Sample
{
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

        [Fact]
        public void RealTypeIsPriority()
        {
            var derivedAsBase = new Derived() as Base;

            Assert.Equal(123, derivedAsBase.GetPrivate("_private"));
        }

        [Fact]
        public void CanSpecifyObjectTypeForSeaching()
        {
            var derived = new Derived();

            Assert.Equal("base", derived.GetPrivate("_private", typeof(Base)));
        }

        [Fact]
        public void CanSpecifyValueTypeForSeaching()
        {
            var derived = new Derived();

            Assert.Equal("base", derived.GetPrivate<string>("_private"));
            Assert.Equal(123, derived.GetPrivate<int>("_private"));
        }

        [Fact]
        public void GettingValueTypeNotAllowsBaseType()
        {
            Assert.Throws<ArgumentException>(
                () => new Derived().GetPrivate<object>("_private"));
        }

        [Fact]
        public void SettingValueTypeAllowsDerivedType()
        {
            var derived = new Derived();

            derived.SetPrivate("_object", "string");

            Assert.Equal("string", derived.GetPrivate("_object"));
        }

        [Fact]
        public void MethodIsIgnored()
        {
            var derived = new Derived();

            Assert.Equal("property", derived.GetPrivate("Property"));
        }
    }
}
