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
using Zirpl.FluentReflection.Tests.Mocks;

namespace Zirpl.FluentReflection.Tests
{
    [TestFixture]
    public class TypeNameCriteriaTests
    {
        [Test, Combinatorial]
        public void TestShouldRunFilter(
            [Values(0, 1, 2)]int numberOfNames,
            [Values(true, false)]bool ignoreCase,
            [Values(true, false)]bool useFullName,
            [Values(NameHandlingTypeMock.Whole, NameHandlingTypeMock.StartsWith, NameHandlingTypeMock.Contains, NameHandlingTypeMock.EndsWith)]NameHandlingTypeMock nameHandling)
        {
            var criteria = new TypeNameCriteria()
            {
                IgnoreCase = ignoreCase,
                NameHandling = (NameHandlingType)nameHandling,
                UseFullName = useFullName
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
            if (numberOfNames > 0)
            {
                result.Should().BeTrue();
            }
            else
            {
                result.Should().BeFalse();
            }
        }

        [Test]
        public void TestNames_EdgeCases()
        {
            new Action(() => new TypeNameCriteria().Names = null).ShouldThrow<ArgumentNullException>();
            new Action(() => new TypeNameCriteria().Names = new String[0]).ShouldThrow<ArgumentException>();
            new Action(() => new TypeNameCriteria().Names = new String[] { null }).ShouldThrow<ArgumentException>();
            new Action(() => new TypeNameCriteria().Names = new[] { String.Empty }).ShouldThrow<ArgumentException>();
        }

        [Test]
        public void TestFilterMatches_EdgeCases()
        {
            new TypeNameCriteria().FilterMatches(new Type[0]).Should().NotBeNull();
            new TypeNameCriteria().FilterMatches(new Type[0]).Should().BeEmpty();
            new TypeNameCriteria().FilterMatches(null).Should().BeNull();
        }

        [TestCaseSource("TestFilterMatches_NotFullName_TestCases")]
        public void TestFilterMatches(bool ignoreCase, bool useFullName, NameHandlingTypeMock nameHandling, String[] names, String[] expectedResultNames)
        {
            var fields = new MemberInfo[]
            {
                typeof (Foo),
                typeof (Bar)
            };
            var criteria = new TypeNameCriteria()
            {
                IgnoreCase = ignoreCase,
                NameHandling = (NameHandlingType)nameHandling,
                UseFullName = useFullName
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

        private IEnumerable TestFilterMatches_NotFullName_TestCases
        {
            get
            {
                String[] criteriaNone = null;

                var criteriaShort1Whole = new[] { "Bar" };
                var criteriaShort1WholeWrongCase = new[] { "bar" };
                var criteriaShort1StartsWith = new[] { "Ba" };
                var criteriaShort1StartsWithWrongCase = new[] { "ba" };
                var criteriaShort1Contains = new[] { "a" };
                var criteriaShort1ContainsWrongCase = new[] { "A" };
                var criteriaShort1EndsWith = new[] { "ar" };
                var criteriaShort1EndsWithWrongCase = new[] { "AR" };

                var criteriaFull1Whole = new[] { "Zirpl.FluentReflection.Tests.Bar" };
                var criteriaFull1WholeWrongCase = new[] { "Zirpl.FluentReflection.tEsts.bar" };
                var criteriaFull1StartsWith = new[] { "Zirpl.FluentReflection.Tests.Ba" };
                var criteriaFull1StartsWithWrongCase = new[] { "Zirpl.FluentReflection.tEsts.ba" };
                var criteriaFull1Contains = new[] { "Tests.Ba" };
                var criteriaFull1ContainsWrongCase = new[] { "tEsts.bA" };
                var criteriaFull1EndsWith = new[] { "Tests.Bar" };
                var criteriaFull1EndsWithWrongCase = new[] { "tEsts.bAR" };

                var criteriaShort2Whole = new[] { "Bar", "Foo" };
                var criteriaShort2WholeWrongCase = new[] { "bar", "foo" };
                var criteriaShort2StartsWith = new[] { "Ba", "Fo" };
                var criteriaShort2StartsWithWrongCase = new[] { "ba", "fo" };
                var criteriaShort2Contains = new[] { "a", "o" };
                var criteriaShort2ContainsWrongCase = new[] { "A", "O" };
                var criteriaShort2EndsWith = new[] { "ar", "oo" };
                var criteriaShort2EndsWithWrongCase = new[] { "AR", "OO" };

                var criteriaFull2Whole = new[] { "Zirpl.FluentReflection.Tests.Bar", "Zirpl.FluentReflection.Tests.Foo" };
                var criteriaFull2WholeWrongCase = new[] { "Zirpl.FluentReflection.tEsts.bar", "Zirpl.FluentReflection.tEsts.foo" };
                var criteriaFull2StartsWith = new[] { "Zirpl.FluentReflection.Tests.Ba", "Zirpl.FluentReflection.Tests.Fo" };
                var criteriaFull2StartsWithWrongCase = new[] { "Zirpl.FluentReflection.tEsts.ba", "Zirpl.FluentReflection.tEsts.fo" };
                var criteriaFull2Contains = new[] { "Tests.Ba", "Tests.Fo" };
                var criteriaFull2ContainsWrongCase = new[] { "tEsts.ba", "tEsts.fo" };
                var criteriaFull2EndsWith = new[] { "Tests.Bar", "Tests.Foo" };
                var criteriaFull2EndsWithWrongCase = new[] { "tEsts.bAr", "tEsts.fOo" };

                var expectedNone = new String[0];
                var expectedFoo = new[] { "Foo" };
                var expectedBar = new[] { "Bar" };
                var expectedBoth = new[] { "Foo", "Bar" };

                #region Handling: Whole
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaNone, expectedBoth).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:0");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort1Whole, expectedBar).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-short-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort1StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-short-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort1Contains, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-short-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort1EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-short-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort2Whole, expectedBoth).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-short-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort2StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-short-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort2Contains, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-short-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort2EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-short-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull1Whole, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-full-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull1StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-full-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull1Contains, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-full-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull1EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-full-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull2Whole, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-full-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull2StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-full-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull2Contains, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-full-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull2EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-full-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Whole, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:false, Names:2-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaNone, expectedBoth).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:0");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort1Whole, expectedBar).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-short-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort1WholeWrongCase, expectedBar).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort1StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-short-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort1Contains, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-short-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort1EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-short-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort2Whole, expectedBoth).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-short-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort2WholeWrongCase, expectedBoth).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort2StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-short-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort2Contains, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-short-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort2EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-short-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull1Whole, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-full-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull1StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-full-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull1Contains, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-full-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull1EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-full-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull2Whole, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-full-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull2StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-full-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull2Contains, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-full-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull2EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-full-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Whole, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:false, IgnoreCase:true, Names:2-matches-full-endswith-ignorecase");

                
                // start checking full names
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaNone, expectedBoth).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:0");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort1Whole, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-short-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort1StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-short-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort1Contains, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-short-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort1EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-short-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort2Whole, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-short-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort2StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-short-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort2Contains, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-short-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort2EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-short-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull1Whole, expectedBar).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-full-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull1StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-full-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull1Contains, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-full-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull1EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-full-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull2Whole, expectedBoth).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-full-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull2StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-full-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull2Contains, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-full-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull2EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-full-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Whole, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:false, Names:2-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaNone, expectedBoth).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:0");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort1Whole, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-short-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort1StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-short-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort1Contains, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-short-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort1EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-short-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort2Whole, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-short-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort2StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-short-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort2Contains, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-short-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort2EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-short-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull1Whole, expectedBar).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-full-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull1WholeWrongCase, expectedBar).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull1StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-full-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull1Contains, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-full-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull1EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-full-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull2Whole, expectedBoth).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-full-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull2WholeWrongCase, expectedBoth).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull2StartsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-full-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull2Contains, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-full-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull2EndsWith, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-full-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Whole, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:Whole, UseFullName:true, IgnoreCase:true, Names:2-matches-full-endswith-ignorecase");
                #endregion

                #region Handling: StartsWith
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaNone, expectedBoth).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:0");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort1Whole, expectedBar).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort1StartsWith, expectedBar).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort1Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort1EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort2Whole, expectedBoth).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort2StartsWith, expectedBoth).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort2Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort2EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull1Whole, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull1StartsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull1Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull1EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull2Whole, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull2StartsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull2Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull2EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.StartsWith, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaNone, expectedBoth).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:0");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort1Whole, expectedBar).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort1WholeWrongCase, expectedBar).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort1StartsWith, expectedBar).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort1StartsWithWrongCase, expectedBar).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort1Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort1EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort2Whole, expectedBoth).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort2WholeWrongCase, expectedBoth).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort2StartsWith, expectedBoth).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort2StartsWithWrongCase, expectedBoth).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort2Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort2EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull1Whole, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull1StartsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull1Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull1EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull2Whole, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull2StartsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull2Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull2EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.StartsWith, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-endswith-ignorecase");


                // start checking full names
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaNone, expectedBoth).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:0");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort1Whole, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort1StartsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort1Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort1EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort2Whole, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort2StartsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort2Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort2EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull1Whole, expectedBar).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull1StartsWith, expectedBar).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull1Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull1EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull2Whole, expectedBoth).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull2StartsWith, expectedBoth).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull2Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull2EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.StartsWith, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaNone, expectedBoth).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:0");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort1Whole, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort1StartsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort1Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort1EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort2Whole, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort2StartsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort2Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort2EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull1Whole, expectedBar).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull1WholeWrongCase, expectedBar).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull1StartsWith, expectedBar).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull1StartsWithWrongCase, expectedBar).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull1Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull1EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull2Whole, expectedBoth).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull2WholeWrongCase, expectedBoth).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull2StartsWith, expectedBoth).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull2StartsWithWrongCase, expectedBoth).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull2Contains, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull2EndsWith, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.StartsWith, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:StartsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-endswith-ignorecase");
                #endregion

                #region Handling: Contains
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaNone, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:0");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort1Whole, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-short-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort1StartsWith, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-short-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort1Contains, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-short-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort1EndsWith, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-short-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort2Whole, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-short-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort2StartsWith, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-short-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort2Contains, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-short-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort2EndsWith, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-short-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull1Whole, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-full-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull1StartsWith, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-full-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull1Contains, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-full-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull1EndsWith, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-full-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull2Whole, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-full-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull2StartsWith, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-full-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull2Contains, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-full-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull2EndsWith, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-full-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.Contains, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:false, Names:2-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaNone, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:0");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort1Whole, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-short-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort1WholeWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort1StartsWith, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-short-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort1StartsWithWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort1Contains, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-short-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort1ContainsWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort1EndsWith, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-short-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort1EndsWithWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort2Whole, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-short-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort2WholeWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort2StartsWith, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-short-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort2StartsWithWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort2Contains, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-short-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort2ContainsWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort2EndsWith, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-short-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaShort2EndsWithWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull1Whole, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-full-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull1StartsWith, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-full-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull1Contains, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-full-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull1EndsWith, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-full-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull2Whole, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-full-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull2StartsWith, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-full-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull2Contains, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-full-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull2EndsWith, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-full-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.Contains, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:false, IgnoreCase:true, Names:2-matches-full-endswith-ignorecase");


                // start checking full names
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaNone, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:0");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort1Whole, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-short-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort1StartsWith, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-short-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort1Contains, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-short-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort1EndsWith, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-short-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort2Whole, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-short-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort2StartsWith, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-short-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort2Contains, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-short-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort2EndsWith, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-short-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull1Whole, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-full-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull1StartsWith, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-full-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull1Contains, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-full-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull1EndsWith, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-full-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull2Whole, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-full-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull2StartsWith, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-full-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull2Contains, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-full-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull2EndsWith, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-full-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.Contains, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:Contains, UseFullName:true, IgnoreCase:false, Names:2-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaNone, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:0");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort1Whole, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-short-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort1WholeWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort1StartsWith, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-short-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort1StartsWithWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort1Contains, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-short-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort1ContainsWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort1EndsWith, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-short-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort1EndsWithWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort2Whole, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-short-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort2WholeWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort2StartsWith, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-short-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort2StartsWithWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort2Contains, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-short-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort2ContainsWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort2EndsWith, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-short-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaShort2EndsWithWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull1Whole, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-full-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull1WholeWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull1StartsWith, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-full-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull1StartsWithWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull1Contains, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-full-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull1ContainsWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull1EndsWith, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-full-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull1EndsWithWrongCase, expectedBar).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull2Whole, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-full-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull2WholeWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull2StartsWith, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-full-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull2StartsWithWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull2Contains, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-full-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull2ContainsWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull2EndsWith, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-full-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.Contains, criteriaFull2EndsWithWrongCase, expectedBoth).SetName("Handling:Contains, UseFullName:true, IgnoreCase:true, Names:2-matches-full-endswith-ignorecase");
                #endregion

                #region Handling: EndsWith
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaNone, expectedBoth).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:0");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort1Whole, expectedBar).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort1StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort1Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort1EndsWith, expectedBar).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort2Whole, expectedBoth).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort2StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort2Contains, expectedFoo).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort2EndsWith, expectedBoth).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull1Whole, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull1StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull1Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull1EndsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull2Whole, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-whole");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull2StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-startswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull2Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-contains");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull2EndsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-endswith");
                yield return new TestCaseData(false, false, NameHandlingTypeMock.EndsWith, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:false, Names:2-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaNone, expectedBoth).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:0");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort1Whole, expectedBar).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort1WholeWrongCase, expectedBar).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort1StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort1Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort1EndsWith, expectedBar).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort1EndsWithWrongCase, expectedBar).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort2Whole, expectedBoth).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort2WholeWrongCase, expectedBoth).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort2StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort2Contains, expectedFoo).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort2ContainsWrongCase, expectedFoo).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort2EndsWith, expectedBoth).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaShort2EndsWithWrongCase, expectedBoth).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull1Whole, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull1StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull1Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull1EndsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull2Whole, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-whole");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull2StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-startswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull2Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-contains");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull2EndsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-endswith");
                yield return new TestCaseData(true, false, NameHandlingTypeMock.EndsWith, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:false, IgnoreCase:true, Names:2-matches-full-endswith-ignorecase");


                // start checking full names
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaNone, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:0");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort1Whole, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort1WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort1StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort1Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort1EndsWith, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort1EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort2Whole, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort2WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort2StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort2Contains, expectedFoo).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort2ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort2EndsWith, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaShort2EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull1Whole, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull1WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull1StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull1Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull1EndsWith, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull1EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull2Whole, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-whole");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull2WholeWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull2StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-startswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull2Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-contains");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull2EndsWith, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-endswith");
                yield return new TestCaseData(false, true, NameHandlingTypeMock.EndsWith, criteriaFull2EndsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:false, Names:2-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaNone, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:0");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort1Whole, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort1WholeWrongCase, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort1StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort1StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort1Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort1ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort1EndsWith, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort1EndsWithWrongCase, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort2Whole, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort2WholeWrongCase, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort2StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort2StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort2Contains, expectedFoo).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort2ContainsWrongCase, expectedFoo).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort2EndsWith, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaShort2EndsWithWrongCase, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-short-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull1Whole, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull1WholeWrongCase, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull1StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull1StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull1Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull1ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull1EndsWith, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull1EndsWithWrongCase, expectedBar).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:1-matches-full-endswith-ignorecase");

                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull2Whole, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-whole");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull2WholeWrongCase, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-whole-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull2StartsWith, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-startswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull2StartsWithWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-startswith-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull2Contains, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-contains");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull2ContainsWrongCase, expectedNone).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-contains-ignorecase");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull2EndsWith, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-endswith");
                yield return new TestCaseData(true, true, NameHandlingTypeMock.EndsWith, criteriaFull2EndsWithWrongCase, expectedBoth).SetName("Handling:EndsWith, UseFullName:true, IgnoreCase:true, Names:2-matches-full-endswith-ignorecase");
                #endregion
            }
        }
    }
}
