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
    }
}
