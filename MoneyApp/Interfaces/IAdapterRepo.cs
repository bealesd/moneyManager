using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IAdapterRepo
    {
        void CreateMoneyAccountForUser(Guid userGuid, string accountName);
        void RemoveMoneyAccountFromUser(Guid userGuid, Guid accountGuid);
        IAccount GetMoneyAccount(Guid accountGuid);
        void CreateMoneySpentItem(Guid accountGuid, string itemName, float itemCost, DateTime dateTime);
        void DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid);
        IEnumerable<IUser> GetAllUsers();
        IUser GetUser(Guid userGuid);
        //IUser UserLogin(string username, string password);
        void CreateUser(string username, Guid userGuid);
        void DeleteUser(Guid userGuid);
    }
}