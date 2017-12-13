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
        {
            return username == String.Empty ? null : _userRepo.GetUser(username);
        }

        public IEnumerable<IUser> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public bool AddUser(string username)
        {
            return _userRepo.AddUser(username);
        }

        public IAccount GetAccount(string username, string accountName)
        {
            var user = _userRepo.GetUser(username);
            if (user == null) return null;

            Guid accountGuid = user.AccountGuid.FirstOrDefault(guid => _accountRepo.GetAccount(guid).AccountName == accountName);
            if (accountGuid == Guid.Empty) return null;

            return _accountRepo.GetAccount(accountGuid);
        }

        public bool AddNewAccount(string username, string accountName)
        {
            User user = _userRepo.GetUser(username);
            if (user == null)
                return false;
            
            if (user.AccountGuid.Any(guid => _accountRepo.GetAccount(guid).AccountName == accountName))
                return false;

            _userRepo.AddAccount(username, _accountRepo.CreateAccount(accountName));
            return true;
        }

        public bool RemoveAccount(string username, string accountName)
        {
            User user = _userRepo.GetUser(username);
            if (user == null)
                return false;

            Guid accountGuid =user.AccountGuid.FirstOrDefault(guid => _accountRepo.GetAccount(guid).AccountName == accountName);
            if (accountGuid.Equals(null))
            {
                return false;
            }
            _userRepo.DeleteAccount(accountGuid, username);
            _accountRepo.DeleteAccount(accountGuid);
            return true;
        }
    }
}