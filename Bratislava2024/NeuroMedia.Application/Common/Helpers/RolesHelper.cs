using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuroMedia.Domain.Enums;

namespace NeuroMedia.Application.Common.Helpers
{
    public static class RolesHelper
    {
        public static Roles GetEnumRoles(string? roles)
        {
            if (string.IsNullOrEmpty(roles))
            {
                return Roles.None;
            }

            var enumRoles = Roles.None;
            var rolesArr = roles.Split(',');

            foreach (var role in rolesArr)
            {
                if (Enum.TryParse<Roles>(role, true, out var enumRole))
                {
                    enumRoles |= enumRole;
                }
            }

            return enumRoles;
        }
    }
}
