using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Common.Helpers
{
    public static class VisitHelper
    {
        public static Visit AutoUpdateEntries(Visit visit, ClaimsPrincipal claims)
        {
            BaseAuditableEntityHelper.UpdateAuditableEntries(visit, claims);
            return visit;
        }
    }
}
