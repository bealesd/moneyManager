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

        public AdapterRepo(IUserRepo userRepo, IAccountRepo accountRepo)
        {
            _userRepo = userRepo;
            _accountRepo = accountRepo;
        }

        public IUser GetUser(string username)
        {
            return _userRepo.GetUser(username);
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public void CreateUser(string username)
        {
            _userRepo.CreateUser(username);
        }

        public void DeleteUser(string username)
        {
            this.GetUser(username).AccountGuid.ForEach(a => this.DeleteAccount(a));
            _userRepo.DeleteUser(username);
        }

        public IAccount GetMoneyAccount(Guid accountGuid)
        {
            return _accountRepo.GetMoneyAccount(accountGuid);
        }

        private Guid CreateMoneyAccount(string accountName)
        {
            return _accountRepo.CreateMoneyAccount(accountName);
        }

        public void CreateMoneyAccountForUser(string username, string accountName)
        {
            _userRepo.AddAccountToUser(username, CreateMoneyAccount(accountName));
        }

        private void DeleteAccount(Guid accountGuid)
        {
            _accountRepo.DeleteMoneyAccount(accountGuid);
        }

        public void RemoveAccount(string username, Guid accountGuid)
        {
            _userRepo.RemoveAccount(username, accountGuid);
        }

        public void CreateTransaction(Guid accountGuid, string itemName, float itemCost, DateTime dateTime)
        {
            if (!itemCost.ValidFloat())
                throw new Exception("Invalid Number.");

            _accountRepo.CreateMoneySpentItem(accountGuid, itemName, itemCost, dateTime);
        }

        public void DeleteTransaction(Guid accountGuid, Guid moneyItemGuid)
        {
            _accountRepo.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
        }
    }
}