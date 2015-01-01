using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class ConstructorQuery : MemberQueryBase<ConstructorInfo, IConstructorQuery>, 
        IConstructorQuery
    {
        internal ConstructorQuery(Type type)
            :base(type)
        {
            _memberTypeFlagsBuilder.Constructor = true;
        }
    }
}
