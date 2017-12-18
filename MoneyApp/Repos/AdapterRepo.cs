using System;
using System.Collections.Generic;
using System.Linq;
using MoneyApp.Interfaces;
using MoneyApp.Models;

namespace MoneyApp.Repos
{
    public class AdapterRepo : IAdapterRepo
    {
        private IUserRepo _userRepo;
        private IAccountRepo _accountRepo;
        private UserLoginRepo _userLoginRepo;

        public AdapterRepo(IUserRepo userRepo, IAccountRepo accountRepo, UserLoginRepo userLoginRepo)
        {
            _userRepo = userRepo;
            _accountRepo = accountRepo;
            _userLoginRepo = userLoginRepo;
        }

        public IUser GetUser(Guid userGuid)
        {
            return userGuid == Guid.Empty ? null : _userRepo.GetUser(userGuid);
        }

        public IUser UserLogin(string username)
        {// add a password, which is checked against a hashed table, if password/username correct return account Guids, which are hashed using the password.
            var userGuid = _userLoginRepo.GetUserGuid(username);
            return GetUser(userGuid);
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public Guid CreateUser(string username)
        {
            var userGuid = _userRepo.CreateUser(username);

            if (userGuid != Guid.Empty)
                _userLoginRepo.CreateUser(username, userGuid);

            return userGuid;
        }

        public bool DeleteUser(Guid userGuid)
        {
            var user = _userRepo.GetUser(userGuid);
            if (user == null)
                return false;

            if (user.AccountGuid.Count > 0)
                return false;
            return _userRepo.DeleteUser(userGuid);
        }

        public IAccount GetMoneyAccount(Guid accountGuid)
        {
            return _accountRepo.GetMoneyAccount(accountGuid);
        }

        public Guid CreateMoneyAccount(string accountName)
        {
            return _accountRepo.CreateMoneyAccount(accountName);
        }

        public bool CreateMoneyAccountForUser(Guid userGuid, string accountName)
        {
            User user = _userRepo.GetUser(userGuid);
            if (user == null)
                return false;

            if (user.AccountGuid.Any(guid => _accountRepo.GetMoneyAccount(guid).AccountName == accountName))
                return false;

            var accountGuid = CreateMoneyAccount(accountName);
            _userRepo.AddAccountToUser(userGuid, accountGuid);
            return true;
        }

        public bool DeleteMoneyAccount(Guid accountGuid)
        {
            return _accountRepo.DeleteMoneyAccount(accountGuid);
        }

        public bool RemoveMoneyAccountFromUser(Guid userGuid, Guid accountGuid)
        {
            User user = _userRepo.GetUser(userGuid);
            if (user == null)
                return false;

            _userRepo.RemoveAccountFromUser(userGuid, accountGuid);
            return true;
        }

        public IAccount CreateMoneySpentItem(Guid accountGuid, string itemName, float itemCost, DateTime dateTime)
        {
            var account = this.GetMoneyAccount(accountGuid);
            if (Object.Equals(account, null))
                return null;
            _accountRepo.CreateMoneySpentItem(account.AccountGuid, itemName, itemCost, dateTime);
            return account;
        }

        public IAccount DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            var account = this.GetMoneyAccount(accountGuid);
            if (Object.Equals(account, null))
                return null;
            _accountRepo.DeleteMoneySpentItem(account.AccountGuid, moneyItemGuid);
            return account;
        }
    }
}