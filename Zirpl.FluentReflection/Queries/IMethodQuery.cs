using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IMethodQuery : INamedMemberQuery<MethodInfo, IMethodQuery>
    {
        ITypeSubQuery<MethodInfo, IMethodQuery> OfReturnType();
        IMethodQuery WithParameters(Type[] typesOfParameters);
    }
}