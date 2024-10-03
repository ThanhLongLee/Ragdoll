using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Unity.Core.Interface;
using Unity.Common.Configuration;
using Unity.Core.Interface.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Web.Admin.Infrastructure;
using Web.Admin.Infrastructure.Filters;
using Web.Admin.Models;

namespace Web.Admin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDeviceService _deviceService;
        private IdentityUserManager _userManager;
        private IdentitySignInManager _signInManager;
        const string SessionRequest = "CountRequest";
        public AccountController(IdentityUserManager userManager, IdentitySignInManager signInManager, IAppUserService userService, IDeviceService deviceService)
        {
            SignInManager = signInManager;
            _deviceService = deviceService;
            UserManager = userManager;
        }

        public IdentityUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<IdentityUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public IdentitySignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<IdentitySignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="returnUrl">Url yêu cầu đăng nhập</param>
        /// <returns></returns>
        public ActionResult SignIn(string returnUrl)
        {
            try
            {
                Session[SessionRequest] = 0;

                if (Request.IsAuthenticated)
                    return RedirectToAction("Index", "Home", new { area = "" });

                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
                _deviceService.WriteError("Error in AccountController at SignIn() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(SignInViewModel model, string returnUrl)
        {
            try
            {
                if (Request.IsAuthenticated)
                    return RedirectToAction("Index", "Home", new { area = "" });

                ViewBag.ReturnUrl = returnUrl;

                //if (!await Recaptcha())
                //    return View(model);

                if (!ModelState.IsValid)
                    return View(model);

                // Check Account Role
                var userLogin = await UserManager.FindByNameAsync(model.UserName.ToLower().Trim());
                if (userLogin != null)
                {
                    var listUserType = new List<AccountRole>() { AccountRole.Admin, AccountRole.Employee };

                    // Check is not account client
                    if (!listUserType.Contains(userLogin.UserRole))
                    {
                        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                        return View(model);
                    }
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = await SignInManager.PasswordSignInAsync(model.UserName.ToLower().Trim(), model.Password, false, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        await _deviceService.LoginHistory(userLogin.Id, Request.UserHostAddress);
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                _deviceService.WriteError("Error in AccountController at SignIn() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }

        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee })]
        public ActionResult SignOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        //private const string XsrfKey = "XsrfId";
        private const string XsrfKey = "AdminAppGomi";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}