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
        private float _balance;
        public float Balance
        {
            get => _balance - ItemCost; 
            set => _balance = value; 
        }

        public DateTime Datetime { get; set; }
    }
}
