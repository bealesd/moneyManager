using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Framework;
using FakeItEasy;
using MoneyApp.IO;
using MoneyApp.Models;
using MoneyApp.Repos;

namespace UnitTest.UnitTests
{
    [TestFixture]
    public class UserRepoTest
    {
        //[Test]
        //public void Add_A_User_Dave_Persists_Dave_To_File()
        //{
        //    var username = "dave";
        //    var fakeUserData = new List<User>
        //    {
        //        new User() { UserGuid = Guid.NewGuid(), Username = username }
        //    };

        //    var fakeReaderWriter = A.Fake<IReaderWriter>();
        //    A.CallTo(() => fakeReaderWriter.ReadEnumerable<User>("path")).Returns(fakeUserData);

        //    var userRepo = new UserRepo(fakeReaderWriter, "path");
        //    userRepo.AddUser(username);

        //    var userGuid = userRepo.GetUser(username);

        //    A.CallTo(() => fakeReaderWriter.WriteEnumerable<User>("path", fakeUserData))
        //                    .MustHaveHappened(Repeated.AtLeast.Once);

        //    Assert.That("dave", Is.EqualTo("dave"));
        //}
    }
}
