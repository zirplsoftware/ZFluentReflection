using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMatchEvaluator
    {
        bool IsMatchCheckRequired();
        bool IsMatch(MemberInfo memberInfo);
    }
}