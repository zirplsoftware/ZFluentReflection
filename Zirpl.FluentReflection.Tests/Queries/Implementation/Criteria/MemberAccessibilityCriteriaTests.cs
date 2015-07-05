using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using Zirpl.FluentReflection.Queries;

namespace Zirpl.FluentReflection.Tests.Queries.Implementation.Criteria
{
    [TestFixture]
    public class MemberAccessibilityCriteriaTests
    {

        #region ShouldRunFilter
        [Test, Combinatorial]
        public void TestShouldRun(
            [Values(true, false)]bool _public,
            [Values(true, false)]bool _private,
            [Values(true, false)]bool _protected,
            [Values(true, false)]bool _internal,
            [Values(true, false)]bool protectedInternal)
        {
            var criteria = new AccessibilityCriteria()
            {
                Public = _public,
                Family = _protected,
                FamilyOrAssembly = protectedInternal,
                Private = _private,
                Assembly = _internal
            };

            if ((_private
                    || _protected
                    || _internal
                    || protectedInternal)
                && !(_private
                    && _protected
                    && _internal
                    && protectedInternal))
            {
                criteria.ShouldRun.Should().BeTrue();
            }
            else
            {
                criteria.ShouldRun.Should().BeFalse();
            }
        }

        #endregion

        #region IsMatch
        [Test, Combinatorial]
        public void TestIsMatch_Private(
            [Values(true, false)]bool _public,
            [Values(true, false)]bool _private,
            [Values(true, false)]bool _protected,
            [Values(true, false)]bool _internal,
            [Values(true, false)]bool protectedInternal,
            [Values("PrivateField", "PrivateEvent", "PrivateMethod", "PrivateNestedType", "PrivateProperty", null)]String name)
        {
            var isMatchMethod = typeof (AccessibilityCriteria).GetMethod("IsMatch",
                BindingFlags.Instance | BindingFlags.NonPublic);
            // sanity check since a name change won't give a compile error
            isMatchMethod.Should().NotBeNull();

            MemberInfo memberInfo = null;
            if (name != null)
            {
                if (name.EndsWith("Event"))
                {
                    memberInfo = typeof (Foo).GetEvent(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Field"))
                {
                    memberInfo = typeof (Foo).GetField(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Method"))
                {
                    memberInfo = typeof(Foo).GetMethod(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("NestedType"))
                {
                    memberInfo = typeof(Foo).GetNestedType(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Property"))
                {
                    memberInfo = typeof(Foo).GetProperty(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                // sanity check
                memberInfo.Should().NotBeNull();
            }

            var criteria = new AccessibilityCriteria()
            {
                Public = _public,
                Family = _protected,
                FamilyOrAssembly = protectedInternal,
                Private = _private,
                Assembly = _internal
            };
            var isMatch = (bool)isMatchMethod.Invoke(criteria, new Object[] { memberInfo });
            isMatch.Should().Be(_private && name != null);
        }

        [Test, Combinatorial]
        public void TestIsMatch_Public(
            [Values(true, false)]bool _public,
            [Values(true, false)]bool _private,
            [Values(true, false)]bool _protected,
            [Values(true, false)]bool _internal,
            [Values(true, false)]bool protectedInternal,
            [Values("PublicField", "PublicEvent", "PublicMethod", "PublicNestedType", "PublicProperty", null)]String name)
        {
            var isMatchMethod = typeof(AccessibilityCriteria).GetMethod("IsMatch",
                BindingFlags.Instance | BindingFlags.NonPublic);
            // sanity check since a name change won't give a compile error
            isMatchMethod.Should().NotBeNull();

            MemberInfo memberInfo = null;
            if (name != null)
            {
                if (name.EndsWith("Event"))
                {
                    memberInfo = typeof(Foo).GetEvent(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Field"))
                {
                    memberInfo = typeof(Foo).GetField(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Method"))
                {
                    memberInfo = typeof(Foo).GetMethod(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("NestedType"))
                {
                    memberInfo = typeof(Foo).GetNestedType(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Property"))
                {
                    memberInfo = typeof(Foo).GetProperty(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                // sanity check
                memberInfo.Should().NotBeNull();
            }

            var criteria = new AccessibilityCriteria()
            {
                Public = _public,
                Family = _protected,
                FamilyOrAssembly = protectedInternal,
                Private = _private,
                Assembly = _internal
            };
            var isMatch = (bool)isMatchMethod.Invoke(criteria, new Object[] { memberInfo });
            isMatch.Should().Be(_public && name != null);
        }

        [Test, Combinatorial]
        public void TestIsMatch_Protected(
            [Values(true, false)]bool _public,
            [Values(true, false)]bool _private,
            [Values(true, false)]bool _protected,
            [Values(true, false)]bool _internal,
            [Values(true, false)]bool protectedInternal,
            [Values("ProtectedField", "ProtectedEvent", "ProtectedMethod", "ProtectedNestedType", "ProtectedProperty", null)]String name)
        {
            var isMatchMethod = typeof(AccessibilityCriteria).GetMethod("IsMatch",
                BindingFlags.Instance | BindingFlags.NonPublic);
            // sanity check since a name change won't give a compile error
            isMatchMethod.Should().NotBeNull();

            MemberInfo memberInfo = null;
            if (name != null)
            {
                if (name.EndsWith("Event"))
                {
                    memberInfo = typeof(Foo).GetEvent(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Field"))
                {
                    memberInfo = typeof(Foo).GetField(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Method"))
                {
                    memberInfo = typeof(Foo).GetMethod(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("NestedType"))
                {
                    memberInfo = typeof(Foo).GetNestedType(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Property"))
                {
                    memberInfo = typeof(Foo).GetProperty(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                // sanity check
                memberInfo.Should().NotBeNull();
            }

            var criteria = new AccessibilityCriteria()
            {
                Public = _public,
                Family = _protected,
                FamilyOrAssembly = protectedInternal,
                Private = _private,
                Assembly = _internal
            };
            var isMatch = (bool)isMatchMethod.Invoke(criteria, new Object[] { memberInfo });
            isMatch.Should().Be(_protected && name != null);
        }



        [Test, Combinatorial]
        public void TestIsMatch_ProtectedInternal(
            [Values(true, false)]bool _public,
            [Values(true, false)]bool _private,
            [Values(true, false)]bool _protected,
            [Values(true, false)]bool _internal,
            [Values(true, false)]bool protectedInternal,
            [Values("ProtectedInternalField", "ProtectedInternalEvent", "ProtectedInternalMethod", "ProtectedInternalNestedType", "ProtectedInternalProperty", null)]String name)
        {
            var isMatchMethod = typeof(AccessibilityCriteria).GetMethod("IsMatch",
                BindingFlags.Instance | BindingFlags.NonPublic);
            // sanity check since a name change won't give a compile error
            isMatchMethod.Should().NotBeNull();

            MemberInfo memberInfo = null;
            if (name != null)
            {
                if (name.EndsWith("Event"))
                {
                    memberInfo = typeof(Foo).GetEvent(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Field"))
                {
                    memberInfo = typeof(Foo).GetField(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Method"))
                {
                    memberInfo = typeof(Foo).GetMethod(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("NestedType"))
                {
                    memberInfo = typeof(Foo).GetNestedType(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Property"))
                {
                    memberInfo = typeof(Foo).GetProperty(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                // sanity check
                memberInfo.Should().NotBeNull();
            }

            var criteria = new AccessibilityCriteria()
            {
                Public = _public,
                Family = _protected,
                FamilyOrAssembly = protectedInternal,
                Private = _private,
                Assembly = _internal
            };
            var isMatch = (bool)isMatchMethod.Invoke(criteria, new Object[] { memberInfo });
            isMatch.Should().Be(protectedInternal && name != null);
        }

        [Test, Combinatorial]
        public void TestIsMatch_Internal(
            [Values(true, false)]bool _public,
            [Values(true, false)]bool _private,
            [Values(true, false)]bool _protected,
            [Values(true, false)]bool _internal,
            [Values(true, false)]bool protectedInternal,
            [Values("InternalField", "InternalEvent", "InternalMethod", "InternalNestedType", "InternalProperty", null)]String name)
        {
            var isMatchMethod = typeof(AccessibilityCriteria).GetMethod("IsMatch",
                BindingFlags.Instance | BindingFlags.NonPublic);
            // sanity check since a name change won't give a compile error
            isMatchMethod.Should().NotBeNull();

            MemberInfo memberInfo = null;
            if (name != null)
            {
                if (name.EndsWith("Event"))
                {
                    memberInfo = typeof(Foo).GetEvent(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Field"))
                {
                    memberInfo = typeof(Foo).GetField(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Method"))
                {
                    memberInfo = typeof(Foo).GetMethod(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("NestedType"))
                {
                    memberInfo = typeof(Foo).GetNestedType(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                else if (name.EndsWith("Property"))
                {
                    memberInfo = typeof(Foo).GetProperty(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                }
                // sanity check
                memberInfo.Should().NotBeNull();
            }

            var criteria = new AccessibilityCriteria()
            {
                Public = _public,
                Family = _protected,
                FamilyOrAssembly = protectedInternal,
                Private = _private,
                Assembly = _internal
            };
            var isMatch = (bool)isMatchMethod.Invoke(criteria, new Object[] { memberInfo });
            isMatch.Should().Be(_internal && name != null);
        }

        [Test, Combinatorial]
        public void TestIsMatch_Constructors(
            [Values(true, false)] bool _public,
            [Values(true, false)] bool _private,
            [Values(true, false)] bool _protected,
            [Values(true, false)] bool _internal,
            [Values(true, false)] bool protectedInternal)
        {
            var isMatchMethod = typeof(AccessibilityCriteria).GetMethod("IsMatch",
                BindingFlags.Instance | BindingFlags.NonPublic);
            // sanity check since a name change won't give a compile error
            isMatchMethod.Should().NotBeNull();

            // since it's the call to IsMatch that actually handles the logic
            // this will be a light test, especially because it makes assumptions
            var criteria = new AccessibilityCriteria()
            {
                Public = _public,
                Family = _protected,
                FamilyOrAssembly = protectedInternal,
                Private = _private,
                Assembly = _internal
            };

            var memberList = new List<ConstructorInfo>();
            memberList.AddRange(typeof(Foo).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            // sanity check:
            memberList.Count.Should().Be(5);

            foreach (var constructorInfo in memberList)
            {
                var isMatch = (bool)isMatchMethod.Invoke(criteria, new Object[] { constructorInfo });
                if (constructorInfo.IsPublic)
                {
                    isMatch.Should().Be(_public);
                }
                else if (constructorInfo.IsPrivate)
                {
                    isMatch.Should().Be(_private);
                }
                else if (constructorInfo.IsFamily)
                {
                    isMatch.Should().Be(_protected);
                }
                else if (constructorInfo.IsAssembly)
                {
                    isMatch.Should().Be(_internal);
                }
                else
                {
                    isMatch.Should().Be(protectedInternal);
                }
            }
        }

        #endregion

        #region GetMatches
        
        [Test]
        public void TestGetMatches_EdgeCases()
        {
            new AccessibilityCriteria().GetMatches(new MemberInfo[0]).Should().NotBeNull();
            new AccessibilityCriteria().GetMatches(new MemberInfo[0]).Should().BeEmpty();
            new AccessibilityCriteria().GetMatches(null).Should().BeNull();
        }

        [Test, Combinatorial]
        public void TestGetMatches_Constructors(
            [Values(true, false)] bool _public,
            [Values(true, false)] bool _private,
            [Values(true, false)] bool _protected,
            [Values(true, false)] bool _internal,
            [Values(true, false)] bool protectedInternal)
        {

            // since it's the call to IsMatch that actually handles the logic
            // this will be a light test, especially because it makes assumptions
            var criteria = new AccessibilityCriteria()
            {
                Public = _public,
                Family = _protected,
                FamilyOrAssembly = protectedInternal,
                Private = _private,
                Assembly = _internal
            };

            var memberList = new List<ConstructorInfo>();
            memberList.AddRange(typeof(Foo).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            // sanity check:
            memberList.Count.Should().Be(5);

            var result = criteria.GetMatches(memberList.ToArray());
            result.Should().NotBeNull();

            if (criteria.ShouldRun)
            {
                result.Any(memberInfo => ((ConstructorInfo)memberInfo).IsPrivate).Should().Be(_private);
                result.Any(memberInfo => ((ConstructorInfo)memberInfo).IsPublic).Should().Be(_public);
                result.Any(memberInfo => ((ConstructorInfo)memberInfo).IsFamily).Should().Be(_protected);
                result.Any(memberInfo => ((ConstructorInfo)memberInfo).IsAssembly).Should().Be(_internal);
                result.Any(memberInfo => ((ConstructorInfo)memberInfo).IsFamilyOrAssembly).Should().Be(protectedInternal);
            }
            else
            {
                result.Count().Should().Be(memberList.Count);
            }
        }

        [Test, Combinatorial]
        public void TestGetMatches(
            [Values(true, false)]bool _public,
            [Values(true, false)]bool _private,
            [Values(true, false)]bool _protected,
            [Values(true, false)]bool _internal,
            [Values(true, false)]bool protectedInternal,
            [Values("Private", "Public", "Protected", "ProtectedInternal", "Internal")]String namePrefix)
        {
            // since it's the call to IsMatch that actually handles the logic
            var criteria = new AccessibilityCriteria()
            {
                Public = _public,
                Family = _protected,
                FamilyOrAssembly = protectedInternal,
                Private = _private,
                Assembly = _internal
            };
            
            var memberList = new List<MemberInfo>();
            memberList.Add(typeof (Foo).GetEvent(namePrefix + "Event",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            memberList.Add(typeof (Foo).GetField(namePrefix + "Field",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            memberList.Add(typeof (Foo).GetMethod(namePrefix + "Method",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            memberList.Add(typeof (Foo).GetNestedType(namePrefix + "NestedType",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            memberList.Add(typeof (Foo).GetProperty(namePrefix + "Property",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            // sanity check:
            memberList.Count.Should().Be(5);

            var result = criteria.GetMatches(memberList.ToArray());
            result.Should().NotBeNull();
            if (criteria.ShouldRun)
            {
                switch (namePrefix)
                {
                    case "Private":
                        if (_private)
                        {
                            result.Should().NotBeEmpty();
                            result.Count().Should().Be(memberList.Count);
                        }
                        else
                        {
                            result.Should().BeEmpty();
                        }
                        break;
                    case "Public":
                        if (_public)
                        {
                            result.Should().NotBeEmpty();
                            result.Count().Should().Be(memberList.Count);
                        }
                        else
                        {
                            result.Should().BeEmpty();
                        }
                        break;
                    case "Protected":
                        if (_protected)
                        {
                            result.Should().NotBeEmpty();
                            result.Count().Should().Be(memberList.Count);
                        }
                        else
                        {
                            result.Should().BeEmpty();
                        }
                        break;
                    case "ProtectedInternal":
                        if (protectedInternal)
                        {
                            result.Should().NotBeEmpty();
                            result.Count().Should().Be(memberList.Count);
                        }
                        else
                        {
                            result.Should().BeEmpty();
                        }
                        break;
                    case "Internal":
                        if (_internal)
                        {
                            result.Should().NotBeEmpty();
                            result.Count().Should().Be(memberList.Count);
                        }
                        else
                        {
                            result.Should().BeEmpty();
                        }
                        break;
                    default:
                        throw new Exception("Unexpected case");
                }
            }
            else
            {
                result.Should().NotBeEmpty();
                result.Count().Should().Be(memberList.Count);
            }
        }
        #endregion

        #region Helper functionality

        [Flags]
        public enum RetrievalFlags
        {
            None = 0,
            Public = 1,
            Private = 2,
            Protected = 4,
            Internal = 8,
            ProtectedInternal = 16
        }
        public class Foo
        {
            public event EventHandler PublicEvent;
            private event EventHandler PrivateEvent;
            protected event EventHandler ProtectedEvent;
            internal event EventHandler InternalEvent;
            protected internal event EventHandler ProtectedInternalEvent;

            public Foo()
            {
                
            }
            private Foo(int a)
            {
                
            }

            protected Foo(int a, int b)
            {
                
            }

            internal Foo(int a, int b, int c)
            {
                
            }

            protected internal Foo(int a, int b, int c, int d)
            {
                
            }

            public class PublicNestedType
            {
                
            }

            private class PrivateNestedType
            {
                
            }

            protected class ProtectedNestedType
            {
                
            }

            internal class InternalNestedType
            {
                
            }

            protected internal class ProtectedInternalNestedType 
            {
                
            }

            public int PublicField;
            private int PrivateField;
            protected int ProtectedField;
            internal int InternalField;
            protected internal int ProtectedInternalField;

            public int PublicProperty { get; set; }
            private int PrivateProperty { get; set; }
            protected int ProtectedProperty { get; set; }
            internal int InternalProperty { get; set; }
            protected internal int ProtectedInternalProperty { get; set; }

            public void PublicMethod()
            {
                
            }

            private void PrivateMethod()
            {
                
            }

            protected void ProtectedMethod()
            {
                
            }

            internal void InternalMethod()
            {
                
            }

            protected internal void ProtectedInternalMethod()
            {
                
            }

        }
        #endregion
    }
}
