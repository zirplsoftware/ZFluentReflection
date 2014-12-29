﻿namespace Zirpl.FluentReflection
{
    internal sealed class MemberTypeFlagsBuilder
    {
        private readonly MemberTypeEvaluator _memberTypeEvaluator;
        internal MemberTypeFlagsBuilder(MemberTypeEvaluator memberTypeEvaluator)
        {
            _memberTypeEvaluator = memberTypeEvaluator;
        }
        internal MemberTypeFlags MemberTypeFlags
        {
            get
            {
                var memberTypes = default(MemberTypeFlags);
                if (_memberTypeEvaluator.Constructor
                    || _memberTypeEvaluator.Event
                    || _memberTypeEvaluator.Field
                    || _memberTypeEvaluator.Method
                    || _memberTypeEvaluator.NestedType
                    || _memberTypeEvaluator.Property)
                {
                    if (_memberTypeEvaluator.Constructor) memberTypes = memberTypes | MemberTypeFlags.Constructor;
                    if (_memberTypeEvaluator.Event) memberTypes = memberTypes | MemberTypeFlags.Event;
                    if (_memberTypeEvaluator.Field) memberTypes = memberTypes | MemberTypeFlags.Field;
                    if (_memberTypeEvaluator.Method) memberTypes = memberTypes | MemberTypeFlags.Method;
                    if (_memberTypeEvaluator.NestedType) memberTypes = memberTypes | MemberTypeFlags.NestedType;
                    if (_memberTypeEvaluator.Property) memberTypes = memberTypes | MemberTypeFlags.Property;
                }
                else
                {
                    // default to ALL 
                    memberTypes = MemberTypeFlags.AllExceptCustomAndTypeInfo;
                }
                return memberTypes;
            }
        }
    }
}
