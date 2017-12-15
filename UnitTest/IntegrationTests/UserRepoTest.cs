using System.IO;
using System.Linq;
using MoneyApp;
using MoneyApp.IO;
using MoneyApp.Models;
using MoneyApp.Repos;
using NUnit.Framework;

namespace UnitTest.IntegrationTests
{
    [TestFixture]
    public class UserRepoTest
    {
        private IUserRepo _userRepo;
        private IAccountRepo _accountRepo;
        private string userPath = new MoneyApp.Helper.Helper().TempPath("UsersTest.txt");
        private string accountPath = new MoneyApp.Helper.Helper().TempPath("AccountTest.txt");

        [SetUp]
        [TearDown]
        public void ClearTempData()
        {
            if (File.Exists(userPath))
                File.Delete(userPath);
            if (File.Exists(accountPath))
                File.Delete(accountPath);
        }

        [Test]
        public void Add_New_User()
        {
            _userRepo = new UserRepo(new JsonReaderWriter(), userPath);

            var username = "davebeales";
            _userRepo.CreateUser(username);

            var users = _userRepo.GetAllUsers().ToList();
            var user = users.FirstOrDefault(u => u.Username == username);

            Assert.AreEqual(username, user.Username);
        }

        [Test]
        public void Add_Two_New_Users()
        {
            _userRepo = new UserRepo(new JsonReaderWriter(), userPath);

            var david = "davebeales";
            _userRepo.CreateUser(david);

            var bob = "bobMarley";
            _userRepo.CreateUser(bob);

            var users = _userRepo.GetAllUsers().ToList();
            var userDavid = users.FirstOrDefault(u => u.Username == david);
            var userBob = users.FirstOrDefault(u => u.Username == bob);

            Assert.AreEqual(david, userDavid.Username);
            Assert.AreEqual(bob, userBob.Username);
            Assert.AreEqual(2, users.Count());
        }

        [Test]
        public void Add_Two_Users_And_Account_To_A_User()
        {
            //_userRepo = new UserRepo(new JsonReaderWriter(), userPath);
            //_accountRepo = new AccountRepo(new JsonReaderWriter(), accountPath);

            //var david = "davebeales";
            //_userRepo.AddAccountToUser(david);

            //var users = _userRepo.GetAllUsers().ToList();
            //var userDavid = users.FirstOrDefault(u => u.Username == david);

            //_accountRepo.CreateMoneyAccountForUser("daveAccount", userDavid.UserGuid);
            //var account = _accountRepo.GetMoneyAccount("daveAccount");
            
            //Assert.AreEqual(account.UserGuid, userDavid.UserGuid);
        }

        [Test]
        public void Add_Two_Accounts_To_A_User()
        {
            //_userRepo = new UserRepo(new JsonReaderWriter(), userPath);
            //_accountRepo = new AccountRepo(new JsonReaderWriter(), accountPath);

            //var david = "davebeales";
            //_userRepo.AddAccountToUser(david);

            //var users = _userRepo.GetAllUsers().ToList();
            //var userDavid = users.FirstOrDefault(u => u.Username == david);

            //_accountRepo.CreateMoneyAccountForUser("davidAccount", userDavid.UserGuid);
            //_accountRepo.CreateMoneyAccountForUser("davidAccount2", userDavid.UserGuid);

            //var davidAccount = _accountRepo.GetMoneyAccount("davidAccount");
            //var davidAccount2 = _accountRepo.GetMoneyAccount("davidAccount2");

            //Assert.AreEqual("davidAccount", davidAccount.AccountName);
            //Assert.AreEqual("davidAccount2", davidAccount2.AccountName);
        }
    }
}
