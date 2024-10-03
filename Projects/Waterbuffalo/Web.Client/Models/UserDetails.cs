using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GomdoriMagazine.Models
{
    public class UserDetails
    {
        public string Username { get; set; }
        public int Score { get; set; }
        public string RankName { get; set; }
        public int RankCount { get; set; }
        public int PointAdd { get; set; }
    }
}