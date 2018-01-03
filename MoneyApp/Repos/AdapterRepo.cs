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

        public IUser GetUser(Guid userGuid)
        {
            if (userGuid == Guid.Empty)
            {
                throw new Exception();
            }
            HarmonizeUserMoneyAccounts(userGuid);
            return _userRepo.GetUser(userGuid);
        }

        private void HarmonizeUserMoneyAccounts(Guid userGuid)
        {
            var user = _userRepo.GetUser(userGuid);
            foreach (var accountGuid in user.AccountGuid)
            {
                try
                {
                    _accountRepo.GetMoneyAccount(accountGuid);
                }
                catch (Exception)
                {
                    this.RemoveMoneyAccountFromUser(userGuid, accountGuid);
                }
            }
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public void CreateUser(string username, Guid userGuid)
        {
            _userRepo.CreateUser(username, userGuid);
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

        private void DeleteMoneyAccount(Guid accountGuid)
        {
            _accountRepo.DeleteMoneyAccount(accountGuid);
        }

        public void RemoveMoneyAccountFromUser(Guid userGuid, Guid accountGuid)
        {
            _userRepo.RemoveAccountFromUser(userGuid, accountGuid);
            if (!this.IsLinkedAccount(accountGuid))
                this.DeleteMoneyAccount(accountGuid);
        }

        private bool IsLinkedAccount(Guid accountGuid)
        {
            var users = this.GetAllUsers();
            return users.Count(u => Equals(u.AccountGuid, accountGuid)) > 1;
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