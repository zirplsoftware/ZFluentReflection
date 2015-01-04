﻿using System.Reflection;

namespace Zirpl.FluentReflection.Queries
{
    public interface IMemberTypeSubQuery : IQueryResult<MemberInfo>
    {
        IMemberTypeSubQuery Constructor();
        IMemberTypeSubQuery Event();
        IMemberTypeSubQuery Field();
        IMemberTypeSubQuery Method();
        IMemberTypeSubQuery NestedType();
        IMemberTypeSubQuery Property();
        IMemberQuery All();
        IMemberQuery And();
    }
}