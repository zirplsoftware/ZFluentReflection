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

        internal IEnumerable<MemberInfo> FindMembers(MemberTypeFlags memberTypeFlags, BindingFlags bindingFlags, IEnumerable<String> names)
        {
            return GetMembers(_type, memberTypeFlags, bindingFlags, names);
        }

        internal IEnumerable<MemberInfo> FindPrivateMembersOnBaseTypes(MemberTypeFlags memberTypes, BindingFlags bindingFlags, int levelsDeep, IEnumerable<String> names)
        {
            var list = new List<MemberInfo>();
            var accessibilityEvaluator = new AccessibilityEvaluator();
            accessibilityEvaluator.Private = true;
            if (levelsDeep > 0)
            {
                var type = _type;
                var levelsDeeper = levelsDeep;
                while (type != null
                    && levelsDeeper > 0)
                {
                    list.AddRange(GetMembers(type, memberTypes, bindingFlags, names).Where(o => !list.Contains(o) && accessibilityEvaluator.IsMatch(o)));
                    type = type.BaseType;
                    levelsDeeper -= 1;
                }
            }
            else
            {
                var type = _type;
                while (type != null)
                {
                    list.AddRange(GetMembers(type, memberTypes, bindingFlags, names).Where(o => !list.Contains(o) && accessibilityEvaluator.IsMatch(o)));
                    type = type.BaseType;
                }
            }
            // TODO: check for hidden by signature
            return list;
        }

        private IEnumerable<MemberInfo> GetMembers(Type type, MemberTypeFlags memberTypes, BindingFlags bindingFlags, IEnumerable<String> names)
        {
            var found = new List<MemberInfo>();
            if (names != null
                && names.Any())
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
                if (names != null
                    && names.Any())
                {
                    if (names.Count() > 1)
                    {
                        foreach (var name in names)
                        {
                            found.Add(type.GetProperty(name, bindingFlags));
                        }
                    }
                    else
                    {
                        found.Add(type.GetProperty(names.Single(), bindingFlags));
                    }
                }
                else
                {
                    found.AddRange(type.GetProperties(bindingFlags));
                }
            }

            //found.AddRange(type.FindMembers(memberTypes, bindingFlags, FindMemberMatch, null));
            return found;
        }
    }
}
