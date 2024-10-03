using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Common.Parameter;
using Unity.Core.Interface.Data;
using Unity.Core.Interface.Service;
using Unity.Core.Model;

namespace Unity.Service.Telegram
{
    public partial class UserDetailService : BaseService<UserDetails>, IUserDetailService
    {
        public UserDetailService(IRepository<UserDetails> repository) : base(repository) { }

        public async Task<UserDetails> GetInfoRank(long id)
        {
            try
            {
                // Định nghĩa các tham số đầu vào và đầu ra
                ParamItem[] arr = new ParamItem[]
                    {
                        new ParamItem("UserId", SqlDbType.BigInt, id),
                    };

                return await Task.FromResult(base.SqlQuery("pro_GetUserRank", Params.Create(arr)).Single());


            }
            catch (Exception ex)
            {
                base.WriteError("Lỗi khi get hoặc create user", ex.Message);
            }

            return null;
        }

        public async Task<UserDetails> GetInfoUser(long id)
        {
            try
            {
                // Định nghĩa các tham số đầu vào và đầu ra
                ParamItem[] arr = new ParamItem[]
                    {
                        new ParamItem("Id", SqlDbType.BigInt, id),
                    };

                return await Task.FromResult(base.SqlQuery("pro_GetUserInfo", Params.Create(arr)).Single());


            }
            catch (Exception ex)
            {
                base.WriteError("Lỗi khi get hoặc create user", ex.Message);
            }

            return null;
        }

        public async Task<List<UserDetails>> GetTop10()
        {
            try
            {      
                return await Task.FromResult(base.SqlQuery("pro_GetTop10Users").ToList());
            }
            catch (Exception ex)
            {
                base.WriteError("Error in get top 10 user rank info", ex.Message);
            }
            return Enumerable.Empty<UserDetails>().ToList();
        }

        public async Task<UserDetails> UpdateUserScore(long userId)
        {
            try
            {
                // Tạo mảng các tham số cho stored procedure
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserId", SqlDbType.BigInt, userId)
                };

                // Thực thi stored procedure và lấy kết quả trả về
                return await Task.FromResult(base.SqlQuery("pro_UpdateUserScore", Params.Create(arr)).Single());       
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu có
                base.WriteError("Lỗi khi cập nhật điểm cho người dùng", ex.Message);
            }

            // Trả về null nếu có lỗi
            return null;
        }
    }
}
