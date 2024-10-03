using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.Core.Model
{
    public class UserWalletConnection
    {
        public int Id { get; set; }                 
        public long UserId { get; set; }               
        public string WalletAddress { get; set; }      
        public string ExchangeName { get; set; }       
        public DateTime ConnectTime { get; set; }     
        public DateTime? DisconnectTime { get; set; }  
        public string Status { get; set; }             
        public DateTime LastUpdated { get; set; }
    }
}
