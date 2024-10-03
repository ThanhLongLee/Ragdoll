using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Core.Model;
using Unity.Common.Configuration;
using Unity.Core.Interface.Service;

namespace Unity.Core.Interface
{
    public interface IAppUserService : IBaseService<AppUser>
    {
        Task<long> Create(AppUser model);

        Task<long> Delete(Guid createdBy, Guid userId);

        Task<long> Update(AppUser model);

        Task<long> UpdateStatus(Guid createdBy, Guid userId, AccountStatus status);

        Task<long> UpdateSecurityStamp(Guid userId, string newSecurityStamp);

        Task<AppUser> FindById(Guid userId);

        Task<AppUser> FindByName(string userName);

        Task<AppUser> FindByEmail(string email);

        Task<List<AppUser>> GetAll();

        Task<IEnumerable<AppUser>> FindBy(string keyword, List<AccountRole> userRoles, AccountStatus status, DateTime startDate, DateTime endDate, bool sortDesc, bool sortByName, bool sortByDate, int beginRow, int numRows);

        Task<long> RemoveAllFromGroup(Guid userId);

        Task<long> RemoveAllFromRole(Guid userId);

    }
}
