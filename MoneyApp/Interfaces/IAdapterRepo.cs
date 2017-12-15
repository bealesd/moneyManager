using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IAdapterRepo
    {
        bool CreateMoneyAccountForUser(Guid userGuid, string accountName);
        bool RemoveMoneyAccountFromUser(Guid userGuid, Guid accountGuid);
        IAccount GetMoneyAccount(Guid accountGuid);
        IAccount CreateMoneySpentItem(Guid accountGuid, string itemName, float itemCost, DateTime dateTime);
        IAccount DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid);
        IEnumerable<IUser> GetAllUsers();
        IUser GetUser(Guid userGuid);
        Guid CreateUser(string username);
        bool DeleteUser(Guid userGuid);
    }
}