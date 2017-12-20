using System;
using System.Collections.Generic;
using MoneyApp.Models;

namespace MoneyApp.Helper
{
    public static class MoneySpentItemsHelper
    {
        public static void Update(this List<MoneySpentItem> moneySpentItems)
        {
            moneySpentItems.ForEach(m =>
            {
                if (Single.IsNaN(m.ItemCost) || m.ItemCost == 0)
                {
                    moneySpentItems.Remove(m);
                }
            });

            moneySpentItems.Sort(new MoneySpentItemComparer());
            foreach (var item in moneySpentItems)
            {
                var index = moneySpentItems.IndexOf(item);
                item.BalanceBefore = index == 0 ? 0 : moneySpentItems[index - 1].BalanceAfter;
            }
        }
    }
}