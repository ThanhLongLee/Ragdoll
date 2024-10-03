using Microsoft.AspNet.Identity;
using System;

namespace Unity.Core.Model
{
    public class AppRole : IRole<Guid>
    {
        public Guid RoleId
        {
            get
            {
                return Id;
            }
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ParentName { get; set; }
        public Guid ParentId { get; set; }

    }
}
