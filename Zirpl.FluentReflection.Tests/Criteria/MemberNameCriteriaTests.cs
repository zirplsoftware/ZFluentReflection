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
        
        [TestCaseSource("TestFilterMatches_TestCases")]
        public void TestFilterMatches(bool ignoreCase, NameHandlingTypeMock nameHandling, String[] names, String[] expectedResultNames)
        {
            var fields = new MemberInfo[]
            {
                typeof (Mock).GetField("Foo"),
                typeof (Mock).GetField("Bar")
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
            var resultNames = result.Select(o => o.Name);
            resultNames.Count().Should().Be(expectedResultNames.Count());
            if (expectedResultNames.Count() > 0)
            {
                resultNames.All(name => expectedResultNames.Contains(name)).Should().BeTrue();
                expectedResultNames.All(name => resultNames.Contains(name)).Should().BeTrue();
            }
        }

        private IEnumerable TestFilterMatches_TestCases
        {
            get
            {
                String[] criteriaNone = null;

                var criteria1Whole = new[] { "Bar" };
                var criteria1WholeWrongCase = new[] { "bar" };
                var criteria1StartsWith = new[] { "Ba" };
                var criteria1StartsWithWrongCase = new[] { "ba" };
                var criteria1Contains = new[] { "a" };
                var criteria1ContainsWrongCase = new[] { "A" };
                var criteria1EndsWith = new[] { "ar" };
                var criteria1EndsWithWrongCase = new[] { "AR" };

                var criteria2Whole = new[] { "Bar", "Foo" };
                var criteria2WholeWrongCase = new[] { "bar", "foo" };
                var criteria2StartsWith = new[] { "Ba", "Fo" };
                var criteria2StartsWithWrongCase = new[] { "ba", "fo" };
                var criteria2Contains = new[] { "a", "o" };
                var criteria2ContainsWrongCase = new[] { "A", "O" };
                var criteria2EndsWith = new[] { "ar", "oo" };
                var criteria2EndsWithWrongCase = new[] { "AR", "OO" };

                var expectedNone = new String[0];
                var expectedFoo = new[] { "Foo" };
                var expectedBar = new[] { "Bar" };
                var expectedBoth = new[] { "Foo", "Bar" };

                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteriaNone, expectedBoth).SetName("EndsWith_0Names");

                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1Whole, expectedBar).SetName("EndsWith_1Names_Whole");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1WholeWrongCase, expectedNone).SetName("EndsWith_1Names_WholeWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1StartsWith, expectedNone).SetName("EndsWith_1Names_StartsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1StartsWithWrongCase, expectedNone).SetName("EndsWith_1Names_StartsWithWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1Contains, expectedNone).SetName("EndsWith_1Names_Contains");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1ContainsWrongCase, expectedNone).SetName("EndsWith_1Names_ContainsWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1EndsWith, expectedBar).SetName("EndsWith_1Names_EndsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1EndsWithWrongCase, expectedNone).SetName("EndsWith_1Names_EndsWithWrongCase");

                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2Whole, expectedBoth).SetName("EndsWith_2Names_Whole");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2WholeWrongCase, expectedBoth).SetName("EndsWith_2Names_WholeWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2StartsWith, expectedNone).SetName("EndsWith_2Names_StartsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2StartsWithWrongCase, expectedNone).SetName("EndsWith_2Names_StartsWithWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2Contains, expectedFoo).SetName("EndsWith_2Names_Contains");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2ContainsWrongCase, expectedFoo).SetName("EndsWith_2Names_ContainsWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2EndsWith, expectedBoth).SetName("EndsWith_2Names_EndsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2EndsWithWrongCase, expectedBoth).SetName("EndsWith_2Names_EndsWithWrongCase");
            }
        }  

        public class Mock
        {
            public int Foo;
            public int Bar;
        }
    }
}
