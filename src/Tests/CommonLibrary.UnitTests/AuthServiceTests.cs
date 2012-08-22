using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;


using ComLib.Authentication;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class AuthTests
    {
        [Test]
        public void CanUseWindowsAuth()
        {
            Auth.Init(new AuthWin());
            Assert.IsTrue(Auth.IsAuthenticated());
            Assert.IsTrue(!Auth.IsGuest());
            Assert.AreEqual(Auth.UserShortName, Environment.UserName);
        }


        [Test]
        public void CanUseCustomAuth()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "kishore", "admin;power", "custom", true)));
            Assert.IsTrue(Auth.IsAdmin());
            Assert.IsTrue(Auth.IsAuthenticated());
            Assert.IsTrue(Auth.IsUser("kishore"));
            Assert.IsTrue(Auth.IsUserInRoles("admin;power"));
            Assert.IsTrue(Auth.IsUserOrAdmin("kishore"));
        }


        [Test]
        public void CanUseCustomAuthNegative()
        {
            Auth.Init(new AuthWin("admin", new UserPrincipal(1, "sheela", "moderator", "custom", false)));
            Assert.IsFalse(Auth.IsAdmin());
            Assert.IsFalse(Auth.IsAuthenticated());
            Assert.IsFalse(Auth.IsUser("kishore"));
            Assert.IsFalse(Auth.IsUserInRoles("admin;power"));
            Assert.IsFalse(Auth.IsUserOrAdmin("kishore"));
        }
    }
}
