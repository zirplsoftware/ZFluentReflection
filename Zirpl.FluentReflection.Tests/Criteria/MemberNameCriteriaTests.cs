using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Zirpl.FluentReflection.Tests
{
    [TestFixture]
    public class MemberNameCriteriaTests
    {
        [TestCase("Test", false, false, false, false, Result = false)]
        [TestCase("Test", true, false, false, false, Result = false)]
        [TestCase("Test", false, true, false, false, Result = true)]
        [TestCase("Test", false, false, true, false, Result = true)]
        [TestCase("Test", false, false, false, true, Result = true)]
        [TestCase("Test", false, true, true, false, Result = true)]
        [TestCase("Test", false, false, true, true, Result = true)]
        [TestCase("Test", false, true, false, true, Result = true)]
        [TestCase("Test", false, true, true, true, Result = true)]
        public bool TestShouldRunFilter_Name(String name, bool ignoreCase, bool startsWith, bool contains, bool endsWith)
        {
            var criteria = new MemberNameCriteria();
            criteria.Name = name;
            criteria.IgnoreCase = ignoreCase;
            criteria.StartsWith = startsWith;
            criteria.Contains = contains;
            criteria.EndsWith = endsWith;
            return criteria.ShouldRunFilter;
        }

        [TestCase(new [] {"Test"}, false, false, false, false, Result = false)]
        [TestCase(new [] {"Test"}, true, false, false, false, Result = false)]
        [TestCase(new [] {"Test"}, false, true, false, false, Result = true)]
        [TestCase(new [] {"Test"}, false, false, true, false, Result = true)]
        [TestCase(new [] {"Test"}, false, false, false, true, Result = true)]
        [TestCase(new [] {"Test"}, false, true, true, false, Result = true)]
        [TestCase(new [] {"Test"}, false, false, true, true, Result = true)]
        [TestCase(new [] {"Test"}, false, true, false, true, Result = true)]
        [TestCase(new [] { "Test" }, false, true, true, true, Result = true)]
        public bool TestShouldRunFilter_Names(IEnumerable<String> names, bool ignoreCase, bool startsWith, bool contains, bool endsWith)
        {
            var criteria = new MemberNameCriteria();
            criteria.Names = names;
            criteria.IgnoreCase = ignoreCase;
            criteria.StartsWith = startsWith;
            criteria.Contains = contains;
            criteria.EndsWith = endsWith;
            return criteria.ShouldRunFilter;
        }

        [TestCase(null, ExpectedException = typeof(ArgumentNullException))]
        [TestCase("", ExpectedException = typeof(ArgumentNullException))]
        public void TestName(String name)
        {
            new MemberNameCriteria().Name = name;
        }

        [TestCase(null, false, ExpectedException = typeof(ArgumentNullException))]
        [TestCase(new[] { "" }, false, ExpectedException = typeof(ArgumentException))]
        [TestCase(new String[] { null }, false, ExpectedException = typeof(ArgumentException))]
        public void TestNames(IEnumerable<String> names, bool junkArg)
        {
            new MemberNameCriteria().Names = names;
        }

        [TestCaseSource("TestFilterMatches_Name_CaseData")]
        public bool TestFilterMatches_Name(MemberInfo[] memberInfos, String name, bool ignoreCase)
        {
            var criteria = new MemberNameCriteria();
            criteria.Name = name;
            criteria.IgnoreCase = ignoreCase;

            memberInfos.Should().NotBeNull();
            memberInfos.Should().NotBeEmpty();
            memberInfos.Count().Should().Be(1);
            memberInfos[0].Should().NotBeNull();
            var filteredMatches = new MemberTypeCriteria().FilterMatches(memberInfos);
            filteredMatches.Should().NotBeNull();

            filteredMatches.Count().Should().BeLessOrEqualTo(1);
            if (filteredMatches.Count() == 0) return false;

            // okay, we have it, just for sanity check
            filteredMatches[0].Should().NotBeNull();
            filteredMatches[0].Should().BeSameAs(memberInfos[0]);
            return true;
        }

        private static IEnumerable TestFilterMatches_Name_CaseData
        {
            get
            {
                // default case
                yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "PublicField", false).Returns(true);
                // wrong name
                yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "ublicField", false).Returns(false);
                // wrong casing
                yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "publicField", false).Returns(false);
                // ignore casing- default case
                yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "PublicField", true).Returns(true);
                // ignore casing- wrong name
                yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "ublicField", true).Returns(false);
                // ignore casing
                yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "publicField", true).Returns(true);
            }
        }

        public class Mock
        {
            public int PublicField;
            private int PrivateField;
        }
    }
}
