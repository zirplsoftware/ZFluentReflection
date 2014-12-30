using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMethodQuery : INamedMemberQuery<MethodInfo, IMethodQuery>
    {
        ITypeSubQuery<MethodInfo, IMethodQuery> OfReturnType();
    }
}