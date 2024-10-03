using Microsoft.AspNet.Identity;
using System;

namespace Unity.Core.Model
{
    public partial class RolesInGroupRole
    {
        public Guid GroupRoleId { get; set; }
        public Guid RoleId { get; set; }

        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }


    public partial class RolesInGroupRole
    {
        public string RoleName { get; set; }
    }
}
