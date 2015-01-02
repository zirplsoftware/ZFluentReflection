using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
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

        internal MemberInfo[] FindPrivateMembersOnBaseTypes(MemberTypeFlags memberTypes, BindingFlags bindingFlags, int levelsDeep, IEnumerable<String> names)
        {
            var list = new List<MemberInfo>();
            var accessibilityEvaluator = new MemberAccessibilityCriteria();
            accessibilityEvaluator.Private = true;
            if (levelsDeep > 0)
            {
                var type = _type;
                var levelsDeeper = levelsDeep;
                while (type != null
                    && levelsDeeper > 0)
                {
                    list.AddRange(accessibilityEvaluator.GetMatches(FindMemberOnType(type, memberTypes, bindingFlags, names)).Where(o => !list.Contains(o)));
                    type = type.BaseType;
                    levelsDeeper -= 1;
                }
            }
            else
            {
                var type = _type;
                while (type != null)
                {
                    list.AddRange(accessibilityEvaluator.GetMatches(FindMemberOnType(type, memberTypes, bindingFlags, names)).Where(o => !list.Contains(o)));
                    type = type.BaseType;
                }
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
                found.AddRange(type.GetEvents(bindingFlags));
            }
            if (memberTypes.HasFlag(MemberTypeFlags.Field))
            {
                found.AddRange(type.GetFields(bindingFlags));
            }
            if (memberTypes.HasFlag(MemberTypeFlags.Method))
            {
                found.AddRange(type.GetMethods(bindingFlags));
            }
            if (memberTypes.HasFlag(MemberTypeFlags.NestedType))
            {
                found.AddRange(type.GetNestedTypes(bindingFlags));
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
