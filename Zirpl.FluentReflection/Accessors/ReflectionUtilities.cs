using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    public static partial class ReflectionUtilities
    {
        public static PropertyAccessor<T> Property<T>(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new PropertyAccessor<T>(InstanceTypeAccessor.Get(obj.GetType()).Property(name), obj);
        }

        public static PropertyAccessor<Object> Property(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new PropertyAccessor<Object>(InstanceTypeAccessor.Get(obj.GetType()).Property(name), obj);
        }

        public static FieldAccessor<T> Field<T>(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new FieldAccessor<T>(InstanceTypeAccessor.Get(obj.GetType()).Field(name), obj);
        }

        public static FieldAccessor<Object> Field(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new FieldAccessor<Object>(InstanceTypeAccessor.Get(obj.GetType()).Field(name), obj);
        }

        public static EventAccessor<T> Event<T>(this Object obj, String name)
            where T: EventArgs
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new EventAccessor<T>(InstanceTypeAccessor.Get(obj.GetType()).Event(name), obj);
        }

        public static EventAccessor<EventArgs> Event(this Object obj, String name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            return new EventAccessor<EventArgs>(InstanceTypeAccessor.Get(obj.GetType()).Event(name), obj);
        }
    }
}
