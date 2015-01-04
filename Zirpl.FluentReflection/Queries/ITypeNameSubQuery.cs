namespace Zirpl.FluentReflection.Queries
{
    public interface ITypeNameSubQuery<out TResult, out TReturnQuery> : INameSubQuery<TResult, TReturnQuery>
    {
        INameSubQuery<TResult, TReturnQuery> Full();
    }
}
