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
        Guid GetUser(string username);
        void AddUser(string username);
        void Save();
        void Load();
    }
}
