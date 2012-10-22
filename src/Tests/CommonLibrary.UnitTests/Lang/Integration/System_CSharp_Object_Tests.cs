using System;
using System.Collections.Generic;
using ComLib.Lang.Types;
using ComLib.Tests;
using NUnit.Framework;


using ComLib.Lang.Tests.Common;


namespace ComLib.Lang.Tests.Integration.System
{

    [TestFixture]
    public class Script_Tests_CSharp_Objects : ScriptTestsBase
    {
        [Test]
        public void Can_Create_Via_Different_Constructors()
        {
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string),    "john",                          "var p = new Person(); var result = p.FirstName;"),
                TestCase("result", typeof(string),    "kish",                          "var p = new Person('ki', 'sh'); var result = p.FirstName + p.LastName; "),
                TestCase("result", typeof(string),    "nonew@email.com",               "var p = new Person('john', 'doe', 'nonew@email.com', true, 10.56); var result = p.Email;  "),
                TestCase("result", typeof(string),    "janedallparams@email.comfalse", "var p = new Person('jane', 'd', 'allparams@email.com', false, 10.56, new Date()); var result = p.FirstName + p.LastName + p.Email + p.IsMale;")
            };
            Parse(testcases, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Create_With_FullName_Via_Different_Constructors()
        {
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string),    "john",                          "var p = new ComLib.Tests.Person(); var result = p.FirstName;"),
                TestCase("result", typeof(string),    "kish",                          "var p = new ComLib.Tests.Person('ki', 'sh'); var result = p.FirstName + p.LastName; "),
                TestCase("result", typeof(string),    "nonew@email.com",               "var p = new ComLib.Tests.Person('john', 'doe', 'nonew@email.com', true, 10.56); var result = p.Email;  "),
                TestCase("result", typeof(string),    "janedallparams@email.comfalse", "var p = new ComLib.Tests.Person('jane', 'd', 'allparams@email.com', false, 10.56, new Date()); var result = p.FirstName + p.LastName + p.Email + p.IsMale;")
            };
            Parse(testcases, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Get_Static_Properties()
        {
            var actualdate1 = DateTime.Today.AddDays(1);
            var expectdate1 = DateTime.Today.AddDays(1);
            var actualtime1 = new TimeSpan(0, 10, 8, 49);
            var expecttime1 = new TimeSpan(0, 10, 8, 49);

            var actualdate2 = DateTime.Today.AddDays(2);
            var expectdate2 = DateTime.Today.AddDays(2);
            var actualtime2 = new TimeSpan(0, 10, 33, 59);
            var expecttime2 = new TimeSpan(0, 10, 33, 59);

            // Set nested class props.
            var nestedInstance = new KlassNested1();
            nestedInstance.SetProps(23.45, "nested@gmail.com", false, actualdate2, actualtime2);

            // Set clas props.
            Klass2.SetClassProps(12.34, "fluentscript", true, actualdate1, actualtime1, nestedInstance);
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(double),   12.34,               "var result = Klass2.KPropNumber"),
                TestCase("result", typeof(string),   "fluentscript",      "var result = Klass2.KPropString"),
                TestCase("result", typeof(bool),     true,                "var result = Klass2.KPropBool"),
                TestCase("result", typeof(DateTime), expectdate1,         "var result = Klass2.KPropDate"),
                TestCase("result", typeof(TimeSpan), expecttime1,         "var result = Klass2.KPropTime"),

                TestCase("result", typeof(double),   23.45,               "var result = Klass2.KPropObject.KPropNumber"),
                TestCase("result", typeof(string),   "nested@gmail.com",  "var result = Klass2.KPropObject.KPropString"),
                TestCase("result", typeof(bool),     false,               "var result = Klass2.KPropObject.KPropBool"),
                TestCase("result", typeof(DateTime), expectdate2,         "var result = Klass2.KPropObject.KPropDate"),
                TestCase("result", typeof(TimeSpan), expecttime2,         "var result = Klass2.KPropObject.KPropTime"),
            };
            Parse(testcases, true, (i) => i.Context.Types.Register(typeof(Klass2), null));
        }


        [Test]
        public void Can_Set_Static_Properties()
        {
            var actualdate1 = DateTime.Today;
            var actualtime1 = new TimeSpan(0, 10, 8, 49);
            
            var expectdate1 = new DateTime(2012, 10, 29);
            var expecttime1 = new TimeSpan(0, 3, 15, 45);

            var actualdate2 = DateTime.Today.AddDays(1);
            var actualtime2 = new TimeSpan(0, 10, 33, 59);

            var expectdate2 = new DateTime(2012, 10, 30);
            var expecttime2 = new TimeSpan(0, 4, 16, 55);

            // Set nested class props.
            var nestedInstance = new KlassNested1();
            nestedInstance.SetProps(23.45, "nested@gmail.com", false, actualdate2, actualtime2);

            // Set clas props.
            Klass2.SetClassProps(12.34, "fluentscript", true, actualdate1, actualtime1, nestedInstance);
            
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(double),   11.22,              "Klass2.KPropNumber = 11.22;                  var result = Klass2.KPropNumber ; "), 
                TestCase("result", typeof(string),   "abcd",             "Klass2.KPropString = 'abcd';                 var result = Klass2.KPropString ; "), 
                TestCase("result", typeof(bool),     false,              "Klass2.KPropBool   = false;                  var result = Klass2.KPropBool   ; "), 
                TestCase("result", typeof(DateTime), expectdate1,        "Klass2.KPropDate   = new Date(2012, 10, 29); var result = Klass2.KPropDate   ; "), 
                TestCase("result", typeof(TimeSpan), expecttime1,        "Klass2.KPropTime   = new Time(0, 3, 15, 45); var result = Klass2.KPropTime   ; "), 

                TestCase("result", typeof(double),   22.33,              "Klass2.KPropObject.KPropNumber = 22.33;                   var result = Klass2.KPropObject.KPropNumber ;"),
                TestCase("result", typeof(string),   "nes1",             "Klass2.KPropObject.KPropString = 'nes1';                  var result = Klass2.KPropObject.KPropString ;"),
                TestCase("result", typeof(bool),     true,               "Klass2.KPropObject.KPropBool   = true;                    var result = Klass2.KPropObject.KPropBool   ;"),
                TestCase("result", typeof(DateTime), expectdate2,        "Klass2.KPropObject.KPropDate   = new Date(2012, 10, 30);  var result = Klass2.KPropObject.KPropDate   ;"),
                TestCase("result", typeof(TimeSpan), expecttime2,        "Klass2.KPropObject.KPropTime   = new Time(4, 16, 55);     var result = Klass2.KPropObject.KPropTime   ;"),
            };
            Parse(testcases, true, (i) => i.Context.Types.Register(typeof(Klass2), null));
        }


        [Test]
        public void Can_Call_Static_Methods()
        {
            var date = new DateTime(2012, 10, 24);
            var time = new TimeSpan(0, 7, 40, 20);
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(double),   12.34,    "var result = Klass2.KMethodRetNumber(12.34, 'test_1', true, new Date(2012, 10, 24), new Time(0, 7, 40, 20));"),
                TestCase("result", typeof(string),   "test_1", "var result = Klass2.KMethodRetString(12.34, 'test_1', true, new Date(2012, 10, 24), new Time(0, 7, 40, 20));"),
                TestCase("result", typeof(bool),     true,     "var result = Klass2.KMethodRetBool(  12.34, 'test_1', true, new Date(2012, 10, 24), new Time(0, 7, 40, 20));"),
                TestCase("result", typeof(DateTime), date,     "var result = Klass2.KMethodRetDate(  12.34, 'test_1', true, new Date(2012, 10, 24), new Time(0, 7, 40, 20));"),
                TestCase("result", typeof(TimeSpan), time,     "var result = Klass2.KMethodRetTime(  12.34, 'test_1', true, new Date(2012, 10, 24), new Time(0, 7, 40, 20));"),
                
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Klass2), null));
        }


        [Test]
        public void Can_Get_Instance_Properties()
        {
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string),   "john",                "var p = new Person(); var result = p.FirstName;" ),
                TestCase("result", typeof(string),   "johndoe@email.com",   "var p = new Person(); var result = p.Email;" ),
                TestCase("result", typeof(bool),     true,                  "var p = new Person(); var result = p.IsMale;" ),
                TestCase("result", typeof(double),   10.5,                  "var p = new Person(); var result = p.Salary;" ),
                TestCase("result", typeof(DateTime), DateTime.Today,        "var p = new Person(); var result = p.BirthDate;" ),                
                TestCase("result", typeof(string),   "Queens",              "var p = new Person(); var result = p.Address.City;" ),
                TestCase("result", typeof(string),   "NY",                  "var p = new Person(); var result = p.Address.State;" )
            };
            Parse(testcases, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Set_Instance_Properties()
        {
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string),   "jane",              "var p = new Person();  p.FirstName = 'jane';      var result = p.FirstName;" ),
                TestCase("result", typeof(string),   "janedoe@email.com", "var p = new Person();  p.Email     = 'janedoe@email.com'; var result = p.Email;" ),
                TestCase("result", typeof(bool),     false,               "var p = new Person();  p.IsMale    =  false;       var result = p.IsMale;" ),
                TestCase("result", typeof(double),   10.8,                "var p = new Person();  p.Salary    = 10.8;         var result = p.Salary;" ),
                TestCase("result", typeof(DateTime), DateTime.Today,      "var p = new Person();  p.BirthDate = new Date();   var result = p.BirthDate;" ),    
                TestCase("result", typeof(string),   "Bronx",             "var p = new Person();  p.Address.City = 'Bronx';   var result = p.Address.City;" ),
                TestCase("result", typeof(string),   "NJ",                "var p = new Person();  p.Address.State = 'NJ';     var result = p.Address.State;" )
            };
            Parse(testcases, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Call_Instance_Methods()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string),    "ki sh",                 "var p = new Person('ki', 'sh', 'comlib@email.com', true, 12.34); var result = p.FullName();"),
                TestCase("result", typeof(string),    "programmer ki sh sr.",  "var p = new Person('ki', 'sh', 'comlib@email.com', true, 12.34); var result = p.FullNameWithPrefix('programmer', true);")                
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Call_Instance_Methods_With_Named_Params()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( first: 'ki', last: 'sh', email: 'comlib@email.com', isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                TestCase("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', last: 'sh', email: 'comlib@email.com', isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                TestCase("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', email: 'comlib@email.com', isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                TestCase("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', 'comlib@email.com', isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                TestCase("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', 'comlib@email.com', true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                TestCase("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', 'comlib@email.com', true, 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Test]
        public void Can_Call_Instance_Methods_With_Named_Params_Using_Nulls()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                TestCase("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( first: 'ki', last: 'sh', email: null, isMale: true, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                TestCase("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', last: 'sh', email: 'comlib@email.com', isMale: null, salary: 12.34, birthday: new Date(2012, 7, 10)); var result = p.FullName();"),
                TestCase("result", typeof(string),    "ki sh",  "var p = new Person(); p.Init( 'ki', 'sh', email: 'comlib@email.com', isMale: true, salary: null, birthday: null); var result = p.FullName();"),
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }
    }
}
