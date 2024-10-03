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
using Unity.Service;

namespace Unity.Service
{
    public partial class UsersInGroupRoleService : BaseService<UsersInGroupRole>, IUsersInGroupRoleService
    {
        public UsersInGroupRoleService(IRepository<UsersInGroupRole> repository) : base(repository) { }


    }

    public partial class UsersInGroupRoleService : IUsersInGroupRoleService
    {
        public async Task<long> Insert(Guid createdBy, Guid userId, List<Guid> groupRoleIds)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] { new DataColumn("id", typeof(Guid)) });

                if (groupRoleIds != null)
                {
                    foreach (var userType in groupRoleIds)
                    {
                        dt.Rows.Add(userType);
                    }
                }

                ParamItem[] arr = new ParamItem[] {
                                new ParamItem("ListGroup", SqlDbType.Structured, dt, "list_guid_table"),
                                new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                                new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),
                };

                return await Task.FromResult(ExecuteSql("pro_UsersInGroupRole_Insert", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UsersInGroupRoleService at Insert() Method", ex.Message);
            }

            return -1;
        }


        public async Task<long> RemoveAllGroupsByUserId(Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] { new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId) };

                return await Task.FromResult(ExecuteSql("pro_UsersInGroupRole_RemoveAllUserGroupsByUserId", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UsersInGroupRoleService at RemoveAllGroupsByUserId() Method", ex.Message);
            }

            return -1;
        }

        public async Task<IEnumerable<UsersInGroupRole>> FindUsersByGroupId(Guid groupId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] { new ParamItem("GroupId", SqlDbType.UniqueIdentifier, groupId) };

                return await Task.FromResult(SqlQuery("pro_UsersInGroupRole_FindUsersByGroupId", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UsersInGroupRoleService at FindUsersByGroupId() Method", ex.Message);
            }

            return Enumerable.Empty<UsersInGroupRole>();
        }



    }
}
