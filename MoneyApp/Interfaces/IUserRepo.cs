using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Interfaces;
using MoneyApp.Models;

namespace MoneyApp.Repos
{

    public interface IUserRepo
    {
        IEnumerable<IUser> GetAllUsers();
        User GetUser(string username);
        bool CreateUser(string username);
        bool DeleteUser(string username);
        void Save();
        void Load();
        void AddAccountToUser(string username, Guid accountGuid);
        bool RemoveAccountFromUser(string username, Guid accountGuid);
    }
}
