using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service
{
    public partial interface IUserCheckinsService: IBaseService<UserCheckins>
    {
        Task<bool> UserCheckin(long userId);
        Task<CheckinStatus> GetCheckinStatus(long userId);

        Task<List<UserCheckins>> GetCheckinRecords(long userId);
    }
}
