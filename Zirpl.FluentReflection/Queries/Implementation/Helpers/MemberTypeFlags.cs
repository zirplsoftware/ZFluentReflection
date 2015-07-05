using System;

namespace Zirpl.FluentReflection.Queries
{
    [Flags]
    internal enum MemberTypeFlags
    {
        Constructor = 1,
        Event = 2,
        Field = 4,
        Method = 8,
        NestedType = 128,
        Property = 16,
        //TypeInfo = 32,
        //Custom = 64,
        All = 191,
        AllExceptCustomAndTypeInfo = 159
    }
}
