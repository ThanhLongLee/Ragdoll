using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Core.Model;
using Unity.Core.Interface.Service;

namespace Unity.Core.Interface
{
    public interface IRolesInGroupRoleService : IBaseService<RolesInGroupRole>
    {
        Task<long> Insert(Guid createdBy, Guid groupId, List<Guid> roleIds);

        Task<long> RemoveAllRolesInGroup(Guid groupId);

        Task<IEnumerable<RolesInGroupRole>> GetListRoleByGroupId(Guid groupId);



    }
}
