using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service
{
    public interface IUserDetailService: IBaseService<UserDetails>
    {
        Task<UserDetails> GetInfoUser(long id);
        Task<UserDetails> UpdateUserScore(long userId);

        Task<List<UserDetails>> GetTop10();
        Task<UserDetails> GetInfoRank(long id);
    }
}
