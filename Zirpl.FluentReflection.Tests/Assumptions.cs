using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Zirpl.FluentReflection.Tests
{
    [TestFixture]
    public class Assumptions
    {
        public class A { }
        public class B : A { }
        public class C : B { }
        public class Mock
        {
            public B publicField;
            private B privateField;
            protected B protectedField;
            internal B internalField;
            protected internal B protectedInternalField;

            public static B publicStaticField;
            private static B privateStaticField;
            protected static B protectedStaticField;
            internal static B internalStaticField;
            protected internal static B protectedInternalStaticField;

            public static readonly int readonlyStaticInt;
            public readonly int readonlyInt;
        }
        public class MockDerived : Mock
        {

        }

        public abstract class AbstractClass
        {
            public abstract void MyMethod();
            private int field1;

            public virtual void SetField(int i)
            {
                field1 = i;
            }
            private void SetTheField(int i)
            {
                field1 = i;
            }
        }

        public class ConcreteClass : AbstractClass
        {
            public override void MyMethod()
            {
            }
            private int field1;

            public override void SetField(int i)
            {
                field1 = i;
            }

            private void SetTheField(int i)
            {
                field1 = i;
            }
        }

        [Test]
        public void TestAssumptions_PrivateHiddenByNameAndSignatureFields()
        {
            var fieldOnBase = typeof(AbstractClass).GetField("field1", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldOnBase.Should().NotBeNull();
            var field = typeof(ConcreteClass).GetField("field1", BindingFlags.Instance | BindingFlags.NonPublic);
            field.Should().NotBeNull();
            var mock = new ConcreteClass();
            field.SetValue(mock, 1);
            field.GetValue(mock).Should().Be(1);
            fieldOnBase.SetValue(mock, 2);
            fieldOnBase.GetValue(mock).Should().Be(2);
            field.GetValue(mock).Should().Be(1);
        }

        [Test]
        public void TestAssumption_PrivateHiddenByNameAndSignatureMethods()
        {
            var fieldOnBase = typeof(AbstractClass).GetField("field1", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldOnBase.Should().NotBeNull();
            var field = typeof(ConcreteClass).GetField("field1", BindingFlags.Instance | BindingFlags.NonPublic);
            field.Should().NotBeNull();

            var methodOnBase = typeof(AbstractClass).GetMethod("SetTheField", BindingFlags.Instance | BindingFlags.NonPublic);
            methodOnBase.Should().NotBeNull();
            var method = typeof(ConcreteClass).GetMethod("SetTheField", BindingFlags.Instance | BindingFlags.NonPublic);
            method.Should().NotBeNull();
            var mock = new ConcreteClass();
            method.Invoke(mock, new Object[] { 1 });
            field.GetValue(mock).Should().Be(1);
            fieldOnBase.GetValue(mock).Should().Be(0);
            // the CONCRETE method will be run, despite the method info reference the abstract class
            methodOnBase.Invoke(mock, new Object[] { 2 });
            fieldOnBase.GetValue(mock).Should().Be(2);
            field.GetValue(mock).Should().Be(1);
        }

        [Test]
        public void TestAssumption_PublicOverriddenMethods()
        {
            var fieldOnBase = typeof(AbstractClass).GetField("field1", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldOnBase.Should().NotBeNull();
            var field = typeof(ConcreteClass).GetField("field1", BindingFlags.Instance | BindingFlags.NonPublic);
            field.Should().NotBeNull();

            var methodOnBase = typeof(AbstractClass).GetMethod("SetField");
            methodOnBase.Should().NotBeNull();
            var method = typeof(ConcreteClass).GetMethod("SetField");
            method.Should().NotBeNull();
            var mock = new ConcreteClass();
            method.Invoke(mock, new Object[] { 1 });
            field.GetValue(mock).Should().Be(1);
            fieldOnBase.GetValue(mock).Should().Be(0);
            // the CONCRETE method will be run, despite the method info reference the abstract class
            methodOnBase.Invoke(mock, new Object[] { 2 });
            fieldOnBase.GetValue(mock).Should().Be(0);
            field.GetValue(mock).Should().Be(2);
        }

        [Test]
        public void TestAssumptions_AbstractClassesAndMembers()
        {
            typeof(AbstractClass).GetMethod("MyMethod").Should().NotBeNull();
            typeof(AbstractClass).GetMethod("MyMethod").IsAbstract.Should().BeTrue();
            typeof(AbstractClass).GetRuntimeMethod("MyMethod", new Type[0]).Should().NotBeNull();
            typeof(AbstractClass).GetRuntimeMethod("MyMethod", new Type[0]).IsAbstract.Should().BeTrue();

            typeof(ConcreteClass).GetMethod("MyMethod").Should().NotBeNull();
            typeof(ConcreteClass).GetMethod("MyMethod").IsAbstract.Should().BeFalse();
            typeof(ConcreteClass).GetRuntimeMethod("MyMethod", new Type[0]).Should().NotBeNull();
            typeof(ConcreteClass).GetRuntimeMethod("MyMethod", new Type[0]).IsAbstract.Should().BeFalse();
        }

        [Test]
        public void TestAssumptions_BindingFlagsAndDefaults()
        {
            // instance fields directly on type, no flags
            typeof(Mock).GetField("publicField").Should().NotBeNull();
            typeof(Mock).GetField("privateField").Should().BeNull();
            typeof(Mock).GetField("protectedField").Should().BeNull();
            typeof(Mock).GetField("internalField").Should().BeNull();
            typeof(Mock).GetField("protectedInternalField").Should().BeNull();

            // static fields directly on type, no flags
            typeof(Mock).GetField("publicStaticField").Should().NotBeNull();
            typeof(Mock).GetField("privateStaticField").Should().BeNull();
            typeof(Mock).GetField("protectedStaticField").Should().BeNull();
            typeof(Mock).GetField("internalStaticField").Should().BeNull();
            typeof(Mock).GetField("protectedInternalStaticField").Should().BeNull();

            // you can't have just the instance flag right?
            typeof(Mock).GetField("publicField", BindingFlags.Instance).Should().BeNull();
            // you can't have just the static flag right?
            typeof(Mock).GetField("publicStaticField", BindingFlags.Static).Should().BeNull();
            // you can't have just the public flag right?
            typeof(Mock).GetField("publicField", BindingFlags.Public).Should().BeNull();
            // you can't have just the nonpublic flag right?
            typeof(Mock).GetField("publicField", BindingFlags.NonPublic).Should().BeNull();

            // everything directly on the class is returned with the correct flag right?

            // instance fields directly on type, no flags
            typeof(Mock).GetField("publicField", BindingFlags.Public | BindingFlags.Instance).Should().NotBeNull();
            typeof(Mock).GetField("privateField", BindingFlags.NonPublic | BindingFlags.Instance).Should().NotBeNull();
            typeof(Mock).GetField("protectedField", BindingFlags.NonPublic | BindingFlags.Instance).Should().NotBeNull();
            typeof(Mock).GetField("internalField", BindingFlags.NonPublic | BindingFlags.Instance).Should().NotBeNull();
            typeof(Mock).GetField("protectedInternalField", BindingFlags.NonPublic | BindingFlags.Instance).Should().NotBeNull();

            // static fields directly on type, no flags
            typeof(Mock).GetField("publicStaticField", BindingFlags.Public | BindingFlags.Static).Should().NotBeNull();
            typeof(Mock).GetField("privateStaticField", BindingFlags.NonPublic | BindingFlags.Static).Should().NotBeNull();
            typeof(Mock).GetField("protectedStaticField", BindingFlags.NonPublic | BindingFlags.Static).Should().NotBeNull();
            typeof(Mock).GetField("internalStaticField", BindingFlags.NonPublic | BindingFlags.Static).Should().NotBeNull();
            typeof(Mock).GetField("protectedInternalStaticField", BindingFlags.NonPublic | BindingFlags.Static).Should().NotBeNull();

            // we can set readonly fields, right?
            var mock = new Mock();
            typeof(Assumptions.Mock).GetField("readonlyInt").SetValue(mock, 1);
            mock.readonlyInt.Should().Be(1);
            Assumptions.Mock.readonlyStaticInt.Should().Be(0);
            typeof(Assumptions.Mock).GetField("readonlyStaticInt", BindingFlags.Public | BindingFlags.Static).SetValue(null, 2);
            Console.WriteLine(Mock.readonlyStaticInt);
            // this line fails if x86, 
            //Assumptions.Mock.readonlyStaticInt.Should().Be(2);

            // now we're testing how derived members are handled so we always use the correct flags

            // public instance fields in the base class are returned with correct binding flags
            // non-private fields in the base class are also returned with correct binding flags
            // private fields in the base class are NEVER returned
            //
            typeof(MockDerived).GetField("publicField", BindingFlags.Public | BindingFlags.Instance).Should().NotBeNull();
            typeof(MockDerived).GetField("privateField", BindingFlags.NonPublic | BindingFlags.Instance).Should().BeNull();
            typeof(MockDerived).GetField("protectedField", BindingFlags.NonPublic | BindingFlags.Instance).Should().NotBeNull();
            typeof(MockDerived).GetField("internalField", BindingFlags.NonPublic | BindingFlags.Instance).Should().NotBeNull();
            typeof(MockDerived).GetField("protectedInternalField", BindingFlags.NonPublic | BindingFlags.Instance).Should().NotBeNull();

            // static fields in the base class are never returned...
            typeof(MockDerived).GetField("publicStaticField", BindingFlags.Public | BindingFlags.Static).Should().BeNull();
            typeof(MockDerived).GetField("privateStaticField", BindingFlags.NonPublic | BindingFlags.Static).Should().BeNull();
            typeof(MockDerived).GetField("protectedStaticField", BindingFlags.NonPublic | BindingFlags.Static).Should().BeNull();
            typeof(MockDerived).GetField("internalStaticField", BindingFlags.NonPublic | BindingFlags.Static).Should().BeNull();
            typeof(MockDerived).GetField("protectedInternalStaticField", BindingFlags.NonPublic | BindingFlags.Static).Should().BeNull();
            // except with the flatten heirarchy
            // but even then, private fields are not returned
            typeof(MockDerived).GetField("publicStaticField", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).Should().NotBeNull();
            typeof(MockDerived).GetField("privateStaticField", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).Should().BeNull();
            typeof(MockDerived).GetField("protectedStaticField", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).Should().NotBeNull();
            typeof(MockDerived).GetField("internalStaticField", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).Should().NotBeNull();
            typeof(MockDerived).GetField("protectedInternalStaticField", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy).Should().NotBeNull();

            // other randoms
            typeof(Mock).GetField("publicStaticField", BindingFlags.Public | BindingFlags.Instance).Should().BeNull();
        }

    }
}
