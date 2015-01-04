using System;
using System.Reflection;

namespace Zirpl.FluentReflection.Accessors
{
    public sealed class EventAccessor<T>
        where T : EventArgs
    {
        private readonly EventInfo _eventInfo;
        private readonly Object _obj;
        
        internal EventAccessor(EventInfo eventInfo, Object obj)
        {
            _eventInfo = eventInfo;
            _obj = obj;
        }

        public bool Exists
        {
            get { return _eventInfo != null; }
        }

        public event EventHandler<T> Event
        {
            add
            {
                Type delegateType = _eventInfo.EventHandlerType;

                // Create an instance of the delegate. Using the overloads 
                // of CreateDelegate that take MethodInfo is recommended. 
                //
                Delegate handlerAsDelegate = Delegate.CreateDelegate(delegateType, this, value.Method);

                // Get the "add" accessor of the event and invoke it late-
                // bound, passing in the delegate instance. This is equivalent 
                // to using the += operator in C#, or AddHandler in Visual 
                // Basic. The instance on which the "add" accessor is invoked
                // is the form; the arguments must be passed as an array. 
                //
                MethodInfo addHandler = _eventInfo.GetAddMethod();
                Object[] addHandlerArgs = { handlerAsDelegate };
                addHandler.Invoke(_obj, addHandlerArgs);
            }
            remove
            {
                // same as above, but with Remove handler
                Type delegateType = _eventInfo.EventHandlerType;
                Delegate handlerAsDelegate = Delegate.CreateDelegate(delegateType, this, value.Method);
                MethodInfo removeHandler = _eventInfo.GetRemoveMethod();
                Object[] removeHandlerArgs = { handlerAsDelegate };
                removeHandler.Invoke(_obj, removeHandlerArgs); 
            }
        }

        public EventInfo EventInfo { get { return _eventInfo; } }
    }
}
