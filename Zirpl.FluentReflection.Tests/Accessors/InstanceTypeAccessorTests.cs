using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Zirpl.FluentReflection.Accessors;

namespace Zirpl.FluentReflection.Tests.Accessors
{
    [TestFixture]
    public class InstanceTypeAccessorTests
    {
        [TestCase(typeof(A), "PublicPropertyOnA", true, TestName = "PublicPropertyOnA")]
        [TestCase(typeof(A), "ProtectedPropertyOnA", true, TestName = "ProtectedPropertyOnA")]
        [TestCase(typeof(A), "PrivatePropertyOnA", true, TestName = "PrivatePropertyOnA")]
        [TestCase(typeof(A), "ProtectedInternalPropertyOnA", true, TestName = "ProtectedInternalPropertyOnA")]
        [TestCase(typeof(A), "InternalPropertyOnA", true, TestName = "InternalPropertyOnA")]
        [TestCase(typeof(A), "publicpropertyona", true, TestName = "publicpropertyona")]
        [TestCase(typeof(A), "protectedpropertyona", true, TestName = "protectedpropertyona")]
        [TestCase(typeof(A), "privatepropertyona", true, TestName = "privatepropertyona")]
        [TestCase(typeof(A), "protectedinternalpropertyona", true, TestName = "protectedinternalpropertyona")]
        [TestCase(typeof(A), "internalpropertyona", true, TestName = "internalpropertyona")]
        [TestCase(typeof(B), "PublicPropertyOnA", true, TestName = "PublicPropertyOnA-ThroughB")]
        [TestCase(typeof(B), "ProtectedPropertyOnA", true, TestName = "ProtectedPropertyOnA-ThroughB")]
        [TestCase(typeof(B), "PrivatePropertyOnA", true, TestName = "PrivatePropertyOnA-ThroughB")]
        [TestCase(typeof(B), "ProtectedInternalPropertyOnA", true, TestName = "ProtectedInternalPropertyOnA-ThroughB")]
        [TestCase(typeof(B), "InternalPropertyOnA", true, TestName = "InternalPropertyOnA-ThroughB")]
        [TestCase(typeof(B), "publicpropertyona", true, TestName = "publicpropertyona-ThroughB")]
        [TestCase(typeof(B), "protectedpropertyona", true, TestName = "protectedpropertyona-ThroughB")]
        [TestCase(typeof(B), "privatepropertyona", true, TestName = "privatepropertyona-ThroughB")]
        [TestCase(typeof(B), "protectedinternalpropertyona", true, TestName = "protectedinternalpropertyona-ThroughB")]
        [TestCase(typeof(B), "internalpropertyona", true, TestName = "internalpropertyona-ThroughB")]
        public void TestPropertyInfo(Type type, String name, bool expectedToBeFound)
        {
            var accessor = InstanceTypeAccessor.Get(type);
            var property = accessor.PropertyInfo(name);
            if (expectedToBeFound)
            {
                property.Should().NotBeNull();
            }
            else
            {
                property.Should().BeNull();
            }
        }

        public class A
        {
            public int PublicPropertyOnA { get; set; }
            protected int ProtectedPropertyOnA { get; set; }
            private int PrivatePropertyOnA { get; set; }
            protected internal int ProtectedInternalPropertyOnA { get; set; }
            internal int InternalPropertyOnA { get; set; }
        }

        public class B : A
        {
            public int PublicPropertyOnB { get; set; }
            protected int ProtectedPropertyOnB { get; set; }
            private int PrivatePropertyOnB { get; set; }
            protected internal int ProtectedInternalPropertyOnB { get; set; }
            internal int InternalPropertyOnB { get; set; }
        }
    }
}
