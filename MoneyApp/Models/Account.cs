using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Interfaces;

namespace MoneyApp.Models
{
    public class Account : IAccount
    {
        public string AccountName { get; set; }
        public Guid AccountGuid { get; set; }
    }
}
