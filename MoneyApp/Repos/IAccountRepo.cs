using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Models;

namespace MoneyApp.Repos
{
    public interface IAccountRepo
    {
        void CreateAccount(string accountName, Guid userGuid);
        Account GetAccount(string accountName);
        void Load();
        void Save();
    }
}
