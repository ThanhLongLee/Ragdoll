using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Unity.Core.Interface.Service;
using Unity.Core.Model;
using Unity.Service;
using GomdoriMagazine.Models;
using Unity.Core.Interface.Service.UserActions;
using Unity.Core.Interface.Service.Link;
using Unity.Core.Interface.Service.Booster;
using Unity.Core.Interface.Service.UserBoosters;
using Unity.Core.Interface.Service.Invite;
using Unity.Core.Interface.Service.UserWallet;
using Unity.Service.UserWallet;

namespace Web.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDeviceService _deviceService;
        private readonly IUserTelegramService _userTelegramService;
        private readonly IUserDetailService _userDetailService;
        private readonly IUserCheckinsService _userCheckinService;
        private readonly ICheckinTrackersService _checkinTrackersService;
        private readonly IUserLinkService _userLinkService;
        private readonly ILinkService _linkService;    
        private readonly IBoostService _boostService;
        private readonly IUserBoostersService _userBoostersService;
        private readonly IInviteService _inviteService;
        private readonly IUserWallet _userWallet;

        public HomeController(IDeviceService deviceService, IUserTelegramService userTelegramService, 
            IUserDetailService userDetailService, IUserCheckinsService userService, 
            ICheckinTrackersService checkinTrackersService, IUserLinkService userLinkService, ILinkService linkService
            ,IBoostService boostService, IUserBoostersService userBoostersService, IInviteService inviteService, IUserWallet userWallet
         )
        {
            _deviceService = deviceService;
            _userTelegramService = userTelegramService;
            _userDetailService = userDetailService;
            _userCheckinService = userService;
            _checkinTrackersService = checkinTrackersService;
            _userLinkService = userLinkService;
            _linkService = linkService;     
            _boostService = boostService;
            _userBoostersService = userBoostersService;
            _inviteService = inviteService;
            _userWallet = userWallet;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buffalo()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> ProcessUser(UserTelegrams user)
        {
            // Gọi phương thức để insert hoặc cập nhật người dùng
            await _userTelegramService.InsertUser(user);

            // Gọi phương thức để lấy thông tin người dùng bằng ID của người dùng
            var userDetails = await _userDetailService.GetInfoUser(user.Id);

            return Json(userDetails);
        }

        [HttpPost]
        public async Task<ActionResult> GetWalletStatus(long userId)
        {
            var status = await _userWallet.GetStatus(userId);
            return Json(status);
        }

        [HttpPost]
        public async Task<ActionResult> ConnectWallet(long userId, string walletAddress, string exchangeName)
        {
            var result = await _userWallet.Connect(userId, walletAddress, exchangeName);
            if (result > 0)
            {
                return Json(new { success = true, message = "Wallet connected successfully!" });
            }
            return Json(new { success = false, message = "Failed to connect wallet." });
        }

        [HttpPost]
        public async Task<ActionResult> DisconnectWallet(long userId)
        {
            var result = await _userWallet.Disconnect(userId);
            if (result > 0)
            {
                return Json(new { success = true, message = "Wallet disconnected successfully!" });
            }
            return Json(new { success = false, message = "Failed to disconnect wallet." });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUserScore(long userId)
        {
            // Gọi service để cập nhật điểm của người dùng
            var userDetails = await _userDetailService.UpdateUserScore(userId);

            // Trả về thông tin đã cập nhật cho client
            return Json(userDetails);
        }

        // Action xử lý điểm danh
        [HttpPost]
        public async Task<ActionResult> UserCheckin(long userId)
        {
            try
            {
                // Gọi service để thực hiện điểm danh
                var result = await _userCheckinService.UserCheckin(userId);

                // Trả về JSON với thông tin thành công
                return Json(new { success = true, message = "Check-in successful" });
            }
            catch (Exception ex)
            {
                // Trả về JSON với thông báo lỗi nếu có ngoại lệ
                return Json(new { success = false, message = ex.Message });
            }
        }


        // Action lấy trạng thái điểm danh
        [HttpGet]
        public async Task<ActionResult> GetCheckinStatus(long userId)
        {
            try
            {
                // Lấy thông tin điểm danh từ service
                var checkinTrackers = await _checkinTrackersService.GetCheckinTrackers(userId);
                var checkinRecords = await _userCheckinService.GetCheckinRecords(userId);

                // Tạo đối tượng CheckinStatus để trả về
                var status = new CheckinStatus
                {
                    CurrentDayCount = checkinTrackers?.CurrentDayCount ?? 0,
                    CheckedDays = checkinRecords?.Select(r => r.CheckinDate.Day).ToList() ?? new List<int>()
                };

                // Trả về JSON chứa thông tin trạng thái điểm danh
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Trả về lỗi 500 nếu có lỗi
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }



        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Rank()
        {        
            return View();
        }

        public async Task<ActionResult> UserAndTop10(long userId)
        {
            try
            {
                var userDetails = await _userDetailService.GetInfoRank(userId);
                var top10Users = await _userDetailService.GetTop10();

                var result = new
                {
                    UserDetails = userDetails,
                    Top10Users = top10Users
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }


        public ActionResult Earn()
        {          
            return View();
        }
  
        public async Task<ActionResult> LayDanhSachLinks(long userId)
        {
            var links = await _linkService.GetUserLinks(userId);
            return Json(links, JsonRequestBehavior.AllowGet);
        }

        // Action để cập nhật điểm sau khi click vào link
        [HttpPost]
        public async Task<ActionResult> CheckClickLink(long userId, int linkId)
        {
            var scoreAwarded = await _userLinkService.UpdateUserScore(userId, linkId);
            return Json(scoreAwarded, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Booster()
        {
            return View();
        }

        public async Task<ActionResult> LayDanhSachBoosters(long userId)
        {
            var links = await _boostService.GetBoost(userId);
            return Json(links, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public async Task<ActionResult> CheckClickBoost(long userId, int bootstId)
        {
            var scoreAwarded = await _userBoostersService.HandleBoost(userId, bootstId);
            return Json(scoreAwarded, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Soon()
        {
           return View();
        }

        public ActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SendInvites(long senderId, InviteDetails invites)
        {    
            if (invites == null)
            {
                return new HttpStatusCodeResult(400, "No receivers specified");
            }

            await _inviteService.HandleInvitesAsync(senderId, invites);

            return RedirectToAction("Buffalo", "Home");
        }

        public ActionResult Mision()
        {
            return View();
        }
    }
}