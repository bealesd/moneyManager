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

        public void AddUser(string username)
        {
            if (!_userRepo.AddUser(username))
            {
                throw new InvalidOperationException();
            }
        }

        public IAccount GetAccount(string username, string accountName)
        {
            var user = _userRepo.GetUser(username);
            if (user == null) throw new InvalidOperationException();

            Guid accountGuid = user.AccountGuid.FirstOrDefault(guid => _accountRepo.GetAccount(guid).AccountName == accountName);
            if (accountGuid == Guid.Empty) throw new InvalidOperationException();

            return _accountRepo.GetAccount(accountGuid);
        }

        public void AddAccount(string username, string accountName)
        {
            User user = _userRepo.GetUser(username);
            if (user == null) throw new InvalidOperationException();

            if (user.AccountGuid.Any(guid => _accountRepo.GetAccount(guid).AccountName == accountName))
                throw new InvalidOperationException();

            _userRepo.AddAccount(username, _accountRepo.CreateAccount(accountName));
        }
    }
}