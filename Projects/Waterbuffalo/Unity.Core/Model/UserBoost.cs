using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class UserBoost
    {
        public int UserBoostID { get; set; }

        public long UserID { get; set; }

        public int BoostId { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime ActionDate { get; set; } = DateTime.Now;
    }
}
