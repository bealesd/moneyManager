﻿using System;
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
        private IUser _user;

        [SetUp]
        public void SettingUp()
        {
            _user = new User()
            {
                AccountGuid = new List<Guid>(),
                UserGuid = Guid.NewGuid(),
                Username = "dave"
            };
        }

        [Test]
        public void Get_User_David_Returns_A_Single_User()
        {
            var fakeAdapterRepo = A.Fake<IAdapterRepo>();
            A.CallTo(() => fakeAdapterRepo.GetUser(_user.UserGuid)).Returns(_user);

            var userController = new UserController(fakeAdapterRepo);
            var result = userController.GetUser(_user.UserGuid) as ObjectResult;
            var userResult = result.Value as IUser;

            Assert.That(_user.Username, Is.EqualTo(userResult.Username));
            Assert.That(_user.UserGuid, Is.EqualTo(userResult.UserGuid));
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
            var result = userController.GetUsers() as ObjectResult;
            var users = result.Value as List<User>;

            Assert.That(2, Is.EqualTo(users.Count));
            Assert.That("dave", Is.EqualTo(users[0].Username));
        }

        [Test]
        public void Posting_A_User_Creates_A_New_User()
        {
            var fakeAdapterRepo = A.Fake<IAdapterRepo>();
            A.CallTo(() => fakeAdapterRepo.CreateUser(_user.Username)).Returns(_user.UserGuid);

            var userController = new UserController(fakeAdapterRepo);
            var result = userController.PostUser(_user.Username) as RedirectToActionResult;

            A.CallTo(() => fakeAdapterRepo.CreateUser(_user.Username)).MustHaveHappened(Repeated.AtLeast.Times(1));
            Assert.That("getuser", Is.EqualTo(result.ActionName.ToLower(CultureInfo.InvariantCulture)));
            Assert.That(_user.Username, Is.EqualTo(result.RouteValues["username"]));
        }

        [Test]
        public void Add_A_New_Account_To_A_Valid_User_Returns_A_New_Account()
        {
            var account = new Account() { AccountGuid = Guid.NewGuid(), AccountName = "isa"};
            var fakeAdapterRepo = A.Fake<IAdapterRepo>();
            A.CallTo(() => fakeAdapterRepo.CreateMoneyAccountForUser(_user.UserGuid, account.AccountName)).Returns(true);
            A.CallTo(() => fakeAdapterRepo.GetUser(_user.UserGuid)).Returns(_user);

            var controller = new UserController(fakeAdapterRepo);
            var result = controller.CreateMoneyAccount(_user.UserGuid, account.AccountName) as RedirectToActionResult;

            Assert.That("GetUser", Is.EqualTo(result.ActionName));
            Assert.That(_user.UserGuid, Is.EqualTo(result.RouteValues["userGuid"]));
        }

        [Test]
        public void Using_A_Valid_Username_And_Account_Name_Returns_An_Account()
        {
            var account = new Account() { AccountGuid = Guid.NewGuid(), AccountName = "isa"};
            var fakeAdapterRepo = A.Fake<IAdapterRepo>();
            A.CallTo(() => fakeAdapterRepo.GetUser(_user.UserGuid)).Returns(_user);
            A.CallTo(() => fakeAdapterRepo.GetMoneyAccount(account.AccountGuid)).Returns(account);

            var userController = new UserController(fakeAdapterRepo);
            var result = userController.GetMoneyAccount(account.AccountGuid) as ObjectResult;
            var accountResult = result.Value as Account;

            Assert.That(account.AccountGuid, Is.EqualTo(accountResult.AccountGuid));
            Assert.That(account.AccountName, Is.EqualTo(accountResult.AccountName));
        }

        [Test]
        public void Delete_A_Money_Account_From_A_User_Removes_That_Account()
        {
            var account = new Account() { AccountGuid = Guid.NewGuid(), AccountName = "isa" };
            var fakeAdapterRepo = A.Fake<IAdapterRepo>();
            A.CallTo(() => fakeAdapterRepo.RemoveAccount(_user.UserGuid, account.AccountGuid)).Returns(true);

            var userController = new UserController(fakeAdapterRepo);
            var result = userController.RemoveMoneyAccountFromUser(_user.UserGuid, account.AccountGuid) as RedirectToActionResult;

            A.CallTo(() => fakeAdapterRepo.RemoveAccount(A<Guid>.Ignored, A<Guid>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
            Assert.That("GetUser", Is.EqualTo(result.ActionName));
        }
    }
}
