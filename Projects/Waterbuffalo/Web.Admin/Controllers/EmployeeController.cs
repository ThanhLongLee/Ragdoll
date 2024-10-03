using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Unity.Common.Configuration;
using Unity.Common.Extensions;
using Unity.Common.Helper;
using Unity.Core.Interface;
using Unity.Core.Model;
using Web.Admin.Infrastructure.Filters;
using Web.Admin.Infrastructure.Helper;
using Web.Admin.Models;

namespace Web.Admin.Controllers
{
    public class EmployeeController : BasicController
    {
        private readonly IAppUserService _userService;


        private readonly IAppRoleService _roleService;
        private readonly IAppUserRoleService _userRoleService;

        private readonly IGroupRoleService _groupRoleService;
        private readonly IUsersInGroupRoleService _usersInGroupRoleService;
        private readonly IRolesInGroupRoleService _rolesInGroupRoleService;


        private IdentityRoleManager _roleManager;
        private IdentityUserManager _userManager;

        public EmployeeController(IAppRoleService roleService, IAppUserService userService, IdentityRoleManager roleManager, IdentityUserManager userManager, IGroupRoleService groupRoleService, IUsersInGroupRoleService usersInGroupRoleService, IRolesInGroupRoleService rolesInGroupRoleService, IAppUserRoleService userRoleService) : base(userService)
        {
            _roleService = roleService;
            _userService = userService;
            UserManager = userManager;
            RoleManager = roleManager;
            _groupRoleService = groupRoleService;
            _usersInGroupRoleService = usersInGroupRoleService;
            _rolesInGroupRoleService = rolesInGroupRoleService;
            _userRoleService = userRoleService;
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

        public IdentityRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<IdentityRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-view")]
        public async Task<ActionResult> List()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at List() Method", ex.Message);
                return RedirectToRoute("ErrorCode");// Error Page
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/employee/search")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-view")]
        public async Task<JsonResult> Ajax_GetEmployee(int pageNo, string keyword, bool sortByDate)
        {
            var model = new List<UserViewModel>();
            try
            {
                if (!Request.IsAjaxRequest())
                    return Json(new { status = false, message = StrErrorCode });

                var beginRow = Calculator.BeginRow(pageNo);
                var numRows = Calculator.NumRows(pageNo);

                var userLogging = await _userService.FindById(User.Identity.GetUserId().ToGuid());
                var listRoles = new List<AccountRole> { AccountRole.Employee };

                if (userLogging != null && userLogging.UserRole == AccountRole.Admin)
                    listRoles.Add(AccountRole.Admin);

                var result = await _userService.FindBy(keyword ?? "", listRoles, AccountStatus.Undefined, new DateTime().DefaultDate(), new DateTime().DefaultDate(), true, false, true, beginRow, numRows);

                var roles = await _userManager.GetRolesAsync(User.Identity.GetUserId().ToGuid());

                foreach (var item in result)
                {
                    if (userLogging.UserRole == AccountRole.Admin || userLogging.Id == item.Id)
                    {
                        var itemVM = new UserViewModel();
                        itemVM.FromModel(item);
                        itemVM.TotalPage = Calculator.TotalPage(item.TotalRows);
                        model.Add(itemVM);
                    }
                }

                var listUser = new ListUserViewModel();
                listUser.Users = model;
                listUser.Roles = roles.ToList();
                listUser.IdLogin = User.Identity.GetUserId().ToGuid();

                return Json(new { status = true, totalPage = model.Any() ? model.ElementAt(0).TotalPage : 0, view = RenderHelper.PartialView(this, @"~\Views\Employee\_List.cshtml", listUser) });
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at Ajax_GetEmployee() Method", ex.Message);
                return Json(new { status = false, message = StrErrorCode });// Error Page   
            }
        }


        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee })]
        public async Task<ActionResult> ProfileUser()
        {
            try
            {
                var userLogging = await _userService.FindById(User.Identity.GetUserId().ToGuid());
                if (userLogging == null)
                    return RedirectToRoute("ErrorCode");

                var userInfo = new UserViewModel();
                userInfo.Id = userLogging.UserId;
                userInfo.UserName = userLogging.UserName;
                userInfo.Profile.FullName = userLogging.FullName;
                userInfo.Email = userLogging.Email;
                ViewBag.UserInfo = userInfo;

                return View(new ChangePasswordViewModel());
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at ProfileUser() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee })]
        public async Task<ActionResult> ChangePasswordByUserSelf(ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Warning("Lỗi! Vui lòng nhập đầy đủ thông tin", true);
                    return RedirectToAction("ProfileUser");
                }

                //Change Password
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId().ToGuid(), model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    Warning("Mật khẩu cũ không đúng", true);
                    return RedirectToAction("ProfileUser");
                }

                // update SecurityStamp
                await _userService.UpdateSecurityStamp(User.Identity.GetUserId().ToGuid(), Guid.NewGuid().ToString());

                //Đăng xuất
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return RedirectToAction("SignIn", "Account", new { area = "" });
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at ChangePasswordByUserSelf() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }


        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-add")]
        public async Task<ActionResult> Add()
        {
            try
            {
                var model = new AddUserViewModel();

                // Hiện tại một tài khoản chỉ áp dụng được 1 nhóm quyền
                var groupRoles = await SelectListGroupRole(true);
                ViewBag.GroupRoles = groupRoles;

                // Get All Roles            
                model.Roles = await RolesGetAll();

                return View(model);
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at Add() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-add")]
        public async Task<ActionResult> Add(AddUserViewModel model)
        {
            try
            {
                // var birthDay = model.Birthday.ToDateTime(AppSettings.DateFormat);

                if (!ModelState.IsValid)
                    return View(model);

                var newUser = new AppUser();
                newUser.FromViewModel(model);
                newUser.Id = Guid.NewGuid();
                newUser.UserType = AccountType.Application;
                newUser.UserRole = AccountRole.Employee;
                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (!result.Succeeded)
                    return RedirectToRoute("ErrorCode");

                var newGroupRole = new GroupRole();
                newGroupRole.Id = Guid.NewGuid();
                newGroupRole.Name = newUser.FullName;
                newGroupRole.Description = newUser.Email;
                var insertGroupRole = await _groupRoleService.Insert(User.Identity.GetUserId().ToGuid(), newGroupRole);

                if (insertGroupRole == -1)
                    return RedirectToRoute("ErrorCode");

                var insertToGroupRole = await _usersInGroupRoleService.Insert(User.Identity.GetUserId().ToGuid(), newUser.Id, new List<Guid>() { newGroupRole.Id });
                if (insertToGroupRole == -1)
                    return RedirectToRoute("ErrorCode");

                if (model.RolesId != null && model.RolesId.Any())
                {
                    var insertRolesGroup = await _rolesInGroupRoleService.Insert(User.Identity.GetUserId().ToGuid(), newGroupRole.Id, model.RolesId);
                    if (insertRolesGroup == -1)
                        return RedirectToRoute("ErrorCode");
                }

                var rolesInGroup = await _rolesInGroupRoleService.GetListRoleByGroupId(newGroupRole.Id);
                if (rolesInGroup != null)
                {
                    // Thêm mới lại role cho user
                    await _userRoleService.AddFromListRole(newUser.Id, rolesInGroup.Select(s => s.RoleId).ToList());
                }

                Success(string.Format(StrAddSuccess, model.FullName), true);
                return RedirectToAction("List", "Employee");
            }
            catch (FormatException ex)
            {
                ModelState.AddModelError("ErrorDateFormat", "Vui lòng nhập định dạng ngày sinh: dd/MM/yyyy");
                return View(model);
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at Add() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }



        //[CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-edit")]
        public async Task<ActionResult> Update(Guid id)
        {
            try
            {
                var employee = await _userService.FindById(id);
                if (employee == null)
                    return RedirectToRoute("NotFound");

                var userLogging = await _userService.FindById(User.Identity.GetUserId().ToGuid());
                ViewBag.UserRole = userLogging.UserRole;
                if (userLogging == null)
                    return RedirectToRoute("ErrorCode");

                if (employee.UserRole == AccountRole.Admin && userLogging.UserRole != AccountRole.Admin)
                    return RedirectToRoute("ErrorCode");

                var model = new UpdateUserViewModel();
                model.FromModel(employee);

                // Get All Roles                
                model.Roles = await RolesGetAll();
                model.RolesId = new List<Guid>();

                // Hiện tại một tài khoản chỉ áp dụng được 1 nhóm quyền
                var currentGroupRole = await _groupRoleService.FindGroupsByUserId(employee.Id);
                //var groupRoles = await SelectListGroupRole(true, currentGroupRole != null && currentGroupRole.Any() ? currentGroupRole.ElementAt(0).Id.ToString() : null);
                model.GroupRoleId = currentGroupRole != null && currentGroupRole.Any() ? currentGroupRole.ElementAt(0).Id : Guid.Empty;
                //ViewBag.GroupRoles = groupRoles;


                var rolesInGroup = await _rolesInGroupRoleService.GetListRoleByGroupId(currentGroupRole.ElementAt(0).Id);
                if (rolesInGroup != null && rolesInGroup.Any())
                {
                    foreach (var item in rolesInGroup)
                    {
                        model.RolesId.Add(item.RoleId);
                    }
                }


                return View(model);
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at Update() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-edit")]
        public async Task<ActionResult> Update(UpdateUserViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var user = await _userManager.FindByIdAsync(model.Id);
                user.FromViewModel(model);

                var userLogging = await _userService.FindById(User.Identity.GetUserId().ToGuid());
                if (userLogging == null)
                    return RedirectToRoute("ErrorCode");
                if (user.UserRole == AccountRole.Admin && userLogging.UserRole != AccountRole.Admin)
                    return RedirectToRoute("ErrorCode");

                //== Update Info
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return RedirectToRoute("ErrorCode");

                if (model.GroupRoleId != Guid.Empty)
                {
                    var insertToGroupRole = await _usersInGroupRoleService.Insert(User.Identity.GetUserId().ToGuid(), user.Id, new List<Guid>() { model.GroupRoleId });
                    if (insertToGroupRole == -1)
                        return RedirectToRoute("ErrorCode");

                    // Reset lại toàn bộ role của group
                    if (model.RolesId != null && model.RolesId.Any())
                    {
                        var insertRolesGroup = await _rolesInGroupRoleService.Insert(User.Identity.GetUserId().ToGuid(), model.GroupRoleId, model.RolesId);
                        if (insertRolesGroup == -1)
                            return RedirectToRoute("ErrorCode");
                    }
                    else
                    {
                        await _rolesInGroupRoleService.RemoveAllRolesInGroup(model.GroupRoleId);
                    }

                    var rolesInGroup = await _rolesInGroupRoleService.GetListRoleByGroupId(model.GroupRoleId);
                    if (rolesInGroup != null)
                    {
                        // Xóa hết tất cả role của user
                        await _userService.RemoveAllFromRole(user.Id);

                        // Thêm mới lại role cho user
                        await _userRoleService.AddFromListRole(user.Id, rolesInGroup.Select(s => s.RoleId).ToList());
                    }
                }

                //== Update Status
                var statusUpdate = model.IsActive ? AccountStatus.Active : AccountStatus.Locked;
                if (statusUpdate != user.Status)
                {
                    var updateStatus = await _userService.UpdateStatus(User.Identity.GetUserId().ToGuid(), model.Id, statusUpdate);
                    if (updateStatus == -1)
                        return RedirectToRoute("ErrorCode");
                }

                await _userService.UpdateSecurityStamp(model.Id, Guid.NewGuid().ToString());

                Success(string.Format(StrUpdateSuccess, model.FullName), true);
                return RedirectToAction("List", "Employee");
            }
            catch (FormatException ex)
            {
                ModelState.AddModelError("ErrorDateFormat", "Vui lòng nhập định dạng ngày sinh: dd/MM/yyyy");
                return View(model);
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at Update() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("admin/employee/change-password", Name = "AdminEmployeeChangePassword")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-edit")]
        public async Task<JsonResult> ResetPassword(Guid userId, string newPassword, bool autoCreate)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                    return Json(new { status = false, message = StrErrorCode });

                var user = await UserManager.FindByIdAsync(userId);
                if (user == null)
                    return Json(new { status = false, message = StrErrorCode });

                //if (!UserManager.HasPassword(user.Id))
                //    return Json(new { status = false, message = StrErrorCode });

                if (autoCreate)
                    newPassword = new Random().Next(0, 99999999).ToString();

                // Removed Password
                var removePassword = UserManager.RemovePassword(user.Id);
                if (!removePassword.Succeeded)
                    return Json(new { status = false, message = StrErrorCode });

                // Add New Password
                var addPassword = UserManager.AddPassword(user.Id, newPassword);
                if (!addPassword.Succeeded)
                    return Json(new { status = false, message = StrErrorCode });

                // update SecurityStamp
                await _userService.UpdateSecurityStamp(user.Id, Guid.NewGuid().ToString());

                // Random new Password
                if (autoCreate)
                    return Json(new { status = true, result = true, content = string.Format("Mật khẩu mới đã tạo: <b>{0}</b>", newPassword) });

                // Set new Password
                return Json(new { status = true, result = true, content = string.Format(StrUpdateSuccess, user.UserName) });
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at ResetPassword() Method", ex.Message);
                return Json(new { status = false, message = StrErrorCode });
            }
        }

        //Xoá tài khoảng
        [HttpPost]
        [Route("employee/delete", Name = "")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-delete")]
        public async Task<JsonResult> Delete(Guid id, int pageNo, string keyword)
        {
            try
            {
                /// Kiểm tra
                var user = await _userService.FindById(id);
                if (user == null)
                    return Json(new { status = false, message = StrErrorCode });// Error Page 
                //== Update Status
                var updateStatus = await _userService.UpdateStatus(User.Identity.GetUserId().ToGuid(), id, AccountStatus.Disabled);
                if (updateStatus == -1)
                    return Json(new { status = false, message = StrErrorCode });// Error Page 


                var model = new List<UserViewModel>();

                var beginRow = Calculator.BeginRow(pageNo);
                var numRows = Calculator.NumRows(pageNo);

                var userLogging = await _userService.FindById(User.Identity.GetUserId().ToGuid());
                var listRoles = new List<AccountRole> { AccountRole.Employee };

                if (userLogging != null && userLogging.UserRole == AccountRole.Admin)
                    listRoles.Add(AccountRole.Admin);

                var result = await _userService.FindBy(keyword ?? "", listRoles, AccountStatus.Undefined, new DateTime().DefaultDate(), new DateTime().DefaultDate(), true, false, true, beginRow, numRows);

                foreach (var item in result)
                {
                    var itemVM = new UserViewModel();
                    itemVM.FromModel(item);
                    itemVM.TotalPage = Calculator.TotalPage(item.TotalRows);
                    model.Add(itemVM);
                }
                var content = string.Format(StrUpdateSuccess, user.FullName);
                return Json(new { status = true, content, totalPage = model.Any() ? model.ElementAt(0).TotalPage : 0, view = RenderHelper.PartialView(this, @"~\Views\Employee\_List.cshtml", model) });

            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at Delete() Method", ex.Message);
                return Json(new { status = false, message = StrErrorCode });// Error Page 
            }
        }


        [HttpPost]
        [Route("admin/user/verify-username", Name = "admin-user-verify")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee }, Roles = "employee-view")]
        public async Task<JsonResult> VerifyUserName(string userName)
        {
            try
            {
                if (!userName.IsEmpty())
                {
                    bool result = await _userService.FindByName(userName.TrimEmpty().ToLower()) != null;
                    return Json(!result);
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at VerifyUserName() Method", ex.Message);
                return Json(false);
            }
        }


        private async Task<List<ParentRoleViewModel>> RolesGetAll()
        {
            var model = new List<ParentRoleViewModel>();

            try
            {
                var parentRoles = await _roleService.GetAllParentRole();
                if (parentRoles != null)
                {
                    foreach (var item in parentRoles)
                    {
                        var itemRoleVM = new ParentRoleViewModel();
                        itemRoleVM.FromModel(item);
                        var roles = await _roleService.FindByParentId(item.Id);
                        if (roles != null)
                        {
                            foreach (var itemChild in roles)
                            {
                                var roleVM = new RoleViewModel();
                                roleVM.FromModel(itemChild);
                                itemRoleVM.Children.Add(roleVM);
                            }
                        }
                        model.Add(itemRoleVM);
                    }
                }
            }
            catch (Exception ex)
            {
                _roleService.WriteError("Error Admin in EmployeeController at RolesGetAll() Method", ex.Message);

            }
            return model;
        }

        // Kiểm tra phân quyền Thêm mới nhân viên
        [ChildActionOnly]
        [Route("admin/render-AddEmployee")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee })]
        public async Task<ActionResult> RenderAddEmployee()
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    var roles = await _userManager.GetRolesAsync(User.Identity.GetUserId().ToGuid());

                    return PartialView("~/Views/Employee/Partial/_AddEmployee.cshtml", roles.ToList());
                }

                return null;
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in OrderController at RenderAddEmployee() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }

        private async Task<List<SelectListItem>> SelectListGroupRole(bool includeDefault = false, object selected = null)
        {
            var model = new List<SelectListItem>();
            try
            {
                var groupRoles = await _groupRoleService.FindBy("", Status.Activated, 0, 0);
                foreach (var item in groupRoles)
                {
                    model.Add(new SelectListItem()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString(),
                        Selected = selected != null && selected.ToString().ToGuid() == item.Id
                    });
                }

                if (includeDefault)
                    model.AppendItemByIndex("--Nhóm quyền--", "", 0, model.FirstOrDefault(x => x.Selected) != null ? false : true);
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in EmployeeController at SelectListGroupRole() Method", ex.Message);
            }
            return model;
        }

        #region------- Render Html


        [ChildActionOnly]
        [Route("admin/render-menu")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee })]
        public async Task<ActionResult> RenderMenu()
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    var roles = await _userManager.GetRolesAsync(User.Identity.GetUserId().ToGuid());


                    return PartialView("~/Views/Shared/Partial/_Menu.cshtml", roles.ToList());
                }

                return null;
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in BasicController at Ajax_GetUserInfoNavbar() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }


        [ChildActionOnly]
        [Route("admin/users/render-html-right-navbar")]
        [CustomAuthorize(new[] { AccountRole.Admin, AccountRole.Employee })]
        public async Task<ActionResult> RenderRightNavbar()
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    var userInfo = await _userService.FindById(User.Identity.GetUserId().ToGuid());

                    if (userInfo == null)
                        return RedirectToRoute("ErrorCode");

                    var model = new MenuUserPanelViewModel();
                    model.FullName = userInfo.FullName;
                    model.UserName = userInfo.UserName;
                    Random rnd = new Random();
                    int random = rnd.Next(1, 9);
                    model.ImgUser = random + ".png";

                    return PartialView("~/Views/Shared/Partial/_RightNavbar.cshtml", model);
                }

                return null;
            }
            catch (Exception ex)
            {
                _userService.WriteError("Error Admin in BasicController at Ajax_GetUserInfoNavbar() Method", ex.Message);
                return RedirectToRoute("ErrorCode");
            }
        }

        #endregion...../.


    }
}