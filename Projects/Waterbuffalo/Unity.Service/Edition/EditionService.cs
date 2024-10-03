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
using Unity.Common.Extensions;

namespace Unity.Service
{
    public partial class EditionService : BaseService<Edition>, IEditionService
    {
        public EditionService(IRepository<Edition> repository) : base(repository) { }

    }

    public partial class EditionService : IEditionService
    {

        public async Task<long> Insert(Guid createdBy, Edition edition)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                    new ParamItem("Title", SqlDbType.NVarChar, edition.Title),
                    new ParamItem("ThumbnailImg", SqlDbType.NVarChar, edition.ThumbnailImg),
                    new ParamItem("BannerImg", SqlDbType.NVarChar, edition.BannerImg),
                    new ParamItem("TitleOnBanner", SqlDbType.NVarChar, edition.TitleOnBanner),
                    new ParamItem("SubTitleOnBanner", SqlDbType.NVarChar, edition.SubTitleOnBanner),
                    new ParamItem("IntroduceThumbnail", SqlDbType.NVarChar, edition.IntroduceThumbnail),
                    new ParamItem("IntroduceContent", SqlDbType.NText, edition.IntroduceContent),
                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(base.ExecuteSql("pro_Edition_Insert", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionService at Insert() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> Update(Guid createdBy, Edition edition)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                    new ParamItem("Id", SqlDbType.Int, edition.Id),
                    new ParamItem("Title", SqlDbType.NVarChar, edition.Title),
                    new ParamItem("ThumbnailImg", SqlDbType.NVarChar, edition.ThumbnailImg),
                    new ParamItem("BannerImg", SqlDbType.NVarChar, edition.BannerImg),
                    new ParamItem("TitleOnBanner", SqlDbType.NVarChar, edition.TitleOnBanner),
                    new ParamItem("SubTitleOnBanner", SqlDbType.NVarChar, edition.SubTitleOnBanner),
                    new ParamItem("IntroduceThumbnail", SqlDbType.NVarChar, edition.IntroduceThumbnail),
                    new ParamItem("IntroduceContent", SqlDbType.NText, edition.IntroduceContent),
                    new ParamItem("ModifiedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(base.ExecuteSql("pro_Edition_Update", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionService at Update() Method", ex.Message);
            }
            return -1;
        }

        public async Task<Edition> FindById(int editionId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("Id", SqlDbType.Int, editionId),
                };

                return await Task.FromResult(base.SqlQuery("pro_Edition_FindById", Params.Create(arr)).FirstOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionService at FindById() Method", ex.Message);
            }
            return null;
        }

        public async Task<IEnumerable<Edition>> GetAll(EditionStatus status)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("Status", SqlDbType.TinyInt, (int)status),
                };
                return await Task.FromResult(base.SqlQuery("pro_Edition_GetAll", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionService at GetAll() Method", ex.Message);
            }

            return Enumerable.Empty<Edition>();
        }


        public async Task<IEnumerable<Edition>> FindBy(string keyword, EditionStatus status, int beginRow, int numRows)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("Keyword", SqlDbType.NVarChar,  keyword.KeywordContains()),
                    new ParamItem("Status", SqlDbType.TinyInt, (int)status),
                    new ParamItem("BeginRow", SqlDbType.Int, beginRow),
                    new ParamItem("NumRows", SqlDbType.Int, numRows),

                };
                return await Task.FromResult(base.SqlQuery("pro_Edition_FindBy", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in EditionService at FindBy() Method", ex.Message);
            }

            return Enumerable.Empty<Edition>();
        }

    }
}
