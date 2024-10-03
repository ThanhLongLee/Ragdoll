using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class UserLink
    {
        public int UserLinkID { get; set; }
        public long UserID { get; set; }
        public int LinkID { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; } = false;
    }
}
