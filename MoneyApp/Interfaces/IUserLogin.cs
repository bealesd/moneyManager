using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyApp.Interfaces
{
    public interface IUserLogin
    {
        void CreateUser(string username, string password);
        Guid GetUserGuid(string username, string password);
        void DeleteUser(Guid userGuid);//string username, string password
    }
}
