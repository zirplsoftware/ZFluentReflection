using System;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Accessors
{
    public sealed class MethodsAccessor<T>
    {
        private readonly String _methodName;
        private readonly Object _obj;

        internal MethodsAccessor(String methodName, Object obj)
        {
            _methodName = methodName;
            _obj = obj;
        }

        public bool Exists
        {
            get
            {
                return _obj.GetType()
                    .QueryMethods()
                    .OfAccessibility().All()
                    .OfScope().All()
                    .Named().ExactlyIgnoreCase(_methodName)
                    .Result().Any();
            }
        }

        public T Invoke(params Object[] args)
        {
            var matches = _obj.GetType()
                .QueryMethods()
                .OfAccessibility().All()
                .OfScope().All()
                .Named().ExactlyIgnoreCase(_methodName)
                .Result()
                .Where(method =>
                    method.ReturnParameter == null
                    && method.GetParameters().Count() == (args == null ? 0 : args.Count())
                    && method.GetParameters().Select(
                        (parameter, index) => parameter.ParameterType == args[index].GetType()).Aggregate(
                            true, (a, b) => a && b));
            if (matches.Count() > 1)
            {
                throw new AmbiguousMatchException("Multiple methods could be targeted");
            }
            else if (matches.Count() == 0)
            {
                throw new MissingMemberException("Could not find method");
            }
            else
            {
                return (T)matches.Single().Invoke(_obj, args);
            }
        }

        public MethodInfo[] MethodInfos
        {
            get
            {
                return _obj.GetType()
                .QueryMethods()
                .OfAccessibility().All()
                .OfScope().All()
                .Named().ExactlyIgnoreCase(_methodName)
                .Result().ToArray();
            }
        }
    }
}
