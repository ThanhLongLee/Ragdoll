using Unity.Core.Interface;
using Unity.Core.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Admin.Infrastructure.UserStore
{
    public class RoleStore: IRoleStore<AppRole, Guid>, IQueryableRoleStore<AppRole, Guid>
       
    {
        public RoleStore()
        {
            
        }


        public IAppRoleService RoleService
        {
            get
            {
                return DependencyResolver.Current.GetService<IAppRoleService>();
            }
        }

        public Task CreateAsync(AppRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            if (role.Id == default(Guid))
                role.Id = Guid.NewGuid();

            var result = Task.Run(() => RoleService.Create(role)).Result;
           return Task.FromResult(result);

        }

        public Task DeleteAsync(AppRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            return Task.Run(() => RoleService.Delete(role.Id));
        }

        public Task<AppRole> FindByIdAsync(Guid roleId)
        {
            var result = Task.Run(() => RoleService.FindById(roleId)).Result;
            return Task.FromResult(result);

        }

        public Task<AppRole> FindByNameAsync(string roleName)
        {
            if (String.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");
            var result = Task.Run(() => RoleService.FindByName(roleName)).Result;
            return Task.FromResult(result);


        }

        public Task UpdateAsync(AppRole role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            return Task.Run(() => RoleService.Update(role));
        }


        public void Dispose()
        {
        }
        /* IQueryableRoleStore
        ---------------------------*/

        public IQueryable<AppRole> Roles
        {
            get
            {
                var result = Task.Run(() => RoleService.GetAll()).Result;
                var listUser = new List<AppRole>();
                foreach (var item in result)
                {
                    listUser.Add(item);
                }

                return Task.Run(() => listUser).Result.AsQueryable();
            }
        }
    }
}
