﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using MoneyApp.Controllers;
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

            var fakeUserRepo = A.Fake<IUserRepo>();
            var fakeAccountRepo = A.Fake<IAccountRepo>();
            
            A.CallTo(() => fakeUserRepo.GetUser(username)).Returns(daveGuid);

            var userController = new UserController(fakeUserRepo, fakeAccountRepo);
            var result = userController.Get(username) as ObjectResult;

            Assert.That(daveGuid, Is.EqualTo(result.Value));
        }

        [Test]
        public void Get_Users_Returns_Two_Users()
        {
            var fakeUsers = new List<User>
            {
                new User() { UserGuid = Guid.NewGuid(), Username = "dave" },
                new User() { UserGuid = Guid.NewGuid(), Username = "ethan" }
            };

            var fakeUserRepo = A.Fake<IUserRepo>();
            var fakeAccountRepo = A.Fake<IAccountRepo>();

            A.CallTo(() => fakeUserRepo.GetAllUsers()).Returns(fakeUsers);

            var userController = new UserController(fakeUserRepo, fakeAccountRepo);
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

            var fakeUserRepo = A.Fake<IUserRepo>();
            var fakeAccountRepo = A.Fake<IAccountRepo>();
            
            A.CallTo(() => fakeUserRepo.GetUser(username)).Returns(daveGuid);

            var userController = new UserController(fakeUserRepo, fakeAccountRepo);
            var result = userController.Post(username) as RedirectToActionResult;

            A.CallTo(() => fakeUserRepo.AddUser(username)).MustHaveHappened(Repeated.AtLeast.Times(1));
            Assert.That("get", Is.EqualTo(result.ActionName.ToLower(CultureInfo.InvariantCulture)));
            Assert.That("dave", Is.EqualTo(result.RouteValues["username"]));
        }

        [Test]
        public void Add_A_New_Account_To_A_Valid_User_Returns_A_New_Account()
        {
            var username = "dave";
            var daveGuid = Guid.NewGuid();
            var account = new Account(){AccountGuid = Guid.NewGuid(), AccountName = "isa", UserGuid = daveGuid};

            var fakeUserRepo = A.Fake<IUserRepo>();
            var fakeAccountRepo = A.Fake<IAccountRepo>();

            A.CallTo(() => fakeAccountRepo.GetAccount(account.AccountName)).Returns(null);
            A.CallTo(() => fakeUserRepo.GetUser(username)).Returns(daveGuid);

            var controller = new UserController(fakeUserRepo, fakeAccountRepo);
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
            var account = new Account() { AccountGuid = Guid.NewGuid(), AccountName = "isa", UserGuid = daveGuid };

            var fakeUserRepo = A.Fake<IUserRepo>();
            var fakeAccountRepo = A.Fake<IAccountRepo>();

            A.CallTo(() => fakeUserRepo.GetUser(username)).Returns(daveGuid);
            A.CallTo(() => fakeAccountRepo.GetAccount(account.AccountName)).Returns(account);

            var userController = new UserController(fakeUserRepo, fakeAccountRepo);
            var result = userController.GetMoneyAccount(username, account.AccountName) as ObjectResult;
            var accountResult = result.Value as Account;

            Assert.That(account.AccountGuid, Is.EqualTo(accountResult.AccountGuid));
            Assert.That(daveGuid, Is.EqualTo(accountResult.UserGuid));

        }
    }
}
