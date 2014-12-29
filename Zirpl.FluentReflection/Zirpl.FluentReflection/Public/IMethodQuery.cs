using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMethodQuery : INamedMemberQuery<MethodInfo, IMethodQuery>
    {
        ITypeQuery<MethodInfo, IMethodQuery> OfReturnType();
    }
}