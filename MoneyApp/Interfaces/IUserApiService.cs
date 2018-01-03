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
        IUser CreateUser(string username, string password);
        UserDto GetUserDto(Guid userGuid);
        Guid GetUserGuid(string username, string password);
        void DeleteUser(Guid userGuid);
        void CreateMoneyAccountForUser(string accountName, Guid userGuid); //Guid userGuid, string accountName);
        Account LoadMoneyAccount(Guid accountGuid);
        void DeleteMoneyAccountFromUser(Guid userGuid, Guid accountGuid);
        void CreateMoneySpentItem(Guid accouGuid, MoneySpentItemDto moneySpentItem);
        void DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid);
    }
}
