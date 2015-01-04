// taken with permission from: http://weblogs.asp.net/dixin/a-todynamic-extension-method-for-fluent-reflection

using System.Reflection;

namespace Zirpl.FluentReflection
{
    internal static class FieldInfoExtensions
    {
        #region Methods

        internal static void SetValue<T>(this FieldInfo field, ref T obj, object value)
        {
#if !PORTABLE
            if (typeof(T).IsValueType)
            {
                field.SetValueDirect(__makeref(obj), value);
            }
            else
            {
                field.SetValue(obj, value);
            }
#else
                field.SetValue(obj, value);
#endif
        }

        #endregion
    }
}