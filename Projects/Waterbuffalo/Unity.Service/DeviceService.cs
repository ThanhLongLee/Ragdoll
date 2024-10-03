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

namespace Unity.Service
{
    public partial class DeviceService : BaseService<Device>, IDeviceService
    {
        public DeviceService(IRepository<Device> repository) : base(repository) { }
    }

    public partial class DeviceService : IDeviceService
    {
        public async Task<long> LoginHistory(Guid userId, string iPAddress)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] { new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                                                    new ParamItem("IPAddress", SqlDbType.VarChar, iPAddress),
                                                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(ExecuteSql("pro_LoginHistory_Insert", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in DeviceService at LoginHistory() Method", ex.Message);
            }

            return -1;
        }

    }
}
