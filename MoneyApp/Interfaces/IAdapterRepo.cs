using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IAdapterRepo
    {
        bool AddAccount(string username, string accountName);
        bool RemoveAccount(string username, string accountName);
        IAccount GetAccount(string username, string accountName);
        IEnumerable<IUser> GetAllUsers();
        IUser GetUser(string username);
        bool AddUser(string username);
    }
}