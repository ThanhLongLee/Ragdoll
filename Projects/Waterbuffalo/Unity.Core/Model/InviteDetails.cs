using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class InviteDetails
    {
        public int InviteID { get; set; }
        public long SenderID { get; set; }
        public long ReceiverID { get; set; }
        public bool IsSuccessful { get; set; }
        public string ReceiverFirstname { get; set; }
        public string ReceiverLastname { get; set; }
        public string ReceiverUsername { get; set; }
    }
}
