using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    internal sealed class EventQuery : NamedMemberQueryBase<EventInfo, IEventQuery>, 
        IEventQuery
    {
        private readonly TypeCriteria _eventHandlerTypeCriteria;

        internal EventQuery(Type type)
            :base(type)
        {
            MemberTypeFlagsBuilder.Event = true;
            _eventHandlerTypeCriteria = new TypeCriteria(TypeSource.EventHandlerType);
            QueryCriteriaList.Add(_eventHandlerTypeCriteria);
        }

        IEventQuery IEventQuery.OfEventHandlerType(Action<ITypeCriteriaBuilder> builder)
        {
            builder(new TypeCriteriaBuilder(_eventHandlerTypeCriteria));
            return this;
        }
    }
}
