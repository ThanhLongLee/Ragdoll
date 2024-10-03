using Unity.Common.Configuration;
using System;

namespace Unity.Core.Model
{
    public class AppUserProfile
    {
        public AppUserProfile Clone()
        {
            return (AppUserProfile)base.MemberwiseClone();
        }

        public string FullName { get; set; }
        public AccountType UserType { get; set; }
        public AccountRole UserRole { get; set; }
        public AccountStatus Status { get; set; }
        public Guid CreatedBy { get; set; }

        public string Note { get; set; }
        public bool IsNew { get; set; }


    }
}
