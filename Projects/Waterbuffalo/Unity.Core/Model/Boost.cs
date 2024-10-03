using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class Boost
    {
        public int BoostId { get; set; }

        public int ScoreAwarded { get; set; }

        public int Click { get; set; }

        public bool IsCompleted { get; set; }
    }
}
