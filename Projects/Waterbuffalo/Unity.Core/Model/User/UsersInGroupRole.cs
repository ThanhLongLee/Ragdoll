using Microsoft.AspNet.Identity;
using System;

namespace Unity.Core.Model
{
    public class UsersInGroupRole
    {
        public Guid GroupRoleId { get; set; }
        public Guid UserId { get; set; }

        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
