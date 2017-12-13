using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking;
using MoneyApp.Interfaces;
using MoneyApp.IO;
using MoneyApp.Models;
using MoneyApp.Repos;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace UnitTest.UnitTests
{
    [TestFixture()]
    public class AdapterRepoTest
    {
        private string _filePath;
        private User _user;
        private List<User> _users;
        private IUserRepo _fakeIUserRepo;
        private IAccountRepo _fakeIAccountRepo;

        [SetUp]
        public void SettingUp()
        {
            _filePath = "abstractPath.txt";
            _user = new User() { UserGuid = Guid.NewGuid(), Username = "davebeales", AccountGuid = new List<Guid>() { Guid.NewGuid() } };
            _users = new List<User>()
            {
                _user,
                new User(){ UserGuid = Guid.NewGuid(), Username = "jackjones", AccountGuid = new List<Guid>() }
            };
            _fakeIUserRepo = A.Fake<IUserRepo>();
            _fakeIAccountRepo = A.Fake<IAccountRepo>();
        }

        [Test]
        public void Get_An_Existing_User_Returns_That_User()
        {
            A.CallTo(() => _fakeIUserRepo.GetUser(_user.Username)).Returns(_user);

            var adapterRepo = new AdapterRepo(_fakeIUserRepo, _fakeIAccountRepo);
            IUser user = adapterRepo.GetUser(_user.Username);

            Assert.That(_user.Username, Is.EqualTo(user.Username));
        }

        [Test]
        public void Get_All_User_Returns_Two_Users()
        {
            A.CallTo(() => _fakeIUserRepo.GetAllUsers()).Returns(_users);

            var adapterRepo = new AdapterRepo(_fakeIUserRepo, _fakeIAccountRepo);
            IEnumerable<IUser> users = adapterRepo.GetAllUsers();

            Assert.That(2, Is.EqualTo(users.Count()));
        }

        [Test]
        public void Add_User_Returns_A_User()
        {
            A.CallTo(() => _fakeIUserRepo.AddUser(_user.Username)).Returns(true);

            var adapterRepo = new AdapterRepo(_fakeIUserRepo, _fakeIAccountRepo);
            var isUserAdded = adapterRepo.AddUser(_user.Username);

            Assert.That(true, Is.EqualTo(isUserAdded));
        }

        [Test]
        public void Get_Existing_Account_Returns_Account()
        {
            var account = new Account() { AccountGuid = _user.AccountGuid[0], AccountName = "davesAccount" };
            A.CallTo(() => _fakeIUserRepo.GetUser(_user.Username)).Returns(_user);
            A.CallTo(() => _fakeIAccountRepo.GetAccount(_user.AccountGuid[0])).Returns(account);

            var adapterRepo = new AdapterRepo(_fakeIUserRepo, _fakeIAccountRepo);
            var davesAccount = adapterRepo.GetAccount(_user.Username, account.AccountName);

            Assert.That(account.AccountGuid, Is.EqualTo(davesAccount.AccountGuid));
        }

        [Test]
        public void Add_New_Account_To_User_Returns_User_With_That_Account()
        {
            var account = new Account() { AccountGuid = _user.AccountGuid[0], AccountName = "davesAccount" };
            A.CallTo(() => _fakeIUserRepo.GetUser(_users[1].Username)).Returns(_users[1]);
            A.CallTo(() => _fakeIAccountRepo.GetAccount(A<Guid>.Ignored)).Returns(null);
            A.CallTo(() => _fakeIAccountRepo.CreateAccount(account.AccountName)).Returns(account.AccountGuid);

            var adapterRepo = new AdapterRepo(_fakeIUserRepo, _fakeIAccountRepo);
            var isAccountAdded = adapterRepo.AddNewAccount(_users[1].Username, account.AccountName);

            Assert.That(true, Is.EqualTo(isAccountAdded));
        }

        [Test]
        public void Remove_An_Existing_Account_From_A_User()
        {
            var account = new Account() { AccountGuid = _user.AccountGuid[0], AccountName = "davesAccount" };
            A.CallTo(() => _fakeIUserRepo.GetUser(_user.Username)).Returns(_user);
            A.CallTo(() => _fakeIAccountRepo.GetAccount(account.AccountGuid)).Returns(account);
            A.CallTo(() => _fakeIUserRepo.DeleteAccount(account.AccountGuid, _user.Username)).Returns(true);
            A.CallTo(() => _fakeIAccountRepo.DeleteAccount(account.AccountGuid)).Returns(true);

            var adapterRepo = new AdapterRepo(_fakeIUserRepo, _fakeIAccountRepo);
            var isAccountRemoved = adapterRepo.RemoveAccount(_user.Username, account.AccountName);

            Assert.That(true, Is.EqualTo(isAccountRemoved));
        }
    }
}
