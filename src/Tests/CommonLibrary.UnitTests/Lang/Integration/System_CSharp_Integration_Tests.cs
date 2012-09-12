using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;
using System.Linq.Expressions;


using ComLib;
using ComLib.Lang;
using ComLib.Lang.Plugins;
using ComLib.Tests;
using ComLib.Lang.Tests.Common;


namespace ComLib.Lang.Tests.Integration.System
{
    [TestFixture]
    public class Script_Tests_CSharp_Integration : ScriptTestsBase
    {
        [Test]
        public void Can_Call_Function_With_Params()
        {
            var func = "function add( a, b ) { return a + b; }";
            var i = new Interpreter();
            i.Parse(func);
            Assert.AreEqual(5, i.Call("add", true, 2, 3));
            Assert.AreEqual(7, i.Call("add", true, 4, 3));
            Assert.AreEqual(9, i.Call("add", true, 2, 7));        
        }


        [Test]
        public void Can_Call_Function_Without_Params()
        {
            var func = "function add() { return 2 + 3; }";
            var i = new Interpreter();
            i.Parse(func);
            Assert.AreEqual(5, i.Call("add", true));
        }


        [Test]
        public void Can_Call_Function_Using_Different_Types_Of_Params1()
        {
            var func = "function argsTest(index, num, text, date1, isActive, time1) "
                     + " { "
                     + "    if( index == 1 ) return num; "
                     + "    if( index == 2 ) return text; "
                     + "    if( index == 3 ) return date1.getMonth(); "
                     + "    if( index == 4 ) return isActive; "
                     + "    return time1.Hours; "
                     + " } ";

            var i = new Interpreter();
            i.Parse(func);
            Assert.AreEqual(12.34,           i.Call("argsTest", true, 1, 12.34, "fluent script", new DateTime(2012, 8, 10), false, new TimeSpan(9, 30, 25)));
            Assert.AreEqual("fluent script", i.Call("argsTest", true, 2, 12.34, "fluent script", new DateTime(2012, 8, 10), false, new TimeSpan(9, 30, 25)));
            Assert.AreEqual(8,               i.Call("argsTest", true, 3, 12.34, "fluent script", new DateTime(2012, 8, 10), false, new TimeSpan(9, 30, 25)));
            Assert.AreEqual(false,           i.Call("argsTest", true, 4, 12.34, "fluent script", new DateTime(2012, 8, 10), false, new TimeSpan(9, 30, 25)));
            Assert.AreEqual(9,               i.Call("argsTest", true, 5, 12.34, "fluent script", new DateTime(2012, 8, 10), false, new TimeSpan(9, 30, 25)));
        }


        [Test]
        public void Can_Call_Function_Using_Generic_List_Of_Basic_Types()
        {
            var func = "function argsTest(index, list) { return list[index]; }";
            var i = new Interpreter();
            i.Parse(func);
            Assert.AreEqual(3.3, i.Call("argsTest", true, 2, new List<double>() { 1.1, 2.2, 3.3 }));
        }


        [Test]
        public void Can_Call_Function_Using_Generic_List_Of_Objects()
        {
            var func = "function argsTest(index, list) { list.RemoveAt(1); return list[0].Name; }";
            var i = new Interpreter();
            i.Parse(func);
            Assert.AreEqual("user1", i.Call("argsTest", true, 1, new List<User>() { new User("user1"), new User("user2") }));
        }


        [Test]
        public void Can_Call_Function_Using_Dictionary()
        {
            var func = "function argsTest(usePropStyle, map) { if(usePropStyle) return map.username; return map['username'];}";
            var i = new Interpreter();
            var map = new Dictionary<string, object>();
            map["username"] = "user2";
            map["isactive"] = true;
            i.Parse(func);
            Assert.AreEqual("user2", i.Call("argsTest", true, true, map));
            Assert.AreEqual("user2", i.Call("argsTest", true, false, map));
        }
    }
}
