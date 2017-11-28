//using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
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
        private string userPath = @"C:\Users\dave\Desktop\UsersTest.txt";
        private string accountPath = @"C:\Users\dave\Desktop\AccountTest.txt";

        [Test]
        public void Test_Add_New_User()
        {
            _userRepo = new UserRepo(new JsonReaderWriter(), userPath);

            var username = "David Beales";
            _userRepo.AddUser(username);

            User user = _userRepo.GetUser(username);

            Assert.AreEqual(username, user.Username);
        }

        [Test]
        public void Test_Add_Two_New_Users()
        {
            _userRepo = new UserRepo(new JsonReaderWriter(), userPath);

            var username1 = "David Beales";
            _userRepo.AddUser(username1);

            var username2 = "Bob Marley";
            _userRepo.AddUser(username2);

            User user1 = _userRepo.GetUser(username1);
            User user2 = _userRepo.GetUser(username2);

            IEnumerable<User> users = _userRepo.GetAllUsers();

            Assert.AreEqual(username1, user1.Username);
            Assert.AreEqual(username2, user2.Username);
            Assert.AreEqual(2, users.Count());
        }

        [Test]
        public void Test_Add_Two_Users_And_Account_To_A_User()
        {
            _userRepo = new UserRepo(new JsonReaderWriter(), userPath);
            _accountRepo = new AccountRepo(new JsonReaderWriter(), accountPath);

            var david = "David Beales";
            _userRepo.AddUser(david);

            User davidUser = _userRepo.GetUser(david);
            _accountRepo.CreateAccount("daveAccount", davidUser.UserGuid);
            var account = _accountRepo.GetAccount("daveAccount");
            
            Assert.AreEqual(account.UserGuid, davidUser.UserGuid);
        }

        [Test]
        public void Test_Add_Two_Accounts_To_A_User()
        {
            _userRepo = new UserRepo(new JsonReaderWriter(), userPath);
            _accountRepo = new AccountRepo(new JsonReaderWriter(), accountPath);

            var username1 = "David Beales";
            _userRepo.AddUser(username1);

            User user1 = _userRepo.GetUser(username1);

            _accountRepo.CreateAccount("davidAccount", user1.UserGuid);
            _accountRepo.CreateAccount("davidAccount2", user1.UserGuid);

            var account1 =_accountRepo.GetAccount("davidAccount");
            var account2 = _accountRepo.GetAccount("davidAccount2");

            Assert.AreEqual("davidAccount", account1.AccountName);
            Assert.AreEqual("davidAccount2", account2.AccountName);
        }
    }
}
