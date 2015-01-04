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

        //[Test]
        //public void TestAssumptions_RuntimeMembers()
        //{
        //    foreach (var field in typeof(Mock).GetTypeInfo().GetRuntimeProperty())
        //    {
        //        Console.WriteLine(field.Name);
        //    }
        //}

        [Test]
        public void TestAssumptions()
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
            typeof(Mock).GetField("readonlyInt").SetValue(mock, 1);
            mock.readonlyInt.Should().Be(1);
            typeof(Mock).GetField("readonlyStaticInt").SetValue(null, 1);
            Mock.readonlyStaticInt.Should().Be(1);

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
