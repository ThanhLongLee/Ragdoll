using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service.Link;
using Unity.Core.Model;

namespace Unity.Service.Link
{
    public partial class LinkService: BaseService<Links>, ILinkService
    {
        public LinkService(IRepository<Links> repository) : base(repository) { }

        public async Task<List<Links>> GetUserLinks(long userId)
        {
            try
            {

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserID", SqlDbType.Decimal,  userId),
                };
                return await Task.FromResult(base.SqlQuery("sp_GetUserLinks", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error get user links", ex.Message);
            }
            return Enumerable.Empty<Links>().ToList();
        }
    }
}
