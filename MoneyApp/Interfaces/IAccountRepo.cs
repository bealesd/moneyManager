using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Models;

namespace MoneyApp.Repos
{
    public interface IAccountRepo
    {
        Guid CreateAccount(string accountName);
        Account GetAccount(Guid accountGuid);
        void Load();
        void Save();
    }
}
