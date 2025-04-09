using System.Security.Claims;

using NeuroMedia.Domain.Common;

namespace NeuroMedia.Application.Common.Helpers
{
    public static class BaseAuditableEntityHelper
    {
        public static void UpdateAuditableEntries(BaseAuditableEntity auditableEntity, ClaimsPrincipal claims)
        {
            if (string.IsNullOrEmpty(auditableEntity.CreatedBy))
            {
                auditableEntity.CreatedBy = claims.GetUserId();
                auditableEntity.CreatedDate = DateTime.UtcNow;
            }

            auditableEntity.UpdatedBy = claims.GetUserId();
            auditableEntity.UpdatedDate = DateTime.UtcNow;
        }
    }
}
