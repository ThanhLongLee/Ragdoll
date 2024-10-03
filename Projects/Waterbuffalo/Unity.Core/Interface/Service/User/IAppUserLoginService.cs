using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Core.Model;
using Unity.Core.Interface.Service;

namespace Unity.Core.Interface
{
    public interface IAppUserLoginService : IBaseService<AppUserLogin>
    {
        Task<long> AddLogin(string loginProvider, string providerKey, Guid userId);

        Task<long> RemoveLogin(string loginProvider, string providerKey, Guid userId);

        Task<Guid> Find(string loginProvider, string providerKey);

        Task<IList<AppUserLogin>> GetLogins(Guid userId);

    }
}
