namespace Zirpl.FluentReflection
{
    internal sealed class MemberTypeFlagsBuilder
    {
        internal bool Constructor { get; set; }
        internal bool Event { get; set; }
        internal bool Field { get; set; }
        internal bool Method { get; set; }
        internal bool NestedType { get; set; }
        internal bool Property { get; set; }

        internal MemberTypeFlags MemberTypeFlags
        {
            get
            {
                var memberTypes = default(MemberTypeFlags);
                if (Constructor
                    || Event
                    || Field
                    || Method
                    || NestedType
                    || Property)
                {
                    if (Constructor) memberTypes = memberTypes | MemberTypeFlags.Constructor;
                    if (Event) memberTypes = memberTypes | MemberTypeFlags.Event;
                    if (Field) memberTypes = memberTypes | MemberTypeFlags.Field;
                    if (Method) memberTypes = memberTypes | MemberTypeFlags.Method;
                    if (NestedType) memberTypes = memberTypes | MemberTypeFlags.NestedType;
                    if (Property) memberTypes = memberTypes | MemberTypeFlags.Property;
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
