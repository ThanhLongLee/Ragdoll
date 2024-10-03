using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public partial class AppUser : IUser<Guid>
    {
        public Guid UserId
        {
            get
            {
                return Id;
            }
        }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset LockoutEndDateUtc{get; set;}
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public int TotalRows { get; set; }
        public DateTime LastDateLogin { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser, Guid> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("UserName", UserName));
            userIdentity.AddClaim(new Claim("FullName", FullName));
            userIdentity.AddClaim(new Claim("UserType", ((int)UserType).ToString()));
            userIdentity.AddClaim(new Claim("UserRole", ((int)UserRole).ToString()));

            return userIdentity;
        }
    }


    public partial class AppUser : AppUserProfile
    {
        public AppUser Clone()
        {
            return (AppUser)base.MemberwiseClone();
        }
    }

}
