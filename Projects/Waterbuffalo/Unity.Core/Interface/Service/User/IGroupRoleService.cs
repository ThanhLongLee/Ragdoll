using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;
using Unity.Common.Configuration;
using Unity.Core.Interface.Service;
namespace Unity.Core.Interface
{
    public interface IGroupRoleService : IBaseService<GroupRole>
    {
        Task<long> Insert(Guid createdBy, GroupRole model);

        Task<long> Update(Guid createdBy, GroupRole model);

        Task<GroupRole> Verify(string name);

        Task<IEnumerable<GroupRole>> FindGroupsByUserId(Guid userId);

        Task<GroupRole> FindById(Guid id);

        Task<IEnumerable<GroupRole>> FindBy(string keyword, Status status, int beginRow, int numRows);


    }
}
