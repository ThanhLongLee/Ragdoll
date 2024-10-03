using Unity.Core.Interface;
using Unity.Core.Model;
using Unity.Common.Configuration;
using Unity.Common.Extensions;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Unity.Service
{
    public partial class AppUserService : BaseService<AppUser>, IAppUserService
    {   
        public AppUserService(IRepository<AppUser> repository) : base(repository) { }
    }

    public partial class AppUserService : IAppUserService
    {
        public async Task<long> Create(AppUser model)
        {
            try
            {
                ParamItem[] arr = new ParamItem[] {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, model.CreatedBy),
                    new ParamItem("Id", SqlDbType.UniqueIdentifier, model.Id),
                    new ParamItem("UserName", SqlDbType.NVarChar, model.UserName),
                    new ParamItem("Email", SqlDbType.NVarChar, model.Email),
                    new ParamItem("EmailConfirmed", SqlDbType.Bit, model.EmailConfirmed),
                    new ParamItem("PasswordHash", SqlDbType.NVarChar, (object) model.PasswordHash ?? DBNull.Value),
                    new ParamItem("PhoneNumber", SqlDbType.NVarChar, (object) model.PhoneNumber ?? DBNull.Value),
                    new ParamItem("PhoneNumberConfirmed", SqlDbType.Bit, model.PhoneNumberConfirmed),
                    new ParamItem("SecurityStamp", SqlDbType.NVarChar, model.SecurityStamp),
                    new ParamItem("TwoFactorEnabled", SqlDbType.Bit, model.TwoFactorEnabled),
                    new ParamItem("LockoutEndDateUtc", SqlDbType.DateTime, (DateTime)model.LockoutEndDateUtc.UtcDateTime),
                    new ParamItem("LockoutEnabled", SqlDbType.Bit, model.LockoutEnabled),
                    new ParamItem("AccessFailedCount", SqlDbType.Int, model.AccessFailedCount),

                    new ParamItem("FullName", SqlDbType.NVarChar, model.FullName + ""),
                    new ParamItem("UserType", SqlDbType.TinyInt, (int)model.UserType),
                    new ParamItem("UserRole", SqlDbType.TinyInt, (int)model.UserRole),
                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),

                };

                return await Task.FromResult(base.ExecuteSql("pro_User_Create", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at Create() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> Delete(Guid createdBy, Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),

                };

                return await Task.FromResult(base.ExecuteSql("pro_User_Delete", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at Delete() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> Update(AppUser model)
        {

            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, model.CreatedBy),
                    new ParamItem("Id", SqlDbType.UniqueIdentifier, model.Id),
                    new ParamItem("UserName", SqlDbType.NVarChar, model.UserName),
                    new ParamItem("Email", SqlDbType.NVarChar, model.Email),
                    new ParamItem("EmailConfirmed", SqlDbType.Bit, model.EmailConfirmed),
                    new ParamItem("PasswordHash", SqlDbType.NVarChar, (object) model.PasswordHash ?? DBNull.Value),
                    new ParamItem("PhoneNumber", SqlDbType.NVarChar, (object) model.PhoneNumber ?? DBNull.Value),
                    new ParamItem("PhoneNumberConfirmed", SqlDbType.Bit, model.PhoneNumberConfirmed),
                    new ParamItem("SecurityStamp", SqlDbType.NVarChar, model.SecurityStamp),
                    new ParamItem("TwoFactorEnabled", SqlDbType.Bit, model.TwoFactorEnabled),
                    new ParamItem("LockoutEndDateUtc", SqlDbType.DateTime, (DateTime) model.LockoutEndDateUtc.UtcDateTime),
                    new ParamItem("LockoutEnabled", SqlDbType.Bit, model.LockoutEnabled),
                    new ParamItem("AccessFailedCount", SqlDbType.Int, model.AccessFailedCount),

                    new ParamItem("FullName", SqlDbType.NVarChar, model.FullName + ""),
                    new ParamItem("Note", SqlDbType.NVarChar, model.Note + ""),
                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),

                };

                return await Task.FromResult(base.ExecuteSql("pro_User_Update", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at Update() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> UpdateStatus(Guid createdBy, Guid userId, AccountStatus status)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("CreatedBy", SqlDbType.UniqueIdentifier, createdBy),
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                    new ParamItem("Status", SqlDbType.TinyInt, (int)status),
                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),

                };

                return await Task.FromResult(base.ExecuteSql("pro_User_UpdateStatus", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at UpdateStatus() Method", ex.Message);
            }
            return -1;
        }

        public async Task<long> UpdateSecurityStamp(Guid userId, string newSecurityStamp)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                    new ParamItem("NewSecurityStamp", SqlDbType.NVarChar, newSecurityStamp),
                    new ParamItem("CreatedDate", SqlDbType.DateTime, DateTime.Now),

                };

                return await Task.FromResult(base.ExecuteSql("pro_Uset_UpdateSecurityStamp", Params.Create(arr)));
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at UpdateSecurityStamp() Method", ex.Message);
            }
            return -1;
        }

        public async Task<AppUser> FindById(Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                };

                return await Task.FromResult(base.SqlQuery("pro_User_FindById", Params.Create(arr)).FirstOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at FindById() Method", ex.Message);
            }
            return null;
        }

        public async Task<AppUser> FindByName(string userName)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserName", SqlDbType.NVarChar, userName),
                };

                return await Task.FromResult(base.SqlQuery("pro_User_FindByName", Params.Create(arr)).SingleOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at FindByName() Method", ex.Message);
            }
            return null;
        }
        
        public async Task<AppUser> FindByEmail(string email)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("Email", SqlDbType.NVarChar, email),
                };

                return await Task.FromResult(base.SqlQuery("pro_User_FindByEmail", Params.Create(arr)).FirstOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at FindByEmail() Method", ex.Message);
            }
            return null;
        }

        public async Task<List<AppUser>> GetAll()
        {
            try
            {
                return await Task.FromResult(base.SqlQuery("pro_User_GetAll").ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at GetAll() Method", ex.Message);
            }
            return null;
        }

        public async Task<IEnumerable<AppUser>> FindBy(string keyword, List<AccountRole> userRoles, AccountStatus status, DateTime startDate, DateTime endDate, bool sortDesc, bool sortByName, bool sortByDate, int beginRow, int numRows)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[1] { new DataColumn("type", typeof(byte)) });

                if (userRoles != null)
                {
                    foreach (var userType in userRoles)
                    {
                        dt.Rows.Add((int)userType);
                    }
                }

                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("Keyword", SqlDbType.NVarChar,  keyword.KeywordContains()),
                    new ParamItem("ListUserRole", SqlDbType.Structured, dt, "list_type_table"),
                    new ParamItem("Status", SqlDbType.TinyInt, (int)status),
                    new ParamItem("StartDate", SqlDbType.DateTime, startDate.IsDefaultDate() ? DBNull.Value : (object)startDate),
                    new ParamItem("EndDate", SqlDbType.DateTime, endDate.IsDefaultDate() ? DBNull.Value : (object)endDate),
                    new ParamItem("SortByName", SqlDbType.Int, sortByName),
                    new ParamItem("SortByDate", SqlDbType.Int, sortByDate),
                    new ParamItem("SortDesc", SqlDbType.Int, sortDesc),
                    new ParamItem("BeginRow", SqlDbType.Int, beginRow),
                    new ParamItem("NumRows", SqlDbType.Int, numRows),

                };
                return await Task.FromResult(base.SqlQuery("pro_User_FindBy", Params.Create(arr)).ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at FindBy() Method", ex.Message);
            }

            return Enumerable.Empty<AppUser>();
        }

        public async Task<long> RemoveAllFromGroup(Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                };
                return await Task.FromResult(base.ExecuteSql("pro_User_RemoveAllUserGroupsByUserId", Params.Create(arr)));

            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at RemoveAllFromGroups() Method", ex.Message);
            }

            return -1;
        }

        public async Task<long> RemoveAllFromRole(Guid userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.UniqueIdentifier, userId),
                };
                return await Task.FromResult(base.ExecuteSql("pro_UserRole_RemoveAllUserRolesByUserId", Params.Create(arr)));

            }
            catch (Exception ex)
            {
                base.WriteError("Error in UserService at RemoveAllFromRoles() Method", ex.Message);
            }

            return -1;
        }

        public async Task<AppUser> VerifyEmail(string email)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("Email", SqlDbType.VarChar, email),
                };

                return await Task.FromResult(base.SqlQuery("pro_User_VerifyEmail", Params.Create(arr)).FirstOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in ApplicationUserService at VerifyEmail() Method", ex.Message);
            }
            return null;
        }

        public async Task<AppUser> VerifyPhoneNumber(string phoneNumber)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("PhoneNumber", SqlDbType.VarChar, phoneNumber),
                };

                return await Task.FromResult(base.SqlQuery("pro_User_VerifyPhoneNumber", Params.Create(arr)).FirstOrDefault());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in ApplicationUserService at VerifyPhoneNumber() Method", ex.Message);
            }
            return null;
        }
    }
}
