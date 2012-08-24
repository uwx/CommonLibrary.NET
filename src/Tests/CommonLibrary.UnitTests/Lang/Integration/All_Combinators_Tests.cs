using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NUnit.Framework;

using ComLib;
using ComLib.Lang;
using ComLib.Lang.Extensions;
using ComLib.Tests;

using CommonLibrary.Tests.Common;
using ComLib.Lang.Tests.Common;


namespace ComLib.Lang.Tests.Integration
{

    [TestFixture]
    public class Plugin_Integration_Positives : ScriptTestsBase
    {
        public static TestType _testType = TestType.Integration;


        [Test]
        public void Can_Use_Alias_In_Script_Plugin()
        {
            RunTests(CommonTestCases_Plugins.AliasInScript, _testType);
        }


        [Test]
        public void Can_Use_Aggregate_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Aggregate, _testType);
        }


        [Test]
        public void Can_Use_Bool_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Bool, _testType);
        }


        [Test]
        public void Can_Use_Compare_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Compare, _testType);
        }


        [Test]
        public void Can_Use_Date_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Date, _testType);
        }


        [Test]
        public void Can_Use_DateNumber_Plugin()
        {
            RunTests(CommonTestCases_Plugins.DateNumber, _testType);
        }


        [Test]
        public void Can_Use_Def_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Def, _testType);
        }


        [Test]
        public void Can_Use_Email()
        {
            RunTests(CommonTestCases_Plugins.Email, _testType);
        }


        [Test]
        public void Can_Use_HashComment_Plugin()
        {
            RunTests(CommonTestCases_Plugins.HashComment, _testType);
        }


        [Test]
        public void Can_Use_Holiday_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Holiday, _testType);
        }


        [Test]
        public void Can_Use_MachineInfo_Plugin()
        {
            RunTests(CommonTestCases_Plugins.MachineInfo, _testType);
        }


        [Test]
        public void Can_Use_Money_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Money, _testType);
        }


        [Test]
        public void Can_Use_Percent_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Percent, _testType);
        }


        [Test]
        public void Can_Use_Records2_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Records, _testType);
        }


        [Test]
        public void Can_Use_Repeat_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Repeat, _testType);
        }


        [Test]
        public void Can_Use_Round_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Round, _testType);
        }


        [Test]
        public void Can_Use_Run_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Run, _testType);
        }


        [Test]
        public void Can_Use_Set_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Set, _testType);
        }


        [Test]
        public void Can_Use_Time_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Time, _testType);
        }


        [Test]
        public void Can_Use_TypeOf_Plugin()
        {
            RunTests(CommonTestCases_Plugins.TypeOf, _testType);
        }


        [Test]
        public void Can_Use_TypeOps_Plugin()
        {
            RunTests(CommonTestCases_Plugins.TypeOps, _testType);
        }


        [Test]
        public void Can_Use_Uri_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Uri, _testType);
        }


        [Test]
        public void Can_Use_VariablePath_Plugin()
        {
            RunTests(CommonTestCases_Plugins.VarPath, _testType);
        }


        [Test]
        public void Can_Use_Const_Plugin()
        {
            var dt = new DateTime(2012, 3, 10);
            var ts = new TimeSpan(15, 30, 0);
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("VAL",    typeof(DateTime),  dt,        "VAL = 3/10/2012"),
                new Tuple<string,Type, object, string>("VAL",    typeof(DateTime),  dt,        "VAL = March 10th 2012"),
                new Tuple<string,Type, object, string>("VAL",    typeof(TimeSpan),  ts,        "VAL = 3:30pm"),
                new Tuple<string,Type, object, string>("VAL",    typeof(string),    "www.yahoo.com",  "VAL = www.yahoo.com"),                
                
                new Tuple<string,Type, object, string>("VAL",    typeof(bool),      true,      "VAL = true"),
                new Tuple<string,Type, object, string>("VAL",    typeof(bool),      false,     "VAL = false"),
                new Tuple<string,Type, object, string>("VAL",    typeof(double),    20,        "VAL = 20"),
                new Tuple<string,Type, object, string>("VAL",    typeof(string),    "consts",  "VAL = 'consts'"),
                new Tuple<string,Type, object, string>("result", typeof(double),    20,        "VAL = 20; result = VAL;"),
                new Tuple<string,Type, object, string>("result", typeof(double),    25,        "VAL = 20; result = 1 + VAL + 4;"),
                new Tuple<string,Type, object, string>("result", typeof(double),    1,         "VAL = 20; result = 0; if( VAL >  2) result = 1"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "20test",  "VAL = 20; result = VAL + 'test';"),
                new Tuple<string,Type, object, string>("result", typeof(double),    21,        "VAL = 20; function inc( a ) { return a + 1; } result = inc(VAL);"),               
                
            };
            Parse(statements, true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Do_Unary_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                // var age = 20; age += 2
                // var a = 2; a += 2
                new Tuple<string,Type, object, string>("result", typeof(double), 22, "var result = 20; result += 2"),
                new Tuple<string,Type, object, string>("result", typeof(bool),   false, "var result = 1; result = !result; "),                
                new Tuple<string,Type, object, string>("result", typeof(string), 3, "var result = 2; result++; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; result--; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 2; result += 2; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; result -= 2; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 6, "var result = 2; result *= 3; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 6; result /= 2; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   false, "var result = true; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   true,  "var result = false; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   false, "var result = 'abc'; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(string), "abcdef", "var result = 'abc'; result += 'def'; "),
            };
            Parse(statements, true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Date_Number_With_Times_Plugin()
        {
            string yearNow = DateTime.Now.Year.ToString();
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = 10/2/2011 at 9:30 am;     if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2 && date.getHours() == 9  && date.getMinutes() == 30) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = 10/2/2011 at 10:45:20 pm; if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2 && date.getHours() == 22 && date.getMinutes() == 45) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = 10/2/2011 at 09:45:20 pm; if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2 && date.getHours() == 21 && date.getMinutes() == 45) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = 10/2/2011 at 12pm;        if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2 && date.getHours() == 12 && date.getMinutes() == 0 ) i = 1;"),

                
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = 10/2/2011;    if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = 10\\2\\2011;  if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = 10-2-2011;    if (date.getFullYear() == 2011 && date.getMonth() == 10 && date.getDate() == 2) i = 1;")
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.Register(new DateNumberPlugin());
                i.Context.Plugins.Register(new DatePlugin());
            });
        }
        

        [Test]
        public void Can_Use_Defect_With_Tables_Linq_Print()
        {
            var text = ContentLoader.GetTextFileContent("Lang.Js.defects.table_linq_print.js");
            var i = new Interpreter();
            i.Context.Plugins.RegisterAll();
            i.LexReplace("set", "var");
            i.Execute(text);
            Assert.IsTrue(i.Result.Success);
                
        }


        [Test]
        public void Can_Use_Day_And_Date_Plugins()
        {
            var date = DateTime.Now.AddDays(10);
            var monthName = date.ToString("M");
            var dayName = DateTime.Now.DayOfWeek.ToString();
            var tommorrow = DateTime.Now.AddDays(1).DayOfWeek.ToString();
            var dateStr = monthName + " " + date.Year;
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var day2 = Monday; if day2 is Monday then i = 1;"),                
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; if today is before " + dateStr + " then i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; if tomorrow is " + tommorrow + " then i = 1;"),
                
            };
            Parse(statements, true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Print_With_All_Plugins()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; if( i == 0 ){ print test\r i = 1; }"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; if( i == 0 ){ print test\rprint test\r i = 1; }"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; if( i == 0 ){ print('test'); i = 1; }"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; if( i == 0 ){ printl('test'); i = 1; }"),
                new Tuple<string,Type, object, string>(null, typeof(double), null, "print hi"),
                
            };
            Parse(statements, true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Linq_Plugin()
        {
            var items = "var books = ["
                            + "{ name: 'book 1', pages: 200, author: 'homey'  },\r\n"
                            + "{ name: 'book 2', pages: 120, author: 'kdog'   },\r\n"
                            + "{ name: 'book 3', pages: 140, author: 'kdog'   } \r\n"
                        + "\t];\r\n"
                        + "// Case 1: start with source books> and system auto creates variable book\r\n";


            var records = "var books = [ name        |   pages    |   author    \r\n"
                        + "             'batman'     |   200      |   'john'    \r\n"
                        + "             'xmen'       |   120      |   'lee'     \r\n"
                        + "             'ddevil'     |   140      |   'maleev'  \r\n"
                        + "];\r\n";

            
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("name", typeof(string), "ddevil", records + "var favorites = books where book.pages less than 150 and book.author is 'maleev' \r\nvar name = favorites[0].name;"),
                new Tuple<string,Type, object, string>("name", typeof(string), "book 2", items   + "var favorites = books where book.pages < 140 and book.author == 'kdog';\r\nvar name = favorites[0].name;"),
                new Tuple<string,Type, object, string>("name", typeof(string), "xmen",   records + "var favorites = books where book.pages < 150 and book.author == 'lee';\r\nvar name = favorites[0].name;"),                
                new Tuple<string,Type, object, string>("name", typeof(string), "xmen",   records + "var favorites = books where book.pages less than 150 and book.author is 'lee';\r\nvar name = favorites[0].name;"),
                new Tuple<string,Type, object, string>("name", typeof(string), "ddevil", records + "var favorites = books where book.pages less than 150 and book.author is 'maleev';\r\nvar name = favorites[0].name;"),
            };
            Parse(statements, true, i => 
            {
                i.Context.Plugins.RegisterAll();
                i.LexReplace("and", "&&");
                i.LexReplace("or", "||");
            });
        }


        [Test]
        public void Can_Use_Variables_Named_With_Plugin_Keywords()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("count", typeof(double), 1,  "var count = 101; if(count > 1) count = 1;")    
            };
            Parse(statements, true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Aggregates_In_Function_Call()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>(null, typeof(double), null,   "var nums = [1, 2]; print(sum(nums));"),    
                new Tuple<string,Type, object, string>("result", typeof(double), 4,  "var nums = [1, 2]; function inc( a ) { return a + 1; } var result = inc(sum(nums));")    
            };
            Parse(statements, true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Sort_With_Set()
        {
            var i = new Interpreter();
            i.Context.Settings.MaxLoopLimit = 100;
            i.Context.Settings.MaxFuncParams = 10;
            i.Context.Settings.MaxCallStack = 10;
            i.Context.Settings.MaxStringLength = 1000;
            i.Context.Settings.MaxExceptions = 5;
            i.Context.Plugins.RegisterAll();
            i.LexReplace("set", "var");

            var source = "set numbers = [2, 3, 1]; sort numbers asc; sort numbers desc; set result = numbers[2];";
            i.Execute(source);
            Assert.IsTrue(i.Result.Success);
            i.Execute(source);
            Assert.IsTrue(i.Result.Success);
        }


        [Test]
        public void Can_Use_New_Lines_As_End_of_Stmt()
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
            Parse(statements, true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Print_With_Space_With_Parenthesis()
        {
            // print ( 'kishore' )
            Parse(new Tuple<string, Type, object, string>(null, typeof(double), null, "print ( age )")
                 , true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Word_Plugin()
        {
            var register = "@words( IBM, nasdaq, fluent script, default pricing model ); ";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "IBM", register + " var result = IBM;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "nasdaq", register + " var result = nasdaq;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "fluent script", register + " var result = fluent script;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "fluent script lang", register + " var result = fluent script + ' lang';"),                
                new Tuple<string,Type, object, string>("result", typeof(string), "default pricing model lang", register + " var result = default pricing model + ' lang';")  
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
            });
        }


        [Test]
        public void Can_Use_Units()
        {
            var register = "enable units;";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(double), 6 , register + " var u = 2 feet + 12 inches + 1 yard; var result = u.Value"),
                new Tuple<string, Type, object, string>("result", typeof(double), 6 , register + " var u = 2 feet + 1 yard + 12 inches; var result = u.Value"), 
                new Tuple<string, Type, object, string>("result", typeof(double), 2 , register + " var u = 1 yard + 12 inches + 2 feet; var result = u.Value"),
                new Tuple<string, Type, object, string>("result", typeof(double), 2 , register + " var u = 1 yard + 2 feet + 12 inches; var result = u.Value"),
                new Tuple<string, Type, object, string>("result", typeof(double), 63, register + " var u = 3 inches + 2 feet + 1 yard;  var result = u.BaseValue"),
                new Tuple<string, Type, object, string>("result", typeof(double), 63, register + " var u = 3 inches + 1 yard + 2 feet;  var result = u.BaseValue")                
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
            });
        }


        [Test]
        public void Can_Use_Suffix()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 12, "var result = 0; function add( a )       { return a + 1; } result = 11 add;" ),
                new Tuple<string,Type, object, string>("result", typeof(double), 12, "var result = 1; function inc( a )       { return a + 1; } result = 11 inc;" ),
                new Tuple<string,Type, object, string>("result", typeof(double), 12, "var result = 2; function increment( a ) { return a + 1; } result = 11 increment;" )
            };
            Parse(statements, true, i => i.Context.Plugins.Register(new SuffixPlugin()));
        }


        [Test]
        public void Can_Use_FunctionDocs()
        {
            var code =
                "# @summary: creates an order the stock supplied with the specified fields of date, price, shares and policy."
                + Environment.NewLine + "# this will create scheduled order in the date is t+1."
                + Environment.NewLine + "# @example: conventional sytax, 'orderToBuy( shares(300), \"IBM\", 40.5, new Date(2012, 7, 10)', \"premium policy\""
                + Environment.NewLine + "# @example: fluent syntax,      'order to buy 300 shares, IBM, $40.5, 7/10/2012, premium policy'"
                + Environment.NewLine + "# @arg: name: shares, desc: Number of shares to buy,     type: number,  alias: shares, examples: 300 shares | shares(300) | new Shares(300)"
                + Environment.NewLine + "# @arg: name: symbol,  desc: The name of the stock,      type: string,  alias: of,     examples: 'IBM' | 'MSFT'"
                + Environment.NewLine + "# @arg: name: price,  desc: The price at which to buy,   type: number,  alias: at,     examples: $40.50"
                + Environment.NewLine + "# @arg: name: date,   desc: The date to buy the stock,   type: date  ,  alias: on,     examples: July 10th 2012 | 7/10/2012"
                + Environment.NewLine + "# @arg: name: policy, desc: The policy type for pricing, type: string,  alias: using,  examples: 'default pricing' | 'premium pricing'"
                + Environment.NewLine + "function 'order to buy', 'order_to_buy'( shares, symbol, price, date, policy ) { }";
            var i = new Interpreter();
            i.Context.Plugins.RegisterAll();
            i.Execute(code);
            Assert.IsTrue(i.Context.Functions.Contains("order to buy"));
            Assert.IsTrue(i.Context.Functions.Contains("order_to_buy"));

            var fsmeta = i.Context.Symbols.GetSymbol<SymbolTypeFunc>("order to buy").Meta;
            Assert.AreEqual(fsmeta.Arguments.Count, 5);
            Assert.AreEqual(fsmeta.ArgumentsLookup["shares"].Alias, "shares");
            Assert.AreEqual(fsmeta.ArgumentsLookup["symbol"].Alias, "of");
            Assert.AreEqual(fsmeta.ArgumentsLookup["price"].Alias, "at");
            Assert.AreEqual(fsmeta.ArgumentsLookup["date"].Alias, "on");
            Assert.AreEqual(fsmeta.ArgumentsLookup["policy"].Alias, "using");
        }
    }


    /*
         *  Case 1. func                                        notify
         *  Case 2. func parameter                              notify          'group1'         
         *  Case 3. func parameter parameter			        print 	        'kishore' 			'reddy'
         *  Case 4. class  method 		                        user  	        IsRegistered 	
         *  Case 5. method class  		                        IsRegistered    user
         *  Case 6. class parameter method                      user            'kreddy'            exists
         *  Case 7. method parameter class			            activate        'admin'		        User
         *  Case 8. class methodPart methodPart [parameter]     user            has                 savings account
         *  Case 9. methodPart class methodPart [parameter]	    add 	        user				to role 'admin', 'division2'
        */
}
