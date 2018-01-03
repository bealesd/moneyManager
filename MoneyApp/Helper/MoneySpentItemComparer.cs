using System;
using System.Collections.Generic;
using MoneyApp.Models;

namespace MoneyApp.Helper
{
    public class MoneySpentItemComparer : IComparer<MoneySpentItem>
    {
        public int Compare(MoneySpentItem item1, MoneySpentItem item2)
        {
            return DateTime.Compare(item2.Datetime, item1.Datetime);
        }
    }
}