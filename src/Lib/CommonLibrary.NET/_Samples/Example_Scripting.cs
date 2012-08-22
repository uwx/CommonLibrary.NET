using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Threading;

//<doc:using>
using ComLib;
using ComLib.Lang;
//</doc:using>
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Scheduling namespace.
    /// </summary>
    public class Example_Scripting : App
    {
        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            //<doc:example>
            var i = new Interpreter();

			// 1. Execute script and get variable.
            i.Execute(" var fullname = 'kishore' + ' reddy';");
            string name = i.Memory.Get<string>("fullname");

            // 2. Execute script with functions
            i.Execute(" function min(a, b) { if(a < b) return a; return b; } var minval = min(2,3);");
            double minval = i.Memory.Get<double>("minval");

            // 3. Register custom type and execute script.
            i.Context.Types.Register(typeof(User), null);
            i.Execute("var user = new User('kishore', 'reddy'); var fullname = user.FullName();");
            string fullname = i.Memory.Get<string>("fullname");

            // 4. Get the result of the script execution.
            i.Execute("var list1 = [1,2,3,4,5]; var total = list1[1] + list1[2];");
            var total = i.Memory.Get<double>("total");
            var result = i.Result;
            Console.WriteLine(string.Format("Success: {0}, StartTime: {1}, EndTime: {2}", result.Success, result.StartTime.ToShortTimeString(), result.EndTime.ToShortTimeString()));

            // 5. Set limits on the interpreter.
            // This set the total number of loops that can be run to 5. ( just as an example ).
            // An LimitException occurrs in the interpreter.
            i.Context.Settings.MaxLoopLimit = 5;
            i.Execute("var count = 0; for(var ndx = 1; ndx <= 6; ndx++) { count = ndx; }");
            var count = i.Memory.Get<double>("count");
            Console.WriteLine(string.Format("Success: {0}, Message: {1}", i.Result.Success, i.Result.Message));

            // 6. Register plugins which extend the syntax and functionality
            i.Context.Plugins.Register(new ComLib.Lang.Extensions.DatePlugin());
            i.Execute("var date = January 1st 2012 at 9:30 am;");
            var date = i.Memory.Get<DateTime>("date");
            Console.WriteLine("Success: {0}, Date: {1}", i.Result.Success, date.ToShortDateString());

            // 7. Register all plugins provided by commonlibrary
            i.Context.Plugins.RegisterAll();

            // 8. Register replacement words ( at the lexical level )
            i.LexReplace("and", "&&");
            i.LexReplace("or", "||");
            i.Execute(" var result = 2; if ( result > 1 and result < 4 ) print('and replacement works'");
            //</doc:example>
            return BoolMessageItem.True;
        }


        /// <summary>
        /// Test class to show scripting
        /// </summary>
        class User
        {
            public User() { }

            public User(string first, string last)
            {
                First = first;
                Last = last;
            }


            public string First { get; set; }
            public string Last { get; set; }
            public bool IsActive { get; set; }


            public string FullName() { return First + " " + Last; }
        }
    }
}
