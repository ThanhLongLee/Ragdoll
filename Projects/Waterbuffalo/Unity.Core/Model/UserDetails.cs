using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class UserDetails
    {
        public long RowNum { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Score { get; set; }
        public string RankName { get; set; }
        public int RankCount { get; set; }
        public int PointAdd { get; set; }
        public int CurrentRankPosition { get; set; }
        public int click { get; set; }
        public long MaxPoint { get; set; }
        public long MinPoint { get; set; }
        public int TotalUsers { get; set; }
    }
}
