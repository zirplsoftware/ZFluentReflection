using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using Telerik.JustMock.Helpers;
using Zirpl.FluentReflection.Queries;

namespace Zirpl.FluentReflection.Tests.Queries.Implementation.Criteria
{
    [TestFixture]
    public class ScopeCriteriaTests
    {
        [Test, Combinatorial]
        public void TestShouldRun(
            [Values(true, false)]bool instance,
            [Values(true, false)]bool _static,
            [Values(true, false)]bool declaredOnThisType,
            [Values(true, false)]bool declaredOnBaseType)
        {
            var shouldBeTrue = (declaredOnThisType && !declaredOnBaseType)
                               || (!declaredOnThisType && declaredOnBaseType);

            var scopeCriteria = new ScopeCriteria(typeof (MockType));
            scopeCriteria.Instance = instance;
            scopeCriteria.Static = _static;
            scopeCriteria.DeclaredOnThisType = declaredOnThisType;
            scopeCriteria.DeclaredOnBaseTypes = declaredOnBaseType;

            scopeCriteria.ShouldRun.Should().Be(shouldBeTrue);
        }

        [Test, Combinatorial]
        public void TestGetMatch(
            [Values(true, false)]bool declaredOnThisType,
            [Values(true, false)]bool declaredOnBaseType)
        {
            var scopeCriteria = new ScopeCriteria(typeof(MockType));
            scopeCriteria.DeclaredOnThisType = declaredOnThisType;
            scopeCriteria.DeclaredOnBaseTypes = declaredOnBaseType;

            var memberList = new List<MemberInfo>();
            foreach (var namePrefix in new[] {"Instance", "Static"})
            {
                foreach (var nameSuffix in new[] {"", "OnBase"})
                {
                    var bindingFlags = BindingFlags.Public | (namePrefix == "Instance" ? BindingFlags.Instance : BindingFlags.Static);
                    var type = nameSuffix == "OnBase" ? typeof(MockTypeBase) : typeof(MockType);
                    memberList.Add(type.GetEvent(namePrefix + "Event" + nameSuffix, bindingFlags));
                    memberList.Add(type.GetField(namePrefix + "Field" + nameSuffix, bindingFlags));
                    memberList.Add(type.GetMethod(namePrefix + "Method" + nameSuffix, bindingFlags));
                    memberList.Add(type.GetProperty(namePrefix + "Property" + nameSuffix, bindingFlags));
                }
            }
            memberList.Add(typeof(ScopeCriteriaTests.MockType).GetNestedType("NestedType",
                BindingFlags.Static | BindingFlags.Public));
            memberList.Add(typeof(ScopeCriteriaTests.MockTypeBase).GetNestedType("NestedTypeOnBase",
                BindingFlags.Static | BindingFlags.Public));
            // sanity check:
            memberList.Count(o => o != null).Should().Be(18);

            if (scopeCriteria.ShouldRun)
            {
                var matches = scopeCriteria.GetMatches(memberList.ToArray());
                matches.Count(o => o.Name.Contains("OnBase")).Should().Be(declaredOnBaseType ? 9 : 0);
                matches.Count(o => !o.Name.Contains("OnBase")).Should().Be(declaredOnThisType ? 9 : 0);
            }
        }

        #region Helpers

        // instance
        // static
        // declared on this type
        // declared on base type

        protected class MockType : MockTypeBase
        {
            // constructors???
            // nested types???

            public event EventHandler InstanceEvent;
            public static event EventHandler StaticEvent;

            public int InstanceField;
            public static int StaticField;

            public int InstanceProperty { get; set; }
            public static int StaticProperty { get; set; }

            public void InstanceMethod()
            {

            }

            public static void StaticMethod()
            {

            }

            public class NestedType
            {
                
            }
        }

        protected class MockTypeBase
        {
            // constructors???
            // nested types???

            public event EventHandler InstanceEventOnBase;
            public static event EventHandler StaticEventOnBase;

            public int InstanceFieldOnBase;
            public static int StaticFieldOnBase;

            public int InstancePropertyOnBase { get; set; }
            public static int StaticPropertyOnBase { get; set; }

            public void InstanceMethodOnBase()
            {
                
            }

            public static void StaticMethodOnBase()
            {
                
            }

            public class NestedTypeOnBase
            {

            }
        }
        #endregion
    }
}
