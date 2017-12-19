using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Models;

namespace MoneyApp.Helper
{
    public static class AccountHelper
    {
        public static void Update(this Account account)
        {
            account.AccountBalance = account.MoneySpentItems.Count != 0 
                ? account.MoneySpentItems[account.MoneySpentItems.Count - 1].BalanceAfter 
                : 0;
        }
    }
}
