using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NUnit.Framework;

using ComLib;
using ComLib.Lang;
using ComLib.Lang.Plugins;
using ComLib.Lang.Types;
using ComLib.Tests;

using CommonLibrary.Tests.Common;
using ComLib.Lang.Tests.Common;


namespace ComLib.Lang.Tests.Integration
{
    [TestFixture]
    public class Fluent_Function_Call_Tests : ScriptTestsBase
    {
        [Test]
        public void Can_Use_Named_Parameters()
        {
            var dt = new DateTime(2012, 8, 2, 9, 30, 0);
            var pricing = "default pricing";
            var func = "function order( index, amount, of, at, on, using ) { var args = [amount, of, at, on, using]; return args[index]; }";
            var statements = new List<Tuple<string, Type, object, string>>()
            {   
                TestCase( "result", typeof(object), LObjects.Null,      func +  "result = order( 0, amount: null, of: 'ibm', at: $45.50, on: 8/2/2012 at 9:30 am,     using: 'default pricing' )"),
                TestCase( "result", typeof(object), LObjects.Null,      func +  "result = order( 1,         300,      null,  at: $45.50, on: 8/2/2012 at 9:30 am,     using: 'default pricing' )"),
                TestCase( "result", typeof(double), 300,                 func + "result = order( 0, amount: 300,  of: 'ibm', at: $45.50, on: 8/2/2012 at 9:30 am,     using: 'default pricing' )"),
                TestCase( "result", typeof(string), "ibm",               func + "result = order( 1,         300,  of: 'ibm', at: $45.50, on: 8/2/2012 at 9:30 am,     using: 'default pricing' )"),
                TestCase( "result", typeof(double), 45.50,               func + "result = order( 2,         300,      'ibm', at: $45.50, on: 8/2/2012 at 9:30 am,     using: 'default pricing' )"),
                TestCase( "result", typeof(DateTime), dt,                func + "result = order( 3,         300,      'ibm',     $45.50, on: 8/2/2012 at 9:30 am,     using: 'default pricing' )"),
                TestCase( "result", typeof(string), pricing,             func + "result = order( 4,         300,      'ibm',     $45.50,     8/2/2012 at 9:30 am,     using: 'default pricing' )"),
                
                TestCase( "result", typeof(object), LObjects.Null,      func +  "result = order( 0, amount: null, of: 'ibm', at: $45.50, on: Aug 2nd 2012 at 9:30 am, using: 'default pricing' )"),
                TestCase( "result", typeof(object), LObjects.Null,      func +  "result = order( 1,         300,      null,  at: $45.50, on: Aug 2nd 2012 at 9:30 am, using: 'default pricing' )"),
                TestCase( "result", typeof(double), 300,                 func + "result = order( 0, amount: 300,  of: 'ibm', at: $45.50, on: Aug 2nd 2012 at 9:30 am, using: 'default pricing' )"),
                TestCase( "result", typeof(string), "ibm",               func + "result = order( 1,         300,  of: 'ibm', at: $45.50, on: Aug 2nd 2012 at 9:30 am, using: 'default pricing' )"),
                TestCase( "result", typeof(double), 45.50,               func + "result = order( 2,         300,      'ibm', at: $45.50, on: Aug 2nd 2012 at 9:30 am, using: 'default pricing' )"),
                TestCase( "result", typeof(DateTime), dt,                func + "result = order( 3,         300,      'ibm',     $45.50, on: Aug 2nd 2012 at 9:30 am, using: 'default pricing' )"),
                TestCase( "result", typeof(string), pricing,             func + "result = order( 4,         300,      'ibm',     $45.50,     Aug 2nd 2012 at 9:30 am, using: 'default pricing' )"),
            };
            Parse(statements, true, i => i.Context.Plugins.RegisterAll());
        }


        [Test]
        public void Can_Use_Named_Parameters_Fluently()
        {
            var func = "function order( index, amount, of, at, on, using ) { var args = [amount, of, at, on, using]; return args[index]; }";
            var policy = "default pricing";
            var date = new DateTime(2012, 8, 2, 9, 0, 0);
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase( "result", typeof(double),   300,    func + "result =  order  index: 0, amount: 300, of: 'ibm', at: $40, on: Aug 2nd 2012 at 9am, using: 'default pricing'  "),
                TestCase( "result", typeof(string),   "ibm",  func + "result =  order  index: 1  amount: 300  of: 'ibm'  at: $40  on: Aug 2nd 2012 at 9am  using: 'default pricing'  "),
                TestCase( "result", typeof(double),   40,     func + "result =  order  index  2, amount  300, of  'ibm', at  $40, on  Aug 2nd 2012 at 9am, using  'default pricing'  "),
                TestCase( "result", typeof(DateTime), date,   func + "result =  order  index  3  amount  300  of  'ibm'  at  $40  on  Aug 2nd 2012 at 9am  using  'default pricing'  "),                
                TestCase( "result", typeof(string),   policy, func + "result =  order  index  4  amount  300  of  'ibm'  at  $40  on  Aug 2nd 2012 at 9am  using  'default pricing'  "),                
                
                TestCase( "result", typeof(double),   300,    func + "result =  order  index: 0, amount: 300, of: 'ibm', at: $40, on: 8/2/2012 at 9am,     using: 'default pricing'  "),
                TestCase( "result", typeof(string),   "ibm",  func + "result =  order  index: 1  amount: 300  of: 'ibm'  at: $40  on: 8/2/2012 at 9am      using: 'default pricing'  "),
                TestCase( "result", typeof(double),   40,     func + "result =  order  index  2, amount  300, of  'ibm', at  $40, on  8/2/2012 at 9am,     using  'default pricing'  "),
                TestCase( "result", typeof(DateTime), date,   func + "result =  order  index  3  amount  300  of  'ibm'  at  $40  on  8/2/2012 at 9am      using  'default pricing'  "),                
                TestCase( "result", typeof(string),   policy, func + "result =  order  index  4  amount  300  of  'ibm'  at  $40  on  8/2/2012 at 9am      using  'default pricing'  "),                
                
                TestCase( "result", typeof(double),   300,    func + "result =  order( index: 0, amount: 300, of: 'ibm', at: $40, on: Aug 2nd 2012 at 9am, using: 'default pricing' )"),
                TestCase( "result", typeof(string),   "ibm",  func + "result =  order( index: 1  amount: 300  of: 'ibm'  at: $40  on: Aug 2nd 2012 at 9am  using: 'default pricing' )"),
                TestCase( "result", typeof(double),   40,     func + "result =  order( index  2, amount  300, of  'ibm', at  $40, on  Aug 2nd 2012 at 9am, using  'default pricing' )"),
                TestCase( "result", typeof(DateTime), date,   func + "result =  order( index  3  amount  300  of  'ibm'  at  $40  on  Aug 2nd 2012 at 9am  using  'default pricing' )"),
                TestCase( "result", typeof(string),   policy, func + "result =  order( index  4  amount  300  of  'ibm'  at  $40  on  Aug 2nd 2012 at 9am  using  'default pricing' )"),
                
            };
            Parse(statements, true, i => i.Context.Plugins.RegisterAll());
        }
    }



    [TestFixture]
    public class Fluent_Member_Access_Tests : ScriptTestsBase
    {
        [Test]
        public void Can_Use_Fluent_Member_Get_Property()
        {
            var date = new DateTime(2012, 6, 1);
            Klass.Reset();
            var statements = new List<Tuple<string, Type, object, string>>()
             {
		         // INSTANCE PROPS -----------------------------------------------------------------------------------
		         // Static prop get
                 TestCase("result", typeof(double), 123,      "var result = klass Prop1;"),
                 TestCase("result", typeof(string), "fluent", "var result = klass Prop2;"),
                 TestCase("result", typeof(bool),   true,     "var result = klass Prop3;"),
                 TestCase("result", typeof(double), date,     "var result = klass Prop4;"),
		                                                                                    
		         // Static prop get fluent                                                  
		         TestCase("result", typeof(double), 123,      "var result = Prop1 klass"),
                 TestCase("result", typeof(string), "fluent", "var result = Prop2 klass"),
                 TestCase("result", typeof(bool),   true,     "var result = Prop3 klass"),
                 TestCase("result", typeof(double), date,     "var result = Prop4 klass"), 
                 
                 
                 // STATIC PROPS -----------------------------------------------------------------------------------
		         // Static prop get
                 TestCase("result", typeof(double), 1234,     "var result = Klass KProp1;"),
                 TestCase("result", typeof(string), "Fluent", "var result = Klass KProp2;"),
                 TestCase("result", typeof(bool),   true,     "var result = Klass KProp3;"),
                 TestCase("result", typeof(double), date,     "var result = Klass KProp4;"),
		 
		         // Static prop get fluent
		         TestCase("result", typeof(double), 1234,     "var result = KProp1 Klass"),
                 TestCase("result", typeof(string), "Fluent", "var result = KProp2 Klass"),
                 TestCase("result", typeof(bool),   true,     "var result = KProp3 Klass"),
                 TestCase("result", typeof(double), date,     "var result = KProp4 Klass"),		         
             };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
                i.Context.Types.Register(typeof(Klass), null);
                i.Context.Symbols.Global.DefineVariable("klass", typeof(Klass));
                i.Context.Memory.SetValue("klass", new LClass(new Klass()));
            });
        }


        [Test]
        public void Can_Use_Fluent_Member_Set_Property()
        {
            var date1 = new DateTime(2012, 6, 2);
            var date2 = new DateTime(2012, 6, 3);
            Klass.Reset();
            var statements = new List<Tuple<string, Type, object, string>>()
             {
		         // INSTANCE PROPS -----------------------------------------------------------------------------------
		         // Static prop get
                 TestCase("result", typeof(double), 124,      "klass Prop1 = 124;                 result = klass Prop1;"),
                 TestCase("result", typeof(string), "fluent2","klass Prop2 = 'fluent2';           result = klass Prop2;"),
                 TestCase("result", typeof(bool),   false,    "klass Prop3 = false;               result = klass Prop3;"),
                 TestCase("result", typeof(double), date1,    "klass Prop4 = new Date(2012,6,2);  result = klass Prop4;"),
       
		         // Static prop get fluent                                                  
		         TestCase("result", typeof(double), 124,      "Prop1 klass = 124;                 result = klass Prop1;"),
                 TestCase("result", typeof(string), "fluent2","Prop2 klass = 'fluent2';           result = klass Prop2;"),
                 TestCase("result", typeof(bool),   false,    "Prop3 klass = false;               result = klass Prop3;"),
                 TestCase("result", typeof(double), date1,    "Prop4 klass = new Date(2012,6,2);  result = klass Prop4;"), 

                 
                 // STATIC PROPS -----------------------------------------------------------------------------------
		         // Static prop get
                 TestCase("result", typeof(double), 1235,     "Klass KProp1 = 1235;               result = Klass KProp1;"),
                 TestCase("result", typeof(string), "Fluent2","Klass KProp2 = 'Fluent2';          result = Klass KProp2;"),
                 TestCase("result", typeof(bool),   false,    "Klass KProp3 = false;              result = Klass KProp3;"),
                 TestCase("result", typeof(double), date2,    "Klass KProp4 = new Date(2012,6,3); result = Klass KProp4;"),
		 
		         // Static prop get fluent
		         TestCase("result", typeof(double), 1235,     "KProp1 Klass = 1235;               result = Klass KProp1;"),
                 TestCase("result", typeof(string), "Fluent2","KProp2 Klass = 'Fluent2';          result = Klass KProp2;"),
                 TestCase("result", typeof(bool),   false,    "KProp3 Klass = false;              result = Klass KProp3;"),
                 TestCase("result", typeof(double), date2,    "KProp4 Klass = new Date(2012,6,3); result = Klass KProp4;"),		         
             };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
                i.Context.Types.Register(typeof(Klass), null);
                i.Context.Symbols.Global.DefineVariable("klass", typeof(Klass));
                i.Context.Memory.SetValue("klass", new LClass(new Klass()));
            });
        }


        [Test]
        public void Can_Use_Fluent_Member_Method_Calls()
        {
            var date = new DateTime(2012, 6, 1);
            Klass.Reset();
            var statements = new List<Tuple<string, Type, object, string>>()
            {
		        // INSTANCE PROPS -----------------------------------------------------------------------------------
		        // Static prop get
                TestCase("result", typeof(double), 12 ,      "var result = klass Method1 12, 'fs', true, new Date(2012, 6, 1);"),
                TestCase("result", typeof(string), "fs",     "var result = klass Method2 12, 'fs', true, new Date(2012, 6, 1);"),
                TestCase("result", typeof(bool),   true,     "var result = klass Method3 12, 'fs', true, new Date(2012, 6, 1);"),
                TestCase("result", typeof(double), date,     "var result = klass Method4 12, 'fs', true, new Date(2012, 6, 1);"),
		                                                                                   
		        // Static prop get fluent                                                  
		        TestCase("result", typeof(double), 12 ,      "var result = Method1 klass $12, 'fs', on, 6/1/2012;"),
                TestCase("result", typeof(string), "fs",     "var result = Method2 klass $12, 'fs', on, 6/1/2012;"),
                TestCase("result", typeof(bool),   true,     "var result = Method3 klass $12, 'fs', on, 6/1/2012;"),
                TestCase("result", typeof(double), date,     "var result = Method4 klass $12, 'fs', on, 6/1/2012;"), 
                
                
                // STATIC PROPS -----------------------------------------------------------------------------------
		        // Static prop get
                TestCase("result", typeof(double), 12  ,     "var result = Klass KMethod1 12, 'fs', true, new Date(2012, 6, 1);"),
                TestCase("result", typeof(string), "fs",     "var result = Klass KMethod2 12, 'fs', true, new Date(2012, 6, 1);"),
                TestCase("result", typeof(bool),   true,     "var result = Klass KMethod3 12, 'fs', true, new Date(2012, 6, 1);"),
                TestCase("result", typeof(double), date,     "var result = Klass KMethod4 12, 'fs', true, new Date(2012, 6, 1);"),
		 
		        // Static prop get fluent
		        TestCase("result", typeof(double), 12  ,     "var result = KMethod1 Klass $12, 'fs', on, 6/1/2012;"),
                TestCase("result", typeof(string), "fs",     "var result = KMethod2 Klass $12, 'fs', on, 6/1/2012;"),
                TestCase("result", typeof(bool),   true,     "var result = KMethod3 Klass $12, 'fs', on, 6/1/2012;"),
                TestCase("result", typeof(double), date,     "var result = KMethod4 Klass $12, 'fs', on, 6/1/2012;"),
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
                i.Context.Types.Register(typeof(Klass), null);
                i.Context.Symbols.Global.DefineVariable("klass", typeof(Klass));
                i.Context.Memory.SetValue("klass", new LClass(new Klass()));
            });
        }
    }



    [TestFixture]
    public class Fluent_Call_Tests : ScriptTestsBase
    {


        //[Test]
        public void Can_Handle_Ambiguity()
        {
            throw new NotImplementedException("ambiguity not 100% fixed");
            // Test:
            // 1. words
            // 2. fluent funcs
            // 3. fluent members
            // 4. function wildcard
            var words = "@words( create user, update user ); ";
            var wild = "function 'update user set' *( wildcard, parts, args ) { return 'func-wildcard'; } ";
            var func = "function 'update user'( name ) { return 'func-fluent'; } ";
            var all = words + wild + func;
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string), "update user",    all + " var result = update user"),
                //TestCase("result", typeof(string), "func-wildcard",  all + " var result = update user set name('kishore');"),
                TestCase("result", typeof(string), "func-fluent",    all + " var result = update user('kishore');"),
                TestCase("result", typeof(string), "obj-user-del",   all + " var user = new User(); var result = delete user"),
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
                i.Context.Types.Register(typeof(User), null);
            });
        }


        [Test]
        public void Can_Call_WildCard()
        {
            var func = "function 'find user by' * ( fname, parts, args) { return fname + ' ' + parts[args[0]] + ' ' + args[1]; } ";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string), "name name fluent", func + "result = find user by name ( 0, 'fluent' ); "),
                TestCase("result", typeof(string), "name group group doctor", func + "result = find user by name group ( 1, 'doctor' ); "),
                TestCase("result", typeof(string), "name group level group senior", func + "result = find user by name group level ( 1, 'senior', 'marketer' ); ")
            };
            Parse(statements, true, i => i.Context.Plugins.Register(new FuncWildCardPlugin()));
        }


        [Test]
        public void Can_Call_WildCard_With_Underscores()
        {
            var func = "function find_user_by * ( fname, parts, args) { return fname + ' ' + parts[args[0]] + ' ' + args[1]; } ";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string), "name name fluent",              func + "result = find user by name ( 0, 'fluent' ); "),
                TestCase("result", typeof(string), "name group group doctor",       func + "result = find user by name group ( 1, 'doctor' ); "),
                TestCase("result", typeof(string), "name group level group senior", func + "result = find user by name group level ( 1, 'senior', 'marketer' ); "),

                TestCase("result", typeof(string), "name name fluent",              func + "result = find user by name: 0, 'fluent'; "),
                TestCase("result", typeof(string), "name group group doctor",       func + "result = find user by name group: 1, 'doctor'\r\n"),
                TestCase("result", typeof(string), "name group level group senior", func + "result = find user by name group level: 1, 'senior', 'marketer'"),

                TestCase("result", typeof(string), "name name fluent",              func + "result = find_user_by name ( 0, 'fluent' ); "),
                TestCase("result", typeof(string), "name group group doctor",       func + "result = find_user_by name group ( 1, 'doctor' ); "),
                TestCase("result", typeof(string), "name group level group senior", func + "result = find_user_by name group level ( 1, 'senior', 'marketer' ); "),

                TestCase("result", typeof(string), "name name fluent",              func + "result = find_user_by name: 0, 'fluent'; "),
                TestCase("result", typeof(string), "name group group doctor",       func + "result = find_user_by name group: 1, 'doctor'\r\n"),
                TestCase("result", typeof(string), "name group level group senior", func + "result = find_user_by name group level: 1, 'senior', 'marketer'"),
            };
            Parse(statements, true, i => i.Context.Plugins.Register(new FuncWildCardPlugin()));
        }


        [Test]
        public void Can_Call_MultiWord_Function_With_Underscores()
        {
            var dt = new DateTime(2012, 8, 2, 9, 30, 0);
            var func = "function order_to_buy, order_to_purchase( index, amount, of, at, on, using ) { var args = [amount, of, at, on, using]; return args[index]; }";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase( "result", typeof(double), 300,                func + "result =  order to buy (            0,         300,     'ibm',     $40,     Aug 2nd 2012 at 9:30am,        'default pricing' )"),
                TestCase( "result", typeof(string), "ibm",              func + "result =  order_to_buy (            1,         300,     'ibm',     $40,     Aug 2nd 2012 at 9:30am,        'default pricing' )"),
                TestCase( "result", typeof(double), 40.0,               func + "result =  order to buy (     index: 2, amount: 300, of: 'ibm', at: $40, on: Aug 2nd 2012 at 9:30am, using: 'default pricing' )"),
                TestCase( "result", typeof(DateTime), dt,               func + "result =  order to buy       index: 3  amount: 300  of: 'ibm'  at: $40  on: Aug 2nd 2012 at 9:30am  using: 'default pricing'  "),
                TestCase( "result", typeof(string), "default pricing",  func + "result =  order to purchase( index  4, amount  300, of  'ibm', at  $40, on  Aug 2nd 2012 at 9:30am, using  'default pricing' )"),
            };
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
                i.Context.Types.Register(typeof(User), null);
            });
        }


        [Test]
        public void Can_Call_MultiWord_Function_With_Underscores_With_String_Literals()
        {
            var dt = new DateTime(2012, 8, 2, 9, 30, 0);
            var func = "function 'order_to_buy', 'order_to_purchase'( index, amount, of, at, on, using ) { var args = [amount, of, at, on, using]; return args[index]; }";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase( "result", typeof(double), 300,                func + "result =  order to buy (            0,         300,     'ibm',     $40,     Aug 2nd 2012 at 9:30am,        'default pricing' )"),
                TestCase( "result", typeof(string), "ibm",              func + "result =  order_to_buy (            1,         300,     'ibm',     $40,     Aug 2nd 2012 at 9:30am,        'default pricing' )"),
                TestCase( "result", typeof(double), 40.0,               func + "result =  order to buy (     index: 2, amount: 300, of: 'ibm', at: $40, on: Aug 2nd 2012 at 9:30am, using: 'default pricing' )"),
                TestCase( "result", typeof(DateTime), dt,               func + "result =  order to buy       index: 3  amount: 300  of: 'ibm'  at: $40  on: Aug 2nd 2012 at 9:30am  using: 'default pricing'  "),
                TestCase( "result", typeof(string), "default pricing",  func + "result =  order to purchase( index  4, amount  300, of  'ibm', at  $40, on  Aug 2nd 2012 at 9:30am, using  'default pricing' )"),
            }; 
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
                i.Context.Types.Register(typeof(User), null);
            });
        }


        [Test]
        public void Can_Call_MultiWord_Function_With_Spaces_With_String_Literals()
        {
            var dt = new DateTime(2012, 8, 2, 9, 30, 0);
            var func = "function 'order to buy', 'order to purchase'( index, amount, of, at, on, using ) { var args = [amount, of, at, on, using]; return args[index]; }";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase( "result", typeof(double), 300,                func + "result =  order to buy (            0,         300,     'ibm',     $40,     Aug 2nd 2012 at 9:30am,        'default pricing' )"),
                TestCase( "result", typeof(double), 40.0,               func + "result =  order to buy (     index: 2, amount: 300, of: 'ibm', at: $40, on: Aug 2nd 2012 at 9:30am, using: 'default pricing' )"),
                TestCase( "result", typeof(DateTime), dt,               func + "result =  order to buy       index: 3  amount: 300  of: 'ibm'  at: $40  on: Aug 2nd 2012 at 9:30am  using: 'default pricing'  "),
                TestCase( "result", typeof(string), "default pricing",  func + "result =  order to purchase( index  4, amount  300, of  'ibm', at  $40, on  Aug 2nd 2012 at 9:30am, using  'default pricing' )"),
            }; 
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
                i.Context.Types.Register(typeof(User), null);
            });
        }


        [Test]
        public void Can_Call_MultiWord_Function_With_CamelCasing()
        {
            var dt = new DateTime(2012, 8, 2, 9, 30, 0);
            var func = "function orderToBuy, orderToPurchase( index, amount, of, at, on, using ) { var args = [amount, of, at, on, using]; return args[index]; }";
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase( "result", typeof(double), 300,                func + "result =  order to buy (            0,         300,     'ibm',     $40,     Aug 2nd 2012 at 9:30am,        'default pricing' )"),
                TestCase( "result", typeof(string), "ibm",              func + "result =  orderToBuy   (            1,         300,     'ibm',     $40,     Aug 2nd 2012 at 9:30am,        'default pricing' )"),
                TestCase( "result", typeof(double), 40.0,               func + "result =  order to buy (     index: 2, amount: 300, of: 'ibm', at: $40, on: Aug 2nd 2012 at 9:30am, using: 'default pricing' )"),
                TestCase( "result", typeof(DateTime), dt,               func + "result =  order to buy       index: 3  amount: 300  of: 'ibm'  at: $40  on: Aug 2nd 2012 at 9:30am  using: 'default pricing'  "),
                TestCase( "result", typeof(string), "default pricing",  func + "result =  order to purchase( index  4, amount  300, of  'ibm', at  $40, on  Aug 2nd 2012 at 9:30am, using  'default pricing' )"),
            }; 
            Parse(statements, true, i =>
            {
                i.Context.Plugins.RegisterAll();
                i.Context.Types.Register(typeof(User), null);
            });
        }


        
    }
}
