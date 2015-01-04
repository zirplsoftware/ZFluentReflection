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
    public class MemberTypesFlagBuilderTests
    {
        [Test, Combinatorial]
        public void TestMemberTypeFlags(
            [Values(true, false)]bool constructors, 
            [Values(true, false)]bool events, 
            [Values(true, false)]bool fields, 
            [Values(true, false)]bool methods, 
            [Values(true, false)]bool nestedTypes,
            [Values(true, false)]bool properties)
        {
            var builder = new MemberTypeFlagsBuilder()
            {
                Constructor = constructors,
                Event = events,
                Field = fields,
                Method = methods,
                NestedType = nestedTypes,
                Property = properties
            };
            var result = builder.MemberTypeFlags;
            if (constructors
                || events
                || fields
                || methods
                || nestedTypes
                || properties)
            {
                result.HasFlag(MemberTypeFlags.Constructor).Should().Be(constructors);
                result.HasFlag(MemberTypeFlags.Event).Should().Be(events);
                result.HasFlag(MemberTypeFlags.Field).Should().Be(fields);
                result.HasFlag(MemberTypeFlags.Method).Should().Be(methods);
                result.HasFlag(MemberTypeFlags.NestedType).Should().Be(nestedTypes);
                result.HasFlag(MemberTypeFlags.Property).Should().Be(properties);
            }
            else
            {
                result.HasFlag(MemberTypeFlags.Constructor).Should().Be(true);
                result.HasFlag(MemberTypeFlags.Event).Should().Be(true);
                result.HasFlag(MemberTypeFlags.Field).Should().Be(true);
                result.HasFlag(MemberTypeFlags.Method).Should().Be(true);
                result.HasFlag(MemberTypeFlags.NestedType).Should().Be(true);
                result.HasFlag(MemberTypeFlags.Property).Should().Be(true);
            }
        }
    }
}
