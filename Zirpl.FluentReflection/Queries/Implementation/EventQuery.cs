﻿using System;
using System.Reflection;
using Zirpl.FluentReflection.Queries.Criteria;
using Zirpl.FluentReflection.Queries.SubQueries;

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

        ITypeSubQuery<EventInfo, IEventQuery> IEventQuery.OfEventHandlerType()
        {
            return new TypeSubQuery<EventInfo, IEventQuery>(this, _eventHandlerTypeCriteria);
        }
    }
}