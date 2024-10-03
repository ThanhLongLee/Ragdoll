using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Core.Model;
using Unity.Core.Interface.Service;

namespace Unity.Core.Interface
{
    public interface IAppRoleService : IBaseService<AppRole>
    {
        Task<long> Create(AppRole model);

        Task<long> Delete(Guid roleId);

        Task<long> Update(AppRole model);

        Task<AppRole> FindById(Guid roleId);

        Task<AppRole> FindByName(string roleName);

        Task<List<AppRole>> GetAll();

        Task<List<AppRole>> GetAllWithParent();

        Task<List<AppRole>> GetAllParentRole();

        Task<List<AppRole>> FindByParentId(Guid parentId);

        
    }
}
