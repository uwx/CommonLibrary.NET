using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;


using ComLib;
using ComLib.Lang;


namespace CommonLibrary.Tests
{

    [TestFixture]
    public class Lang_Expression_Tests
    {
        [Test]
        public void Can_Do_AssignmentExpressions()
        {
            Scope scope = new Scope();
            scope.SetValue("age", 32);
            scope.SetValue("isMale", true);
            scope.SetValue("name", "kishore");

            // Strings
            Assign(scope, "str1", "kishore1", true, "kishore1");
            Assign(scope, "str2", "name", false, "kishore");

            // Numbers
            Assign(scope, "num1", 2, true, 2);
            Assign(scope, "num3", 34.56, true, 34.56);
            Assign(scope, "num2", "age", false, 32);
            

            // bool
            Assign(scope, "b1", true, true, true);
            Assign(scope, "b2", false, true, false);
            Assign(scope, "b2", "isMale", false, true);
        }


        [Test]
        public void Can_Do_Math_Expressions_On_Constants()
        {
            Math(null, 5, 2, Operator.Multiply, 10);
            Math(null, 4, 2, Operator.Divide, 2);
            Math(null, 5, 2, Operator.Add, 7);
            Math(null, 5, 2, Operator.Subtract, 3);
            Math(null, 5, 2, Operator.Modulus, 1);
        }


        [Test]
        public void Can_Do_Math_Expressions_On_Variables()
        {
            Scope scope = new Scope();
            scope.SetValue("four", 4);
            scope.SetValue("five", 5);
            scope.SetValue("two", 2);
            Math(scope, "five", "two", Operator.Multiply, 10);
            Math(scope, "four", "two", Operator.Divide, 2);
            Math(scope, "five", "two", Operator.Add, 7);
            Math(scope, "five", "two", Operator.Subtract, 3);
            Math(scope, "five", "two", Operator.Modulus, 1);
        }


        [Test]
        public void Can_Do_Unary_Operations()
        {
            Scope scope = new Scope();
            scope.SetValue("one", 1);
            scope.SetValue("two", 2);
            scope.SetValue("three", 3);
            scope.SetValue("four", 4);
            scope.SetValue("five", 5);
            scope.SetValue("six", 6);

            Unary(scope, "one", 0, Operator.PlusPlus, 2);
            Unary(scope, "two", 2, Operator.PlusEqual, 4);
            Unary(scope, "three", 0, Operator.MinusMinus, 2);
            Unary(scope, "four", 2, Operator.MinusEqual, 2);
            Unary(scope, "five", 2, Operator.MultEqual, 10);
            Unary(scope, "six", 2, Operator.DivEqual, 3);
        }


        [Ignore]       
        public void Can_Do_String_Method_Tests()
        {
            Scope scope = new Scope();
            scope.SetValue("email", "kishore@mixedmartialarts.com");
            
            var expressions = new List<Tuple<Type, object, Expression>>()
            {
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "charCodeAt",   new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "concat",       new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "fromCharCode", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "indexOf",      new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "lastIndexOf",  new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "match",        new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "replace",      new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "search",       new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "slice",        new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "split",        new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "substr",       new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "substring",    new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "toLowerCase",  new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "toUpperCase",  new List<object>() { 1 })),
                new Tuple<Type, object, Expression>(typeof(string), "a", new FunctionCallExpression(scope, "email", "valueOf",      new List<object>() { 1 }))
            };
        }


        [Ignore]
        public void Can_Do_Date_Method_Tests()
        {

            Scope scope = new Scope();
            scope.SetValue("email", "kishore@mixedmartialarts.com");
            
            var expressions = new List<Tuple<Type, object, Expression>>()
            {
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getDate				", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getDay				", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getFullYear			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getHours			    ", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getMilliseconds		", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getMinutes			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getMonth			    ", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getSeconds			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getTime				", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getTimezoneOffset	", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getUTCDate			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getUTCDay			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getUTCFullYear		", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getUTCHours			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getUTCMilliseconds	", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getUTCMinutes		", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getUTCMonth			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "getUTCSeconds		", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "parse				", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setDate				", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setFullYear			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setHours			    ", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setMilliseconds		", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setMinutes			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setMonth			    ", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setSeconds			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setTime				", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setUTCDate			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setUTCFullYear		", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setUTCHours			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setUTCMilliseconds	", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setUTCMinutes		", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setUTCMonth			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "setUTCSeconds		", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "toDateString		    ", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "toLocaleDateString	", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "toLocaleTimeString	", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "toLocaleString		", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "toString			    ", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "toTimeString		    ", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "toUTCString			", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "UTC					", new List<object>() { 1 })),
                new Tuple<Type, object, Expression>( typeof(double), 2, new FunctionCallExpression(scope, "testdate", "valueOf				", new List<object>() { 1 }))
            };
        }


        [Test]
        public void Can_Do_Array_Expressions()
        {
            Scope scope = new Scope();
            scope.SetValue("email", "kishore@mixedmartialarts.com");
            
            var expressions = new List<Tuple<Type, object, Expression>>()
            {
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "concat	", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "indexOf	", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "join		", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "pop		", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "push		", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "reverse	", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "shift	", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "slice	", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "sort		", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "splice	", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "toString	", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "unshift	", new List<object>(){ 1})),
                new Tuple<Type, object, Expression>(typeof(LArray), 2, new FunctionCallExpression(scope, "testarray", "valueOf	", new List<object>(){ 1}))
            };
        }


        [Test]
        public void Can_Do_Math_Expressions_On_Constants_And_Variables()
        {
            Scope scope = new Scope();
            scope.SetValue("four", 4);
            scope.SetValue("five", 5);
            scope.SetValue("two", 2);            
            Math(scope, "five", 2, Operator.Multiply, 10);
            Math(scope, "four", 2, Operator.Divide, 2);
            Math(scope, "five", 2, Operator.Add, 7);
            Math(scope, "five", 2, Operator.Subtract, 3);
            Math(scope, "five", 2, Operator.Modulus, 1);

            Math(scope, 5, "two", Operator.Multiply, 10);
            Math(scope, 4, "two", Operator.Divide, 2);
        }


        [Test]
        public void Can_Do_Compare_Expressions_On_Constants()
        {
            // MORE THAN
            Compare(5, 4, Operator.MoreThan, true);
            Compare(5, 5, Operator.MoreThan, false);
            Compare(5, 6, Operator.MoreThan, false);

            // MORE THAN EQUAL
            Compare(5, 4, Operator.MoreThanEqual, true);
            Compare(5, 5, Operator.MoreThanEqual, true);
            Compare(5, 6, Operator.MoreThanEqual, false);

            // LESS THAN
            Compare(5, 6, Operator.LessThan, true);
            Compare(5, 5, Operator.LessThan, false);
            Compare(5, 4, Operator.LessThan, false);

            // LESS THAN EQUAL
            Compare(5, 6, Operator.LessThanEqual, true);
            Compare(5, 5, Operator.LessThanEqual, true);
            Compare(5, 4, Operator.LessThanEqual, false);

            // EQUAL
            Compare(5, 6, Operator.EqualEqual, false);
            Compare(5, 5, Operator.EqualEqual, true);
            Compare(5, 4, Operator.EqualEqual, false);

            // NOT EQUAL
            Compare(5, 6, Operator.NotEqual, true);
            Compare(5, 5, Operator.NotEqual, false);
            Compare(5, 4, Operator.NotEqual, true);
        }


        private void Compare(object left, object right, Operator op, bool expected)
        {
            // LESS THAN
            var exp = new CompareExpression(new ConstantExpression(left), op, new ConstantExpression(right));
            Assert.AreEqual(expected, exp.EvaluateAs<bool>());
        }


        private void Math(Scope scope, object left, object right, Operator op, double expected)
        {
            Expression expLeft = (left.GetType() == typeof(string))
                        ? (Expression)new VariableExpression(left.ToString(), scope)
                        : (Expression)new ConstantExpression(left);

            Expression expRight = (right.GetType() == typeof(string))
                         ? (Expression)new VariableExpression(right.ToString(), scope)
                         : (Expression)new ConstantExpression(right);

            var exp = new BinaryExpression(expLeft, op, expRight);
            Assert.AreEqual(expected, exp.EvaluateAs<double>());
        }


        private void Unary(Scope scope, string left, double inc, Operator op, double expected)
        {
            var exp = new UnaryExpression(left, inc, op, scope);
            Assert.AreEqual(expected, exp.EvaluateAs<double>());
            Assert.AreEqual(expected, scope.Get<double>(left));
        }


        private void Assign(Scope scope, string name, object val, bool isConst, object expected)
        {
            Expression expr = isConst
                            ? (Expression)new ConstantExpression(val)
                            : (Expression)new VariableExpression(val.ToString(), scope);
            var exp = new AssignmentStatement(name, expr, scope);
            exp.Execute();
            Assert.AreEqual(expected, scope.Get<object>(name));
        }
    }


    [TestFixture]
    public class Lang_Parse_Tests
    {
        [Test]
        public void Can_Do_Single_Assignment_Constant_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("name", typeof(object), null,        "var name;"),
                new Tuple<string,Type, object, string>("name", typeof(string), "kishore",   "var name = 'kishore';"),
                new Tuple<string,Type, object, string>("name", typeof(string), "kishore",   "var name = \"kishore\";"),
                new Tuple<string,Type, object, string>("age", typeof(double),   32,         "var age = 32;"),
                new Tuple<string,Type, object, string>("isActive", typeof(bool), true,      "var isActive = true;"),
                new Tuple<string,Type, object, string>("isActive", typeof(bool), false,     "var isActive = false;"),
            };
            Parse(statements);
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
        public void Can_Do_Single_Assignment_Constant_Math_Expressions_With_Mixed_Types()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "comlibext",  "var result = 'comlib' + 'ext';"),
                new Tuple<string,Type, object, string>("result", typeof(string), "comlib2",    "var result = 'comlib' + 2;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "3comlib",    "var result = 3 + 'comlib';"),
                new Tuple<string,Type, object, string>("result", typeof(string), "comlibtrue", "var result = 'comlib' + true;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "comlibfalse","var result = 'comlib' + false;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,            "var result = 2 + false;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,            "var result = 2 + true;" ),
                
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Single_Assignment_Constant_Compare_Expressions()
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
        public void Can_Do_If_Statements_With_Constants()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 2; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 3; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 4; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 5, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 5; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 6; }"),
            };
            Parse(statements);
        }


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
        public void Can_Do_For_Statements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; for(var ndx = 0; ndx < 5; ndx++) { result = ndx; }")
            };
            Parse(statements);
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
                new Tuple<string,Type, object, string>("result", typeof(string), 3, "var result = 2; result++; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; result--; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 2; result += 2; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; result -= 2; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 6, "var result = 2; result *= 3; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 6; result /= 2; "),
            };
            Parse(statements);
        }


        [Test]
        public void Can_Do_Funcion_Calls_As_Statements()
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
        public void Can_Do_Funcion_Calls_As_Expressions()
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


        private void Parse(List<Tuple<string, Type, object, string>> statements, bool execute = true, Action<Interpreter> initializer = null)
        {
            foreach (var stmt in statements)
            {

                Interpreter i = new Interpreter();
                if (initializer != null)
                    initializer(i);

                if (execute)
                {
                    i.Execute(stmt.Item4);
                    Assert.AreEqual(i.Scope[stmt.Item1], stmt.Item3);
                }
                else
                {
                    i.Parse(stmt.Item4);
                }
            }
        }


        private void ParseFuncCalls(List<Tuple<string, int, Type, object, string>> statements)
        {
            foreach (var stmt in statements)
            {

                Interpreter i = new Interpreter();
                object result = null;

                string funcCallTxt = stmt.Item5;

                // Handle calls to "user.create".
                i.SetFunctionCallback(stmt.Item1, (exp) =>
                {
                    // 1. Check number of parameters match
                    Assert.AreEqual(exp.ParamList.Count, stmt.Item2);

                    // 2. Check name of func
                    Assert.AreEqual(exp.Name, stmt.Item1);

                    if (stmt.Item2 > 0)
                    {
                        // 3. return the type
                        result = exp.ParamList[Convert.ToInt32(exp.ParamList[0])];
                        return result;
                    }
                    result = 1;
                    return result;
                });

                i.Execute(funcCallTxt);

                // 4. Check return value
                Assert.AreEqual(result, stmt.Item4);
            }
        }
    }
}
