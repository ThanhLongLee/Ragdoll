using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class UserCheckins
    {
        public int CheckinId { get; set; }
        public long UserId { get; set; }
        public DateTime CheckinDate { get; set; }
        public int PointsAwarded { get; set; }
    }
}
