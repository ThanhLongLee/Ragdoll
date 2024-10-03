using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Core.Interface;
using Unity.Core.Model;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service;
using Unity.Core.Model;
using Unity.Service;

namespace Unity.Service
{
    public partial class RolesInGroupRoleService : BaseService<RolesInGroupRole>, IRolesInGroupRoleService
    {
        public RolesInGroupRoleService(IRepository<RolesInGroupRole> repository) : base(repository) { }
    }

    public partial class RolesInGroupRoleService : IRolesInGroupRoleService
    {
        public async Task<long> Insert(Guid createdBy, Guid groupId, List<Guid> roleIds)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] { new DataColumn("id", typeof(Guid)) });

                if (roleIds != null)
                {
                    foreach (var userType in roleIds)
                    {
                        dt.Rows.Add(userType);
                    }
                }

                ParamItem[] arr = new ParamItem[] {
                                new ParamItem("ListRole", SqlDbType.Structured, dt, "list_guid_table"),
                                new ParamItem("GroupId", SqlDbType.UniqueIdentifier, groupId),
                                new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(ExecuteSql("pro_RolesInGroupRole_Insert", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RolesInGroupRoleService at Insert() Method", ex.Message);
            }

            return -1;
        }

        public async Task<long> RemoveAllRolesInGroup(Guid groupId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] { new ParamItem("GroupId", SqlDbType.UniqueIdentifier, groupId) };

                return await Task.FromResult(ExecuteSql("pro_RolesInGroupRole_RemoveAllRolesInGroup", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RolesInGroupRoleService at RemoveAllRolesInGroup() Method", ex.Message);
            }

            return -1;
        }


        public async Task<IEnumerable<RolesInGroupRole>> GetListRoleByGroupId(Guid groupId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] { new ParamItem("GroupId", SqlDbType.UniqueIdentifier, groupId) };

                return await Task.FromResult(SqlQuery("pro_RolesInGroupRole_GetListRoleByGroupId", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RolesInGroupRoleService at GetListRoleByGroupId() Method", ex.Message);
            }

            return Enumerable.Empty<RolesInGroupRole>();
        }

        
    }
}
