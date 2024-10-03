using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service;
using Unity.Core.Model;

namespace Unity.Service.Telegram
{
    public partial class UserTelegramService : BaseService<UserTelegrams>, IUserTelegramService
    {
        public UserTelegramService(IRepository<UserTelegrams> repository) : base(repository) { }
        public async Task<long> InsertUser(UserTelegrams user)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("Id",SqlDbType.BigInt, user.Id != 0 ? user.Id : 0),
                    new ParamItem("Firstname", SqlDbType.NVarChar, user.Firstname ?? string.Empty),
                    new ParamItem("Lastname", SqlDbType.NVarChar, user.Lastname ?? string.Empty),
                    new ParamItem("Username", SqlDbType.NVarChar, user.Username ?? string.Empty),
                    new ParamItem("Click", SqlDbType.Int, 1500),
                };
                var result = await Task.FromResult(ExecuteSql("pro_InsertOrUpdateUser", Params.Create(arr)));
                return result;
            }
            catch (Exception ex)
            {
                base.WriteError("Lỗi khi save user", ex.Message);
            }
            return -1;
        }
    }
}
