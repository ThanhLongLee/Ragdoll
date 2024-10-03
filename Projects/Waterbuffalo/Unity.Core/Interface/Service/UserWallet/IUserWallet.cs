using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service.UserWallet
{
    public partial interface IUserWallet: IBaseService<UserWalletConnection>
    {
        Task<long> Connect(long userId, string walletAddress, string exchangeName);
        Task<long> Disconnect(long userId);
        Task<long> GetStatus(long userId);
    }
}
