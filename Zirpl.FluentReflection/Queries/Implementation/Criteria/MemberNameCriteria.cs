using System;
using System.Collections.Generic;
using System.Linq;

namespace Zirpl.FluentReflection.Queries.Criteria
{
    internal sealed class MemberNameCriteria : NameCriteria	
    {
        protected internal override bool ShouldRun
        {
            get
            { 
                // at the moment, member names are ALWAYS handled by the service, so we can ignore things here
                // UNLESS it is one of these nifty guys
                return Names != null && NameHandling != NameHandlingType.Whole;
            }
        }

        internal IEnumerable<String> GetNamesForDirectLookup()
        {
            if (Names != null
                && NameHandling == NameHandlingType.Whole)
            {
                return Names.ToArray();
            }
            return null;
        }
    }
}
