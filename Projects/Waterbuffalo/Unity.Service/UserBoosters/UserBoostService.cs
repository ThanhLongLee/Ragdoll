using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service.UserBoosters;
using Unity.Core.Model;

namespace Unity.Service.UserBoosters
{
    public partial class UserBoostService: BaseService<UserBoost>, IUserBoostersService
    {
        public UserBoostService(IRepository<UserBoost> repository) : base(repository) { }

        public async Task<long> HandleBoost(long userId, int linkId)
        {
            try
            {

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserID", SqlDbType.BigInt,  userId),
                    new ParamItem("BoostId", SqlDbType.Int,  linkId),
                };
                var result = await Task.FromResult(ExecuteSql("sp_Boost", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error get user links", ex.Message);
            }
            return 1;
        }
    }
}
