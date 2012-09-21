using System;
using System.Collections.Generic;
using NUnit.Framework;


using ComLib.Lang.Core;
using ComLib.Lang.Types;
using ComLib.Lang.Parsing;
using ComLib.Lang.AST;
using ComLib.Tests;
using ComLib.Lang.Tests.Common;

namespace ComLib.Lang.Tests.Unit
{

    public class Lang_Type_Tests
    {
        public FunctionCallExpr BuildFuncCallExpr(Context ctx, string varName, string memName, List<object> args)
        {
            var exp = new FunctionCallExpr();
            var memExp = new MemberAccessExpr(new VariableExpr(varName), memName, false);
            memExp.Ctx = ctx;
            ((VariableExpr)memExp.VariableExp).Ctx = ctx;
            exp.NameExp = memExp;
            exp.ParamList = args;
            exp.Ctx = ctx;
            return exp;
        }
    }



    [TestFixture]
    public class Lang_LArray2_Tests : Lang_Type_Tests
    {
        private LArray BuildTestArray1()
        {
            return new LArray(new List<object>() { "a", "b", "c", "d" });
        }


        private LArray BuildTestArray2()
        {
            return new LArray(new List<object>() { "a", "b", "c", "d", "b" });
        }


        private void CheckArray(LArray larray, int newLength, object[] expectedItems)
        {
            var array = larray.Raw;
            Assert.AreEqual(array.Count, newLength);
            var ndx = 0;
            foreach (var item in array)
            {
                var expected = expectedItems[ndx];
                Assert.AreEqual(expected, item);
                ndx++;
            }
        }


        [Test]
        public void Can_Test_Methods()
        {
            var lsMethods = new LJSArrayMethods();
            lsMethods.Init();
                       
            // Concat
            var concat1 = BuildTestArray1();
            CheckArray(lsMethods.Concat(concat1, new List<object>() { "e", "f" }), 6, new object []{ "a", "b", "c", "d", "e", "f" });
            Assert.AreEqual(concat1.Raw.Count, 4);

            // Index of
            Assert.AreEqual(2, lsMethods.IndexOf(BuildTestArray1(), "c", 0));
            Assert.AreEqual(4, lsMethods.IndexOf(BuildTestArray2(), "b", 2));
            
            // Join
            Assert.AreEqual("a,b,c,d", lsMethods.Join(BuildTestArray1()));

            // Last index of
            Assert.AreEqual(4, lsMethods.LastIndexOf(BuildTestArray2(), "b", 0));
            Assert.AreEqual(4, lsMethods.LastIndexOf(BuildTestArray2(), "b", 2));

            // Pop
            Assert.AreEqual("d", lsMethods.Pop(BuildTestArray1()));
            
            // Push
            var a1 = BuildTestArray1();
            lsMethods.Push(a1, "e", "f");
            CheckArray(a1, 6, new object[]{"a", "b", "c", "d", "e", "f"});

            // Reverse
            CheckArray(lsMethods.Reverse(BuildTestArray1()), 4, new object[] { "d", "c", "b", "a" });

            // Shift
            Assert.AreEqual("a", lsMethods.Shift(BuildTestArray1()));

            // Slice
            CheckArray(lsMethods.Slice(BuildTestArray1(), 1, -1), 3, new object[] { "b", "c", "d" });
            
            // Sort ?? TODO: how to pass in lambda?

            // Splice
            CheckArray(lsMethods.Splice(BuildTestArray1(), 1, 2, new object[] { "e", "f" }), 2, new object[]{ "b", "c"});

            var access1 = BuildTestArray1();
            Assert.AreEqual("b", lsMethods.IndexerGet(access1, 1));

            var access2 = BuildTestArray1();
            lsMethods.IndexerSet(access2, 1, "k");
            Assert.AreEqual("k", lsMethods.IndexerGet(access2, 1));
        }
    }



    [TestFixture]
    public class Lang_LString2_Tests : Lang_Type_Tests
    {
        [Test]
        public void Can_Test_Methods()
        {
            var lsMethods = new LJSStringMethods();
            lsMethods.Init();

            Assert.AreEqual("u",                lsMethods.CharAt("fluent_script", 2));
            Assert.AreEqual("fluent_script",    lsMethods.Concat("fluent", new []{ "_", "script"}));
            Assert.AreEqual(6,                  lsMethods.IndexOf("fluent_script", "_script", 0));
            Assert.AreEqual(6,                  lsMethods.LastIndexOf("fluent_script", "_script", -1));
            Assert.AreEqual(13,                 lsMethods.Length("fluent_script"));
            Assert.AreEqual("fluent_fluent",    lsMethods.Replace("fluent_script", "script", "fluent"));
            Assert.AreEqual(7,                  lsMethods.Search("fluent_script", "script"));
            Assert.AreEqual("_sc",              lsMethods.Substr("fluent_script", 6, 3));
            Assert.AreEqual("_sc",              lsMethods.Substring("fluent_script", 6, 8));
            Assert.AreEqual("fluent_script",    lsMethods.ToLowerCase("Fluent_script"));
            Assert.AreEqual("FLUENT_SCRIPT",    lsMethods.ToUpperCase("Fluent_script"));
        }


        [Test]
        public void Can_Call_Execute()
        {
            var lsMethods = new LJSStringMethods();
            lsMethods.Init();
            var ls = new LString2("abc", "fluent");
            Assert.AreEqual("u",                lsMethods.ExecuteMethod(new LString2("abc", "fluent"),          "charAt"     , new object[] { 2     }));
            Assert.AreEqual("fluent_script",    lsMethods.ExecuteMethod(new LString2("abc", "fluent"),          "concat"     , new object[] { "_", "script"}));
            Assert.AreEqual(6,                  lsMethods.ExecuteMethod(new LString2("abc", "fluent_script"),   "indexOf"    , new object[] { "_script", 0           }));
            Assert.AreEqual(6,                  lsMethods.ExecuteMethod(new LString2("abc", "fluent_script"),   "lastIndexOf", new object[] { "_script", -1          }));
            Assert.AreEqual(13,                 lsMethods.ExecuteMethod(new LString2("abc", "fluent_script"),   "length"     , null));
            Assert.AreEqual("fluent_fluent",    lsMethods.ExecuteMethod(new LString2("abc", "fluent_script"),   "replace"    , new object[] { "script", "fluent"     }));
            Assert.AreEqual(7,                  lsMethods.ExecuteMethod(new LString2("abc", "fluent_script"),   "search"     , new object[] { "script"               }));
            Assert.AreEqual("_sc",              lsMethods.ExecuteMethod(new LString2("abc", "fluent_script"),   "substr"     , new object[] { 6, 3                   }));
            Assert.AreEqual("_sc",              lsMethods.ExecuteMethod(new LString2("abc", "fluent_script"),   "substring"  , new object[] { 6, 8                   }));
            Assert.AreEqual("fluent_script",    lsMethods.ExecuteMethod(new LString2("abc", "fluent_script"),   "toLowerCase", null ));
            Assert.AreEqual("FLUENT_SCRIPT",    lsMethods.ExecuteMethod(new LString2("abc", "fluent_script"),   "toUpperCase", null ));
        }
    }


    [TestFixture]
    public class Lang_LDate2_Tests : Lang_Type_Tests
    {
        [Test]
        public void Can_Do_Date_GetMethods()
        {
            var lsmethods = new LJSDateMethods();
            lsmethods.Init();

            var date = DateTime.Now;
            var dutc = date.ToUniversalTime();
            Assert.AreEqual( date.Day,            lsmethods.GetDate             ( new LDate(date))); 
            Assert.AreEqual( (int)date.DayOfWeek, lsmethods.GetDay              ( new LDate(date))); 
            Assert.AreEqual( date.Year,           lsmethods.GetFullYear         ( new LDate(date))); 
            Assert.AreEqual( date.Hour,           lsmethods.GetHours            ( new LDate(date))); 
            Assert.AreEqual( date.Millisecond,    lsmethods.GetMilliseconds     ( new LDate(date))); 
            Assert.AreEqual( date.Minute,         lsmethods.GetMinutes          ( new LDate(date))); 
            Assert.AreEqual( date.Month,          lsmethods.GetMonth            ( new LDate(date))); 
            Assert.AreEqual( date.Second,         lsmethods.GetSeconds          ( new LDate(date)));
 
            Assert.AreEqual( dutc.Day,            lsmethods.GetUtcDate          ( new LDate(dutc))); 
            Assert.AreEqual( (int)dutc.DayOfWeek, lsmethods.GetUtcDay           ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Year,           lsmethods.GetUtcFullYear      ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Hour,           lsmethods.GetUtcHours         ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Millisecond,    lsmethods.GetUtcMilliseconds  ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Minute,         lsmethods.GetUtcMinutes       ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Month,          lsmethods.GetUtcMonth         ( new LDate(dutc)));
            Assert.AreEqual( dutc.Second,         lsmethods.GetUtcSeconds       ( new LDate(date)));
        }

        [Test]
        public void Can_Do_Date_SetMethods()
        {
            var lsmethods = new LJSDateMethods();
            lsmethods.Init();

            var date = DateTime.Now;
            var dutc = date.ToUniversalTime();
            Assert.AreEqual( date.Day,            lsmethods.SetDate             ( new LDate(date))); 
            Assert.AreEqual( date.Year,           lsmethods.SetFullYear         ( new LDate(date))); 
            Assert.AreEqual( date.Hour,           lsmethods.SetHours            ( new LDate(date))); 
            Assert.AreEqual( date.Millisecond,    lsmethods.SetMilliseconds     ( new LDate(date))); 
            Assert.AreEqual( date.Minute,         lsmethods.SetMinutes          ( new LDate(date))); 
            Assert.AreEqual( date.Month,          lsmethods.SetMonth            ( new LDate(date))); 
            Assert.AreEqual( date.Second,         lsmethods.SetSeconds          ( new LDate(date)));
 
            Assert.AreEqual( dutc.Day,            lsmethods.SetUtcDate          ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Year,           lsmethods.SetUtcFullYear      ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Hour,           lsmethods.SetUtcHours         ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Millisecond,    lsmethods.SetUtcMilliseconds  ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Minute,         lsmethods.SetUtcMinutes       ( new LDate(dutc))); 
            Assert.AreEqual( dutc.Month,          lsmethods.SetUtcMonth         ( new LDate(dutc)));
            Assert.AreEqual( dutc.Second,         lsmethods.SetUtcSeconds       ( new LDate(date)));
        }
    }



    [TestFixture]
    public class Lang_LDate_Tests : Lang_Type_Tests
    {
        [Test]
        public void Can_Do_Date_GetMethod_Tests()
        {
            var ctx = new Context();            
            var memory = new Memory();            
            DateTime date = DateTime.Now;
            DateTime dutc = date.ToUniversalTime();
            memory.SetValue("testdate", date);

            var expressions = new List<Tuple<Type, object, Expr>>()
            {
                new Tuple<Type, object, Expr>( typeof(double), date.Day,            BuildFuncCallExpr(ctx, "testdate", "getDate",            new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), (int)date.DayOfWeek, BuildFuncCallExpr(ctx, "testdate", "getDay",             new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), date.Year,           BuildFuncCallExpr(ctx, "testdate", "getFullYear",        new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), date.Hour,           BuildFuncCallExpr(ctx, "testdate", "getHours",           new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), date.Millisecond,    BuildFuncCallExpr(ctx, "testdate", "getMilliseconds",    new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), date.Minute,         BuildFuncCallExpr(ctx, "testdate", "getMinutes",         new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), date.Month,          BuildFuncCallExpr(ctx, "testdate", "getMonth",           new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), date.Second,         BuildFuncCallExpr(ctx, "testdate", "getSeconds",         new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), dutc.Day,            BuildFuncCallExpr(ctx, "testdate", "getUTCDate",         new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), (int)dutc.DayOfWeek, BuildFuncCallExpr(ctx, "testdate", "getUTCDay",          new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), dutc.Year,           BuildFuncCallExpr(ctx, "testdate", "getUTCFullYear",     new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), dutc.Hour,           BuildFuncCallExpr(ctx, "testdate", "getUTCHours",        new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), dutc.Millisecond,    BuildFuncCallExpr(ctx, "testdate", "getUTCMilliseconds", new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), dutc.Minute,         BuildFuncCallExpr(ctx, "testdate", "getUTCMinutes",      new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), dutc.Month,          BuildFuncCallExpr(ctx, "testdate", "getUTCMonth",        new List<object>())),
                new Tuple<Type, object, Expr>( typeof(double), dutc.Second,         BuildFuncCallExpr(ctx, "testdate", "getUTCSeconds",      new List<object>()))
            };
            LangTestsHelper.RunExpression2(ctx, memory, expressions);
        }


        [Test]
        public void Can_Do_Date_SetMethod_Tests()
        {
            var ctx = new Context();                        
            var memory = new Memory();
            DateTime date = DateTime.Parse("7/14/2011 10:06:27 AM");
            DateTime dutc = date.ToUniversalTime();
            memory.SetValue("testdate", date);

            var expressions = new List<Tuple<Type, object, Expr>>()
            {
                //new Tuple<Type, object, Expr>( typeof(double), 2, new FunctionCallExpr(scope, "testdate", "parse				", new List<object>() { 1 })),
                new Tuple<Type, object, Expr>( typeof(double), date.AddDays(2),           BuildFuncCallExpr(ctx, "testdate", "setDate",            new List<object>() { date.Day + 2            })),
                new Tuple<Type, object, Expr>( typeof(double), date.AddYears(2),          BuildFuncCallExpr(ctx, "testdate", "setFullYear",        new List<object>() { date.Year + 2           })),
                new Tuple<Type, object, Expr>( typeof(double), date.AddHours(2),          BuildFuncCallExpr(ctx, "testdate", "setHours",           new List<object>() { date.Hour + 2           })),
                new Tuple<Type, object, Expr>( typeof(double), date.AddMilliseconds(2),   BuildFuncCallExpr(ctx, "testdate", "setMilliseconds",    new List<object>() { date.Millisecond + 2    })),
                new Tuple<Type, object, Expr>( typeof(double), date.AddMinutes(2),        BuildFuncCallExpr(ctx, "testdate", "setMinutes",         new List<object>() { date.Minute + 2         })),
                new Tuple<Type, object, Expr>( typeof(double), date.AddMonths(2),         BuildFuncCallExpr(ctx, "testdate", "setMonth",           new List<object>() { date.Month + 2          })),
                new Tuple<Type, object, Expr>( typeof(double), date.AddSeconds(2),        BuildFuncCallExpr(ctx, "testdate", "setSeconds",         new List<object>() { date.Second + 2         })),
                //new Tuple<Type, object, Expr>( typeof(double), date.AddYears(2).Year,   BuildFuncCallExpr(ctx, "testdate", "setTime	", new List<object>() { date.Year + 2 })),
                new Tuple<Type, object, Expr>( typeof(double), dutc.AddDays(2),           BuildFuncCallExpr(ctx, "testdate", "setUTCDate",         new List<object>() { dutc.Day + 2            })),
                new Tuple<Type, object, Expr>( typeof(double), dutc.AddYears(2),          BuildFuncCallExpr(ctx, "testdate", "setUTCFullYear",     new List<object>() { dutc.Year + 2           })),
                new Tuple<Type, object, Expr>( typeof(double), dutc.AddHours(2),          BuildFuncCallExpr(ctx, "testdate", "setUTCHours",        new List<object>() { dutc.Hour + 2           })),
                new Tuple<Type, object, Expr>( typeof(double), dutc.AddMilliseconds(2),   BuildFuncCallExpr(ctx, "testdate", "setUTCMilliseconds", new List<object>() { dutc.Millisecond + 2    })),
                new Tuple<Type, object, Expr>( typeof(double), dutc.AddMinutes(2),        BuildFuncCallExpr(ctx, "testdate", "setUTCMinutes",      new List<object>() { dutc.Minute + 2         })),
                new Tuple<Type, object, Expr>( typeof(double), dutc.AddMonths(2),         BuildFuncCallExpr(ctx, "testdate", "setUTCMonth",        new List<object>() { dutc.Month + 2          })),
                new Tuple<Type, object, Expr>( typeof(double), dutc.AddSeconds(2),        BuildFuncCallExpr(ctx, "testdate", "setUTCSeconds",      new List<object>() { dutc.Second + 2         })),
            };

            ctx.Memory = memory;
            foreach (var exp in expressions)
            {
                exp.Item3.Ctx = ctx;
                ctx.Memory.SetValue("testdate", date);
                var result = exp.Item3.Evaluate();
                var newDate = exp.Item3.Ctx.Memory.Get<DateTime>("testdate");
                Assert.AreEqual(newDate, exp.Item2);
            }
        }


        [Test]
        public void Can_Do_Date_ToStringMethod_Tests()
        {

            var ctx = new Context();
            var memory = new Memory();
            DateTime date = DateTime.Parse("7/14/2011 10:06:27 AM");
            memory.SetValue("testdate", date);

            var expressions = new List<Tuple<Type, object, Expr>>()
            {
                new Tuple<Type, object, Expr>( typeof(string), date.ToString("ddd MMM dd yyyy"),                              BuildFuncCallExpr(ctx, "testdate", "toDateString",       new List<object>())),
                new Tuple<Type, object, Expr>( typeof(string), date.ToLocalTime().ToString("ddd MMM dd yyyy"),                BuildFuncCallExpr(ctx, "testdate", "toLocaleDateString", new List<object>())),
                new Tuple<Type, object, Expr>( typeof(string), date.ToLocalTime().ToString("hh mm ss"),                       BuildFuncCallExpr(ctx, "testdate", "toLocaleTimeString", new List<object>())),
                new Tuple<Type, object, Expr>( typeof(string), date.ToLocalTime().ToString("ddd MMM dd yyyy hh mm ss"),       BuildFuncCallExpr(ctx, "testdate", "toLocaleString",     new List<object>())),
                new Tuple<Type, object, Expr>( typeof(string), date.ToString("ddd MMM dd yyyy hh mm ss"),                     BuildFuncCallExpr(ctx, "testdate", "toString",           new List<object>())),
                new Tuple<Type, object, Expr>( typeof(string), date.ToString("hh mm ss"),                                     BuildFuncCallExpr(ctx, "testdate", "toTimeString",       new List<object>())),
                new Tuple<Type, object, Expr>( typeof(string), date.ToUniversalTime().ToString("ddd MMM dd yyyy hh mm ss"),   BuildFuncCallExpr(ctx, "testdate", "toUTCString",        new List<object>()))
            };
            LangTestsHelper.RunExpression2(ctx, memory, expressions);
        }

    }


    [TestFixture]
    public class Lang_LString_Tests : Lang_Type_Tests
    {
        [Test]
        public void Can_Do_String_Method_Tests()
        {
            var ctx = new Context();
            var memory = new Memory();
            memory.SetValue("email", "Common@nET");

            var expressions = new List<Tuple<Type, object, Expr>>()
            {
                //new Tuple<Type, object, Expr>(typeof(string), "a",            new FunctionCallExpr(scope, "email", "fromCharCode", new List<object>() { 1 })),
                //new Tuple<Type, object, Expr>(typeof(string), "a",                new FunctionCallExpr(scope, "email", "match",        new List<object>() { 1 })),
                //new Tuple<Type, object, Expr>(typeof(string), "a",                new FunctionCallExpr(scope, "email", "slice",        new List<object>() { 1 })),
                //new Tuple<Type, object, Expr>(typeof(string), "a",                new FunctionCallExpr(scope, "email", "split",        new List<object>() { 1 })),
                //new Tuple<Type, object, Expr>(typeof(string), "a",                new FunctionCallExpr(scope, "email", "valueOf",      new List<object>() { 1 }))
                new Tuple<Type, object, Expr>(typeof(string), "@",                BuildFuncCallExpr(ctx, "email", "charCodeAt",   new List<object>() { 6 })),
                new Tuple<Type, object, Expr>(typeof(string), "Common@nETbaby",   BuildFuncCallExpr(ctx, "email", "concat",       new List<object>() { "baby" })),
                new Tuple<Type, object, Expr>(typeof(double), 5,                  BuildFuncCallExpr(ctx, "email", "indexOf",      new List<object>() { "n" })),
                new Tuple<Type, object, Expr>(typeof(double), 7,                  BuildFuncCallExpr(ctx, "email", "lastIndexOf",  new List<object>() { "n" })),
                new Tuple<Type, object, Expr>(typeof(string), "ComLib@nET",       BuildFuncCallExpr(ctx, "email", "replace",      new List<object>() { "mon", "Lib" })),
                new Tuple<Type, object, Expr>(typeof(double), 3,                  BuildFuncCallExpr(ctx, "email", "search",       new List<object>() { "mon" })),
                new Tuple<Type, object, Expr>(typeof(string), "mmon",             BuildFuncCallExpr(ctx, "email", "substr",       new List<object>() { 2, 4 })),
                new Tuple<Type, object, Expr>(typeof(string), "mmo",              BuildFuncCallExpr(ctx, "email", "substring",    new List<object>() { 2, 4 })),
                new Tuple<Type, object, Expr>(typeof(string), "common@net",       BuildFuncCallExpr(ctx, "email", "toLowerCase",  new List<object>() { 1 })),
                new Tuple<Type, object, Expr>(typeof(string), "COMMON@NET",       BuildFuncCallExpr(ctx, "email", "toUpperCase",  new List<object>() { 1 }))                
            };
            LangTestsHelper.RunExpression2(ctx, memory, expressions);
        }
    }


    
    [TestFixture]
    public class Lang_LArray_Tests
    {
        [Test]
        public void Can_Do_Array_Expressions()
        {
            Context ctx = new Context();
            Scope scope = new Scope();

            // 1. variable name
            // 2. length
            // 3. objects
            var testcases = new List<Tuple<string, int, Type, object, Func<LArray, object>>>()
            {
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 4, typeof(LArray), "c", (arr) => arr.GetByIndex(2)),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 4, typeof(LArray), "k", (arr) => arr.SetByIndex(2, "k")),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 4, typeof(LArray), "a:b:c:d", (arr) => arr.Join(":")),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 3, typeof(LArray), "d", (arr) => arr.Pop()),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 5, typeof(LArray), 5, (arr) => arr.Push("e")),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 6, typeof(LArray), new LArray(ctx, new List<object>(){ "a", "b", "c", "d", "e", "f" }), (arr) => arr.Concat(new LArray(ctx, new List<object>() { "e", "f"  }))),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 3, typeof(LArray), "a", (arr) => arr.Shift()),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 4, typeof(LArray), new LArray(ctx, new List<object>(){ "d", "c","b", "a" }), (arr) => arr.Reverse()),                
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 4, typeof(LArray), new LArray(ctx, new List<object>(){ "b", "c" }), (arr) => arr.Slice(1,2)),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 2, typeof(LArray), new LArray(ctx, new List<object>(){ "b", "c", "d" }), (arr) => arr.Splice(1,3, "k")),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 4, typeof(LArray), "a,b,c,d", (arr) => arr.ToString()),
                new Tuple<string, int, Type, object, Func<LArray, object>>("result", 6, typeof(LArray), 6, (arr) => arr.UnShift("e", "f")),
                
            };
            Action reset = () => scope.SetValue("testarray", new LArray(ctx, new List<object>() { "a", "b", "c", "d" }));
            foreach (var testcase in testcases)
            {
                reset();
                var array = scope.Get<LArray>("testarray");
                var result = testcase.Item5(array);

                Assert.AreEqual(array.Raw.Count, testcase.Item2);

                if (testcase.Item4 is LArray)
                {
                    LArray resultArray = result as LArray;
                    LArray expectedArray = testcase.Item4 as LArray;
                    for (int ndx = 0; ndx < expectedArray.Raw.Count; ndx++)
                    {
                        var expectedVal = expectedArray.Raw[ndx];
                        var actualVal = resultArray.Raw[ndx];
                        Assert.AreEqual(expectedVal, actualVal);
                    }
                }
                else
                    Assert.AreEqual(result, testcase.Item4);
            }
        }
    }


    
    [TestFixture]
    public class Lang_Custom_DataType_Tests : Lang_Type_Tests
    {
        [Test]
        public void Can_Create_Types_Via_Context()
        {
            var context = new Context();
            context.Types.Register(typeof(Blog), null);
            var blog = context.Types.Create("Blog");

            Assert.IsNotNull(blog);
            Assert.AreEqual(blog.GetType(), typeof(Blog));
        }


        [Test]
        public void Can_Create_Types_Via_NewExpression_ShortName()
        {
            var context = new Context();
            context.Types.Register(typeof(Blog), null);
            var exp = new NewExpr();
            exp.Ctx = context;
            exp.TypeName = "Blog";
            var blog = exp.Evaluate();

            Assert.IsNotNull(blog);
            Assert.AreEqual(blog.GetType(), typeof(Blog));
        }


        [Test]
        public void Can_Create_Types_Via_NewExpression_FullName()
        {
            var context = new Context();
            context.Types.Register(typeof(Blog), null);
            var exp = new NewExpr();
            exp.Ctx = context;
            exp.TypeName = "ComLib.Tests.Blog";
            var blog = exp.Evaluate();

            Assert.IsNotNull(blog);
            Assert.AreEqual(blog.GetType(), typeof(Blog));
        }


        [Test]
        public void Can_Access_Custom_Object_Member_Properties()
        {
            var ctx = new Context();
            var bdate = DateTime.Now;
            var person = new Person("kishore", "reddy", "kishore@mail.com", true, 15.58, bdate);

            ctx.Types.Register(typeof(Person), null);
            ctx.Memory.SetValue("person", person);
            var memory = ctx.Memory;

            var expressions = new List<Tuple<Type, object, Expr>>()
            {
                new Tuple<Type, object, Expr>(typeof(string),     "kishore",          BuildFuncCallExpr(ctx, "person", "FirstName", new List<object>())),
                new Tuple<Type, object, Expr>(typeof(string),     "reddy",            BuildFuncCallExpr(ctx, "person", "LastName",  new List<object>())),
                new Tuple<Type, object, Expr>(typeof(string),     "kishore@mail.com", BuildFuncCallExpr(ctx, "person", "Email",     new List<object>())),
                new Tuple<Type, object, Expr>(typeof(bool),       true,               BuildFuncCallExpr(ctx, "person", "IsMale",    new List<object>())),
                new Tuple<Type, object, Expr>(typeof(double),     15.58,              BuildFuncCallExpr(ctx, "person", "Salary",    new List<object>())),
                new Tuple<Type, object, Expr>(typeof(DateTime),   bdate,              BuildFuncCallExpr(ctx, "person", "BirthDate", new List<object>()))   
            };
            LangTestsHelper.RunExpression2(ctx, memory, expressions);
        }


        [Test]
        public void Can_Access_Member_Method_With_No_Args()
        {
            var context = new Context();
            context.Types.Register(typeof(Person), null);
            context.Memory.SetValue("person", new Person());

            var func = BuildFuncCallExpr(context, "person", "FullName", new List<object>());
            var name = func.Evaluate();

            Assert.AreEqual(name, "john doe");
        }
    }


    [TestFixture]
    public class Lang_LMap_Tests
    {
        [Test]
        public void Can_Get_Property()
        {
            var map = new LMap(null, new Dictionary<string,object>()
            {
                { "FirstName"       ,     "john"                 },                
                { "Email"           ,     "johndoe@email.com"     },
                { "IsMale"          ,     true                   },
                { "Salary"          ,     10.5                   },
                { "BirthDate"       ,     DateTime.Today         },
                { "Address"         ,     new LMap(null, new Dictionary<string,object>()
                                          { 
                                              { "City", "Queens" },
                                              { "State", "NY"    }
                                          })
                }
            });
            Assert.AreEqual("john", map.GetValue("FirstName"));
            Assert.AreEqual("johndoe@email.com", map.GetValue("Email"));
            Assert.AreEqual(true, map.GetValue("IsMale"    ));
            Assert.AreEqual(10.5, map.GetValue("Salary"));
            Assert.AreEqual("Queens", ((LMap)map.GetValue("Address")).GetValue("City"));
            Assert.AreEqual("NY", ((LMap)map.GetValue("Address")).GetValue("State"));
            Assert.AreEqual(6, map.Length);
        }


        [Test]
        public void Can_Set_Property()
        {
            var map = new LMap(null, new Dictionary<string, object>());
            map.SetValue("FirstName", "john");
            map.SetValue("Address", new LMap(null, new Dictionary<string, object>()));
            map.GetValueAs<LMap>("Address").SetValue("City", "Queens");

            Assert.AreEqual("john", map.GetValue("FirstName"));
            Assert.AreEqual("Queens", ((LMap)map.GetValue("Address")).GetValue("City"));
            Assert.AreEqual(2, map.Length);
        }
    }
}
