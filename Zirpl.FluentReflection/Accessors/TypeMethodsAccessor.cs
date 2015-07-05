using System;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection.Accessors
{
    internal class TypeMethodsAccessor : ITypeMethodsAccessor
    {
        private readonly String _methodName;
        private readonly Type _type;

        internal TypeMethodsAccessor(String methodName, Type type)
        {
            _methodName = methodName;
            _type = type;
        }

        public bool Exists
        {
            get { return MethodInfos.Any(); }
        }

        protected Type Type { get { return _type; } }

        public MethodInfo[] MethodInfos 
        { 
            get 
            { 
                return _type
                .QueryMethods()
                .OfAccessibility(b => b.All())
                .OfScope(b => b.All())
                .Named(b => b.ExactlyIgnoreCase(_methodName))
                .Result().ToArray(); 
            } 
        }
    }
}
