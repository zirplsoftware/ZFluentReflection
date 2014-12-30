﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.FluentReflection
{
    internal sealed class MemberNameCriteria : NameCriteria	
    {
        protected override bool ShouldRunFilter
        {
            get
            { // at the moment, member names are ALWAYS handled by the service, so we can ignore things here
                // UNLESS it is one of these nifty guys
                return StartsWith || EndsWith || Contains;
            }
        }

        internal IEnumerable<String> GetNamesForDirectLookup()
        {
            if ((Name != null
                 || Names != null)
                && !StartsWith
                && !EndsWith
                && !Contains)
            {
                return Names ?? new[] {Name};
            }
            return null;
        }
    }
}
