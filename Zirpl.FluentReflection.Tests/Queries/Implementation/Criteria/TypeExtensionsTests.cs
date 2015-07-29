using System;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using Zirpl.FluentReflection.Queries;

namespace Zirpl.FluentReflection.Tests.Queries
{
    [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void TestGetAssociatedType()
        {
            MemberInfo memberInfo = null;
            Type resultingType = null;

            // Event
            memberInfo = typeof (MockType).GetEvent("InstanceEvent");
            resultingType = memberInfo.GetAssociatedType(TypeSource.EventHandlerType);
            resultingType.Should().NotBeNull();
            resultingType.Should().Be(typeof (EventHandler));
            new Action(() => memberInfo.GetAssociatedType(TypeSource.FieldType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.MethodReturnType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.PropertyType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.Self)).ShouldThrow<Exception>();

            // Field
            memberInfo = typeof(MockType).GetField("InstanceField");
            resultingType = memberInfo.GetAssociatedType(TypeSource.FieldType);
            resultingType.Should().NotBeNull();
            resultingType.Should().Be(typeof(Int32));
            new Action(() => memberInfo.GetAssociatedType(TypeSource.EventHandlerType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.MethodReturnType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.PropertyType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.Self)).ShouldThrow<Exception>();

            // Property
            memberInfo = typeof(MockType).GetProperty("InstanceProperty");
            resultingType = memberInfo.GetAssociatedType(TypeSource.PropertyType);
            resultingType.Should().NotBeNull();
            resultingType.Should().Be(typeof(Int32));
            new Action(() => memberInfo.GetAssociatedType(TypeSource.EventHandlerType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.MethodReturnType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.FieldType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.Self)).ShouldThrow<Exception>();

            // Property
            memberInfo = typeof(MockType).GetMethod("InstanceMethod");
            resultingType = memberInfo.GetAssociatedType(TypeSource.MethodReturnType);
            resultingType.Should().NotBeNull();
            resultingType.Should().Be(typeof(void));
            new Action(() => memberInfo.GetAssociatedType(TypeSource.EventHandlerType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.PropertyType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.FieldType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.Self)).ShouldThrow<Exception>();

            // Property
            memberInfo = typeof(MockType).GetNestedType("NestedType");
            resultingType = memberInfo.GetAssociatedType(TypeSource.Self);
            resultingType.Should().NotBeNull();
            resultingType.Should().Be(typeof(MockType.NestedType));
            new Action(() => memberInfo.GetAssociatedType(TypeSource.EventHandlerType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.PropertyType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.FieldType)).ShouldThrow<Exception>();
            new Action(() => memberInfo.GetAssociatedType(TypeSource.MethodReturnType)).ShouldThrow<Exception>();
        }

        #region Helpers

        // instance
        // static
        // declared on this type
        // declared on base type

        protected class MockType
        {
            // constructors???
            // nested types???

            public event EventHandler InstanceEvent;

            public int InstanceField;

            public int InstanceProperty { get; set; }

            public void InstanceMethod()
            {

            }

            public class NestedType
            {

            }
        }


        #endregion
    }
}
