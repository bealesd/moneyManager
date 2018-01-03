using System;
using System.Collections.Generic;
using System.Linq;
using MoneyApp.Helper;
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
            _accounts = _accounts.OrderBy(a => a.AccountName).ToList();
            _readerWriter.WriteEnumerable(_filePath, _accounts);
        }

        public void Load()
        {
            var currentAccounts = _readerWriter.ReadEnumerable<Account>(_filePath);
            if (currentAccounts != null)
                _accounts = currentAccounts.ToList();
        }
        public Guid CreateMoneyAccount(string accountName)
        {
            var account = new Account()
            {
                AccountGuid = Guid.NewGuid(),
                AccountName = accountName,
                MoneySpentItems = new List<MoneySpentItem>()
            };
            _accounts.Add(account);
            Save();
            return account.AccountGuid;
        }

        public Account GetMoneyAccount(Guid accountGuid)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountGuid == accountGuid);
            if (Equals(account, null))
                throw new Exception();
            account.MoneySpentItems.Update();
            account.Update();
            Save();
            return account;
        }

        public void DeleteMoneyAccount(Guid accountGuid)
        {
            _accounts.Remove(this.GetMoneyAccount(accountGuid));
            Save();
        }

        public Account CreateMoneySpentItem(Guid accountGuid, string itemName, float itemCost, DateTime dateTime)
        {
            if (!itemCost.ValidFloat())
                throw new Exception();

            var account = this.GetMoneyAccount(accountGuid);
            if (account.MoneySpentItems.Any(m => DateTime.Compare(m.Datetime, dateTime) == 0))
                throw new Exception();

            account.MoneySpentItems.Add(new MoneySpentItem()
            {
                MoneySpentItemGuid = Guid.NewGuid(),
                ItemName = itemName,
                ItemCost = itemCost,
                Datetime = dateTime
            });

            account.MoneySpentItems.Update();
            account.Update();
            Save();
            return account;
        }

        public Account DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            var account = this.GetMoneyAccount(accountGuid);

            var moneySpentItem = account.MoneySpentItems.FirstOrDefault(m => m.MoneySpentItemGuid == moneyItemGuid);
            if (Equals(moneySpentItem, null))
                throw new Exception();

            account.MoneySpentItems.Remove(moneySpentItem);
            account.MoneySpentItems.Update();
            account.Update();
            Save();
            return account;
        }
    }
}