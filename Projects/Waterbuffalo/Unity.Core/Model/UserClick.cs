using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class UserClick
    {
        public int ClickID { get; set; }
        public long UserID { get; set; }
        public DateTime ClickTime { get; set; } = DateTime.Now;
    }
}
