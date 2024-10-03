using Unity.Core.Interface;
using Unity.Core.Model;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Unity.Service
{
    public partial class AppUserRoleService : BaseService<AppUserRole>, IAppUserRoleService
    {   
        public AppUserRoleService(IRepository<AppUserRole> repository) : base(repository) { }

       
    }

    public partial class AppUserRoleService : IAppUserRoleService
    {
        public async Task<long> AddToRole(Guid userId, string roleName)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                    new ParamItem("RoleName", SqlDbType.NVarChar, roleName),
                };

                return await Task.FromResult(base.ExecuteSql("pro_UserRole_AddToRole", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at AddToRole() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> AddFromListRole(Guid userId, List<Guid> roleIds)
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
                                new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                };


                return await Task.FromResult(base.ExecuteSql("pro_UserRole_AddFromListRole", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at AddFromListRole() Method", ex.Message);
            }
            return -1;
          

        }

        public async Task<long> RemoveFromRole(Guid userId, string roleName)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                    new ParamItem("RoleName", SqlDbType.NVarChar, roleName),
                };

                return await Task.FromResult(base.ExecuteSql("pro_UserRole_RemoveFromRole", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at RemoveFromRole() Method", ex.Message);
            }
            return -1;
        }

        public async Task<IList<string>> GetRoles(Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                };

                var result = await Task.FromResult(base.SqlQuery("pro_UserRole_GetRoles", Params.Create(arr)).ToList());
                return result.Select(x => x.RoleName).ToList();
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at GetRoles() Method", ex.Message);
            }
            return null;
        }
    }
}
