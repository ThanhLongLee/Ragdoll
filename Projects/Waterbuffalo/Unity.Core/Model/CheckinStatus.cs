using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class CheckinStatus
    {
        public int CurrentDayCount { get; set; }
        public List<int> CheckedDays { get; set; }
    }
}
