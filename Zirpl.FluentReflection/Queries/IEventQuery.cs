using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IEventQuery : INamedMemberQuery<EventInfo, IEventQuery>
    {
        ITypeSubQuery<EventInfo, IEventQuery> OfEventHandlerType();
    }
}