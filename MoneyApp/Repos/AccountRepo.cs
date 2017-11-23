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
        public void CreateAccount(string accountName, Guid userGuid)
        {
            var account = new Account()
            {
                AccountGuid = Guid.NewGuid(),
                AccountName = accountName,
                UserGuid = userGuid
            };
            _accounts.Add(account);

            Save();
        }

        public Account GetAccount(string accountName)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountName == accountName);
            return account;
        }
    }
}
