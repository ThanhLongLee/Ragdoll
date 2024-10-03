using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GomdoriMagazine.Models
{
    public class CheckinViewModel
    {
        public int CurrentDayCount { get; set; }
        public List<int> CheckedDays { get; set; } = new List<int>();
    }
}