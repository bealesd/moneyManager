using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IUserLogin
    {
        void CreateUser(string username, Guid userGuid);
        Guid GetUserGuid(string username);
    }
}
