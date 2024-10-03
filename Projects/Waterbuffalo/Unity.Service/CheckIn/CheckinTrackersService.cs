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

namespace Unity.Service.CheckIn
{
    public partial class CheckinTrackersService : BaseService<CheckinTrackers>, ICheckinTrackersService
    {
        public CheckinTrackersService(IRepository<CheckinTrackers> repository) : base(repository) { }

        public async Task<CheckinTrackers> GetCheckinTrackers(long userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserID", SqlDbType.BigInt, userId)
                };
                return await Task.FromResult(base.SqlQuery("pro_UserCheckin", Params.Create(arr)).Single());
            }
            catch (Exception ex)
            {
                base.WriteError("Lỗi khi điểm danh", ex.Message);
            }
            return null;
        }
    }
}
