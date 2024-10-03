using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Admin.Models
{

    public class SignInViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [EmailAddress(ErrorMessage = "{0} không đúng.")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(32, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự", MinimumLength = 6)]
        public string Password { get; set; }
        
    }
}