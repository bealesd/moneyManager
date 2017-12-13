using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IAdapterRepo
    {
        bool AddNewAccount(string username, string accountName);
        bool RemoveAccount(string username, string accountName);
        IAccount GetAccount(string username, string accountName);
        IAccount AddMoneySpentItem(string username, string accountName, string itemName, float itemCost, DateTime dateTime);
        IAccount RemoveMoneySpentItem(string username, string accountName, Guid moneyItemGuid);
        IEnumerable<IUser> GetAllUsers();
        IUser GetUser(string username);
        bool AddUser(string username);
    }
}