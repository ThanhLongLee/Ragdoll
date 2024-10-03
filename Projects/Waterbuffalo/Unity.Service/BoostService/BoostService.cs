using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Interface.Service;
using Unity.Core.Interface.Data;
using Unity.Core.Model;
using Unity.Core.Interface.Service.Booster;
using System.Data;
using Unity.Common.Parameter;


namespace Unity.Service.BoostService
{
    public partial class BoostService : BaseService<Boost>, IBoostService
    {
        public BoostService(IRepository<Boost> repository) : base(repository) { }

        public async Task<List<Boost>> GetBoost(long userId)
        {
            try
            {

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserID", SqlDbType.Decimal,  userId),
                };
                return await Task.FromResult(base.SqlQuery("sp_GetBoot", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error get user links", ex.Message);
            }
            return Enumerable.Empty<Boost>().ToList();
        }
    }
}
