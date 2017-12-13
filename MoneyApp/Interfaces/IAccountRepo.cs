using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Models;

namespace MoneyApp.Repos
{
    public interface IAccountRepo
    {
        Guid CreateAccount(string accountName);
        Account GetAccount(Guid accountGuid);
        Account AddMoneySpentItem(Guid accountGuid, string itemName, float itemCost, DateTime dateTime);
        bool DeleteAccount(Guid accountGuid);
        void Load();
        void Save();
    }
}
