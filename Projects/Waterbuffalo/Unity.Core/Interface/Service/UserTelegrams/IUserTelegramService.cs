using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service
{
    public interface IUserTelegramService: IBaseService<UserTelegrams>
    {
        Task<long> InsertUser(UserTelegrams user);        
    }
}
