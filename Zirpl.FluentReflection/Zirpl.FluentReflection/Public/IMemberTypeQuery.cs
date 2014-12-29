using System.Reflection;

namespace Zirpl.FluentReflection
{
    public interface IMemberTypeQuery : IQueryResult<MemberInfo>
    {
        IMemberTypeQuery Constructor();
        IMemberTypeQuery Event();
        IMemberTypeQuery Field();
        IMemberTypeQuery Method();
        IMemberTypeQuery NestedType();
        IMemberTypeQuery Property();
        IMemberQuery All();
        IMemberQuery And();
    }
}