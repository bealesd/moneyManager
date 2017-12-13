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
                MoneySpentItems = new List<MoneySpentItem>()
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
            if (Object.Equals(null, account))
                return false;
            _accounts.Remove(account);
            Save();
            return true;
        }

        public Account AddMoneySpentItem(Guid accountGuid, string itemName, float itemCost, DateTime dateTime)
        {
            var account = this.GetAccount(accountGuid);
            if (Object.Equals(null, account))
                return null;

            // datetime compare to previous datetimes in list
            foreach (var currentMoneySpentItem in account.MoneySpentItems)
            {
                var result = DateTime.Compare(currentMoneySpentItem.Datetime, dateTime);
                if (result == 0)
                    return null;
            }

            account.MoneySpentItems.Add(new MoneySpentItem()
            {
                MoneySpentItemGuid = Guid.NewGuid(),
                ItemName = itemName,
                ItemCost = itemCost,
                Balance = account.AccountBalance,
                Datetime = dateTime
            });
            account.AccountBalance -= itemCost;
            Save();
            return account;
        }

        public Account RemoveMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            var account = this.GetAccount(accountGuid);
            if (Object.Equals(null, account))
                return null;

            var moneySpentItem = account.MoneySpentItems.FirstOrDefault(m => m.MoneySpentItemGuid == moneyItemGuid);
            if (Object.Equals(moneySpentItem, null))
                return account;

            var moneySpentItemCost =  moneySpentItem.ItemCost;
            account.AccountBalance += moneySpentItemCost;

            // TODO: change in balance needs to adjust the balance attribute of each moneySpentItem.
            // TODO: will iterate through each item, assuming not ordered and where datetime is after change, add balance change to balance property.
            foreach (var currentMoneySpentItem in account.MoneySpentItems)
            {
                var result = DateTime.Compare(currentMoneySpentItem.Datetime, moneySpentItem.Datetime);
                if (result == 1)
                    currentMoneySpentItem.Balance += moneySpentItemCost;
            }

            account.MoneySpentItems.Remove(moneySpentItem);
            Save();
            return account;
        }
    }
}