using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    public enum MemberInfoAccessibility
    {
        Public = 1,
        FamilyOrAssembly = 2,
        Assembly = 3,
        Family = 4,
        Private = 5,
        Internal = 3,
        Protected = 4,
        ProtectedOrInternal = 2
    }
}
