using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity.Common.Configuration;
using Unity.Common.Extensions;
using Unity.Core.Model;

namespace Web.Admin.Models
{
    public partial class UserViewModel
    {
        public UserViewModel()
        {
            Profile = new UserProfile();
        }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset LockoutEndDateUtc { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserProfile Profile { get; set; }
        public double TotalPage { get; set; }
        public DateTime LastDateLogin { get; set; }
        public bool IsLockout { get; set; }

    }

    public class UserProfile
    {
        public Guid UserId { get; set; }
        public long IdentityCode { get; set; }

        public string FullName { get; set; }
        public bool IsNew { get; set; }
        public string Note { get; set; }

        public AccountType UserType { get; set; }

        public AccountStatus Status { get; set; }
    }

    public class UserActivatingViewModel
    {
        public UserActivatingViewModel()
        {
            ActivityLogs = new List<UserActivityLogViewModel>();
        }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<UserActivityLogViewModel> ActivityLogs { get; set; }
    }

    public class UserActivityLogViewModel
    {
        public Guid SessionId { get; set; }
        public string IPAddress { get; set; }
        public string BrowserName { get; set; }
        public DateTime LastTimeActivity { get; set; }

    }

    public class MenuUserPanelViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string ImgUser { get; set; }

    }

    public class AddUserViewModel
    {
        public AddUserViewModel()
        {
            IsActive = true;
        }
        public Guid Id { get; set; }

        [Required(ErrorMessage = " ")]
        [EmailAddress(ErrorMessage = "Sai định dạnh Email")]
        [Display(Name = "Tên đăng nhập *")]
        [Remote("admin-user-verify", ErrorMessage = "Tên đăng nhập đã tồn tại", HttpMethod = "POST")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " ")]
        [EmailAddress(ErrorMessage = "Sai định dạnh Email")]
        [Display(Name = "Email *")]
        public string Email { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(32, ErrorMessage = "Mật khẩu từ 6 đến 32 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu *")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu *")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Xác nhận mật khẩu không đúng")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        public AccountType UserType { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Họ tên *")]
        public string FullName { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Nhóm quyền")]
        public Guid GroupRoleId { get; set; }

        [Display(Name = "Quyền truy cập")]
        public List<Guid> RolesId { get; set; }

        public List<ParentRoleViewModel> Roles { get; set; }
    }

    public class UpdateUserViewModel
    {

        public Guid Id { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage = " ")]
        [EmailAddress(ErrorMessage = "Sai định dạnh Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public AccountType UserType { get; set; }

        public AccountStatus Status { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Display(Name = "Nhóm quyền")]
        public Guid GroupRoleId { get; set; }

        [Display(Name = "Quyền truy cập")]
        public List<Guid> RolesId { get; set; }

        public List<ParentRoleViewModel> Roles { get; set; }

    }

    public class ChangePasswordViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Mật khẩu cũ")]
        [StringLength(32, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = " ")]
        [StringLength(32, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận lại mật khẩu")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Xác nhận mật khẩu không đúng.")]
        public string ConfirmPassword { get; set; }
    }

    public static partial class MapperHelper
    {
        public static void FromModel(this UserViewModel obj, AppUser model)
        {
            obj.Id = model.Id;
            obj.UserName = model.UserName;
            obj.Email = model.Email;
            obj.PhoneNumber = model.PhoneNumber;
            obj.LockoutEndDateUtc = model.LockoutEndDateUtc;
            obj.CreatedDate = model.CreatedDate;

            obj.Profile.UserId = model.UserId;
            obj.Profile.FullName = model.FullName;
            obj.Profile.Status = model.Status;
            obj.Profile.UserType = model.UserType;
            obj.Profile.IsNew = model.IsNew;
            obj.Profile.Note = model.Note;

        }

        public static void FromViewModel(this AppUser obj, AddUserViewModel model)
        {
            obj.Email = model.Email.TrimEmpty();
            obj.UserName = model.UserName.Trim().ToLower();
            obj.PhoneNumber = model.PhoneNumber.TrimEmpty();

            obj.FullName = model.FullName.Trim();
        }

        public static void FromModel(this UpdateUserViewModel obj, AppUser model)
        {
            obj.Id = model.Id;
            obj.UserName = model.UserName;
            obj.Email = model.Email;
            obj.PhoneNumber = model.PhoneNumber;
            obj.Status = model.Status;

            obj.FullName = model.FullName;
            obj.IsActive = model.Status == AccountStatus.Active;
        }

        public static void FromViewModel(this AppUser obj, UpdateUserViewModel model)
        {
            obj.Email = model.Email.TrimEmpty();
            obj.PhoneNumber = model.PhoneNumber.TrimEmpty();

            obj.FullName = model.FullName.Trim();
        }
    }

    public partial class ListUserViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public List<string> Roles { get; set; }

        public Guid IdLogin { get; set; }

    }
}