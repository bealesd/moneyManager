using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MoneyApp.Interfaces;

namespace MoneyApp.Models
{
    public class Account : IAccount
    {
        
        public string AccountName { get; set; }
        public Guid AccountGuid { get; set; }
        public float AccountBalance { get; set; }
        public List<MoneySpentItem> MoneySpentItems { get; set; }
    }
}
