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
using ComLib.Lang.Extensions;
using ComLib.Tests;
using ComLib.Lang.Tests.Common;


namespace ComLib.Lang.Tests.Integration.System
{
    [TestFixture]
    public class Script_Tests_Limits : ScriptTestsBase
    {
        private void Run(List<string> scripts, Action<Interpreter, LangSettings> initializer = null, Action<Interpreter> assertCallback = null, bool isValid = false)
        {
            foreach (var script in scripts)
            {
                var i = new Interpreter();
                if (initializer != null)
                    initializer(i, i.Context.Settings);

                i.Execute(script);

                if (isValid)
                    Assert.IsTrue(i.Result.Success);
                else
                {
                    Assert.IsFalse(i.Result.Success);
                    Assert.IsTrue(i.Result.Message.StartsWith("Limit Error"));
                }
                if (assertCallback != null)
                    assertCallback(i);
            }
        }


        [Test]
        public void Can_Set_Loop_Limit()
        {
            Run(new List<string>() { "var result = 1; for(var ndx = 0; ndx < 20; ndx++) { result = ndx; }" },
                (i, settings) => settings.MaxLoopLimit = 10,
                i => Assert.AreEqual(10, i.Memory.Get<int>("result")));
        }


        /// <summary>
        /// Note: This actually works, because 
        /// 1. the loop limt of (10) fails after "result = ndx".
        /// 2. the exception is caught in the catch
        /// 3. the loop is executed again.
        /// </summary>
        [Test]
        public void Can_Set_Loop_Limit_With_Try_Catch()
        {
            Run(new List<string>() { "var result = 0; try{ for(var ndx = result; ndx < 20; ndx++) { result = ndx; } } catch(err){ } " },
                (i, settings) => settings.MaxLoopLimit = 10,
                i => Assert.AreEqual(10, i.Memory.Get<int>("result")));
        }


        /// <summary>
        /// Note: This actually works, because 
        /// 1. the loop limt of (10) fails after "result = ndx".
        /// 2. the exception is caught in the catch
        /// 3. the loop is executed again.
        /// </summary>
        [Test]
        public void Can_Set_Loop_Limit_With_Try_Catch_Then_Loop_Again()
        {
            var i = new Interpreter();
            i.Context.Settings.MaxLoopLimit = 10;
            i.Execute("var result = 0; try{ for(var ndx = result; ndx < 20; ndx++) { result = ndx; } } catch(err){ } "
                    + "for(var ndx = result; ndx < 20; ndx++) { result = ndx; } ");

            //Assert.AreEqual(true, i.Result.Success);
            Assert.IsFalse(i.Result.Success);
            Assert.IsTrue(i.Result.Message.StartsWith("Limit Error"));
            Assert.AreEqual(10, i.Memory.Get<int>("result"));
        }


        [Test]
        public void Can_Set_Recursion_Limit()
        {
            Run(new List<string>() { "function Additive(n) { if (n == 0 )  return 0; return n + Additive(n-1); } var result = Additive(6);" },
                (i, settings) => settings.MaxCallStack = 5,
                i => Assert.IsTrue(i.Result.Message.StartsWith("Limit Error")));
        }


        [Test]
        public void Can_Set_Call_Stack_Cyclic_Limit()
        {
            Run(new List<string>() { "function Add1(n) { var l = n + 1; return Add2(l); } function Add2(n){ var l = n + 2; Add1(l); } var result = Add1(1); " },
                (i, settings) => settings.MaxCallStack = 4,
                i => Assert.IsTrue(i.Result.Message.StartsWith("Limit Error")));

            Run(new List<string>() { "function Add1(n) { var l = n + 1; return Add2(l); } function Add2(n){ if(n > 5 ) return n; var l = n + 2; Add1(l); } var result = Add1(1); " },
                null, (i) => Assert.AreEqual(8, i.Context.Memory.Get<int>("result")), true);
        }


        [Test]
        public void Can_Set_Recursion_LimitLess()
        {
            Run(new List<string>() { "function Additive(n) { if (n == 0 )  return 0; return n + Additive(n-1); } var result = Additive(15);" },
                null,
                i => Assert.AreEqual(120, i.Memory.Get<int>("result")),
                true);
        }


        [Test]
        public void Can_Set_Function_Parameters_Limit()
        {
            Run(new List<string>() { "function add(args){ return args[0]; } add(1,2,3,4,5,6,7,8,9);" },
                (i, settings) => settings.MaxFuncParams = 8);
        }


        [Test]
        public void Can_Set_Statement_Limits()
        {
            Run(new List<string>() { "var a = 1; var b = 2; var c = 3; var d = 4; var e = 5;" },
                (i, settings) => settings.MaxStatements = 4);
        }


        [Test]
        public void Can_Set_Statement_Nested_Limit()
        {
            var text = "var isActive = true; if(isActive){ while(isActive){ if(isActive) { try{ if(isActive) { isActive = false; } }catch(ex){ } } } } }";
            Run(new List<string>() { text }, (i, settings) => settings.MaxStatementsNested = 4);
        }


        [Test]
        public void Can_Set_Member_Expression_Limits()
        {
            Run(new List<string>()
            {
                "var args = [0, 1, 2]; var result = args[0][1][2][3][4][5][6];",
                "var args = [0, 1, 2]; var result = args()()()()()()()()();",
                "var args = [0, 1, 2]; var result = args.a.b.c.d.e.f.g;",
                "var args = [0, 1, 2]; var result = args[0]().a[1]().b[2]();"
            },
            (i, settings) => settings.MaxConsequetiveMemberAccess = 5);
        }


        [Test]
        public void Can_Set_Max_Expression_Limits()
        {
            Run(new List<string>()
            { 
                "var args = 1 + 2 * 3 - 4 / 5 + 6;",
                "var arrs = [ [ [ [ [ [ ['5'],'4'],'3'],'2'],'1'],'0'], '-1'];",
                "var maps = { a: { b: { c: { d: { e: { f: 6 } } } } } };",
                //"var map  = { a: '1', b: '2', c: '3', d: '4', e: '5', f: '6' };"
            },
            (i, settings) => i.Context.Settings.MaxConsequetiveExpressions = 5,
            i => Assert.IsTrue(i.Result.Message.Contains("consequetive expressions (6)")));
        }


        [Test]
        public void Can_Set_Max_Nested_Function_Calls_As_Arguments()
        {
            Run(new List<string>()
            {
                "function add(n){ return n + 1; } var result = add(add(add(add(add(add(1))))));"
            },
            (i, settings) => { i.Context.Settings.MaxFuncCallNested = 5; });
        }


        [Test]
        public void Can_Set_String_Limits()
        {
            Run(new List<string>()
            {
                "var name = 'kr'; for(var ndx = 0; ndx < 10; ndx++) name += name;",
                "var name = '0123456789'; for(var ndx = 0; ndx < 100; ndx++) name += '0123456789';",
                "var name = '0123456789'; for(var ndx = 0; ndx < 98; ndx++) name += '0123456789'; var result = name + '012345678901234567890123456789';",
            },
            (i, settings) => settings.MaxStringLength = 1000,
            i => Assert.IsTrue(i.Result.Message.StartsWith("Limit Error")));
        }


        [Test]
        public void Can_Set_Scope_Variables_Limit()
        {
            Run(new List<string>() { "var a = 1; var b = 2; var c = 3; var d = 4; var e = 5;" },
                (i, settings) => settings.MaxScopeVariables = 4);
        }


        [Test]
        public void Can_Set_Exception_Limit()
        {
            Run(new List<string>() { "var a = 0; try { throw 'error'; } catch(err) { a = 1; } try { throw 'error2'; } catch(er2) { a = 2; }" },
                null, (i) => Assert.AreEqual(2, i.Context.Memory.Get<int>("a")), true);

            Run(new List<string>() { "var a = 0; try { throw 'error'; } catch(err) { a = 1; } try { throw 'error2'; } catch(er2) { a = 2; }" },
                (i, settings) => settings.MaxExceptions = 1, (i) => Assert.AreEqual(1, i.Context.Memory.Get<int>("a")));
        }


        [Test]
        public void Can_Set_Scope_Variables_String_Length_Total_Limit()
        {
            Run(new List<string>() { "var a = '1234567890'; var b = '12345'; var c = 3; var d = '123456789012345'; var e = 5;" },
                (i, settings) => settings.MaxScopeStringVariablesLength = 15);
        }
    }
}
