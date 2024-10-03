using Unity.Core.Interface;
using Unity.Core.Model;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Unity.Service
{
    public partial class AppUserLoginService : BaseService<AppUserLogin>, IAppUserLoginService
    {   
        public AppUserLoginService(IRepository<AppUserLogin> repository) : base(repository) { }
    }

    public partial class AppUserLoginService : IAppUserLoginService
    {
        public async Task<long> AddLogin(string loginProvider, string providerKey, Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("LoginProvider", SqlDbType.NVarChar, loginProvider),
                    new ParamItem("ProviderKey", SqlDbType.NVarChar, providerKey),
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                };

                return await Task.FromResult(base.ExecuteSql("pro_UserLogin_AddLogin", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserLoginService at AddLogin() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> RemoveLogin(string loginProvider, string providerKey, Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("LoginProvider", SqlDbType.NVarChar, loginProvider),
                    new ParamItem("ProviderKey", SqlDbType.NVarChar, providerKey),
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                };

                return await Task.FromResult(base.ExecuteSql("pro_UserLogin_RemoveLogin", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserLoginService at RemoveLogin() Method", ex.Message);
            }
            return -1;
        }

        public async Task<Guid> Find(string loginProvider, string providerKey)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("LoginProvider", SqlDbType.NVarChar, loginProvider),
                    new ParamItem("ProviderKey", SqlDbType.NVarChar, providerKey),
                };

                var result = await Task.FromResult(base.SqlQuery("pro_UserLogin_Find", Params.Create(arr)).FirstOrDefault());
                if (result != null)
                {
                    return result.UserId;
                }
                return Guid.Empty;
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserLoginService at Find() Method", ex.Message);
            }
            return Guid.Empty;
        }

        public async Task<IList<AppUserLogin>> GetLogins(Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                };

                return await Task.FromResult(base.SqlQuery("pro_UserLogin_GetLogins", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserLoginService at GetLogins() Method", ex.Message);
            }
            return null;
        }
    }
}
