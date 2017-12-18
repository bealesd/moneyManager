using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.Dto;
using MoneyApp.Models;

namespace MoneyApp.Interfaces
{
    public interface IUserApiService
    {
        IUser CreateUser(string username);
        UserDto GetUser(string username);
        Account LoadAnAccount(Guid accountGuid);
    }
}
