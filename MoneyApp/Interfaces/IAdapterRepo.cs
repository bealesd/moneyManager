using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IAdapterRepo
    {
        void CreateMoneyAccountForUser(string username, string accountName);
        void RemoveAccount(string username, Guid accountGuid);
        IAccount GetMoneyAccount(Guid accountGuid);
        void CreateTransaction(Guid accountGuid, string itemName, float itemCost, DateTime dateTime);
        void DeleteTransaction(Guid accountGuid, Guid moneyItemGuid);
        IEnumerable<IUser> GetAllUsers();
        IUser GetUser(string username);
        void CreateUser(string username);
        void DeleteUser(string username);
    }
}