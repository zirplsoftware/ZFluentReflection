using System;
using System.Linq;
using System.Reflection;
using Zirpl.FluentReflection.Accessors;

namespace Zirpl.FluentReflection.Accessors
{
    internal sealed class MethodsAccessor<T> : TypeMethodsAccessor, IMethodAccessor<T>
    {
        private readonly Object _obj;

        internal MethodsAccessor(String methodName, Object obj)
            : base (methodName, obj.GetType())
        {
            _obj = obj;
        }

        public T Invoke(params Object[] args)
        {
            var matches = MethodInfos
                .Where(method => method.GetParameters().Count() == (args == null ? 0 : args.Count())
                    && method.GetParameters().Select(
                        (parameter, index) => parameter.ParameterType == args[index].GetType()).Aggregate(
                            true, (a, b) => a && b));
            if (!matches.Any())
            {
                throw new MissingMemberException("Could not find method");
            }
            else if (matches.Count() > 1)
            {
                throw new AmbiguousMatchException("Multiple methods could be targeted");
            }
            else
            {
                return (T)matches.Single().Invoke(_obj, args);
            }
        }
    }
}
