using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IEventQuery : INamedMemberQuery<EventInfo, IEventQuery>
    {
        ITypeQuery<EventInfo, IEventQuery> OfEventHandlerType();
    }
}