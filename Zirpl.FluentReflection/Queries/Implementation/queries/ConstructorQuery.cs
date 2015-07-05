using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class ConstructorQuery : MemberQueryBase<ConstructorInfo, IConstructorQuery>, 
        IConstructorQuery
    {
        internal ConstructorQuery(Type type)
            :base(type)
        {
            MemberTypeFlagsBuilder.Constructor = true;
        }
    }
}
