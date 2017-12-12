using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit;
using NUnit.Framework;
using FakeItEasy;
using MoneyApp.Helper;
using MoneyApp.IO;
using MoneyApp.Models;
using MoneyApp.Repos;

namespace UnitTest.UnitTests
{
    [TestFixture]
    public class UserRepoTest
    {
        [Test]
        public void Can_Get_An_Exisiting_Valid_User()
        {
            var path = "somePath.txt";
            var username = "dave";
            IEnumerable<User> users = new List<User>()
            {
                new User() { UserGuid = Guid.NewGuid(), Username = username }
            };
            var fakeReaderWriter = A.Fake<IReaderWriter>();

            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(path)).Returns(users);

            var userRepo = new UserRepo(fakeReaderWriter, path);
            var result = userRepo.GetUser(username);

            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(path)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Can_Add_A_Valid_User()
        {
            var path = "somePath.txt";
            var username = "davebeales";
            IEnumerable<User> users = new List<User>()
            {
                new User() { UserGuid = Guid.NewGuid(), Username = username }
            };
            var fakeReaderWriter = A.Fake<IReaderWriter>();

            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(path)).Returns(null);

            var userRepo = new UserRepo(fakeReaderWriter, path);
            var isUserAdded = userRepo.AddUser(username);

            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(path)).MustHaveHappened(Repeated.Exactly.Times(1));
            A.CallTo(() => fakeReaderWriter.WriteEnumerable(A<string>.Ignored, A< IEnumerable<User>>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(1));
            Assert.That(true, Is.EqualTo(isUserAdded));
        }

        [Test]
        public void Adding_An_Account_To_An_Exisiting_User_Returns_One_Account_For_User()
        {
            var path = "somePath.txt";
            var username = "dave";
            var accountGuid = Guid.NewGuid();
            IEnumerable<User> users = new List<User>()
            {
               new User() {UserGuid = Guid.NewGuid(), Username = username, AccountGuid = new List<Guid>() }
            };
            var fakeReaderWriter = A.Fake<IReaderWriter>();
            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(path)).Returns(users);
            
            var userRepo = new UserRepo(fakeReaderWriter, path);
            userRepo.AddAccount(username, accountGuid);

            A.CallTo(() => fakeReaderWriter.WriteEnumerable(A<string>.Ignored, A<IEnumerable<User>>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(1));
            Assert.That(1, Is.EqualTo(userRepo.GetUser(username).AccountGuid.Count));
        }

        [Test]
        public void Remove_An_Exisitng_User_On_Delete_User()
        {
            var path = "somePath.txt";
            var username = "dave";
            var user = new User() {UserGuid = Guid.NewGuid(), Username = username, AccountGuid = new List<Guid>()};
            IEnumerable<User> users = new List<User>() {user};
            var fakeReaderWriter = A.Fake<IReaderWriter>();
            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(path)).Returns(users);

            var userRepo = new UserRepo(fakeReaderWriter, path);
            var isUserDeleted = userRepo.DeleteUser(username);

            Assert.That(true, Is.EqualTo(isUserDeleted));
        }
    }
}
