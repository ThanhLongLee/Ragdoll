using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Core.Model;
using Unity.Core.Interface.Service;

namespace Unity.Core.Interface
{
    public interface IAppUserRoleService : IBaseService<AppUserRole>
    {
        Task<long> AddToRole(Guid userId, string roleName);

        Task<long> AddFromListRole(Guid userId, List<Guid> roleIds);

        Task<long> RemoveFromRole(Guid userId, string roleName);
        Task<IList<string>> GetRoles(Guid userId);


    }
}
