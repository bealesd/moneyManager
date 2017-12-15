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
        User GetUser(Guid userGuid);
        bool CreateUser(string username);
        bool DeleteUser(Guid userGuid);
        void Save();
        void Load();
        void AddAccountToUser(Guid userGuid, Guid accountGuid);
        bool RemoveAccountFromUser(Guid userGuid, Guid accountGuid);
    }
}
