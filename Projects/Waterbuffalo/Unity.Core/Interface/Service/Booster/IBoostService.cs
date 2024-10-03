using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service.Booster
{
    public partial interface IBoostService : IBaseService<Boost>
    {
        Task<List<Boost>> GetBoost(long userId);
    }
}
