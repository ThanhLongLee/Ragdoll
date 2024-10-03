using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unity.Core.Model;
using Unity.Core.Interface;

namespace Web.Admin.Infrastructure.UserStore
{
    public class UserStore : IUserStore<AppUser, Guid>,
        IUserPasswordStore<AppUser, Guid>,
        IUserEmailStore<AppUser, Guid>,
        IUserSecurityStampStore<AppUser, Guid>,
        IUserRoleStore<AppUser, Guid>,
        IUserLockoutStore<AppUser, Guid>,
        IUserPhoneNumberStore<AppUser, Guid>,
        IUserTwoFactorStore<AppUser, Guid>,
        IUserLoginStore<AppUser, Guid>,
        IQueryableUserStore<AppUser, Guid>
    {

        public IAppUserService UserService
        {
            get
            {
                return DependencyResolver.Current.GetService<IAppUserService>();
            }
        }

        public UserStore()
        { 
        }

        public IAppUserLoginService UserLoginService
        {
            get { return DependencyResolver.Current.GetService<IAppUserLoginService>(); }
        }

        public IAppUserRoleService AccountRoleService
        {
            get { return DependencyResolver.Current.GetService<IAppUserRoleService>(); }
        }



        /* IUserStore
        ---------------------------*/

        public Task CreateAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            //ensure UserId
            if (user.Id == default(Guid))
                user.Id = Guid.NewGuid();

            //conver to sql min date
            var sqlMinDate = new DateTimeOffset(1753, 1, 1, 0, 0, 0, TimeSpan.FromHours(0));
            if (user.LockoutEndDateUtc < sqlMinDate)
                user.LockoutEndDateUtc = sqlMinDate;

            var result = Task.Run(() => UserService.Create(user)).Result;
            return Task.FromResult(result);
        }

        public Task DeleteAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var result = Task.Run(() => UserService.Delete(Guid.Empty, user.UserId)).Result;
            return Task.FromResult(result);

        }

        public Task<AppUser> FindByIdAsync(Guid userId)
        {
            var result = Task.Run(() => UserService.FindById(userId)).Result;
            return Task.FromResult(result);
        }

        public Task<AppUser> FindByNameAsync(string userName)
        {
            var result = Task.Run(() => UserService.FindByName(userName)).Result;
            return Task.FromResult(result);
        }

        public Task UpdateAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var result = Task.Run(() => UserService.Update(user)).Result;
            return Task.FromResult(result);
        }


        /* IUserPasswordStore
        ---------------------------*/

        public Task<string> GetPasswordHashAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(!String.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        /* IUserEmailStore
        ---------------------------*/

        public Task<AppUser> FindByEmailAsync(string email)
        {
            var result = Task.Run(() => UserService.FindByEmail(email)).Result;
            return Task.FromResult(result);

        }

        public Task<string> GetEmailAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailAsync(AppUser user, string email)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.Email = email;

            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(AppUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.EmailConfirmed = confirmed;

            return Task.FromResult(0);
        }

        /* IUserSecurityStampStore
        ---------------------------*/

        public Task<string> GetSecurityStampAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(AppUser user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        /* IAccountRoleStore
        ---------------------------*/

        public Task AddToRoleAsync(AppUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var result = Task.Run(() => AccountRoleService.AddToRole(user.UserId, roleName)).Result;
            return Task.FromResult(result);


        }

        public Task<IList<string>> GetRolesAsync(AppUser user)
        {
            var result = Task.Run(() => AccountRoleService.GetRoles(user.UserId)).Result;
            return Task.FromResult(result);


        }

        public async Task<bool> IsInRoleAsync(AppUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            var result = await GetRolesAsync(user);

            if (result == null || result.Count == 0)
                return false;

            return result.Contains<string>(roleName);
        }

        public Task RemoveFromRoleAsync(AppUser user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (String.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            var result = Task.Run(() => AccountRoleService.RemoveFromRole(user.UserId, roleName)).Result;
            return Task.FromResult(result);

        }

        /* IUserLockoutStore
        ---------------------------*/

        public Task<int> GetAccessFailedCountAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.LockoutEndDateUtc);
        }

        public Task<int> IncrementAccessFailedCountAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount++;

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.AccessFailedCount = 0;

            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(AppUser user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.LockoutEnabled = enabled;

            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            var sqlMinDate = new DateTimeOffset(1753, 1, 1, 0, 0, 0, TimeSpan.FromHours(0));

            if (lockoutEnd < sqlMinDate)
            {
                lockoutEnd = sqlMinDate;
            }

            user.LockoutEndDateUtc = lockoutEnd;

            return Task.FromResult(0);
        }

        /* IUserPhoneNumberStore
        ---------------------------*/

        public Task<string> GetPhoneNumberAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(AppUser user, string phoneNumber)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PhoneNumber = phoneNumber;

            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(AppUser user, bool confirmed)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PhoneNumberConfirmed = confirmed;

            return Task.FromResult(0);
        }

        /* IUserTwoFactorStore
        ---------------------------*/

        public Task<bool> GetTwoFactorEnabledAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.TwoFactorEnabled = enabled;

            return Task.FromResult(0);
        }

        /* IUserLoginStore
        ---------------------------*/

        public Task AddLoginAsync(AppUser user, UserLoginInfo login)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (login == null)
                throw new ArgumentNullException("login");

            var result = Task.Run(() => UserLoginService.AddLogin(login.LoginProvider, login.ProviderKey, user.UserId)).Result;
            return Task.FromResult(result);

        }

        public Task<AppUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");
            var userId = default(Guid);

            userId = Task.Run(() => UserLoginService.Find(login.LoginProvider, login.ProviderKey)).Result;

            //return user
            if (userId != default(Guid))
                return FindByIdAsync(userId);

            //null user
            return Task.FromResult<AppUser>(null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            IList<UserLoginInfo> userLogins = new List<UserLoginInfo>();

            var results = Task.Run(() => UserLoginService.GetLogins(user.UserId)).Result;
            if (results != null)
            {
                userLogins = results.Select(s => new UserLoginInfo(s.LoginProvider, s.ProviderKey)).ToList();
            }
            return Task.FromResult(userLogins);

        }

        public Task RemoveLoginAsync(AppUser user, UserLoginInfo login)
        {
            if (login == null)
                throw new ArgumentNullException("login");

            var result = Task.Run(() => UserLoginService.RemoveLogin(login.LoginProvider, login.ProviderKey, user.UserId)).Result;
            return Task.FromResult(result);


        }

        public void Dispose()
        {
        }

        /* IQueryableUserStore
        ---------------------------*/

        public IQueryable<AppUser> Users
        {
            get
            {
                var users = Task.Run(() => UserService.GetAll()).Result;
                var lisApplicationUser = new List<AppUser>();
                foreach (var item in users)
                {
                    lisApplicationUser.Add(item);
                }

                return Task.Run(() => lisApplicationUser).Result.AsQueryable();

            }
        }
    }

}
