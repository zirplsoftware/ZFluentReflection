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
    public class MemberTypeCriteriaTests
    {
        [TestCase(MemberTypeFlagsMock.None, Result = false)]
        [TestCase(MemberTypeFlagsMock.Constructor, Result = false)]
        [TestCase(MemberTypeFlagsMock.Event, Result = false)]
        [TestCase(MemberTypeFlagsMock.Field, Result = false)]
        [TestCase(MemberTypeFlagsMock.Method, Result = false)]
        [TestCase(MemberTypeFlagsMock.NestedType, Result = false)]
        [TestCase(MemberTypeFlagsMock.Property, Result = false)]
        [TestCase(MemberTypeFlagsMock.AllExceptCustomAndTypeInfo, Result = false)]
        [TestCase(MemberTypeFlagsMock.All, Result = false)]
        public bool TestShouldRunFilter(MemberTypeFlagsMock memberTypeFlags)
        {
            var criteria = new MemberTypeCriteria();
            if (memberTypeFlags.HasFlag(MemberTypeFlagsMock.Constructor)) criteria.Constructor = true;
            if (memberTypeFlags.HasFlag(MemberTypeFlagsMock.Event)) criteria.Event = true;
            if (memberTypeFlags.HasFlag(MemberTypeFlagsMock.Field)) criteria.Constructor = true;
            if (memberTypeFlags.HasFlag(MemberTypeFlagsMock.Method)) criteria.Constructor = true;
            if (memberTypeFlags.HasFlag(MemberTypeFlagsMock.NestedType)) criteria.Constructor = true;
            if (memberTypeFlags.HasFlag(MemberTypeFlagsMock.Property)) criteria.Constructor = true;
            return criteria.ShouldRunFilter;
        }

        [TestCaseSource("TestFilterMatches_CaseDate")]
        public void TestFilterMatches(MemberInfo[] memberInfos)
        {
            var criteria = new MemberTypeCriteria();
            memberInfos.Should().NotBeNull();
            memberInfos.Should().NotBeEmpty();
            memberInfos.Count().Should().Be(1);
            memberInfos[0].Should().NotBeNull();
            var filteredMatches = new MemberTypeCriteria().FilterMatches(memberInfos);
            filteredMatches.Should().NotBeNull();
            filteredMatches.Should().NotBeEmpty();
            filteredMatches.Count().Should().Be(1);
            filteredMatches[0].Should().NotBeNull();
            filteredMatches[0].Should().BeSameAs(memberInfos[0]);
        }
        private static MemberInfo[][] TestFilterMatches_CaseDate =
        {
            new MemberInfo[] {typeof (Mock).GetField("PublicField")},
            new MemberInfo[] {typeof (Mock).GetField("PrivateField", BindingFlags.Instance | BindingFlags.NonPublic)},
            new MemberInfo[] {typeof (Mock).GetField("ProtectedField", BindingFlags.Instance | BindingFlags.NonPublic)},
            new MemberInfo[] {typeof (Mock).GetField("ProtectedInternalField", BindingFlags.Instance | BindingFlags.NonPublic)},
            new MemberInfo[] {typeof (Mock).GetField("InternalField", BindingFlags.Instance | BindingFlags.NonPublic)},
        };

        [Test]
        public void TestFilterMatches_EdgeCases()
        {
            new MemberTypeCriteria().FilterMatches(new MemberInfo[0]).Should().NotBeNull();
            new MemberTypeCriteria().FilterMatches(new MemberInfo[0]).Should().BeEmpty();
            new MemberTypeCriteria().FilterMatches(null).Should().BeNull();
        }

        public class Mock
        {
            public int PublicField;
            private int PrivateField;
            protected int ProtectedField;
            protected internal int ProtectedInternalField;
            internal int InternalField;
        }

        [Flags]
        public enum MemberTypeFlagsMock
        {
            None = 0,
            Constructor = 1,
            Event = 2,
            Field = 4,
            Method = 8,
            NestedType = 128,
            Property = 16,
            //TypeInfo = 32,
            //Custom = 64,
            All = 191,
            AllExceptCustomAndTypeInfo = 159
        }
    }
}
