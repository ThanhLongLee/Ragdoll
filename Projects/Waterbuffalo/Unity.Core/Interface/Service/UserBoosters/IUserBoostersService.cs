using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service.UserBoosters
{
    public partial interface IUserBoostersService : IBaseService<UserBoost>
    {
        Task<long> HandleBoost(long userId, int linkId);
    }
}
