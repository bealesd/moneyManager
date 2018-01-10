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
        void Register(string username, string password);
        void Login(string username, string password);
        void Logout();
        UserDto GetUserDto();
        void DeleteUser();
        void CreateMoneyAccountForUser(string accountName);
        Account LoadMoneyAccount(Guid accountGuid);
        void DeleteAccount(Guid accountGuid);
        void CreateMoneySpentItem(Guid accouGuid, MoneySpentItemDto moneySpentItem);
        void DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid);
    }
}
