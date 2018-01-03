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
        void CreateUser(string username, Guid userGuid);
        void DeleteUser(Guid userGuid);
        void Save();
        void Load();
        void AddAccountToUser(Guid userGuid, Guid accountGuid);
        void RemoveAccountFromUser(Guid userGuid, Guid accountGuid);
    }
}
