using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IAdapterRepo
    {
        bool CreateMoneyAccount(string username, string accountName);
        bool DeleteMoneyAccount(string username, string accountName);
        IAccount GetMoneyAccount(string username, Guid accountGuid);
        IAccount CreateMoneySpentItem(string username, Guid accountGuid, string itemName, float itemCost, DateTime dateTime);
        IAccount DeleteMoneySpentItem(string username, Guid accountGuid, Guid moneyItemGuid);
        IEnumerable<IUser> GetAllUsers();
        IUser GetUser(string username);
        bool CreateUser(string username);
        bool DeleteUser(string username);
    }
}