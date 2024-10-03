using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;
using Unity.Common.Configuration;
using Unity.Core.Interface.Service;
using Unity.Core.Model;

namespace Unity.Core.Interface
{
    public interface IUsersInGroupRoleService : IBaseService<UsersInGroupRole>
    {
        Task<long> Insert(Guid createdBy, Guid userId, List<Guid> groupRoleIds);

        Task<long> RemoveAllGroupsByUserId(Guid userId);

        Task<IEnumerable<UsersInGroupRole>> FindUsersByGroupId(Guid groupId);



    }
}
