using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service.UserBoosters;
using Unity.Core.Interface.Service.UserWallet;
using Unity.Core.Model;

namespace Unity.Service.UserWallet
{
    public partial class UserWalletService: BaseService<UserWalletConnection>, IUserWallet
    {
        public UserWalletService(IRepository<UserWalletConnection> repository) : base(repository) { }

        public async Task<long> Connect(long userId, string walletAddress, string exchangeName)
        {
            try
            {

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.BigInt,  userId),
                    new ParamItem("WalletAddress", SqlDbType.NVarChar,  walletAddress ?? string.Empty),
                    new ParamItem("ExchangeName", SqlDbType.NVarChar,  exchangeName ?? string.Empty),
                };
                var result = await Task.FromResult(ExecuteSql("ConnectUserWallet", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error connect user wallet", ex.Message);
            }
            return 1;
        }

        public async Task<long> Disconnect(long userId)
        {
            try
            {

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.BigInt,  userId),              
                };
                var result = await Task.FromResult(ExecuteSql("DisconnectUserWallet", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error disconnect user wallet", ex.Message);
            }
            return 1;
        }

        public async Task<long> GetStatus(long userId)
        {
            try
            {

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.BigInt,  userId),
                };
                var result = await Task.FromResult(ExecuteSql("sp_GetUserWalletStatus", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error disconnect user wallet", ex.Message);
            }
            return 1;
        }
    }
}
