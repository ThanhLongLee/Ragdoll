using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service.Invite;
using Unity.Core.Model;

namespace Unity.Service.Invite
{
    public partial class InviteService : BaseService<InviteDetails>, IInviteService
    {
        public InviteService(IRepository<InviteDetails> repository) : base(repository) { }

        public async Task<long> HandleInvitesAsync(long senderId, InviteDetails invite)
        {
            try
            {

                ParamItem[] arr = new ParamItem[]
            {
                    new ParamItem("SenderID", SqlDbType.BigInt,  senderId),
                    new ParamItem("ReceiverID", SqlDbType.BigInt,  invite.ReceiverID),
                    new ParamItem("ReceiverFirstname", SqlDbType.NVarChar,  invite.ReceiverFirstname ?? string.Empty),
                    new ParamItem("ReceiverLastname", SqlDbType.NVarChar,  invite.ReceiverLastname ?? string.Empty),
                    new ParamItem("ReceiverUsername", SqlDbType.NVarChar,  invite.ReceiverUsername ?? string.Empty),
            };
                return await Task.FromResult(ExecuteSql("sp_HandleInvite", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error get user links", ex.Message);
            }
            return -1;
        }
    }
}
