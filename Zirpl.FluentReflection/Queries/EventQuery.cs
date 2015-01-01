using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class EventQuery : NamedMemberQueryBase<EventInfo, IEventQuery>, 
        IEventQuery
    {
        private readonly TypeCriteria _eventHandlerTypeCriteria;

        internal EventQuery(Type type)
            :base(type)
        {
            _memberTypeFlagsBuilder.Event = true;
            _eventHandlerTypeCriteria = new TypeCriteria(TypeSource.EventHandlerType);
            _queryCriteriaList.Add(_eventHandlerTypeCriteria);
        }

        ITypeSubQuery<EventInfo, IEventQuery> IEventQuery.OfEventHandlerType()
        {
            return new TypeSubQuery<EventInfo, IEventQuery>(this, _eventHandlerTypeCriteria);
        }
    }
}
