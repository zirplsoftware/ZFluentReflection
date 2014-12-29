using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class ConstructorQuery : TypeMemberQueryBase<ConstructorInfo, IConstructorQuery>, 
        IConstructorQuery
    {
        internal ConstructorQuery(Type type)
            :base(type)
        {
            _memberTypeEvaluator.Constructor = true;
        }
    }
}
