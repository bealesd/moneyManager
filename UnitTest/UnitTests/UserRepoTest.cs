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
        private User _user;
        private string _path;
        private List<User> _users = new List<User>();

        [SetUp]
        public void SettingUp()
        {
            _user = new User() { UserGuid = Guid.NewGuid(), Username = "davebeales", AccountGuid = new List<Guid>() };
            _path = "somePath.txt";
            _users.Add(_user);
        }

        [Test]
        public void Can_Get_An_Exisiting_Valid_User()
        {
            var fakeReaderWriter = A.Fake<IReaderWriter>();
            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(_path)).Returns(_users);

            var userRepo = new UserRepo(fakeReaderWriter, _path);
            var result = userRepo.GetUser(_user.UserGuid);

            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(_path)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Test]
        public void Can_Add_A_Valid_User()
        {
            var fakeReaderWriter = A.Fake<IReaderWriter>();
            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(_path)).Returns(null);

            var userRepo = new UserRepo(fakeReaderWriter, _path);
            var userGuid = userRepo.CreateUser(_user.Username);

            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(_path)).MustHaveHappened(Repeated.Exactly.Times(1));
            A.CallTo(() => fakeReaderWriter.WriteEnumerable(A<string>.Ignored, A< IEnumerable<User>>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(1));
        }

        [Test]
        public void Adding_An_Account_To_An_Exisiting_User_Returns_One_Account_For_User()
        {
            var accountGuid = Guid.NewGuid();
            var fakeReaderWriter = A.Fake<IReaderWriter>();
            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(_path)).Returns(_users);
            
            var userRepo = new UserRepo(fakeReaderWriter, _path);
            userRepo.AddAccountToUser(_user.UserGuid, accountGuid);

            A.CallTo(() => fakeReaderWriter.WriteEnumerable(A<string>.Ignored, A<IEnumerable<User>>.Ignored)).MustHaveHappened(Repeated.Exactly.Times(1));
            Assert.That(1, Is.EqualTo(userRepo.GetUser(_user.UserGuid).AccountGuid.Count));
        }

        [Test]
        public void Remove_An_Exisitng_User_On_Delete_User()
        {
            var fakeReaderWriter = A.Fake<IReaderWriter>();
            A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>(_path)).Returns(_users);

            var userRepo = new UserRepo(fakeReaderWriter, _path);
            var isUserDeleted = userRepo.DeleteUser(_user.UserGuid);

            Assert.That(true, Is.EqualTo(isUserDeleted));
        }
    }
}
