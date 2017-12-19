using System;
using System.Collections.Generic;
using System.Linq;
using MoneyApp.Helper;
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
            return userGuid == Guid.Empty ? throw new Exception() : _userRepo.GetUser(userGuid);
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

        public void CreateUser(string username)
        {
            var userGuid = _userRepo.CreateUser(username);
            _userLoginRepo.CreateUser(username, userGuid);
        }

        public void DeleteUser(Guid userGuid)
        {
            if (_userRepo.GetUser(userGuid).AccountGuid.Count > 0)
                throw new Exception();

            _userRepo.DeleteUser(userGuid);
        }

        public IAccount GetMoneyAccount(Guid accountGuid)
        {
            return _accountRepo.GetMoneyAccount(accountGuid);
        }

        private Guid CreateMoneyAccount(string accountName)
        {
            return _accountRepo.CreateMoneyAccount(accountName);
        }

        public void CreateMoneyAccountForUser(Guid userGuid, string accountName)
        {
            User user = _userRepo.GetUser(userGuid);
            if (user.AccountGuid.Any(guid => _accountRepo.GetMoneyAccount(guid).AccountName == accountName))
                throw new Exception();

            _userRepo.AddAccountToUser(userGuid, CreateMoneyAccount(accountName));
        }

        public void DeleteMoneyAccount(Guid accountGuid)
        {
            _accountRepo.DeleteMoneyAccount(accountGuid);
        }

        public void RemoveMoneyAccountFromUser(Guid userGuid, Guid accountGuid)
        {
            _userRepo.RemoveAccountFromUser(userGuid, accountGuid);
        }

        public void CreateMoneySpentItem(Guid accountGuid, string itemName, float itemCost, DateTime dateTime)
        {
            if (!itemCost.ValidFloat())
                throw new Exception();

            _accountRepo.CreateMoneySpentItem(accountGuid, itemName, itemCost, dateTime);
        }

        public void DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            _accountRepo.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
        }
    }
}