using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using MoneyApp.Controllers;
using MoneyApp.Interfaces;
using MoneyApp.Models;
using MoneyApp.Repos;
using NUnit.Framework;

namespace UnitTest.ControllerTests
{
    [TestFixture]
    public class UserControllerTest
    {
        [Test]
        public void Get_User_David_Returns_A_Single_User()
        {
            var username = "dave";
            var daveGuid = Guid.NewGuid();

            IUser user = new User()
            {
                AccountGuid = new List<Guid>(),
                UserGuid = daveGuid,
                Username = username
            };

            var fakeAdapterRepo = A.Fake<IAdapterRepo>();

            A.CallTo(() => fakeAdapterRepo.GetUser(username)).Returns(user);

            var userController = new UserController(fakeAdapterRepo);
            var result = userController.Get(username) as ObjectResult;
            var userResult = result.Value as IUser;

            Assert.That(username, Is.EqualTo(userResult.Username));
            Assert.That(daveGuid, Is.EqualTo(userResult.UserGuid));
        }

        [Test]
        public void Get_Users_Returns_Two_Users()
        {
            var fakeUsers = new List<User>
            {
                new User() { UserGuid = Guid.NewGuid(), Username = "dave" },
                new User() { UserGuid = Guid.NewGuid(), Username = "ethan" }
            };

            var fakeAdapterRepo = A.Fake<IAdapterRepo>();

            A.CallTo(() => fakeAdapterRepo.GetAllUsers()).Returns(fakeUsers);

            var userController = new UserController(fakeAdapterRepo);
            var result = userController.Get() as ObjectResult;
            var users = result.Value as List<User>;

            Assert.That(2, Is.EqualTo(users.Count));
            Assert.That("dave", Is.EqualTo(users[0].Username));
        }

        [Test]
        public void Posting_A_User_Creates_A_New_User()
        {
            var username = "dave";
            var daveGuid = Guid.NewGuid();

            var fakeAdapterRepo = A.Fake<IAdapterRepo>();

            var userController = new UserController(fakeAdapterRepo);
            var result = userController.Post(username) as RedirectToActionResult;

            A.CallTo(() => fakeAdapterRepo.AddUser(username)).MustHaveHappened(Repeated.AtLeast.Times(1));
            Assert.That("get", Is.EqualTo(result.ActionName.ToLower(CultureInfo.InvariantCulture)));
            Assert.That("dave", Is.EqualTo(result.RouteValues["username"]));
        }

        [Test]
        public void Add_A_New_Account_To_A_Valid_User_Returns_A_New_Account()
        {
            var username = "dave";
            var daveGuid = Guid.NewGuid();

            IUser user = new User()
            {
                AccountGuid = new List<Guid>(),
                UserGuid = daveGuid,
                Username = username
            };
            var account = new Account() { AccountGuid = Guid.NewGuid(), AccountName = "isa"};

            var fakeAdapterRepo = A.Fake<IAdapterRepo>();

            A.CallTo(() => fakeAdapterRepo.GetAccount(username, account.AccountName)).Returns(null);
            A.CallTo(() => fakeAdapterRepo.GetUser(username)).Returns(user);

            var controller = new UserController(fakeAdapterRepo);
            var result = controller.CreateMoneyAccount(username, account.AccountName) as RedirectToActionResult;

            Assert.That("GetMoneyAccount", Is.EqualTo(result.ActionName));
            Assert.That(account.AccountName, Is.EqualTo(result.RouteValues["accountName"]));
            Assert.That(username, Is.EqualTo(result.RouteValues["username"]));
        }

        [Test]
        public void Using_A_Valid_Username_And_Account_Name_Returns_An_Account()
        {
            var username = "dave";
            var daveGuid = Guid.NewGuid();

            IUser user = new User()
            {
                AccountGuid = new List<Guid>(),
                UserGuid = daveGuid,
                Username = username
            };

            var account = new Account() { AccountGuid = Guid.NewGuid(), AccountName = "isa"};

            var fakeAdapterRepo = A.Fake<IAdapterRepo>();

            A.CallTo(() => fakeAdapterRepo.GetUser(username)).Returns(user);
            A.CallTo(() => fakeAdapterRepo.GetAccount(username,account.AccountName)).Returns(account);

            var userController = new UserController(fakeAdapterRepo);
            var result = userController.GetMoneyAccount(username, account.AccountName) as ObjectResult;
            var accountResult = result.Value as Account;

            Assert.That(account.AccountGuid, Is.EqualTo(accountResult.AccountGuid));
            Assert.That(account.AccountName, Is.EqualTo(accountResult.AccountName));
        }
    }
}
