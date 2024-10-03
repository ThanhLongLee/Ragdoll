using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service.UserActions
{
    public partial interface IUserLinkService : IBaseService<UserLink>
    {       
        Task<long> UpdateUserScore(long userId, int linkId);
    }
}
