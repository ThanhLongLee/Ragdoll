using System;

namespace Unity.Core.Model
{
    public class AppUserRole
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public string RoleName { get; set; }

    }
}
