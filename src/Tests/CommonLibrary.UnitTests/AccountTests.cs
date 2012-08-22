using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

using NUnit.Framework;

using ComLib;
using ComLib.Account;
using ComLib.Entities;
using ComLib.Data;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class AccountTests
    {
        [SetUp]
        public void Setup()
        {
            UserSettings settings = new UserSettings();
            settings.UserNameRegEx = @"[a-zA-Z1-9\._]{3,15}";
            settings.PasswordRegEx = "[a-zA-Z1-9]{5,15}";
            IRepository<User> repo = new UserRepository(new ConnectionInfo("Server=kishore1;Database=testdb;User=testuser1;Password=password;", "System.Data.SqlClient"));
            repo = new RepositoryInMemory<User>();

            User.Init(new UserService(repo, new UserValidator(), settings));
        }


        [Test]
        public void CanCreateUser()
        {
            User user = User.Create("kishore", "kishore@abc.com", "password", "password").Item;
            User fromDb = User.Get(user.Id);
            Assert.AreEqual(user.Id, fromDb.Id);
            Assert.AreEqual(user.UserName, fromDb.UserName);
            Assert.AreEqual(user.Email, fromDb.Email);
        }


        [Test]
        public void IsDuplicateUserName()
        {
            User user1 = User.Create("kishore", "kishore@abc.com", "password", "password").Item;
            MembershipCreateStatus status = MembershipCreateStatus.Success;

            BoolMessageItem<User> result = User.Create("kishore", "kishore1234@abc.com", "password", "password", ref status);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(status, MembershipCreateStatus.DuplicateUserName);
            
        }


        [Test]
        public void IsDuplicateEmail()
        {
            User user1 = User.Create("kishore", "kishore@abc.com", "password", "password").Item;
            MembershipCreateStatus status = MembershipCreateStatus.Success;

            BoolMessageItem<User> result = User.Create("kishore2", "kishore@abc.com", "password", "password", ref status);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(status, MembershipCreateStatus.DuplicateEmail);            
        }


        [Test]
        public void CanLogin()
        {
            User.Create("kishore", "kishore@h.com", "password", "admin");
            var result = User.LogOn("kishore", "password", false);
            var result2 = User.LogOn("kishore", "password2", false);
            Assert.IsTrue(result.Success);
            Assert.IsFalse(result2.Success);
        }


        [Test]
        public void CanVerify()
        {
            User.Create("kishore", "kishore@h.com", "password", "admin");
            var result = User.VerifyUser("kishore", "password");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Item.UserName, "kishore");
        }


        [Test]
        public void CanChangePassword()
        {
            User.Create("kishore", "kdog@g.com", "password", "admin");
            Assert.IsTrue(User.Decrypt(User.Get("kishore").Password) == "password");
            var result = User.ChangePassword("kishore", "password", "password2");
            Assert.IsTrue(result.Success);
            Assert.IsTrue(User.Decrypt(User.Get("kishore").Password) == "password2");
        }


        [Test]
        public void CanValidate()
        {
            User user = new User("", "", "aksjdlf", true);
            user.Validate();
            Assert.IsFalse(user.IsValid);
            Assert.IsNotNull(user.Errors.On("Email"));
            Assert.IsNotNull(user.Errors.On("UserName"));
        }
    }
}
