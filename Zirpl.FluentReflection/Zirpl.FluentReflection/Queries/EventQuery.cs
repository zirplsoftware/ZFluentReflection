using System;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal sealed class EventQuery : NamedTypeMemberQueryBase<EventInfo, IEventQuery>, 
        IEventQuery
    {
        private readonly EventHandlerTypeCriteria _eventHandlerTypeCriteria;

        internal EventQuery(Type type)
            :base(type)
        {
            _memberTypeCriteria.Event = true;
            _eventHandlerTypeCriteria = new EventHandlerTypeCriteria();
            _matchEvaluators.Add(_eventHandlerTypeCriteria);
        }

        ITypeQuery<EventInfo, IEventQuery> IEventQuery.OfEventHandlerType()
        {
            return new TypeSubQuery<EventInfo, IEventQuery>(this, _eventHandlerTypeCriteria);
        }
    }
}
