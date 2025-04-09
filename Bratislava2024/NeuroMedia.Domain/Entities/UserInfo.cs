using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

using NeuroMedia.Domain.Common;

namespace NeuroMedia.Domain.Entities
{
    [Index(nameof(UserOid), IsUnique = true)]
    [Index(nameof(TenantId))]
    public class UserInfo : BaseAuditableEntity
    {
        public string UserOid { get; set; } = default!;
        public string TenantId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Institution { get; set; } = default!;
        public string Roles { get; set; } = default!;

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            var userInfo = obj as UserInfo;

            return userInfo!.UserOid == UserOid
                && userInfo.TenantId == TenantId
                && userInfo.Email == Email
                && userInfo.Name == Name
                && userInfo.Surname == Surname
                && userInfo.Roles == Roles
                && userInfo.Institution == Institution;
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }
    }
}
