using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Models;

namespace MoneyApp.Repos
{
    public interface IAccountRepo
    {
        Guid CreateMoneyAccount(string accountName);
        Account GetMoneyAccount(Guid accountGuid);
        Account CreateMoneySpentItem(Guid accountGuid, string itemName, float itemCost, DateTime dateTime);
        Account DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid);
        bool DeleteMoneyAccount(Guid accountGuid);
        void Load();
        void Save();
    }
}
