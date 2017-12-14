﻿using System.Collections.Generic;
using MoneyApp.Models;

namespace MoneyApp.Helper
{
    public static class MoneySpentItemsHelper
    {
        public static void Update(this List<MoneySpentItem> moneySpentItems)
        {
            moneySpentItems.Sort((IComparer<MoneySpentItem>) new MoneySpentItemComparer());
            foreach (var item in moneySpentItems)
            {
                var index = moneySpentItems.IndexOf(item);
                item.BalanceBefore = index == 0 ? 0 : moneySpentItems[index - 1].BalanceAfter;
            }
        }
    }
}