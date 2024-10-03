using Unity.Common.Configuration;
using Microsoft.AspNet.Identity;
using System;

namespace Unity.Core.Model
{
    public class GroupRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public int TotalRows { get; set; }
        public long RowNum { get; set; }

    }
}
