using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class Device
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ShopId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string IPAddress { get; set; }
        public DateTime LoginDate { get; set; }
    }
}
