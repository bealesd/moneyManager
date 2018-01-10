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
        void CreateUser(string username);
        void DeleteUser(string username);
        void Save();
        void Load();
        void AddAccountToUser(string username, Guid accountGuid);
        void RemoveAccount(string username, Guid accountGuid);
    }
}
