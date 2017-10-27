using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Models
{
    public class MoneySpentItem
    {
        public string ItemName { get; set; }
        public float ItemCost { get; set; }
        public Guid AccountGuid { get; set; }
    }
}
