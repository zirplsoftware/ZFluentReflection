using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zirpl.FluentReflection.Queries.Criteria;

namespace Zirpl.FluentReflection.Queries.Helpers
{
    internal sealed class MemberQueryService
    {
        private readonly Type _type;

        internal MemberQueryService(Type type)
        {
            _type = type;
        }

        internal MemberInfo[] FindMembers(MemberTypeFlags memberTypeFlags, BindingFlags bindingFlags, IEnumerable<String> names)
        {
            return FindMemberOnType(_type, memberTypeFlags, bindingFlags, names);
        }

        internal MemberInfo[] FindPrivateMembersOnBaseTypes(MemberTypeFlags memberTypes, BindingFlags bindingFlags, IEnumerable<String> names)
        {
            var list = new List<MemberInfo>();
            var accessibilityEvaluator = new MemberAccessibilityCriteria();
            accessibilityEvaluator.Private = true;
            var type = _type;
            while (type != null)
            {
                list.AddRange(accessibilityEvaluator.GetMatches(FindMemberOnType(type, memberTypes, bindingFlags, names)).Where(o => !list.Contains(o)));
                type = type.BaseType;
            }
            // TODO: check for hidden by signature
            return list.ToArray();
        }

        private MemberInfo[] FindMemberOnType(Type type, MemberTypeFlags memberTypes, BindingFlags bindingFlags, IEnumerable<String> names)
        {
            var found = new List<MemberInfo>();
            var theNames = names == null ? null : names.ToList();
            var namesCount = theNames == null ? 0 : theNames.Count();
            if (namesCount > 0)
            {
                memberTypes = memberTypes & ~MemberTypeFlags.Constructor;
            }

            if (memberTypes.HasFlag(MemberTypeFlags.Constructor))
            {
                found.AddRange(type.GetConstructors(bindingFlags));  
            }
            if (memberTypes.HasFlag(MemberTypeFlags.Event))
            {
                if (namesCount > 0)
                {
                    if (namesCount > 1)
                    {
                        found.AddRange(theNames.Select(name => type.GetEvent(name, bindingFlags)));
                    }
                    else
                    {
                        found.Add(type.GetEvent(theNames[0], bindingFlags));
                    }
                }
                else
                {
                    found.AddRange(type.GetEvents(bindingFlags));
                }
            }
            if (memberTypes.HasFlag(MemberTypeFlags.Field))
            {
                if (namesCount > 0)
                {
                    if (namesCount > 1)
                    {
                        found.AddRange(theNames.Select(name => type.GetField(name, bindingFlags)));
                    }
                    else
                    {
                        found.Add(type.GetField(theNames[0], bindingFlags));
                    }
                }
                else
                {
                    found.AddRange(type.GetFields(bindingFlags));
                }
            }
            if (memberTypes.HasFlag(MemberTypeFlags.Method))
            {
                if (namesCount > 0)
                {
                    if (namesCount > 1)
                    {
                        found.AddRange(theNames.Select(name => type.GetMethod(name, bindingFlags)));
                    }
                    else
                    {
                        found.Add(type.GetMethod(theNames[0], bindingFlags));
                    }
                }
                else
                {
                    found.AddRange(type.GetMethods(bindingFlags));
                }
            }
            if (memberTypes.HasFlag(MemberTypeFlags.NestedType))
            {
                if (namesCount > 0)
                {
                    if (namesCount > 1)
                    {
                        found.AddRange(theNames.Select(name => type.GetNestedType(name, bindingFlags)));
                    }
                    else
                    {
                        found.Add(type.GetNestedType(theNames[0], bindingFlags));
                    }
                }
                else
                {
                    found.AddRange(type.GetNestedTypes(bindingFlags));
                }
            }
            if (memberTypes.HasFlag(MemberTypeFlags.Property))
            {
                if (namesCount > 0)
                {
                    if (namesCount > 1)
                    {
                        found.AddRange(theNames.Select(name => type.GetProperty(name, bindingFlags)));
                    }
                    else
                    {
                        found.Add(type.GetProperty(theNames[0], bindingFlags));
                    }
                }
                else
                {
                    found.AddRange(type.GetProperties(bindingFlags));
                }
            }

            //found.AddRange(type.FindMembers(memberTypes, bindingFlags, FindMemberMatch, null));
            return found.ToArray();
        }
    }
}
