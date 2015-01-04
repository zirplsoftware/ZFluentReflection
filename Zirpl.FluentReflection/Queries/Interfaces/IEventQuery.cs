using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IEventQuery : INamedMemberQuery<EventInfo, IEventQuery>
    {
        ITypeSubQuery<EventInfo, IEventQuery> OfEventHandlerType();
    }
}