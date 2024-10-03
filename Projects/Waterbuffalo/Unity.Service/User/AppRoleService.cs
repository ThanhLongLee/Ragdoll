using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Unity.Core.Interface;
using Unity.Core.Model;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Service;

namespace Unity.Service
{
    public partial class AppRoleService : BaseService<AppRole>, IAppRoleService
    {   
        public AppRoleService(IRepository<AppRole> repository) : base(repository) { }
    }

    public partial class AppRoleService : IAppRoleService
    { 
        public async Task<long> Create(AppRole model)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {

                    new ParamItem("Id", SqlDbType.UniqueIdentifier, model.Id),
                    new ParamItem("Name", SqlDbType.NVarChar, model.Name),
                    new ParamItem("Description", SqlDbType.NVarChar, model.Description + ""),
                };

                return await Task.FromResult(base.ExecuteSql("pro_Roles_Create", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at Create() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> Delete(Guid roleId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("RoleId", SqlDbType.UniqueIdentifier, roleId),
                };

                return await Task.FromResult(base.ExecuteSql("pro_Roles_Delete", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at Delete() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> Update(AppRole model)
        {

            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("RoleId", SqlDbType.UniqueIdentifier, model.Id),
                    new ParamItem("Name", SqlDbType.NVarChar, model.Name),
                    new ParamItem("Description", SqlDbType.NVarChar, model.Description + ""),
                };

                return await Task.FromResult(base.ExecuteSql("pro_Roles_Update", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at Update() Method", ex.Message);
            }
            return -1;
        }

        public async Task<AppRole> FindById(Guid roleId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("RoleId", SqlDbType.UniqueIdentifier,roleId),

                };

                return await Task.FromResult(base.SqlQuery("pro_Roles_FindById", Params.Create(arr)).SingleOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at FindById() Method", ex.Message);
            }
            return null;
        }

        public async Task<AppRole> FindByName(string name)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("Name", SqlDbType.NVarChar, name),
                };

                return await Task.FromResult(base.SqlQuery("pro_Roles_FindByName", Params.Create(arr)).SingleOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at FindByName() Method", ex.Message);
            }
            return null;
        }

        public async Task<List<AppRole>> GetAll()
        {
            try
            {

                return await Task.FromResult(base.SqlQuery("pro_Roles_GetAll").ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at GetAll() Method", ex.Message);
            }
            return null;
        }


        public async Task<List<AppRole>> GetAllWithParent()
        {
            try
            {
                return await Task.FromResult(base.SqlQuery("pro_Roles_GetAllWithParent").ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at GetAllWithParent() Method", ex.Message);
            }
            return null;
        }
        public async Task<List<AppRole>> GetAllParentRole()
        {
            try
            {
                return await Task.FromResult(base.SqlQuery("pro_Roles_GetAllParentRole").ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at GetAllParentRole() Method", ex.Message);
            }
            return null;
        }
        public async Task<List<AppRole>> FindByParentId(Guid parentId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("ParentId", SqlDbType.UniqueIdentifier, parentId),
                };

                return await Task.FromResult(base.SqlQuery("pro_Roles_FindByParentId", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at FindByParentId() Method", ex.Message);
            }
            return null;
        }

        public async Task<long> AddRolesToGroup(List<Guid> roles, Guid groupId)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] {new DataColumn("id", typeof(Guid)),});

                foreach (var item in roles)
                {
                    dt.Rows.Add(item);
                }

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("ListRole", SqlDbType.Structured, dt, "list_guid_table"),
                    new ParamItem("GroupId", SqlDbType.UniqueIdentifier, groupId),
                };

                return await Task.FromResult(base.ExecuteSql("pro_Role_AddRolesToGroup", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at AddRolesToGroup() Method", ex.Message);
            }
            return -1;
        }

        public async Task<IEnumerable<AppRole>> GetListRoleByGroupId(Guid groupId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("GroupId", SqlDbType.UniqueIdentifier, groupId),
                };

                return await Task.FromResult(base.SqlQuery("pro_Role_GetListRoleByGroupId", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in RoleService at GetListRoleByGroupId() Method", ex.Message);
            }
            return null;
        }
    }
}
