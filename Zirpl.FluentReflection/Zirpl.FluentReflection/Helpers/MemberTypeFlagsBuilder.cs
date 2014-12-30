namespace Zirpl.FluentReflection
{
    internal sealed class MemberTypeFlagsBuilder
    {
        private readonly MemberTypeCriteria _memberTypeCriteria;
        internal MemberTypeFlagsBuilder(MemberTypeCriteria memberTypeCriteria)
        {
            _memberTypeCriteria = memberTypeCriteria;
        }
        internal MemberTypeFlags MemberTypeFlags
        {
            get
            {
                var memberTypes = default(MemberTypeFlags);
                if (_memberTypeCriteria.Constructor
                    || _memberTypeCriteria.Event
                    || _memberTypeCriteria.Field
                    || _memberTypeCriteria.Method
                    || _memberTypeCriteria.NestedType
                    || _memberTypeCriteria.Property)
                {
                    if (_memberTypeCriteria.Constructor) memberTypes = memberTypes | MemberTypeFlags.Constructor;
                    if (_memberTypeCriteria.Event) memberTypes = memberTypes | MemberTypeFlags.Event;
                    if (_memberTypeCriteria.Field) memberTypes = memberTypes | MemberTypeFlags.Field;
                    if (_memberTypeCriteria.Method) memberTypes = memberTypes | MemberTypeFlags.Method;
                    if (_memberTypeCriteria.NestedType) memberTypes = memberTypes | MemberTypeFlags.NestedType;
                    if (_memberTypeCriteria.Property) memberTypes = memberTypes | MemberTypeFlags.Property;
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
