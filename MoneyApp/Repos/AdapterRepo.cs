using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        {// add a password, which is checked against a hashed table, if password/username correct return account Guids, which are hashed using the password.
            return username == String.Empty ? null : _userRepo.GetUser(username);
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public bool CreateUser(string username)
        {
            return _userRepo.CreateUser(username);
        }

        public bool DeleteUser(string username)
        {
            if (_userRepo.GetUser(username).AccountGuid.Count > 0)
                return false;
            return _userRepo.DeleteUser(username);
        }

        public IAccount GetMoneyAccount(string username, string accountName)
        {
            var user = _userRepo.GetUser(username);
            if (user == null) return null;

            Guid accountGuid = user.AccountGuid.FirstOrDefault(guid => _accountRepo.GetMoneyAccount(guid).AccountName == accountName);
            if (accountGuid == Guid.Empty) return null;

            return _accountRepo.GetMoneyAccount(accountGuid);
        }

        public bool CreateMoneyAccount(string username, string accountName)
        {
            User user = _userRepo.GetUser(username);
            if (user == null)
                return false;
            
            if (user.AccountGuid.Any(guid => _accountRepo.GetMoneyAccount(guid).AccountName == accountName))
                return false;
            //why am i creating a user?
            _userRepo.AddAccountToUser(username, _accountRepo.CreateMoneyAccount(accountName));
            return true;
        }

        public bool DeleteMoneyAccount(string username, string accountName)
        {
            User user = _userRepo.GetUser(username);
            if (user == null)
                return false;

            Guid accountGuid =user.AccountGuid.FirstOrDefault(guid => _accountRepo.GetMoneyAccount(guid).AccountName == accountName);
            if (accountGuid == Guid.Empty)
            {
                return false;
            }
            _userRepo.RemoveAccountFromUser(username, accountGuid);
            _accountRepo.DeleteMoneyAccount(accountGuid);
            return true;
        }

        public IAccount CreateMoneySpentItem(string username, string accountName, string itemName, float itemCost, DateTime dateTime)
        {
            var account = this.GetMoneyAccount(username, accountName);
            if (Object.Equals(account, null))
                return null;
            _accountRepo.CreateMoneySpentItem(account.AccountGuid, itemName, itemCost, dateTime);
            return account;
        }

        public IAccount DeleteMoneySpentItem(string username, string accountName, Guid moneyItemGuid)
        {
            var account = this.GetMoneyAccount(username, accountName);
            if (Object.Equals(account, null))
                return null;
            _accountRepo.DeleteMoneySpentItem(account.AccountGuid, moneyItemGuid);
            return account;
        }
    }
}