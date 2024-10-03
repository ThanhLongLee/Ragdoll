using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service.Invite
{
    public partial interface IInviteService: IBaseService<InviteDetails>
    {
        Task<long> HandleInvitesAsync(long senderId, InviteDetails invites);
    }
}
