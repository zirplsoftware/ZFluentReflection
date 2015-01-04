using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using Zirpl.FluentReflection.Queries.Criteria;
using Zirpl.FluentReflection.Tests.Mocks;

namespace Zirpl.FluentReflection.Tests.Queries.Implementation.Criteria
{
    [TestFixture]
    public class MemberNameCriteriaTests
    {
        [Test, Combinatorial]
        public void TestShouldRun(
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
            var result = criteria.ShouldRun;
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
        }

        [Test]
        public void TestGetMatches_EdgeCases()
        {
            new MemberNameCriteria().GetMatches(new MemberInfo[0]).Should().NotBeNull();
            new MemberNameCriteria().GetMatches(new MemberInfo[0]).Should().BeEmpty();
            new MemberNameCriteria().GetMatches(null).Should().BeNull();
        }
        
        [TestCaseSource("TestGetMatches_TestCases")]
        public void TestGetMatches(bool ignoreCase, NameHandlingTypeMock nameHandling, String[] names, String[] expectedResultNames)
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
            var result = criteria.GetMatches(fields);
            result.Should().NotBeNull();
            var resultNames = result.Select(o => o.Name);
            resultNames.Count().Should().Be(expectedResultNames.Count());
            if (expectedResultNames.Count() > 0)
            {
                resultNames.All(name => expectedResultNames.Contains(name)).Should().BeTrue();
                expectedResultNames.All(name => resultNames.Contains(name)).Should().BeTrue();
            }
        }

        private IEnumerable TestGetMatches_TestCases
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

                // wholes will ALWAYS return both since ShouldRunFilter returns false
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteriaNone, expectedBoth).SetName("Whole_0Names");

                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria1Whole, expectedBoth).SetName("Whole_1Names_Whole");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria1WholeWrongCase, expectedBoth).SetName("Whole_1Names_WholeWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria1StartsWith, expectedBoth).SetName("Whole_1Names_StartsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria1StartsWithWrongCase, expectedBoth).SetName("Whole_1Names_StartsWithWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria1Contains, expectedBoth).SetName("Whole_1Names_Contains");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria1ContainsWrongCase, expectedBoth).SetName("Whole_1Names_ContainsWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria1EndsWith, expectedBoth).SetName("Whole_1Names_EndsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria1EndsWithWrongCase, expectedBoth).SetName("Whole_1Names_EndsWithWrongCase");

                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria2Whole, expectedBoth).SetName("Whole_2Names_Whole");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria2WholeWrongCase, expectedBoth).SetName("Whole_2Names_WholeWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria2StartsWith, expectedBoth).SetName("Whole_2Names_StartsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria2StartsWithWrongCase, expectedBoth).SetName("Whole_2Names_StartsWithWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria2Contains, expectedBoth).SetName("Whole_2Names_Contains");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria2ContainsWrongCase, expectedBoth).SetName("Whole_2Names_ContainsWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria2EndsWith, expectedBoth).SetName("Whole_2Names_EndsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.Whole, criteria2EndsWithWrongCase, expectedBoth).SetName("Whole_2Names_EndsWithWrongCase");

                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria1Whole, expectedBoth).SetName("Whole_IgnoreCase_1Names_Whole");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria1WholeWrongCase, expectedBoth).SetName("Whole_IgnoreCase_1Names_WholeWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria1StartsWith, expectedBoth).SetName("Whole_IgnoreCase_1Names_StartsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria1StartsWithWrongCase, expectedBoth).SetName("Whole_IgnoreCase_1Names_StartsWithWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria1Contains, expectedBoth).SetName("Whole_IgnoreCase_1Names_Contains");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria1ContainsWrongCase, expectedBoth).SetName("Whole_IgnoreCase_1Names_ContainsWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria1EndsWith, expectedBoth).SetName("Whole_IgnoreCase_1Names_EndsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria1EndsWithWrongCase, expectedBoth).SetName("Whole_IgnoreCase_1Names_EndsWithWrongCase");

                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria2Whole, expectedBoth).SetName("Whole_IgnoreCase_2Names_Whole");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria2WholeWrongCase, expectedBoth).SetName("Whole_IgnoreCase_2Names_WholeWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria2StartsWith, expectedBoth).SetName("Whole_IgnoreCase_2Names_StartsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria2StartsWithWrongCase, expectedBoth).SetName("Whole_IgnoreCase_2Names_StartsWithWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria2Contains, expectedBoth).SetName("Whole_IgnoreCase_2Names_Contains");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria2ContainsWrongCase, expectedBoth).SetName("Whole_IgnoreCase_2Names_ContainsWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria2EndsWith, expectedBoth).SetName("Whole_IgnoreCase_2Names_EndsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.Whole, criteria2EndsWithWrongCase, expectedBoth).SetName("Whole_IgnoreCase_2Names_EndsWithWrongCase");

                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteriaNone, expectedBoth).SetName("StartsWith_0Names");

                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria1Whole, expectedBar).SetName("StartsWith_1Names_Whole");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria1WholeWrongCase, expectedNone).SetName("StartsWith_1Names_WholeWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria1StartsWith, expectedBar).SetName("StartsWith_1Names_StartsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria1StartsWithWrongCase, expectedNone).SetName("StartsWith_1Names_StartsWithWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria1Contains, expectedNone).SetName("StartsWith_1Names_Contains");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria1ContainsWrongCase, expectedNone).SetName("StartsWith_1Names_ContainsWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria1EndsWith, expectedNone).SetName("StartsWith_1Names_EndsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria1EndsWithWrongCase, expectedNone).SetName("StartsWith_1Names_EndsWithWrongCase");

                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria2Whole, expectedBoth).SetName("StartsWith_2Names_Whole");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria2WholeWrongCase, expectedNone).SetName("StartsWith_2Names_WholeWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria2StartsWith, expectedBoth).SetName("StartsWith_2Names_StartsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria2StartsWithWrongCase, expectedNone).SetName("StartsWith_2Names_StartsWithWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria2Contains, expectedNone).SetName("StartsWith_2Names_Contains");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria2ContainsWrongCase, expectedNone).SetName("StartsWith_2Names_ContainsWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria2EndsWith, expectedNone).SetName("StartsWith_2Names_EndsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.StartsWith, criteria2EndsWithWrongCase, expectedNone).SetName("StartsWith_2Names_EndsWithWrongCase");

                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria1Whole, expectedBar).SetName("StartsWith_IgnoreCase_1Names_Whole");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria1WholeWrongCase, expectedBar).SetName("StartsWith_IgnoreCase_1Names_WholeWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria1StartsWith, expectedBar).SetName("StartsWith_IgnoreCase_1Names_StartsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria1StartsWithWrongCase, expectedBar).SetName("StartsWith_IgnoreCase_1Names_StartsWithWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria1Contains, expectedNone).SetName("StartsWith_IgnoreCase_1Names_Contains");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria1ContainsWrongCase, expectedNone).SetName("StartsWith_IgnoreCase_1Names_ContainsWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria1EndsWith, expectedNone).SetName("StartsWith_IgnoreCase_1Names_EndsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria1EndsWithWrongCase, expectedNone).SetName("StartsWith_IgnoreCase_1Names_EndsWithWrongCase");

                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria2Whole, expectedBoth).SetName("StartsWith_IgnoreCase_2Names_Whole");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria2WholeWrongCase, expectedBoth).SetName("StartsWith_IgnoreCase_2Names_WholeWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria2StartsWith, expectedBoth).SetName("StartsWith_IgnoreCase_2Names_StartsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria2StartsWithWrongCase, expectedBoth).SetName("StartsWith_IgnoreCase_2Names_StartsWithWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria2Contains, expectedNone).SetName("StartsWith_IgnoreCase_2Names_Contains");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria2ContainsWrongCase, expectedNone).SetName("StartsWith_IgnoreCase_2Names_ContainsWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria2EndsWith, expectedNone).SetName("StartsWith_IgnoreCase_2Names_EndsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.StartsWith, criteria2EndsWithWrongCase, expectedNone).SetName("StartsWith_IgnoreCase_2Names_EndsWithWrongCase");

                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteriaNone, expectedBoth).SetName("Contains_0Names");

                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria1Whole, expectedBar).SetName("Contains_1Names_Whole");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria1WholeWrongCase, expectedNone).SetName("Contains_1Names_WholeWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria1StartsWith, expectedBar).SetName("Contains_1Names_StartsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria1StartsWithWrongCase, expectedNone).SetName("Contains_1Names_StartsWithWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria1Contains, expectedBar).SetName("Contains_1Names_Contains");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria1ContainsWrongCase, expectedNone).SetName("Contains_1Names_ContainsWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria1EndsWith, expectedBar).SetName("Contains_1Names_EndsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria1EndsWithWrongCase, expectedNone).SetName("Contains_1Names_EndsWithWrongCase");

                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria2Whole, expectedBoth).SetName("Contains_2Names_Whole");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria2WholeWrongCase, expectedNone).SetName("Contains_2Names_WholeWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria2StartsWith, expectedBoth).SetName("Contains_2Names_StartsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria2StartsWithWrongCase, expectedNone).SetName("Contains_2Names_StartsWithWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria2Contains, expectedBoth).SetName("Contains_2Names_Contains");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria2ContainsWrongCase, expectedNone).SetName("Contains_2Names_ContainsWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria2EndsWith, expectedBoth).SetName("Contains_2Names_EndsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.Contains, criteria2EndsWithWrongCase, expectedNone).SetName("Contains_2Names_EndsWithWrongCase");

                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria1Whole, expectedBar).SetName("Contains_IgnoreCase_1Names_Whole");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria1WholeWrongCase, expectedBar).SetName("Contains_IgnoreCase_1Names_WholeWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria1StartsWith, expectedBar).SetName("Contains_IgnoreCase_1Names_StartsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria1StartsWithWrongCase, expectedBar).SetName("Contains_IgnoreCase_1Names_StartsWithWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria1Contains, expectedBar).SetName("Contains_IgnoreCase_1Names_Contains");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria1ContainsWrongCase, expectedBar).SetName("Contains_IgnoreCase_1Names_ContainsWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria1EndsWith, expectedBar).SetName("Contains_IgnoreCase_1Names_EndsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria1EndsWithWrongCase, expectedBar).SetName("Contains_IgnoreCase_1Names_EndsWithWrongCase");

                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria2Whole, expectedBoth).SetName("Contains_IgnoreCase_2Names_Whole");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria2WholeWrongCase, expectedBoth).SetName("Contains_IgnoreCase_2Names_WholeWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria2StartsWith, expectedBoth).SetName("Contains_IgnoreCase_2Names_StartsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria2StartsWithWrongCase, expectedBoth).SetName("Contains_IgnoreCase_2Names_StartsWithWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria2Contains, expectedBoth).SetName("Contains_IgnoreCase_2Names_Contains");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria2ContainsWrongCase, expectedBoth).SetName("Contains_IgnoreCase_2Names_ContainsWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria2EndsWith, expectedBoth).SetName("Contains_IgnoreCase_2Names_EndsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.Contains, criteria2EndsWithWrongCase, expectedBoth).SetName("Contains_IgnoreCase_2Names_EndsWithWrongCase");

                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteriaNone, expectedBoth).SetName("EndsWith_0Names");

                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1Whole, expectedBar).SetName("EndsWith_1Names_Whole");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1WholeWrongCase, expectedNone).SetName("EndsWith_1Names_WholeWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1StartsWith, expectedNone).SetName("EndsWith_1Names_StartsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1StartsWithWrongCase, expectedNone).SetName("EndsWith_1Names_StartsWithWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1Contains, expectedNone).SetName("EndsWith_1Names_Contains");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1ContainsWrongCase, expectedNone).SetName("EndsWith_1Names_ContainsWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1EndsWith, expectedBar).SetName("EndsWith_1Names_EndsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria1EndsWithWrongCase, expectedNone).SetName("EndsWith_1Names_EndsWithWrongCase");

                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria2Whole, expectedBoth).SetName("EndsWith_2Names_Whole");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria2WholeWrongCase, expectedNone).SetName("EndsWith_2Names_WholeWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria2StartsWith, expectedNone).SetName("EndsWith_2Names_StartsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria2StartsWithWrongCase, expectedNone).SetName("EndsWith_2Names_StartsWithWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria2Contains, expectedFoo).SetName("EndsWith_2Names_Contains");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria2ContainsWrongCase, expectedNone).SetName("EndsWith_2Names_ContainsWrongCase");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria2EndsWith, expectedBoth).SetName("EndsWith_2Names_EndsWith");
                yield return new TestCaseData(false, NameHandlingTypeMock.EndsWith, criteria2EndsWithWrongCase, expectedNone).SetName("EndsWith_2Names_EndsWithWrongCase");

                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria1Whole, expectedBar).SetName("EndsWith_IgnoreCase_1Names_Whole");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria1WholeWrongCase, expectedBar).SetName("EndsWith_IgnoreCase_1Names_WholeWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria1StartsWith, expectedNone).SetName("EndsWith_IgnoreCase_1Names_StartsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria1StartsWithWrongCase, expectedNone).SetName("EndsWith_IgnoreCase_1Names_StartsWithWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria1Contains, expectedNone).SetName("EndsWith_IgnoreCase_1Names_Contains");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria1ContainsWrongCase, expectedNone).SetName("EndsWith_IgnoreCase_1Names_ContainsWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria1EndsWith, expectedBar).SetName("EndsWith_IgnoreCase_1Names_EndsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria1EndsWithWrongCase, expectedBar).SetName("EndsWith_IgnoreCase_1Names_EndsWithWrongCase");

                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2Whole, expectedBoth).SetName("EndsWith_IgnoreCase_2Names_Whole");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2WholeWrongCase, expectedBoth).SetName("EndsWith_IgnoreCase_2Names_WholeWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2StartsWith, expectedNone).SetName("EndsWith_IgnoreCase_2Names_StartsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2StartsWithWrongCase, expectedNone).SetName("EndsWith_IgnoreCase_2Names_StartsWithWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2Contains, expectedFoo).SetName("EndsWith_IgnoreCase_2Names_Contains");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2ContainsWrongCase, expectedFoo).SetName("EndsWith_IgnoreCase_2Names_ContainsWrongCase");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2EndsWith, expectedBoth).SetName("EndsWith_IgnoreCase_2Names_EndsWith");
                yield return new TestCaseData(true, NameHandlingTypeMock.EndsWith, criteria2EndsWithWrongCase, expectedBoth).SetName("EndsWith_IgnoreCase_2Names_EndsWithWrongCase");
            }
        }

        public class Mock
        {
            public int Foo;
            public int Bar;
        }
    }
}
