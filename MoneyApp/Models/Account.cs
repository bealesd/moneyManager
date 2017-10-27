using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Models
{
    public class Account
    {
        public string AccountName { get; set; }
        public Guid AccountGuid { get; set; }
        public Guid UserGuid { get; set; }
    }
}
