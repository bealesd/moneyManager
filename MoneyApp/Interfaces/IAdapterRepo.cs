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
        IAccount GetMoneyAccount(string username, string accountName);
        IAccount CreateMoneySpentItem(string username, string accountName, string itemName, float itemCost, DateTime dateTime);
        IAccount DeleteMoneySpentItem(string username, string accountName, Guid moneyItemGuid);
        IEnumerable<IUser> GetAllUsers();
        IUser GetUser(string username);
        bool CreateUser(string username);
        bool DeleteUser(string username);
    }
}