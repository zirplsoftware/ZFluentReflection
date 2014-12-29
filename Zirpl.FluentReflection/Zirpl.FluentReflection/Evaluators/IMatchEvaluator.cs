using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMatchEvaluator
    {
        bool IsMatch(MemberInfo memberInfo);
    }
}