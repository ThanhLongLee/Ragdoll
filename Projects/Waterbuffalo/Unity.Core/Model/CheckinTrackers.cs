using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class CheckinTrackers
    {
        public int TrackerID { get; set; }
        public long UserID { get; set; }
        public DateTime LastCheckinDate { get; set; }
        public int CurrentDayCount { get; set; }
    }
}
