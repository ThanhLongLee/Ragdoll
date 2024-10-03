using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class Rank
    {
        public int Id { get; set; }
        public string RankName { get; set; }
        public long MinPoint { get; set; }
        public long MaxPoint { get; set; }
        public int PointAdd { get; set; }
    }
}
