using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IMethodQuery : INamedMemberQuery<MethodInfo, IMethodQuery>
    {
        IMethodQuery OfReturnType(Action<ITypeCriteriaBuilder> builder);
        IMethodQuery WithParameters(Type[] typesOfParameters);
    }
}