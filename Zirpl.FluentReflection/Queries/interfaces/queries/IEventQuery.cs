using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IEventQuery : INamedMemberQuery<EventInfo, IEventQuery>
    {
        IEventQuery OfEventHandlerType(Action<ITypeCriteriaBuilder> builder);
    }
}