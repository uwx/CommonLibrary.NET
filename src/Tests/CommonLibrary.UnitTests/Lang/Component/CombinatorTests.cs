using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using NUnit.Framework;

using ComLib;
using ComLib.Lang;
using ComLib.Lang.Plugins;
using ComLib.Lang.Types;
using ComLib.Tests;

using CommonLibrary.Tests.Common;
using ComLib.Lang.Tests.Common;


namespace ComLib.Lang.Tests.Component
{
    public class LangTestsConverter
    {
        private StringBuilder _buffer = new StringBuilder();

        public void Start()
        {
            _buffer.AppendLine("<plugins>");
        }


        public void Finish()
        {
            _buffer.AppendLine("</plugins>");
        }


        public void WriteTo(string filepath)
        {
            using (var writer = new StreamWriter(filepath))
            {
                var text = _buffer.ToString();
                writer.Write(text);
            }
        }



        /// <summary>
        /// Converts a set of testcases into an xml string.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="plugin"></param>
        /// <param name="testcases"></param>
        /// <returns></returns>
        public void Convert(string group, Type plugin, List<Tuple<string, Type, object, string>> testcases)
        {
            _buffer.AppendLine(string.Format(@"<group name=""{0}"" plugin=""{1}"">", group, plugin.Name.Replace("Plugin", "")));
            int index = 1;

            foreach (var testcase in testcases)
            {
                var script = Escape(testcase.Item4);
                var val = testcase.Item3;
                var id = index < 10 ? "0" + index.ToString() : index.ToString();
                _buffer.AppendLine("\t" + string.Format(@"<test case=""{0}"" var=""{1}"" type=""{2}"" expectedval=""{3}"" script=""{4}"" />", id, testcase.Item1, testcase.Item2.Name, val, script));
                index++;
            }

            _buffer.AppendLine("</group>");
        }


        private string Escape(string text)
        {
            return text;
        }
    }

    [TestFixture]
    public class Plugin_Component_Positives : ScriptTestsBase
    {
        public LangTestsConverter _converter = new LangTestsConverter();
        public static TestType _testType = TestType.Component;


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
        public void Can_Use_Fail_Plugin()
        {
            ExpectErrors(CommonTestCases_Plugins.Fail, _testType);
        } 
        

        [Test]
        public void Can_Use_FileExt()
        {
            RunTests(CommonTestCases_Plugins.FileExt, _testType);
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
        public void Can_Use_IO_Plugin()
        {
            RunTests(CommonTestCases_Plugins.IO, _testType);
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
        public void Can_Use_Sort_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Sort, _testType);
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
        public void Can_Use_Version_Plugin()
        {
            RunTests(CommonTestCases_Plugins.Version, _testType);
        }


        [Test]
        public void Can_Use_Exec_Plugin()
        {
            var script = @"exec program: C:\Dev\Tools\NAnt\9.0\bin\nant.exe,"
	                   + @"in: C:\Dev\Tests\nanttests,"
                       + @"args: [ '-buildfile:nanttests.xml', '-D:arg.option=3', 'Execute_A' ]";

            var i = new Interpreter();
            i.Context.Plugins.Register(new UriPlugin());
            i.Context.Plugins.Register(new ExecPlugin());
            //i.Execute(script);
        }


        [Test]
        public void Can_Use_Log_Plugin()
        {
            int level = -1;
            string msg = "";

            Action<int, string, LError> callback = (lev, message, er) => { level = lev; msg = message; };
            var i = new Interpreter();
            i.Context.Plugins.Register(new LogPlugin(callback));

            // 1. Can always log to put
            i.Execute("log configure 'warn', 'callback'; put 'message1'");
            Assert.AreEqual(LogPluginConstants.Put, level);
            Assert.AreEqual("message1", msg);

            // 2. Can not log to info if set to warn
            level = -1; msg = "";
            i.Execute("log configure 'warn', 'callback'; info 'message1'");
            Assert.AreEqual(-1, level);
            Assert.AreEqual("", msg);

            // 3. Can log to warn level and above
            level = -1; msg = "";
            i.Execute("log configure 'warn', 'callback'; warn 'warn1'");
            Assert.AreEqual(LogPluginConstants.Warn, level);
            Assert.AreEqual("warn1", msg);

            // 4. Can log to level above warn
            level = -1; msg = "";
            i.Execute("log configure 'warn', 'callback'; error 'error1'");
            Assert.AreEqual(LogPluginConstants.Error, level);
            Assert.AreEqual("error1", msg);

            // 5. Can get log level
            level = -1; msg = "";
            i.Execute("log configure 'warn', 'callback'; var result = log level");
            var r = i.Context.Memory.Get<string>("result");
            Assert.AreEqual(r, "warn");
        }


        [Test]
        public void Can_Use_Const_Plugin()
        {
            var dt = new DateTime(2012, 3, 10);
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("VAL",    typeof(bool),      true,                       "VAL = true"),
                new Tuple<string,Type, object, string>("VAL",    typeof(bool),      false,                      "VAL = false"),
                new Tuple<string,Type, object, string>("VAL",    typeof(double),    20,                         "VAL = 20"),
                new Tuple<string,Type, object, string>("VAL",    typeof(string),    "consts",                   "VAL = 'consts'"),
                new Tuple<string,Type, object, string>("VAL",    typeof(TimeSpan),  new TimeSpan(8, 30, 10),    "VAL = new Time(8, 30, 10)"),
                new Tuple<string,Type, object, string>("VAL",    typeof(DateTime),  new DateTime(2012, 8, 10),  "VAL = new Date(2012, 8, 10)"),
                new Tuple<string,Type, object, string>("result", typeof(double),    20,        "VAL = 20; result = VAL;"),
                new Tuple<string,Type, object, string>("result", typeof(double),    25,        "VAL = 20; result = 1 + VAL + 4;"),
                new Tuple<string,Type, object, string>("result", typeof(double),    1,         "VAL = 20; result = 0; if( VAL >  2) result = 1"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "20test",  "VAL = 20; result = VAL + 'test';"),
                new Tuple<string,Type, object, string>("result", typeof(double),    21,        "VAL = 20; function inc( a ) { return a + 1; } result = inc(VAL);"),                                
            };
            _converter.Convert("Const", typeof(ConstCapsPlugin), statements);
            Parse(statements, true, i => i.Context.Plugins.Register(new ConstCapsPlugin()), false);

            // Test 2 : Error
            ExpectError(new ConstCapsPlugin(), "VAL = 20; result = 2; VAL = 30;", "Syntax Error");

            var it = new Interpreter();
            it.Context.Plugins.Register(new ConstCapsPlugin());
            it.Execute("VAL1 = 10, VAL2 = 20, VAL3 = 30");
            Assert.AreEqual(it.Context.Memory.Get<double>("VAL1"), 10);
            Assert.AreEqual(it.Context.Memory.Get<double>("VAL2"), 20);
            Assert.AreEqual(it.Context.Memory.Get<double>("VAL3"), 30);
        }         


        [Test]
        public void Can_Use_Linq_Plugin_Using_Basic_Types()
        {
            var items = "var items = [1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5];";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = items where item < 2;                if(favs.length == 1 && favs[0] == 1) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = items where item > 2 && item < 4;    if(favs.length == 3 && favs[0] == 3) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = items where item == 4;               if(favs.length == 4 && favs[0] == 4) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = items where item < 2 || item == 5;   if(favs.length == 2 && favs[0] == 1 && favs[1] == 5) i = 1;"),

                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = from item1 in items where item1 < 2;                if(favs.length == 1 && favs[0] == 1) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = from item1 in items where item1 > 2 && item1 < 4;   if(favs.length == 3 && favs[0] == 3) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = from item1 in items where item1 == 4;               if(favs.length == 4 && favs[0] == 4) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = from item1 in items where item1 < 2 || item1 == 5;  if(favs.length == 2 && favs[0] == 1 && favs[1] == 5) i = 1;"),
            };
            Parse(statements, true, i => i.Context.Plugins.Register(new LinqPlugin()));
        }


        [Test]
        public void Can_Use_Linq_Plugin_Using_Dictionary_Types()
        {
            var items   = "var items = [" 
                            + "{ pages: 100, author: 'amycat' }, "
                            + "{ pages: 120, author: 'kdog' },   "
                            + "{ pages: 140, author: 'kdog' },   "
                            + "{ pages: 180, author: 'kdog' },   "
                            + "{ pages: 200, author: 'amycat' }  "
                        + "];";

            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = items where item.pages < 180;                            if(favs.length == 3 && favs[0].pages == 100) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = items where item.pages < 180 && item.author == 'kdog';   if(favs.length == 2 && favs[1].pages == 140) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = from book in items where book.pages < 180 && book.author == 'kdog';   if(favs.length == 2 && favs[1].pages == 140) i = 1;")
            };
            Parse(statements, true, i => i.Context.Plugins.Register(new LinqPlugin()));
        }


        [Test]
        public void Can_Use_Day_Plugin()
        {            
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var day2 = Monday; if day2 == Monday then i = 1;"),                
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = jan 1st, 2012;  if (date.getFullYear() == 2012 && date.getMonth() == 1 && date.getDate() == 1 && date.getDay() == sunday    ) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = jan 2nd, 2012;  if (date.getFullYear() == 2012 && date.getMonth() == 1 && date.getDate() == 2 && date.getDay() == monday    ) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = jan 3rd, 2012;  if (date.getFullYear() == 2012 && date.getMonth() == 1 && date.getDate() == 3 && date.getDay() == tuesday   ) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = jan 4th, 2012;  if (date.getFullYear() == 2012 && date.getMonth() == 1 && date.getDate() == 4 && date.getDay() == wednesday ) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = jan 5th, 2012;  if (date.getFullYear() == 2012 && date.getMonth() == 1 && date.getDate() == 5 && date.getDay() == thursday  ) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = jan 6th, 2012;  if (date.getFullYear() == 2012 && date.getMonth() == 1 && date.getDate() == 6 && date.getDay() == friday    ) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, "var i = 0; var date = jan 7th, 2012;  if (date.getFullYear() == 2012 && date.getMonth() == 1 && date.getDate() == 7 && date.getDay() == saturday  ) i = 1;")               
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.Register(new DatePlugin());
                i.Context.Plugins.Register(new DayPlugin());
            });
        }        


        [Test]
        public void Can_Use_Swap_Plugin()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("a", typeof(double), 2, "var a = 1; var b = 2; swap a with b;"),                
            };
            Parse(statements, true, i => i.Context.Plugins.Register(new SwapPlugin()));
        }


        [Test]
        public void Can_Use_Env_Plugin()
        {
            var computer = System.Environment.GetEnvironmentVariable("computername");
            //System.Environment.SetEnvironmentVariable("custom1", "1234", EnvironmentVariableTarget.User);
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("a", typeof(string), computer, "var a = env.computername;"),               
                //new Tuple<string,Type, object, string>("a", typeof(string), "1234", "var a = env.user.custom1;")
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.Register(new EnvLexPlugin());
                i.Context.Plugins.Register(new EnvPlugin());
            });
        }


        [Test]
        public void Can_Use_Takeover_Print_Plugin()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("a", typeof(bool), true, "var a = false; print some test" + Environment.NewLine + "a = true;")
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.Register(new PrintPlugin());
                i.Context.Plugins.Register(new PrintExpressionPlugin());
            });
        }
        

        [ExpectedException]
        public void Can_Use_FluentCall_Plugin_With_Multiple_Method_Parts()
        {
            var result = typeof(User).GetMember("IsRegistered");
            var result2 = typeof(User).GetMethod("IsRegistered");
            var result3 = typeof(User).GetMethod("isregistered", BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);


            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var user = new User('kishore'); var result = user has savings account;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var user = new User('kishore'); var result = add user to role 'admin', 'group2';")                
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.Register(new FluentFuncPlugin());
                i.Context.Types.Register(typeof(User), null);
            });
        }
        
        
        


        [Test]
        public void Can_Use_Word_Plugin()
        {
            var register = "@words( IBM, nasdaq, fluent script ); ";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "IBM", register + " var result = IBM;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "nasdaq", register + " var result = nasdaq;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "fluent script", register + " var result = fluent script;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "fluent script lang", register + " var result = fluent script + ' lang';")                
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.Register(new WordsPlugin());
                i.Context.Plugins.Register(new WordsInterpretPlugin());
            });
        }


        [Test]
        public void Can_Use_Marker_Plugin()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 2,  "var result = 1; @todo 'needs some changes' result = 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,  "var result = 1; @note 'needs some changes'; result = 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,  "var result = 1; @bug 'needs some changes'\r\n result = 2;")
                
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.Register(new MarkerPlugin());
                i.Context.Plugins.Register(new MarkerLexPlugin());
            });
        }


        [Test]
        public void Can_Use_Records_Plugin()
        {
            var list1 = "var books = [ name     |   pages    |   isactive \r"
                      + "             'book 1'  |   140      |   true  \r"
                      + "             'book 2'  |   200      |   false \r"
                      + "             'book 3'  |   120      |   true  ";

            var list2 = "var books = [ name     |   pages    |   isactive \r"
                      + "             'book 1'  ,   140      ,   true  \r"
                      + "             'book 2'  ,   200      ,   false \r"
                      + "             'book 3'  ,   120      ,   true  ";

            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 200, list1 + " ];\r var result = books[1].pages; "),                
                new Tuple<string,Type, object, string>("result", typeof(double), 200, list1 + "\r ];\r var result = books[1].pages; "),                
                new Tuple<string,Type, object, string>("result", typeof(double), 200, list1 + "];\r var result = books[1].pages; "),                
                new Tuple<string,Type, object, string>("result", typeof(double), 200, list2 + "];\r var result = books[1].pages; "),   
            }; 
            Parse(statements, true, i => i.Context.Plugins.Register(new RecordsPlugin()));
        }


        [Test]
        public void Can_Use_Sort_Plugin2()
        {
            var list1 = "var list = [4,    3,     1,     5,    2,     6   ];";
            var list2 = "var list = ['b',  'd',   'a',   'c',  'f',   'e' ];";
            var list3 = "var list = [true, false, false, true, false, true];";
            var list4 = "var list = [4.1,  3.2,   1.3,   5.4,  2.5,   6.6 ];";
            var items = "var books = ["
                            + "{ pages: 100, author: 'amycat' }, "
                            + "{ pages: 120, author: 'kdog'   }, "
                            + "{ pages: 140, author: 'kdog'   }, "
                            + "{ pages: 180, author: 'kdog'   }, "
                            + "{ pages: 200, author: 'amycat' }  "
                        + "];";

            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(double), 3,     " var list = sort [2, 1, 4, 3] asc; var result = list[2];"),                
                TestCase("result", typeof(double), 1,     "var numbers = [2, 3, 1]; sort numbers asc; sort numbers desc; var result = numbers[2];"),                
                TestCase("result", typeof(double), 4,     list1 + " sort list asc; sort list desc; var result = list[2];"),
                TestCase("result", typeof(double), 3,     list1 + " sort list asc;  var result = list[2];"),
                TestCase("result", typeof(string), "c",   list2 + " sort list asc;  var result = list[2];"),
                TestCase("result", typeof(bool),   false, list3 + " sort list asc;  var result = list[2];"),
                TestCase("result", typeof(double), 3.2,   list4 + " sort list asc;  var result = list[2];"),
                TestCase("result", typeof(double), 4,     list1 + " sort list desc; var result = list[2];"),
                TestCase("result", typeof(string), "d",   list2 + " sort list desc; var result = list[2];"),
                TestCase("result", typeof(bool),   true,  list3 + " sort list desc; var result = list[2];"),
                TestCase("result", typeof(double), 4.1,   list4 + " sort list desc; var result = list[2];"),
                TestCase("result", typeof(double), 180,   items + " sort books by book.pages desc; var result = books[1].pages;"),
                TestCase("result", typeof(double), 120,   items + " sort books by book.pages asc;  var result = books[1].pages;"),
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.Register(new SortPlugin());
            });
        }     
    }
   
}
