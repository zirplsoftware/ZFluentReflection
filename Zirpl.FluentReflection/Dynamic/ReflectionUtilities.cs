namespace Zirpl.FluentReflection
{
    public static partial class ReflectionUtilities
    {
        public static dynamic ToDynamic<T>(this T value)
        {
            return new DynamicWrapper<T>(ref value);
        }
    }
}