using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service;
using Unity.Core.Interface.Service.UserActions;
using Unity.Core.Model;

namespace Unity.Service.UserActionService
{
    public partial class UserLinkService: BaseService<UserLink>, IUserLinkService
    {
        public UserLinkService(IRepository<UserLink> repository) : base(repository) { }

        public async Task<long> UpdateUserScore(long userId, int linkId)
        {
            try
            {

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserID", SqlDbType.BigInt,  userId),
                    new ParamItem("LinkID", SqlDbType.Int,  linkId),
                };
                var result = await Task.FromResult(ExecuteSql("sp_UpdateUserScore", Params.Create(arr)));            
            }
            catch (Exception ex)
            {
                base.WriteError("Error get user links", ex.Message);
            }
            return 1;
        }
    }
}
