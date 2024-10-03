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

namespace Unity.Service.CheckIn
{
    public partial class UserCheckinsService: BaseService<UserCheckins>, IUserCheckinsService
    {
        public UserCheckinsService(IRepository<UserCheckins> repository) : base(repository) { }

        public async Task<bool> UserCheckin(long userId)
        {
            try
            {
                ParamItem[] arr = new ParamItem[]
                {
                    new ParamItem("UserID", SqlDbType.BigInt, userId)
                };
                await Task.FromResult(SqlQuery("pro_UserCheckin", Params.Create(arr)));
                return true;
            }
            catch (Exception ex)
            {
                base.WriteError("Lỗi khi điểm danh", ex.Message);
            }
            return false;
        }      

        public async Task<List<UserCheckins>> GetCheckinRecords(long userId)
        {
            try
            {
                // Khởi tạo các tham số để truyền vào thủ tục lưu trữ
                ParamItem[] arr = new ParamItem[]
                {
            new ParamItem("UserID", SqlDbType.BigInt, userId)
                };

                // Gọi thủ tục lưu trữ để lấy dữ liệu
                return await Task.FromResult(base.SqlQuery("sp_GetCheckinRecords", Params.Create(arr)).ToList());                            
            }
            catch (Exception ex)
            {
                // Ghi lỗi vào log
                base.WriteError("Lỗi khi lấy thông tin điểm danh", ex.Message);            
            }
            return null;
        }

        public Task<CheckinStatus> GetCheckinStatus(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
