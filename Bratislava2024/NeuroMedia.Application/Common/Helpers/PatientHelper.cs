using System.Security.Claims;

using NeuroMedia.Domain.Entities;

namespace NeuroMedia.Application.Common.Helpers
{
    public static class PatientHelper
    {
        public static Patient AutoUpdateEntries(Patient patient, ClaimsPrincipal claims)
        {
            BaseAuditableEntityHelper.UpdateAuditableEntries(patient, claims);

            if (string.IsNullOrEmpty(patient.UserId))
            {
                patient.UserId = claims.GetUserId();
            }

            if (string.IsNullOrEmpty(patient.TenantId))
            {
                patient.TenantId = claims.GetTenantId();
            }

            if (string.IsNullOrEmpty(patient.Pseudonym))
            {
                patient.Pseudonym = $"{Guid.NewGuid()}";
            }

            return patient;
        }
    }
}
