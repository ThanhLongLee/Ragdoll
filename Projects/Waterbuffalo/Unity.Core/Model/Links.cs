using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class Links
    {
        public int LinkID { get; set; }
        public string LinkUrl { get; set; }

        public int ScoreAwarded { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
