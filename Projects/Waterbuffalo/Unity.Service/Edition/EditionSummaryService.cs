using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Configuration;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service;
using Unity.Core.Model;
using Unity.Core.Model.Edition;

namespace Unity.Service
{
    public partial class EditionSummaryService : BaseService<EditionSummary>, IEditionSummaryService
    {
        public EditionSummaryService(IRepository<EditionSummary> repository) : base(repository) { }

    }

    public partial class EditionSummaryService : IEditionSummaryService
    {

        public async Task<long> Insert(Guid createdBy, EditionSummary editionSummary)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] {
                    new ParamItem("EditionTOCId", SqlDbType.UniqueIdentifier, editionSummary.EditionTOCId),
                    new ParamItem("ThumbnailImg", SqlDbType.UniqueIdentifier, editionSummary.ThumbnailImg),
                    new ParamItem("Title", SqlDbType.UniqueIdentifier, editionSummary.Title),
                    new ParamItem("Description", SqlDbType.UniqueIdentifier, editionSummary.Description),
                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(base.ExecuteSql("pro_EditionSummary_Insert", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionSummaryService at Insert() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> Update(Guid createdBy, EditionSummary editionSummary)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] {
                    new ParamItem("EditionTOCId", SqlDbType.UniqueIdentifier, editionSummary.EditionTOCId),
                    new ParamItem("ThumbnailImg", SqlDbType.UniqueIdentifier, editionSummary.ThumbnailImg),
                    new ParamItem("Title", SqlDbType.UniqueIdentifier, editionSummary.Title),
                    new ParamItem("Description", SqlDbType.UniqueIdentifier, editionSummary.Description),
                    new ParamItem("ModifiedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(base.ExecuteSql("pro_EditionSummary_Update", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionSummaryService at Update() Method", ex.Message);
            }
            return -1;
        }

        public async Task<EditionSummary> FindByEditionTOC(int editionTOCId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("EditionTOCId", SqlDbType.Int, editionTOCId),
                };

                return await Task.FromResult(base.SqlQuery("pro_EditionSummary_FindByEditionTOC", Params.Create(arr)).FirstOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionSummaryService at FindByEditionTOC() Method", ex.Message);
            }
            return null;
        }

        public async Task<IEnumerable<EditionSummary>> FindByEdition(int editionId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("EditionId", SqlDbType.Int, editionId),
                };

                return await Task.FromResult(base.SqlQuery("pro_EditionSummary_FindByEdition", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionSummaryService at FindByEdition() Method", ex.Message);
            }

            return Enumerable.Empty<EditionSummary>();
        }

    }
}
