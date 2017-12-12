using System;
using System.Collections.Generic;
using System.Text;
using FakeItEasy;
using MoneyApp.IO;
using MoneyApp.Models;
using MoneyApp.Repos;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UnitTest.UnitTests
{
    [TestFixture()]
    public class AccountRepoTest
    {
        private Account _account;
        private string _path;
        private List<Account> _accounts = new List<Account>();

        [SetUp]
        public void SettingUp()
        {
            _account = new Account() { AccountGuid = Guid.NewGuid(), AccountName = "davesAccount" };
            _accounts.Add(_account);
            _path = "somePath.txt";
        }

        [Test]
        public void Create_An_Account_Will_Add_Account()
        {
            var fakeReaderWriter = A.Fake<IReaderWriter>();
            A.CallTo(() => fakeReaderWriter.ReadEnumerable<Account>(_path)).Returns(null);

            var accountRepo = new AccountRepo(fakeReaderWriter, _path);
            var accountGuid = accountRepo.CreateAccount(_account.AccountName);

            Assert.That(_account.AccountName, Is.EqualTo(accountRepo.GetAccount(accountGuid).AccountName));
        }

        [Test]
        public void Delete_An_Exisitng_Account_Removes_Account()
        {
            var fakeReaderWriter = A.Fake<IReaderWriter>();
            A.CallTo(() => fakeReaderWriter.ReadEnumerable<Account>(_path)).Returns(_accounts);

            var accountRepo = new AccountRepo(fakeReaderWriter, _path);
            var accountGuid = accountRepo.DeleteAccount(_account.AccountGuid);
        }
    }
}
