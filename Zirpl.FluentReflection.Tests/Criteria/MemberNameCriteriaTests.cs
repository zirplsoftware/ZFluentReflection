using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public enum NameHandlingTypeMock
        {
            Whole = 0,
            StartsWith = 1,
            Contains = 2,
            EndsWith = 3
        }

        [Test, Combinatorial]
        public void TestShouldRunFilter(
            [Values(0, 1, 2)]int numberOfNames,
            [Values(true, false)]bool ignoreCase,
            [Values(NameHandlingTypeMock.Whole, NameHandlingTypeMock.StartsWith, NameHandlingTypeMock.Contains, NameHandlingTypeMock.EndsWith)]NameHandlingTypeMock nameHandling)
        {
            var criteria = new MemberNameCriteria()
            {
                IgnoreCase = ignoreCase,
                NameHandling = (NameHandlingType)nameHandling
            };
            if (numberOfNames > 0)
            {
                var listOfNames = new List<String>();
                for (int i = 0; i < numberOfNames; i++)
                {
                    listOfNames.Add(Guid.NewGuid().ToString());
                }
                criteria.Names = listOfNames;
            }
            var result = criteria.ShouldRunFilter;
            if (numberOfNames > 0
                && nameHandling != NameHandlingTypeMock.Whole)
            {
                result.Should().BeTrue();
            }
            else
            {
                result.Should().BeFalse();
            }
        }

        [Test, Combinatorial]
        public void TestGetNamesForDirectLookup(
            [Values(0, 1, 2)]int numberOfNames,
            [Values(true, false)]bool ignoreCase,
            [Values(NameHandlingTypeMock.Whole, NameHandlingTypeMock.StartsWith, NameHandlingTypeMock.Contains, NameHandlingTypeMock.EndsWith)]NameHandlingTypeMock nameHandling)
        {
            var criteria = new MemberNameCriteria()
            {
                IgnoreCase = ignoreCase,
                NameHandling = (NameHandlingType)nameHandling
            };
            if (numberOfNames > 0)
            {
                var listOfNames = new List<String>();
                for (int i = 0; i < numberOfNames; i++)
                {
                    listOfNames.Add(Guid.NewGuid().ToString());
                }
                criteria.Names = listOfNames;
            }
            var result = criteria.GetNamesForDirectLookup();
            if (numberOfNames == 0
                || nameHandling != NameHandlingTypeMock.Whole)
            {
                result.Should().BeNull();
            }
            else
            {
                result.Should().NotBeNull();
                result.Should().NotBeEmpty();
                result.All(o => criteria.Names.Contains(o)).Should().BeTrue();
            }
        }

        [Test]
        public void TestNames_EdgeCases()
        {
            new Action(() => new MemberNameCriteria().Names = null).ShouldThrow<ArgumentNullException>();
            new Action(() => new MemberNameCriteria().Names = new String[0]).ShouldThrow<ArgumentException>();
            new Action(() => new MemberNameCriteria().Names = new String[] { null }).ShouldThrow<ArgumentException>();
            new Action(() => new MemberNameCriteria().Names = new[] { String.Empty }).ShouldThrow<ArgumentException>();
            new Action(() =>
            {
                var criteria = new MemberNameCriteria();
                criteria.Names = new[] { "tests" };
                criteria.Names = criteria.Names;
            }).ShouldThrow<InvalidOperationException>();
        }
        
        [Test]
        public void TestFilterMatches_EdgeCases()
        {
            new MemberNameCriteria().FilterMatches(new MemberInfo[0]).Should().NotBeNull();
            new MemberNameCriteria().FilterMatches(new MemberInfo[0]).Should().BeEmpty();
            new MemberNameCriteria().FilterMatches(null).Should().BeNull();
        }

        // all of these cases will SKIP the FilterMatch logic because ShouldRunFilter returns false
        [TestCase(false, NameHandlingTypeMock.Whole, null, Result = 2, TestName = "NoFlags_NoNames")]
        [TestCase(false, NameHandlingTypeMock.Whole, new[] { "AnotherField" }, Result = 2, TestName = "NoFlags_1Names_0Matches")]
        [TestCase(false, NameHandlingTypeMock.Whole, new[] { "PublicField" }, Result = 2, TestName = "NoFlags_1Names_1Matches")]
        [TestCase(false, NameHandlingTypeMock.Whole, new[] { "AnotherField1", "AnotherField2" }, Result = 2, TestName = "NoFlags_2Names_0Matches")]
        [TestCase(false, NameHandlingTypeMock.Whole, new[] { "PublicField", "AnotherField1" }, Result = 2, TestName = "NoFlags_2Names_1Matches")]
        [TestCase(false, NameHandlingTypeMock.Whole, new[] { "PublicField", "PrivateField" }, Result = 2, TestName = "NoFlags_2Names_2Matches")]
        // all of these cases will SKIP the FilterMatch logic because ShouldRunFilter returns false
        [TestCase(true, NameHandlingTypeMock.Whole, null, Result = 2, TestName = "IgnoreCase_NoNames")]
        [TestCase(true, NameHandlingTypeMock.Whole, new[] { "AnotherField" }, Result = 2, TestName = "IgnoreCase_1Names_0Matches")]
        [TestCase(true, NameHandlingTypeMock.Whole, new[] { "PublicField" }, Result = 2, TestName = "IgnoreCase_1Names_1Matches")]
        [TestCase(true, NameHandlingTypeMock.Whole, new[] { "AnotherField1", "AnotherField2" }, Result = 2, TestName = "IgnoreCase_2Names_0Matches")]
        [TestCase(true, NameHandlingTypeMock.Whole, new[] { "PublicField", "AnotherField1" }, Result = 2, TestName = "IgnoreCase_2Names_1Matches")]
        [TestCase(true, NameHandlingTypeMock.Whole, new[] { "PublicField", "PrivateField" }, Result = 2, TestName = "IgnoreCase_2Names_2Matches")]
        public int TestFilterMatches(bool ignoreCase, NameHandlingTypeMock nameHandling, String[] names)
        {
            var fields = new MemberInfo[]
            {
                typeof (Mock).GetField("PublicField"),
                typeof (Mock).GetField("PrivateField", BindingFlags.Instance | BindingFlags.NonPublic)
            };
            var criteria = new MemberNameCriteria()
            {
                IgnoreCase = ignoreCase,
                NameHandling = (NameHandlingType)nameHandling
            };
            if (names != null
                && names.Any())
            {
                criteria.Names = names;
            }
            var result = criteria.FilterMatches(fields);
            result.Should().NotBeNull();
            // TODO: need to test that the RIGHT fields are returned- input of the expected fields
            return result.Count();
        }

        //[TestCaseSource("TestFilterMatches_Name_CaseData")]
        //public bool TestFilterMatches_Name(MemberInfo[] memberInfos, String name, bool ignoreCase)
        //{
        //    var criteria = new MemberNameCriteria();
        //    criteria.Names = name;
        //    criteria.IgnoreCase = ignoreCase;

        //    memberInfos.Should().NotBeNull();
        //    memberInfos.Should().NotBeEmpty();
        //    memberInfos.Count().Should().Be(1);
        //    memberInfos[0].Should().NotBeNull();
        //    var filteredMatches = new MemberTypeCriteria().FilterMatches(memberInfos);
        //    filteredMatches.Should().NotBeNull();

        //    filteredMatches.Count().Should().BeLessOrEqualTo(1);
        //    if (filteredMatches.Count() == 0) return false;

        //    // okay, we have it, just for sanity check
        //    filteredMatches[0].Should().NotBeNull();
        //    filteredMatches[0].Should().BeSameAs(memberInfos[0]);
        //    return true;
        //}

        //private static IEnumerable TestFilterMatches_Name_CaseData
        //{
        //    get
        //    {
        //        // default case
        //        yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "PublicField", false).Returns(true);
        //        // wrong name
        //        yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "ublicField", false).Returns(false);
        //        // wrong casing
        //        yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "publicField", false).Returns(false);
        //        // ignore casing- default case
        //        yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "PublicField", true).Returns(true);
        //        // ignore casing- wrong name
        //        yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "ublicField", true).Returns(false);
        //        // ignore casing
        //        yield return new TestCaseData(new MemberInfo[] { typeof(Mock).GetField("PublicField") }, "publicField", true).Returns(true);
        //    }
        //}

        public class Mock
        {
            public int PublicField;
            private int PrivateField;
        }
    }
}
