using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;


using ComLib;
using ComLib.Lang;
using ComLib.Lang.Extensions;
using ComLib.Tests;
using ComLib.Lang.Tests.Common;


namespace ComLib.Lang.Tests.Integration.System
{
    
    [TestFixture]
    public class Script_Tests_Assignment : ScriptTestsBase
    {
        [Test]
        public void Can_Do_Single_Assignment_Constant_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("name", typeof(object), LNull.Instance,  "var name;"),
                new Tuple<string,Type, object, string>("name", typeof(string), "kishore",       "var name = 'kishore';"),
                new Tuple<string,Type, object, string>("name", typeof(string), "kishore",       "var name = \"kishore\";"),
                new Tuple<string,Type, object, string>("age", typeof(double),   32,             "var age = 32;"),
                new Tuple<string,Type, object, string>("isActive", typeof(bool), true,          "var isActive = true;"),
                new Tuple<string,Type, object, string>("isActive", typeof(bool), false,         "var isActive = false;"),
            };
            Parse(statements, replaceSemicolonsWithNewLines: true);
        }


        [Test]
        public void Can_Do_Multiple_Assignment_Constant_Expressions_In_Same_Line()
        {
            var statements = new List<Tuple<List<string>, List<Type>, List<object>, string>>()
            {                
                new Tuple<List<string>, List<Type>, List<object>, string>(new List<string>(){ "name", "age", "isActive"}, new List<Type>(){ typeof(string), typeof(double), typeof(bool)}, new List<object>(){ "kis", 32,   true },  "var name = 'kis', age = 32, isActive = true;"),
                new Tuple<List<string>, List<Type>, List<object>, string>(new List<string>(){ "name", "age", "isActive"}, new List<Type>(){ typeof(string), typeof(double), typeof(bool)}, new List<object>(){ "kis", 32,   LNull.Instance},   "var name = 'kis', age = 32, isActive;"),
                new Tuple<List<string>, List<Type>, List<object>, string>(new List<string>(){ "name", "age", "isActive"}, new List<Type>(){ typeof(string), typeof(double), typeof(bool)}, new List<object>(){ "kis", LNull.Instance, LNull.Instance },  "var name = 'kis', age, isActive;"),
                new Tuple<List<string>, List<Type>, List<object>, string>(new List<string>(){ "name", "age", "isActive"}, new List<Type>(){ typeof(string), typeof(double), typeof(bool)}, new List<object>(){ LNull.Instance,  LNull.Instance, LNull.Instance },  "var name, age, isActive;"),
            };
            for (int ndx = 0; ndx < statements.Count; ndx++)
            {
                var stmt = statements[ndx];
                var i = new Interpreter();
                i.Execute(stmt.Item4);
                for (int ndxV = 0; ndxV < stmt.Item1.Count; ndxV++)
                {
                    string varName = stmt.Item1[ndxV];
                    Type type = stmt.Item2[ndxV];
                    object val = stmt.Item3[ndxV];
                    var actualValue = i.Memory.Get<object>(varName);

                    // Check values are correct.
                    Assert.AreEqual(val, actualValue);
                }
            }
        }


        [Test]
        public void Can_Do_Single_Assignment_Constant_Math_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 8,  "var result = 4 * 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,  "var result = 6 / 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6,  "var result = 4 + 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,  "var result = 4 - 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var result = 5 % 2;")
                
            };
            Parse(statements);
        }


        [Test]
        public void Can_Handle_Escape_Chars_InString()
        {
            var i = new Interpreter();
            i.Execute("var buffer = \"\r\n<h1 class=\\\"title\\\">\";");
            var s = i.Memory.Get<string>("buffer");
            //File.WriteAllText(@"c:\temp\jsnewlines.txt", s);
            //Assert.AreEqual(s, Environment.NewLine);
        }


        [Test]
        public void Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 10,  "var result = 4 + 2 * 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 7,   "var result = 4 + 2 * 3 / 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 10,  "var result = 4 * 2 + 8 / 4;"),            
                new Tuple<string,Type, object, string>("result", typeof(double), 6,   "var result = 4 * 8 / 8 + 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4,   "var result = 4 - 2 + 8 / 4;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,   "var result = 6 - 2 * 8 / 4;"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence_With_Parenthesis()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 18,  "var result = (3 + 5 * 3);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 18,  "var result = (4 + 2) * 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 12,  "var result = (4 + 2) * (4 / 2);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 10,  "var result = 4 * (2 + 8) / 4;"),            
                new Tuple<string,Type, object, string>("result", typeof(double), 6,   "var result = 4 * (8 / 8) + 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), -1,  "var result = 4 - (2 + 8) / 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 8,   "var result = (6 - 2) * 8 / 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool),   true,"var result = (6 - 2) > 4 || ( 1 > 2 || 4 > 3 );"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Single_Assignment_Constant_Math_Expressions_With_Mixed_Types()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "comlibext",   "var result = 'comlib' + 'ext';"),
                new Tuple<string,Type, object, string>("result", typeof(string), "comlib2",     "var result = 'comlib' + 2;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "3comlib",     "var result = 3 + 'comlib';"),
                new Tuple<string,Type, object, string>("result", typeof(string), "comlibtrue",  "var result = 'comlib' + true;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "comlibfalse", "var result = 'comlib' + false;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,             "var result = 2 + false;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,             "var result = 2 + true;" ),
                new Tuple<string,Type, object, string>("result", typeof(double), "comlibnet4.5","var result = 'comlib' + 'net' + 4.5;" ),
                
            };
            Parse(statements);
        }       


        [Test]
        public void Can_Do_Complex_Addition_On_Mixed_Types()
        {
            string text = @"var age = 30.5; "
                        + "var isActive = true; "
                        + "var name = 'john'; "
                        + "var date = new Date(); "
                        + "var ids = [10,11,12]; "
                        + "var addy = { City: 'Queens', State: 'NY' }; "
                        + "var result = name + age + isActive + date + ids[1] + addy.City;";
            Console.WriteLine(text);
            var i = new Interpreter();
            i.Execute(text);
            var result = i.Memory.Get<string>("result");

            // john30.5true8/17/2011 3:14:25 PM11Queens
            Assert.IsTrue(result.StartsWith("john30.5true"));
            Assert.IsTrue(result.EndsWith("11Queens"));
            Assert.IsTrue(result.Contains(DateTime.Now.Date.ToShortDateString()));
        }


        [Test]
        public void Can_Do_Single_Assignment_Constant_Logical_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 >  2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 >= 2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 4 <  2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 4 <= 2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 2 != 2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 2 == 4 || 3 < 4;"),

                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 1 >  2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 1 >= 2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 4 <  2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 4 <= 2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 2 != 2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 2 == 4 || 3 > 4;"),

                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 <  2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 <= 2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 4 >= 2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 <= 2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 2 == 2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 2 != 4 && 3 < 4;"),

                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 1 <  2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 1 <= 2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 4 >= 2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 4 <= 2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 2 == 2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 2 <  4 && 3 == 4;")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Multiple_Assignment_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result2", typeof(string), "kishore", "var result = 'kishore'; var result2 = result;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 8,         "var result = 4; var result2 = result * 2;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 3,         "var result = 6; var result2 = result / 2;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 6,         "var result = 4; var result2 = result + 2;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 2,         "var result = 4; var result2 = result - 2;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 1,         "var result = 5; var result2 = result % 2;"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Unary_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                // var age = 20; age += 2
                // var a = 2; a += 2
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 2; result += 2; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 22, "var result = 20; result += 2"),
                new Tuple<string,Type, object, string>("result", typeof(bool),   false, "var result = 1; result = !result; "),                
                new Tuple<string,Type, object, string>("result", typeof(string), 3, "var result = 2; result++; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; result--; "),                
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; result -= 2; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 6, "var result = 2; result *= 3; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 6; result /= 2; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   false, "var result = true; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   true,  "var result = false; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   false, "var result = 'abc'; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(string), "abcdef", "var result = 'abc'; result += 'def'; "),
            };
            Parse(statements);
        }
    }

    [TestFixture]
    public class Script_Tests_Comparisons : ScriptTestsBase
    {
        [Test]
        public void Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Numbers()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 >  2;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 >= 2;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 <  6;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 <= 6;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 != 2;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 == 4;"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Strings()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'a' == 'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'a' == 'b';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'a' != 'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'a' != 'b';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'a' <  'c';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'b' <  'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'a' >  'c';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' >  'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' <= 'b';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' <= 'c';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'b' <= 'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' >= 'b';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' >= 'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'b' >= 'c';"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Bools()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true == true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true == false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true != true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true != false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true <  true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true <  false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true >  true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true >  false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true <= true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true <= false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true >= true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true >= false;")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Check_For_Nulls_Using_Complex_DataTypes()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; function add(a) { return null; }  if( add(1) == null) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; function add(a) { return 1; }     if( add(1) != null) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; var users = ['a', null, 'b'];               if(users[1] == null)   result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; var users = ['a', null, 'b'];               if(users[0] != null)   result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; var user = { name: 'kishore', age : null }; if(user.age == null )  result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; var user = { name: 'kishore', age : 32 };   if(user.age != null )  result = 1;"),                
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Check_For_Nulls_Using_Variables_Constants()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result ;    if(result == null) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(result != null) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result;     if(null == result) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(null != result) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(null == null)   result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 1; if(null != null)   result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if('abc' != null)  result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(true != null)   result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(false != null)  result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(35 != null)     result = 1;")
            };
            Parse(statements);
        }
    }



    [TestFixture]
    public class Script_Tests_If : ScriptTestsBase
    {
        [Test]
        public void Can_Use_With_Constants()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 2; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 3; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 4; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 5, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 5; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 6; }")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Use_With_Parenthesis_Without_Braces()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) result = 1"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) result = 1\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) result = 1;\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) \r\n result = 1"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) \r\n result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) \r\n result = 1\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) \r\n result = 1;\r\n")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Use_With_Parenthesis_With_Braces()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) { result = 1  }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) { result = 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) { result = 1\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) { result = 1;\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) \r\n{ \r\n result = 1 }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) \r\n{ \r\n result = 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) \r\n{ \r\n result = 1\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) \r\n{ \r\n result = 1;\r\n }\r\n")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Use_Without_Parenthesis_Without_Braces()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 then result = 1"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 then result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 then result = 1\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 then result = 1;\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 \r\n result = 1"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 \r\n result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 \r\n result = 1\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 \r\n result = 1;\r\n")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Use_Without_Parenthesis_With_Braces()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 { result = 1  }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 { result = 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 { result = 1\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 { result = 1;\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 \r\n{ \r\n result = 1 }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 \r\n{ \r\n result = 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 \r\n{ \r\n result = 1\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if 2 < 3 \r\n{ \r\n result = 1;\r\n }\r\n")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Use_With_Constants_Single_line()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 4;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 5, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 5;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 6;"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Use_Else_With_Constants()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; if( 2 < 3 && 4 > 3 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; if( 2 < 3 && 4 > 3 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; if( 2 < 3 && 4 > 3 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; if( 2 < 3 && 4 > 5 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; if( 2 < 3 && 4 > 5 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; if( 2 < 3 && 4 > 5 ) result = 1; else result = 0;"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Use_Else_If_With_Constants()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) result = 1; else if ( 3 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) result = 1; else if ( 3 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 0; if( 3 < 3 ) result = 1; else if ( 3 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 0; if( 3 < 3 ) result = 1; else if ( 3 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; if( 3 < 3 ) result = 1; else if ( 4 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; if( 3 < 3 ) result = 1; else if ( 4 < 4 ) result = 2; else result = 3;")
            };
            Parse(statements);
        }
    }




    [TestFixture]
    public class Script_Tests_Parenthesis : ScriptTestsBase
    {

        [Test]
        public void Can_Do_Complex_Conditions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2, month = 3, day = 4; if ( month < 0 || ( month == 3 && day < 5 ) ) { result--; }")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Start_With_Parenthesis()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = false, a = 1; result = ( a == 0 || a < 2);" ),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = false, a = 2; result = ( a == 0 || ( a >= 1 && a < 4 ) );" ),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = false, a = 3; result = ( a > 0 && a < 4 );" ),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,  "var result = false, a = 1; result = ( a + 4 - 2);" ),
            };
            Parse(statements);
        }
    }



    [TestFixture]
    public class Script_Tests_Loops : ScriptTestsBase
    {

        [Test]
        public void Can_Do_While_Statements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; while( result < 4 ){ result++; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 4; while( result > 1 ){ result--; }")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_While_Statements_Syntax()
        {
            RunTests(CommonTestCases_System.While, TestType.Component);
            RunTests(CommonTestCases_System.While, TestType.Integration);
        }


        [Test]
        public void Can_Do_For_Loop_Statements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; for(var ndx = 0; ndx < 5; ndx++) { result = ndx; }")                
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_For_Each_Statements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 2,     "var result = 0; var ids = [0,1,2]; for(x in ids) { result = x; }"),
                new Tuple<string,Type, object, string>("result", typeof(string), "com", "var result = 0; var ids = {a:'com', b:'com', c:'com'}; for(x in ids) { result = x.Value; }")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Break_Statements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 3; while( result < 4 ){ if( result > 2 ) { break; } result++; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 2; while( result > 1 ){ if( result < 3 ) { break; } result--; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 3; for(var ndx = 0; ndx < 5; ndx++) { if( result > 2 ) { break; } result = ndx; }")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Nested_Loops()
        {
            var i = new Interpreter();
            i.Execute("var result = 0; for(var a = 0; a < 10; a++) { for(var b = 0; b < 10; b++) { result++; } }");

            int result = i.Memory.Get<int>("result");
            Assert.AreEqual(100, result);
        }


        [Test]
        public void Can_Do_Recursion()
        {
            var i = new Interpreter();
            i.Execute("function Additive(n) { if (n == 0 )  return 0; return n + Additive(n-1); } var result = Additive(5);");

            int result = i.Memory.Get<int>("result");
            Assert.AreEqual(15, result);
        }
    }


    [TestFixture]
    public class Script_Tests_Functions : ScriptTestsBase
    {
        [Test]
        public void Can_Use_Aliases()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 11, "var result = 0; function add, inc, increment( a ) { return a + 1; } result = add( 10 );" ),
                new Tuple<string,Type, object, string>("result", typeof(double), 11, "var result = 1; function add, inc, increment( a ) { return a + 1; } result = inc( 10 );" ),
                new Tuple<string,Type, object, string>("result", typeof(double), 11, "var result = 2; function add, inc, increment( a ) { return a + 1; } result = increment( 10 );" )
            };
            Parse(statements);
        }


        [Test]
        public void Can_Return_With_Value()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1,                 "var result = 0; function test(a) { if( a < 2 ) return 1; return 2; } result = test(1);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,                 "var result = 0; function test(a) { if( a < 2 ) return 1; return 2; } result = test(2);")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Return_Without_Value()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(object), LNull.Instance,    "var result = 0; function test(a) { if( a > 2 ) return; return 2; }   result = test(3);"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Have_Implicit_Arguments_Parameter()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 2,     "var result = 0; function test(a, b)       { return arguments.length; } result = test(1, 'a');"),
                new Tuple<string,Type, object, string>("result", typeof(string), "a",   "var result = 0; function test(a, b, c)    { return arguments[a];     } result = test(1, 'a', 2);"),
                new Tuple<string,Type, object, string>("result", typeof(bool),   true,  "var result = 0; function test(a, b, c, d) { return arguments[a];     } result = test(3, 'a', 2, true);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,     "var result = 0; function test(arguments, b, c, d) { return arguments;} result = test(3, 'a', 2, true);"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Make_Calls_With_Extra_Parameters()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 4,     "var result = 0; function test(a, b)       { return arguments[a]; } result = test(2, 'a', 4);")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Make_Calls()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1,     "var result = 0; function test()     { return 1;         } result = test();"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,     "var result = 1; function test(a)    { return a + 1;      } result = test(1);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4,     "var result = 2; function test(a)    { return a + result; } result = test(2);"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,    "var result = 1; function test(a)    { return true;       } result = test(1);"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,   "var result = 1; function test(a)    { return false;      } result = test(1);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 5,     "var result = 1; function test(a, b) { return a + b;      } result = test(2,3);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,     "var result = 1; function test(a, b) { return a - b;      } result = test(4,1);"),
                new Tuple<string,Type, object, string>("result", typeof(string), "com", "var result = 1; function test()     { return 'com';      } result = test();")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Make_Calls_Without_Parenthesis()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1,     "var result = 0; function test()        { return 1;          } result = test;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,     "var result = 1; function inc(a)        { return a + 1;      } result = inc 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4,     "var result = 2; function addr(a)       { return a + result; } result = addr 2;"),                
                new Tuple<string,Type, object, string>("result", typeof(double), 3,     "var result = 1; function add2(a, b)    { return a + b;      } result = add2 1, 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6,     "var result = 1; function add3(a, b, c) { return a + b + c;  } result = add3 1, 2, 3;"),

                new Tuple<string,Type, object, string>("result", typeof(double), 1,     "var result = 0; function test()        { return 1;          } result = test;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,     "var result = 1; function inc(a)        { return a + 1;      } result = inc inc(1);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6,     "var result = 2; function addr(a)       { return a + result; } result = addr addr(2);"),                
                new Tuple<string,Type, object, string>("result", typeof(double), 5,     "var result = 1; function add2(a, b)    { return a + b;      } result = add2 add2(1,2), 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 14,    "var result = 1; function add3(a, b, c) { return a + b + c;  } result = add3 add3(1,2,1), 2, add3(2,3,3);")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Make_Calls_Inside_External_Parenthesis()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; function inc(a) { return a + 1; }  result = inc(inc(1));"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; function inc(a) { return a + 1; }  if( inc(inc(1)) == 3 ) result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; function inc(a) { return a + 1; }  if( inc(1) == 2 ) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 0; var b = 0; function inc(a) { return a + 1; }  while( inc(b) < 3 ){ b++; result = b;}")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Try_Catch()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "test error", "var result = 'default'; try { throw 'test error'; } catch(err) { result = err.message; }")
            };
            Parse(statements);
        }


        [Test]
        public void Can_Make_Calls_External_As_Statements()
        {
            // Tuple ( 0, 1, 2, 3, 4 )
            //         name, number of parameters, return type, return value, function call as string
            var statements = new List<Tuple<string, int, Type, object, string>>()
            {
                new Tuple<string, int, Type, object, string>("user.create", 0, typeof(double), 1,        "user.create();"),
                new Tuple<string, int, Type, object, string>("user.create", 5, typeof(double), "comlib", "user.create (1,  'comlib',  123, true,  30.5);"),
                new Tuple<string, int, Type, object, string>("user.create", 5, typeof(double), 123,      "user.create(2, \"comlib\", 123, false, 30.5);"),
                new Tuple<string, int, Type, object, string>("user.create", 5, typeof(bool),   true,     "user.create (3, \"comlib\", 123, true,  30.5);"),
                new Tuple<string, int, Type, object, string>("user.create", 5, typeof(double), 30.5,     "user.create  (4, \"comlib\", 123, false, 30.5);")
            };
            ParseFuncCalls(statements);
        }


        [Test]
        public void Can_Make_Calls_External_As_Expressions()
        {
            // Tuple ( 0, 1, 2, 3, 4 )
            //         name, number of parameters, return type, return value, function call as string
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(double), 1,        "var result = user.create();"),
                new Tuple<string, Type, object, string>("result", typeof(double), "comlib", "var result = user.create (1,  'comlib',  123, true,  30.5);"),
                new Tuple<string, Type, object, string>("result", typeof(double), 123,      "var result = user.create(2, \"comlib\", 123, false, 30.5);"),
                new Tuple<string, Type, object, string>("result", typeof(bool),   true,     "var result = user.create (3, \"comlib\", 123, true,  30.5);"),
                new Tuple<string, Type, object, string>("result", typeof(double), 30.5,     "var result = user.create  (4, \"comlib\", 123, false, 30.5);")
            };
            Parse(statements, true, (interpreter) => 
            { 
                interpreter.SetFunctionCallback("user.create", (exp) => 
                {
                    if (exp.ParamList.Count == 0) return 1;

                    int indexOfparam= Convert.ToInt32(exp.ParamList[0]);
                    return exp.ParamList[indexOfparam]; 
                });
            });
        }


        [Test]
        public void Can_Make_Calls_With_Mixed_Types()
        {
            string text = "function add(name, age, date, ids, isActive, addy) { var result2 = name + age + isActive + date + ids[1] + addy.City; return result2; }"
                        + "var result = add('john', 30.5, new Date(), [10,11,12], true, { City: 'Queens', State: 'NY' });"; 
            var i = new Interpreter();
            i.Execute(text);
            var result = i.Memory.Get<string>("result");

            // john30.5true8/17/2011 3:14:25 PM11Queens
            Assert.IsTrue(result.StartsWith("john30.5true"));
            Assert.IsTrue(result.EndsWith("11Queens"));
            Assert.IsTrue(result.Contains(DateTime.Now.Date.ToShortDateString()));
        }
    }



    [TestFixture]
    public class Script_Tests_Blocks : ScriptTestsBase
    {
        [Test]
        public void Can_Use_Non_Nested_BlockStatements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(bool),   2, "var result = 5; function inc( a ) { return a + 1; } result = inc(1);"),
                new Tuple<string, Type, object, string>("result", typeof(double), 2, "var result = 5; if ( result > 4 ) { result = 2; }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 2, "var result = 5; if ( result > 6 ) { result = 1; } else { result = 2; }"),
                new Tuple<string, Type, object, string>("result", typeof(double), 2, "var result = 5; while ( result >= 3 ) { result = result - 1; }"),                
                new Tuple<string, Type, object, string>("result", typeof(double), 2, "var result = 5; for( var a = 0; a < 3; a++) { result = a; }"),                
                new Tuple<string, Type, object, string>("result", typeof(double), 2, "var result = 5; try { result = 2; } catch(err) {}"),                
            };
            Parse(statements);
        }
    }



    [TestFixture]
    public class Script_Tests_Memory : ScriptTestsBase
    {
        [Test]
        public void Can_Pop_Memory()
        {
            var i = new Interpreter();
            i.Context.Plugins.RegisterAll();
            i.Execute("var result = 2; if( true ) { var name = 'inif'; } ");
            var result = i.Result;
            Assert.IsFalse(i.Memory.Contains("name"));
        }
    }



    [TestFixture]
    public class Script_Tests_Types : ScriptTestsBase
    {
        [Test]
        public void Can_Do_Check_For_Nulls()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = null; if(result)  result = 0; else result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = null; if(!result) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result ;       if(result == null) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = null; if(result == null) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0;    if(result != null) result = 1;"),                
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Check_For_Nulls2()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(object), LNull.Instance, "var result = null;"),
                new Tuple<string,Type, object, string>("result", typeof(object), LNull.Instance, "var items = [0, null, 2]; var result = items[1];"),
                new Tuple<string,Type, object, string>("result", typeof(object), LNull.Instance, "var items = { a: 0, b: null, c: 2 }; var result = items.b;"),
                new Tuple<string,Type, object, string>("result", typeof(object), LNull.Instance, "function test( a, b ) { if ( a == null ) return null; return b; } result = test( null, 2);"),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Type_Changes()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "fluentscript", "var result = 2; result = 'fluentscript';"),
                new Tuple<string,Type, object, string>("result", typeof(bool),   true,           "var result = 'fluentscript'; result = true;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4,              "var result = 'fluentscript'; result = 4;")
            };
            Parse(statements);
        }        


        [Test]
        public void Can_Do_Single_Assignment_New_Expressions()
        {
            Interpreter i = new Interpreter();
            i.Context.Types.Register(typeof(Person), null);
            i.Execute("var result = new Person();");
            Assert.AreEqual(i.Memory.Get<object>("result").GetType(), typeof(Person));

            i.Execute("var result = new Date();");
            Assert.AreEqual(i.Memory.Get<object>("result").GetType(), typeof(DateTime));
        }


        private void Can_Do_Map_Declarations()
        {
            var statements = new List<Tuple<string, Type, Type, int, string>>()
            {
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(string),   0,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(string),   1,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(double),   2,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(bool),     3,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray),  typeof(double),  4,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(DateTime), 5,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" )
            };
            foreach (var stmt in statements)
            {
                var i = new Interpreter();
                i.Execute(stmt.Item5);
                var array = i.Memory.Get<LArray>("result");

                Assert.AreEqual(array.Length, stmt.Item4);
            }
        }
    }


    [TestFixture]
    public class Script_Tests_MemberAccess : ScriptTestsBase
    {
        
        [Test]
        public void Can_Get_Class_Member_Property()
        {
            var testcases = new List<Tuple<string,Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "john",              "var p = new Person(); var result = p.FirstName;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "johndoe@email.com", "var p = new Person(); var result = p.Email;" ),
                new Tuple<string, Type, object, string>("result", typeof(bool),     true,                "var p = new Person(); var result = p.IsMale;" ),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.5,                "var p = new Person(); var result = p.Salary;" ),
                new Tuple<string, Type, object, string>("result", typeof(DateTime), DateTime.Today,      "var p = new Person(); var result = p.BirthDate;" ),                
                new Tuple<string, Type, object, string>("result", typeof(string),   "Queens",            "var p = new Person(); var result = p.Address.City;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NY",                "var p = new Person(); var result = p.Address.State;" )
            };
            RunTestCases(testcases);
        }


        [Test]
        public void Can_Set_Class_Member_Property()
        {
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "jane",              "var p = new Person();  p.FirstName = 'jane';     var result = p.FirstName;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "janedoe@email.com", "var p = new Person();  p.Email     = 'janedoe@email.com'; var result = p.Email;" ),
                new Tuple<string, Type, object, string>("result", typeof(bool),     false,               "var p = new Person();  p.IsMale    =  false;       var result = p.IsMale;" ),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.8,                "var p = new Person();  p.Salary    = 10.8;         var result = p.Salary;" ),
                new Tuple<string, Type, object, string>("result", typeof(DateTime), DateTime.Today,      "var p = new Person();  p.BirthDate = new Date();   var result = p.BirthDate;" ),    
                new Tuple<string, Type, object, string>("result", typeof(string),   "Bronx",             "var p = new Person();  p.Address.City = 'Bronx';   var result = p.Address.City;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NJ",                "var p = new Person();  p.Address.State = 'NJ';     var result = p.Address.State;" )
            };
            Parse(testcases, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Get_Map_Member_Property()
        {
            string maptxt = @"{ FirstName: 'john', LastName: 'doe', Email: 'johndoe@email.com', IsMale: true, Salary: 10.5, BirthDate: new Date(), Address: { City: 'Queens', State: 'NY' } }";
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "john",              "var p = " + maptxt + "; var result = p.FirstName;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "johndoe@email.com", "var p = " + maptxt + "; var result = p.Email;" ),
                new Tuple<string, Type, object, string>("result", typeof(bool),     true,                "var p = " + maptxt + "; var result = p.IsMale;" ),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.5,                "var p = " + maptxt + "; var result = p.Salary;" ),
                new Tuple<string, Type, object, string>("result", typeof(DateTime), DateTime.Today,      "var p = " + maptxt + "; var result = p.BirthDate;" ),                
                new Tuple<string, Type, object, string>("result", typeof(string),   "Queens",            "var p = " + maptxt + "; var result = p.Address.City;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NY",                "var p = " + maptxt + "; var result = p.Address.State;" )
            };
            RunTestCases(testcases);
        }


        [Test]
        public void Can_Set_Map_Member_Property()
        {
            string maptxt = @"{ FirstName: 'john', LastName: 'doe', Email: 'johndoe@email.com', IsMale: true, Salary: 10.5, BirthDate: new Date(), Address: { City: 'Queens', State: 'NY' } }";            
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "jane",              "var p = " + maptxt + ";  p.FirstName = 'jane';     var result = p.FirstName;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "janedoe@email.com", "var p = " + maptxt + ";  p.Email     = 'janedoe@email.com'; var result = p.Email;" ),
                new Tuple<string, Type, object, string>("result", typeof(bool),     false,               "var p = " + maptxt + ";  p.IsMale    =  false;       var result = p.IsMale;" ),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.8,                "var p = " + maptxt + ";  p.Salary    = 10.8;         var result = p.Salary;" ),
                new Tuple<string, Type, object, string>("result", typeof(DateTime), DateTime.Today,      "var p = " + maptxt + ";  p.BirthDate = new Date();   var result = p.BirthDate;" ),    
                new Tuple<string, Type, object, string>("result", typeof(string),   "Bronx",             "var p = " + maptxt + ";  p.Address.City = 'Bronx';   var result = p.Address.City;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NJ",                "var p = " + maptxt + ";  p.Address.State = 'NJ';     var result = p.Address.State;" )
            };
            RunTestCases(testcases);
        }
    }


    [TestFixture]
    public class Script_Tests_CustomObject : ScriptTestsBase
    {
        [Test]
        public void Can_Create_Custom_Object_Via_Different_Constructors()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "john",                          "var p = new Person(); var result = p.FirstName;"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "kish",                          "var p = new Person('ki', 'sh'); var result = p.FirstName + p.LastName; "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "nonew@email.com",               "var p = new Person('john', 'doe', 'nonew@email.com', true, 10.56); var result = p.Email;  "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "janedallparams@email.comfalse", "var p = new Person('jane', 'd', 'allparams@email.com', false, 10.56, new Date()); var result = p.FirstName + p.LastName + p.Email + p.IsMale;")
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Create_Custom_Object_With_FullName_Via_Different_Constructors()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "john",                          "var p = new ComLib.Tests.Person(); var result = p.FirstName;"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "kish",                          "var p = new ComLib.Tests.Person('ki', 'sh'); var result = p.FirstName + p.LastName; "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "nonew@email.com",               "var p = new ComLib.Tests.Person('john', 'doe', 'nonew@email.com', true, 10.56); var result = p.Email;  "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "janedallparams@email.comfalse", "var p = new ComLib.Tests.Person('jane', 'd', 'allparams@email.com', false, 10.56, new Date()); var result = p.FirstName + p.LastName + p.Email + p.IsMale;")
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Call_Custom_Object_Instance_Methods()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",                 "var p = new Person('ki', 'sh', 'comlib@email.com', true, 12.34); var result = p.FullName();"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "programmer ki sh sr.",  "var p = new Person('ki', 'sh', 'comlib@email.com', true, 12.34); var result = p.FullNameWithPrefix('programmer', true);")                
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Call_Custom_Object_Instance_Methods_With_Named_Params()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( first: 'ki', last: 'sh', email: 'comlib@email.com', isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', last: 'sh', email: 'comlib@email.com', isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', email: 'comlib@email.com', isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', 'comlib@email.com', isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', 'comlib@email.com', true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', 'comlib@email.com', true, 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Call_Custom_Object_Instance_Methods_With_Named_Params_Using_Nulls()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( first: 'ki', last: 'sh', email: null, isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', last: 'sh', email: 'comlib@email.com', isMale: null, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', email: 'comlib@email.com', isMale: true, salary: null, birthday: null); var result = p.FullName();"),
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Call_Custom_Object_Static_Methods()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "k dog",                 "var result = Person.ToFullName('k', 'dog');")
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Access_Custom_Object_Instance_Properties()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "john",                "var result = p.FirstName;"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "doe",                 "var result = p.LastName; "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "johndoe@email.com",   "var result = p.Email;    "),
                new Tuple<string,Type, object, string>("result", typeof(bool),      true,                  "var result = p.IsMale;   "),
                new Tuple<string,Type, object, string>("result", typeof(double),    10.56,                 "var result = p.Salary;   "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "Queens",              "var result = p.Address.City;"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "NY",                  "var result = p.Address.State;")
            };
            Parse(statements, true, (i) =>
            {
                i.Context.Types.Register(typeof(Person), null);
                i.Context.Memory.SetValue("p", new Person("john", "doe", "johndoe@email.com", true, 10.56, DateTime.Now.Date));
            });
        }


        [Test]
        public void Can_Access_Custom_Object_Static_Properties()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "CodeHelix", "var result = Person.Company;"),                
            };
            Parse(statements, true, (i) =>
            {
                i.Context.Types.Register(typeof(Person), null);
                i.Context.Memory.SetValue("p", new Person("john", "doe", "johndoe@email.com", true, 10.56, DateTime.Now.Date));
            });
        }
    }



    [TestFixture]
    public class Script_Tests_Errors_Runtime : ScriptTestsBase
    {
                
        [Test]
        public void Can_Handle_Division_by_Zero()
        {
            var i = new Interpreter();
            i.Execute("var result = 5/0;");            
            Assert.IsTrue(i.Result.Success);
        }


        [Test]
        public void Can_Handle_Variable_Not_Found()
        {
            ExpectError(new Tuple<string, string, string>("Runtime Error", "Property does not exist", "var user = new Person(); var result = user.First;"));
        }


        [Test]
        public void Can_Handle_Custom_Object_Non_Existant_Property()
        {
            ExpectError(new Tuple<string, string, string>("Runtime Error", "Property does not exist", "var user = new Person(); var result = user.First;"));
        }


        [Test]
        public void Can_Handle_Custom_Object_Non_Existant_Method()
        {
            ExpectError(new Tuple<string, string, string>("Runtime Error", "Property does not exist", "var user = new Person(); var result = user.FullName2();"));
        }


        [Test]
        public void Can_Handle_Non_Existant_Map_Property()
        {
            ExpectError(new Tuple<string, string, string>("Runtime Error", "Property does not exist", "var user = { name: 'comlib' }; var result = user.firstname;"));
        }


        [Test]
        public void Can_Handle_Index_Out_Of_Bounds()
        {
            ExpectError(new Tuple<string, string, string>("Runtime Error", "Access of list item", "var nums = [1, 2]; var result = nums[2];"));
        }
        

        [Test]
        public void Can_Handle_Non_Existant_Function()
        {
            ExpectError(new Tuple<string, string, string>("Runtime Error", "Function does not exist", "var result = increment(1);"));
        }
    }


    [TestFixture]
    public class Script_Tests_Errors_Syntax : ScriptTestsBase
    {
        [Test]
        public void Can_Handle_Unexpected_Char_In_Math()
        {
            ExpectError(new Tuple<string, string, string>("Syntax Error", null, "var result = 1 ~ 2;"));
        }


        [Test]
        public void Can_Handle_Unexpected_Char_In_Isolation()
        {
            ExpectError(new Tuple<string, string, string>("Syntax Error", null, "var result = 1 + 2; ~ 2;"));
        }


        [Test]
        public void Can_Handle_Unexpected_Char_At_Start()
        {
            ExpectError(new Tuple<string, string, string>("Syntax Error", null, "~ var result = 1 + 2; ~ 2;"));
        }


        [Test]
        public void Can_Handle_Unexpected_Repetitive_Char()
        {
            ExpectError(new Tuple<string, string, string>("Syntax Error", null, "var result = 1 + 2;; var name = 'test'"));
        }


        [Test]
        public void Can_Handle_Unterminated_String()
        {
            ExpectError(new Tuple<string, string, string>("Syntax Error", "Unterminated string", "var name = 'comlib \";"));
        }

        [Test]
        public void Can_Handle_Multiple_Useless_Parenthesis()
        {
            ExpectError(new Tuple<string, string, string>("Syntax Error", null, "function add( a ) { return a + 1; }var result = add( 1 ));"));
        }


        [Test]
        public void Can_Handle_Array_Syntax_Errors()
        {
            ExpectError(new Tuple<string, string, string>("Syntax Error", null, "var nums = [1 2];"));
        }


        [Test]
        public void Can_Handle_Double_Colon_On_Keys()
        {
            ExpectError(new Tuple<string, string, string>("Syntax Error", null, "var user = { name: : 'comlib'};"));
        }


        [Test]
        public void Can_Handle_Script_Syntax_Errors()
        {
            var statements = new List<Tuple<string,  string, string>>()
            {
                new Tuple<string, string, string>("Syntax Error", null, "repeat 10 times: print('hi');"),
                new Tuple<string, string, string>("Syntax Error", null, "if else { print('ok'); }"),
                new Tuple<string, string, string>("Syntax Error", null, "var;"),
                new Tuple<string, string, string>("Syntax Error", null, "while{};"),
                new Tuple<string, string, string>("Syntax Error", null, "for( in users){}"),
                new Tuple<string, string, string>("Syntax Error", null, "var users = [ ],];"),
                new Tuple<string, string, string>("Syntax Error", null, "function Name a(b,c) { return null; }"),
                new Tuple<string, string, string>("Syntax Error", null, "try print(1); catch er){}"),
                new Tuple<string, string, string>("Syntax Error", "Unexpected end of script",  "var result = 0; result."),                
                
            };
            ExpectErrors(statements);
        }


        [Test]
        public void Can_Prevent_Instantiation_Of_Non_Registered_Types()
        {
            var result = true;
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),   "common@lib",  "var file = new File('c:\tmp\test.txt'); file.Delete();")
            };
            try { Parse(statements); }
            catch (Exception ex) { result = false; }
            Assert.AreEqual(false, result);
        }
    }


    [TestFixture]
    public class Script_Tests_Callbacks : ScriptTestsBase
    {
        [Test]
        public void Can_Do_Statement_Callbacks()
        {
            int statementCount = 0;
            int expressionCount = 0;
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 2,   "var result = 1; var b = 'abc'; if ( result == 1 ) result = 2;")
                
            };
            Parse(statements, true, i =>
            {
                i.Context.Callbacks.Subscribe("statement-on-after-execute", (sender, node) => statementCount++);
                i.Context.Callbacks.Subscribe("expression-on-after-execute", (sender, node) => expressionCount++);
            }, onNewScript: () => { statementCount = 0; expressionCount = 0; });
            Assert.AreEqual(statementCount, 4);
            Assert.AreEqual(expressionCount, 6);
        }
    }



    [TestFixture]
    public class Script_Tests_Syntax : ScriptTestsBase
    {
        [Test]
        public void Can_Do_Lexical_Replace()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true,   "var result = 1 < 2 and 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,  "var result = 1 < 2 and 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,   "var result = 1 < 2 or  3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,  "var result = 1 > 2 or  3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,   "var result = yes;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,  "var result = no;")
                
            };
            Parse(statements, true, i =>
            {
                i.LexReplace("and", "&&");
                i.LexReplace("or",  "||");
                i.LexReplace("yes", "true");
                i.LexReplace("no",  "false");
            });
        }


        [Test]
        public void Can_Do_Lexical_Remove()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "the var result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var the result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var result = the 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var result = 1 the;")                
            };
            Parse(statements, true, i =>
            {
                i.LexRemove("the");
            });
        }


        [Test]
        public void Can_Handle_Print_With_Space_With_Parenthesis()
        {
            // print ( 'kishore' )
            Parse(new Tuple<string, Type, object, string>(null, typeof(double), null, "print ( age )"));
        }


        [Test]
        public void Can_Handle_SingleChar_NewLines()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var age=17;\rvar result=0;\rif(age<18){result=1;}"),
                
            };
            Parse(statements);
        }


        [Test]
        public void Can_Handle_NoSpaces()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var age=17;var result=0;if(age<18){result=1;}"),                
            };
            Parse(statements);
        }


        [Test]
        public void Can_Handle_New_Lines_As_End_of_Stmt()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "common",    "var name1 = 'common'\r\n var result = name1"),                
                new Tuple<string,Type, object, string>("result", typeof(double), 3,           "function inc( a ) { return a + 1 \r\n } var a = inc( 2 )\r\n var result = a"),
                new Tuple<string,Type, object, string>("result", typeof(bool),   true,        "var a = 1 + 3 \r\n var result = a > 2 "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   true,        "var result = false \r\n var a = 1, b = 2 \r\n if( a == 1 ) { if ( b == 2 ) { result = true \r\n } }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,           "var result = 1 \r\n result++ \r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,           "var result = 1 \r\n result++"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,           "var result = 1 \r\n result += 2\r\n"),                
            };
            Parse(statements);
        }
    }
}
