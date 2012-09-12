using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NUnit.Framework;

using ComLib;
using ComLib.Lang;
using ComLib.Lang.Plugins;
using ComLib.Tests;

using CommonLibrary.Tests.Common;
using ComLib.Lang.Tests.Common;


namespace ComLib.Lang.Tests.Integration
{

    [TestFixture]
    public class Plugin_Integration_Failures : ScriptTestsBase
    {
        [Test]
        public void Can_Pass_Variables_To_Function_With_Same_ParameterNames()
        {
            var script =                "function max(a, b) {"
                + Environment.NewLine + "    if (a > b) return a;"
                + Environment.NewLine + "    if (a == b) return a;"
                + Environment.NewLine + "    return b;"
                + Environment.NewLine + "}"
                + Environment.NewLine + ""
                + Environment.NewLine + "a = 1"
                + Environment.NewLine + "b = 2"
                + Environment.NewLine + "res = max(a, b)";

            Parse(new Tuple<string, Type, object, string>("res", typeof(double), 2, script),
                  true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_NewLines_With_Fluent_Plugin()
        {
            Parse(new Tuple<string, Type, object, string>("res2", typeof(double), 3, 
                  "var nums = [ 1, 2, 3 ]\r\n var res1 = nums\r\n var res2 = 2 + nums[0]"), 
                  true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Email_Plugin_With_Incorrect_Chars()
        {
            Parse(new Tuple<string, Type, object, string>("res", typeof(string), "bat.man@gotham.comabc", 
                  "var res = bat.man@gotham.com+ 'abc'"),
                  true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Access_Custom_Object_Instance_Properties()
        {
            string variable = "var user = new Person('john', 'doe', 'johndoe@email.com', true, 10.56);";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "john",                variable + " var result = user.FirstName;"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "doe",                 variable + " var result = user.LastName; "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "johndoe@email.com",   variable + " var result = user.Email;    "),
                new Tuple<string,Type, object, string>("result", typeof(bool),      true,                  variable + " var result = user.IsMale;   "),
                new Tuple<string,Type, object, string>("result", typeof(double),    10.56,                 variable + " var result = user.Salary;   "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "Queens",              variable + " var result = user.Address.City;"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "NY",                  variable + " var result = user.Address.State;"),
                new Tuple<string,Type, object, string>("result", typeof(double),    1980,                  variable + " var result = user.BirthDate.Year;"),
            };
            Parse(statements, true, (i) =>
            {
                i.Context.Plugins.RegisterAll();
                i.Context.Types.Register(typeof(Person), null);
            });
        }


        [Test]
        public void Can_Use_Bool_Plugin()
        {
            // if yes is true then res11 = yes
            Parse(new Tuple<string, Type, object, string>("res", typeof(bool), true,
                  "var res = no; if yes is true then res = yes"),
                  true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Function_In_If()
        {
            Parse(new Tuple<string, Type, object, string>("result", typeof(double), 1,
                  "function add2(a, b) { return a + b; } var result = 1; if( 105 < add2(100, 1) ) result = 0;"),
                  true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Linq_Plugin_Inside_Parenthesis()
        {
            var items = "var items = "
                        + "["
                            + " { pages: 100, author: 'amycat' }, "
                            + " { pages: 120, author: 'kdog'   }, "
                            + " { pages: 140, author: 'kdog'   }, "
                            + " { pages: 180, author: 'kdog'   }, "
                            + " { pages: 200, author: 'amycat' }  "
                        + "];";

            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = (items where item.pages < 180);                            if(favs.length == 3 && favs[0].pages == 100) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = (items where item.pages < 180 && item.author == 'kdog');   if(favs.length == 2 && favs[1].pages == 140) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; var favs = (from book in items where book.pages < 180 && book.author == 'kdog');   if(favs.length == 2 && favs[1].pages == 140) i = 1;")
            };
            Parse(statements, true, i => i.Context.Plugins.Register(new LinqPlugin()));
        }


        [Test]
        public void Can_Use_Linq_Plugin_Inside_Parenthesis_In_If()
        {
            var items = "var items = "
                        + "["
                            + " { pages: 100, author: 'amycat' }, "
                            + " { pages: 120, author: 'kdog'   }, "
                            + " { pages: 140, author: 'kdog'   }, "
                            + " { pages: 180, author: 'kdog'   }, "
                            + " { pages: 200, author: 'amycat' }  "
                        + "];";

            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; if( (items where item.pages == 120)[0].author == 'kdog' ) i = 1;"),
                new Tuple<string,Type, object, string>("i", typeof(double), 1, items + " var i = 0; if( (items where item.pages < 200 and item.author is 'amycat')[0].pages == 100 ) i = 1;")
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
                i.LexReplace("and", "&&");
            });
        }
    }
}
