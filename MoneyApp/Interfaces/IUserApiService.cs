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
        UserDto GetUserDto(string username);
        void DeleteUser(string username);
        void CreateMoneyAccountForUser(Guid userGuid, string accountName);
        Account LoadMoneyAccount(Guid accountGuid);
        void DeleteMoneyAccountFromUser(Guid userGuid, Guid accountGuid);
        void CreateMoneySpentItem(Guid accouGuid, MoneySpentItemDto moneySpentItem);
        void DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid);
    }
}
