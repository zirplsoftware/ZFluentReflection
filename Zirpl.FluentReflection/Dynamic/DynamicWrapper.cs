// taken with permission from: http://weblogs.asp.net/dixin/a-todynamic-extension-method-for-fluent-reflection

using System;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Zirpl.FluentReflection
{
    public class DynamicWrapper<T> : DynamicObject
    {
        #region Constants and Fields

        private readonly bool _isValueType;

        private readonly Type _type;

        private T _value; // Not readonly, for value type scenarios.

        #endregion

        #region Constructors and Destructors

        public DynamicWrapper() // For static.
        {
            this._type = typeof(T);
            this._isValueType = this._type.IsValueType;
        }

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#",
            Justification = "ref is required for value type.")]
        public DynamicWrapper(ref T value) // Uses ref in case of 'value' is value type.
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this._value = value;
            this._type = value.GetType();
            this._isValueType = this._type.IsValueType;
        }

        #endregion

        #region Public Methods

        public T ToStatic()
        {
            return this._value;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            result = this._value;
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            Array array = this._value as Array;
            if (array != null && indexes.All(item => item is int || item is long))
            {
                object resultValue = array.GetValue(indexes.Select(Convert.ToInt32).ToArray());
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            PropertyInfo index = this._type.GetTypeIndex(indexes);
            if (index != null)
            {
                object resultValue = index.GetValue(this._value, indexes);
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            MethodInfo method = this._type.GetInterfaceMethod("get_Item", indexes);
            if (method != null)
            {
                object resultValue = method.Invoke(this._value, indexes);
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            index = this._type.GetBaseIndex(indexes);
            if (index != null)
            {
                object resultValue = index.GetValue(this._value, indexes);
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            result = null;
            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (binder == null)
            {
                throw new ArgumentNullException("binder");
            }

            // Searches in current type's public and non-public properties.
            PropertyInfo property = this._type.GetTypeProperty(binder.Name);
            if (property != null)
            {
                object resultValue = property.GetValue(this._value, null);
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            // Searches in explicitly implemented properties for interface.
            MethodInfo method = this._type.GetInterfaceMethod(string.Concat("get_", binder.Name), null);
            if (method != null)
            {
                object resultValue = method.Invoke(this._value, null);
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            // Searches in current type's public and non-public fields.
            FieldInfo field = this._type.GetTypeField(binder.Name);
            if (field != null)
            {
                object resultValue = field.GetValue(this._value);
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            // Searches in base type's public and non-public properties.
            property = this._type.GetBaseProperty(binder.Name);
            if (property != null)
            {
                object resultValue = property.GetValue(this._value, null);
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            // Searches in base type's public and non-public fields.
            field = this._type.GetBaseField(binder.Name);
            if (field != null)
            {
                object resultValue = field.GetValue(this._value);
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            // The specified member is not found.
            result = null;
            return false;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (binder == null)
            {
                throw new ArgumentNullException("binder");
            }

            MethodInfo method = this._type.GetTypeMethod(binder.Name, args) ??
                                this._type.GetInterfaceMethod(binder.Name, args) ??
                                this._type.GetBaseMethod(binder.Name, args);
            if (method != null)
            {
                // Oops!
                // If the returnValue is a struct, it is copied to heap.
                object resultValue = method.Invoke(this._value, args);
                // And result is a wrapper of that copied struct.
                result = new DynamicWrapper<object>(ref resultValue);
                return true;
            }

            result = null;
            return false;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (this._isValueType)
            {
                throw new NotSupportedException("Setting index on value type is not supported.");
            }

            Array array = this._value as Array;
            if (array != null && indexes.All(item => item is int || item is long))
            {
                array.SetValue(value, indexes.Select(Convert.ToInt32).ToArray());
                return true;
            }

            PropertyInfo index = this._type.GetTypeIndex(indexes);
            if (index != null)
            {
                index.SetValue(this._value, value, indexes);
                return true;
            }

            MethodInfo method = this._type.GetInterfaceMethod("set_Item", indexes);
            if (method != null)
            {
                method.Invoke(this._value, indexes.Concat(Enumerable.Repeat(value, 1)).ToArray());
                return true;
            }

            index = this._type.GetBaseIndex(indexes);
            if (index != null)
            {
                index.SetValue(this._value, value, indexes);
                return true;
            }

            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (binder == null)
            {
                throw new ArgumentNullException("binder");
            }

            if (!this._isValueType)
            {
                PropertyInfo property = this._type.GetTypeProperty(binder.Name);
                if (property != null)
                {
                    property.SetValue(this._value, value, null);
                    return true;
                }
            }

            FieldInfo field = this._type.GetTypeField(binder.Name);
            if (field != null)
            {
                field.SetValue(ref this._value, value);
                return true;
            }

            if (!this._isValueType)
            {
                MethodInfo method = this._type.GetInterfaceMethod(string.Concat("set_", binder.Name), value);
                if (method != null)
                {
                    method.Invoke(this._value, new[] { value });
                }

                PropertyInfo property = this._type.GetBaseProperty(binder.Name);
                if (property != null)
                {
                    property.SetValue(this._value, value, null);
                    return true;
                }
            }

            field = this._type.GetBaseField(binder.Name);
            if (field != null)
            {
                field.SetValue(ref this._value, value);
                return true;
            }

            if (this._isValueType)
            {
                throw new MissingMemberException("The specified field is not found (Setting property is not supported on value type).");
            }

            return false;
        }

        #endregion
    }
}