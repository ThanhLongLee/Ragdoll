using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GomdoriMagazine.Models
{
    public class UserInfoViewModel
    {
        public UserDetails UserDetails { get; set; }
        public List<UserDetails> Top10Users { get; set; }
    }
}