using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using ComLib.Lang;

namespace ComLib.Lang.Tests.Unit
{
    [TestFixture]
    public class ScopeTests
    {
        [Test]
        public void Can_Add_To_Default_Scope()
        {
            var scope = new Scope();
            scope.SetValue("result", 2);

            Assert.AreEqual(0, scope.Find("result"));
            Assert.IsTrue(scope.Contains("result"));
            Assert.AreEqual(2, scope.Get<int>("result"));
        }


        [Test]
        public void Can_Push_Scope()
        {
            var scope = new Scope();
            scope.Push();
            scope.SetValue("result", 2);

            Assert.AreEqual(1, scope.Find("result"));
            Assert.IsTrue(scope.Contains("result"));
            Assert.AreEqual(2, scope.Get<int>("result"));
        }


        [Test]
        public void Can_Check_Variable_Does_Not_Exist()
        {
            var scope = new Scope();
            scope.SetValue("result", 2);

            Assert.AreEqual(-1, scope.Find("result2"));
            Assert.IsFalse(scope.Contains("result2"));
        }


        [Test]
        public void Can_Pop_Scope()
        {
            var scope = new Scope();
            scope.Push();
            scope.SetValue("result", 2);
            scope.Pop();

            Assert.AreEqual(-1, scope.Find("result"));
            Assert.IsFalse(scope.Contains("result"));
        }


        [Test]
        public void Can_Set_Value_From_Different_Scope()
        {
            var scope = new Scope();
            scope.SetValue("result", 2);
            scope.Push();

            Assert.AreEqual(0, scope.Find("result"));
            Assert.IsTrue(scope.Contains("result"));
            Assert.AreEqual(2, scope.Get<int>("result"));
        }


        [Test]
        public void Can_Set_Same_Variable_Name_In_Different_Scopes()
        {
            var scope = new Scope();
            scope.SetValue("result", 2);
            scope.Push();
            scope.SetValue("result", 3, true);

            Assert.AreEqual(1, scope.Find("result"));
            Assert.IsTrue(scope.Contains("result"));
            Assert.AreEqual(3, scope.Get<int>("result"));
        }
    }
}
