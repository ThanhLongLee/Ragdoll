using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Interface;
using Unity.Core.Model;
using Unity.Common.Configuration;
using Unity.Common.Extensions;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service;
using Unity.Core.Model;

namespace Unity.Service
{
    public partial class GroupRoleService : BaseService<GroupRole>, IGroupRoleService
    {
        public GroupRoleService(IRepository<GroupRole> repository) : base(repository) { }

    }

    public partial class GroupRoleService : IGroupRoleService
    {

        public async Task<long> Insert(Guid createdBy, GroupRole model)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] { new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                                                    new ParamItem("Id", SqlDbType.UniqueIdentifier, model.Id),
                                                    new ParamItem("Name", SqlDbType.NVarChar, model.Name),
                                                    new ParamItem("Description", SqlDbType.NVarChar, model.Description ?? ""),
                                                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(ExecuteSql("pro_GroupRole_Insert", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in GroupRoleService at Insert() Method", ex.Message);
            }

            return -1;
        }

        public async Task<long> Update(Guid createdBy, GroupRole model)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] { new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                                                    new ParamItem("Id", SqlDbType.UniqueIdentifier, model.Id),
                                                    new ParamItem("Name", SqlDbType.NVarChar, model.Name),
                                                    new ParamItem("Description", SqlDbType.NVarChar, model.Description ?? ""),
                                                    new ParamItem("Status", SqlDbType.TinyInt, model.Status),
                                                    new ParamItem("ModifiedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(ExecuteSql("pro_GroupRole_Update", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in GroupRoleService at Update() Method", ex.Message);
            }

            return -1;
        }

        public async Task<GroupRole> Verify(string name)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] {  new ParamItem("Name", SqlDbType.NVarChar, name ?? "") };

                return await Task.FromResult(SqlQuery("pro_GroupRole_Verify", Params.Create(arr)).FirstOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in GroupRoleService at Verify() Method", ex.Message);
            }

            return null;
        }

        public async Task<GroupRole> FindById(Guid id)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] { new ParamItem("Id", SqlDbType.UniqueIdentifier, id) };

                return await Task.FromResult(SqlQuery("pro_GroupRole_FindById", Params.Create(arr)).FirstOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in GroupRoleService at FindById() Method", ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<GroupRole>> FindGroupsByUserId(Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] { new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId) };

                return await Task.FromResult(SqlQuery("pro_GroupRole_FindGroupsByUserId", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in GroupRoleService at FindGroupsByUserId() Method", ex.Message);
            }

            return Enumerable.Empty<GroupRole>();
        }


        public async Task<IEnumerable<GroupRole>> FindBy(string keyword, Status status, int beginRow, int numRows)
        {

            try
            {
                ParamItem[] arr = new ParamItem[] {
                        new ParamItem("Keyword", SqlDbType.NVarChar,  keyword.KeywordContains()),
                        new ParamItem("Status", SqlDbType.TinyInt, status),
                        new ParamItem("BeginRow", SqlDbType.Int, beginRow),
                        new ParamItem("NumRows", SqlDbType.Int, numRows),
                };

                return await Task.FromResult(SqlQuery("pro_GroupRole_FindBy", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in GroupRoleService at FindBy() Method", ex.Message);
            }

            return Enumerable.Empty<GroupRole>();
        }


    }
}
