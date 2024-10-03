using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service
{
    public partial interface ICheckinTrackersService: IBaseService<CheckinTrackers>
    {
        Task<CheckinTrackers> GetCheckinTrackers(long userId);
    }
}
