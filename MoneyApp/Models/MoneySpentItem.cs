using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Models
{
    public class MoneySpentItem
    {
        
        public Guid MoneySpentItemGuid { get; set; }
        public string ItemName { get; set; }
        public float ItemCost { get; set; }
        public float BalanceBefore { get; set; }
        public float BalanceAfter { get { return BalanceBefore - ItemCost; }}
        public DateTime Datetime { get; set; }
    }
}
