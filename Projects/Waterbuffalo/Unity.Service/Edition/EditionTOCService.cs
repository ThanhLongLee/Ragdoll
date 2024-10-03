using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Common.Configuration;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service;
using Unity.Core.Model;
using Unity.Core.Model.Edition;

namespace Unity.Service
{
    public partial class EditionTOCService : BaseService<EditionTOC>, IEditionTOCService
    {
        public EditionTOCService(IRepository<EditionTOC> repository) : base(repository) { }

    }

    public partial class EditionTOCService : IEditionTOCService
    {
        public async Task<long> Insert(Guid createdBy, EditionTOC editionTOC)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                    new ParamItem("EditionId", SqlDbType.Int, editionTOC.EditionId),
                    new ParamItem("Title", SqlDbType.NVarChar, editionTOC.Title),
                    new ParamItem("PostTitle", SqlDbType.NVarChar, editionTOC.PostTitle),
                    new ParamItem("PostContent", SqlDbType.NText, editionTOC.PostContent),
                    new ParamItem("IsShowOnHeader", SqlDbType.Bit, editionTOC.IsShowOnHeader),
                    new ParamItem("IsShowOnSummaryList", SqlDbType.Bit, editionTOC.IsShowOnSummaryList),
                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),

                };

                return await Task.FromResult(base.ExecuteSql("pro_EditionTOC_Insert", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionTOCService at Insert() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> Update(Guid createdBy, EditionTOC editionTOC)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                    new ParamItem("Id", SqlDbType.Int, editionTOC.Id),
                    new ParamItem("Title", SqlDbType.NVarChar, editionTOC.Title),
                    new ParamItem("PostTitle", SqlDbType.NVarChar, editionTOC.PostTitle),
                    new ParamItem("PostContent", SqlDbType.NText, editionTOC.PostContent),
                    new ParamItem("IsShowOnHeader", SqlDbType.Bit, editionTOC.IsShowOnHeader),
                    new ParamItem("IsShowOnSummaryList", SqlDbType.Bit, editionTOC.IsShowOnSummaryList),
                    new ParamItem("ModifiedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(base.ExecuteSql("pro_EditionTOC_Update", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionTOCService at Update() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> UpdateIndex(Guid createdBy, List<SortEditionTOCInexModel> sortingModel)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Id", typeof(Int32)), new DataColumn("Index", typeof(Int32)), });

                foreach (var item in sortingModel)
                {
                    dt.Rows.Add(item.Id, item.Index);
                }

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                    new ParamItem("SortTable", SqlDbType.Structured, dt, "list_sort_index"),
                    new ParamItem("ModifiedDate", SqlDbType.DateTime, DateTime.Now),

                };

                return await Task.FromResult(base.ExecuteSql("pro_EditionTOC_UpdateIndex", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionTOCService at UpdateIndex() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> Delete(Guid createdBy, int editionTOCId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                    new ParamItem("Id", SqlDbType.Int, editionTOCId),
                    new ParamItem("ModifiedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(base.ExecuteSql("pro_EditionTOC_Delete", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionTOCService at Delete() Method", ex.Message);
            }
            return -1;
        }

        public async Task<EditionTOC> FindById(int editionTOCId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("Id", SqlDbType.Int, editionTOCId),
                };

                return await Task.FromResult(base.SqlQuery("pro_EditionTOC_FindById", Params.Create(arr)).FirstOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionTOCService at FindById() Method", ex.Message);
            }
            return null;
        }

        public async Task<IEnumerable<EditionTOC>> FindByEdition(int editionId, Status status)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("EditionId", SqlDbType.Int, editionId),
                    new ParamItem("Status", SqlDbType.TinyInt, (int)status),
                };
                return await Task.FromResult(base.SqlQuery("pro_EditionTOC_FindByEdition", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionTOCService at FindByEdition() Method", ex.Message);
            }

            return Enumerable.Empty<EditionTOC>();
        }



    }
}
