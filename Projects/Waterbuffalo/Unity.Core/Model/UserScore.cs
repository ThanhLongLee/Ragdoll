using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class UserScore
    {
        public int ScoreID { get; set; }
        public long UserID { get; set; }
        public int Score { get; set; }
        public int RankID { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
