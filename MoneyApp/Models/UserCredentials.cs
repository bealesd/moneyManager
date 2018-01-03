using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Models
{
    public class UserCredentials
    {
        public Guid UserGuid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        //public byte[] UserGuid { get; set; }
        //public byte[] Username { get; set; }
        //public byte[] Password { get; set; }
    }
}
