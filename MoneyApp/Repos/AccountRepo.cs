using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyApp.IO;
using MoneyApp.Models;

namespace MoneyApp.Repos
{
    public class AccountRepo : IAccountRepo
    {
        private IReaderWriter _readerWriter;
        private string _filePath;
        private List<Account> _accounts = new List<Account>();

        public AccountRepo(IReaderWriter readerWriter, string filePath)
        {
            _readerWriter = readerWriter;
            _filePath = filePath;
            Load();
        }
        public void Save()
        {
            _readerWriter.WriteEnumerable(_filePath, _accounts);
        }

        public void Load()
        {
            var currentAccounts = _readerWriter.ReadEnumerable<Account>(_filePath);
            if (currentAccounts != null)
                _accounts = currentAccounts.ToList();
        }
        public Guid CreateAccount(string accountName)
        {
            var account = new Account()
            {
                AccountGuid = Guid.NewGuid(),
                AccountName = accountName,
            };
            _accounts.Add(account);
            Save();
            return account.AccountGuid;
        }

        public Account GetAccount(Guid accountGuid)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountGuid == accountGuid);
            return account;
        }

        public bool DeleteAccount(Guid accountGuid)
        {
            var account = this.GetAccount(accountGuid);
            if (account.Equals(null))
                return false;
            _accounts.Remove(account);
            return true;
        }
    }
}
