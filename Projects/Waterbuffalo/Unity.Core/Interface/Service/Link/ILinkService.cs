using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Model;

namespace Unity.Core.Interface.Service.Link
{
    public partial interface ILinkService: IBaseService<Links>
    {
        Task<List<Links>> GetUserLinks(long userId);
    }
}
