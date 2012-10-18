using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;


using ComLib;
using ComLib.Lang;
using ComLib.Lang.Core;
using ComLib.Lang.Parsing;
using ComLib.Lang.Types;
using ComLib.Lang.AST;
using ComLib.Lang.Plugins;
using ComLib.Tests;

namespace ComLib.Lang.Tests.Unit
{

    [TestFixture]
    public class Expression_Tests
    {
        [Test]
        public void Can_Do_AssignmentExpressions()
        {
            Memory memory = new Memory();
            memory.SetValue("age",    new LNumber(32));
            memory.SetValue("isMale", new LBool(true));
            memory.SetValue("name",   new LString("kishore"));

            // Strings
            Assign(memory, "str1", "kishore1", true, "kishore1");
            Assign(memory, "str2", "name",     false, "kishore");

            // Numbers
            Assign(memory, "num1", 2, true, 2);
            Assign(memory, "num3", 34.56, true, 34.56);
            Assign(memory, "num2", "age", false, 32);
            

            // bool
            Assign(memory, "b1", true, true, true);
            Assign(memory, "b2", false, true, false);
            Assign(memory, "b2", "isMale", false, true);
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
            var memory = new Memory();
            memory.SetValue("four", new LNumber(4));
            memory.SetValue("five", new LNumber(5));
            memory.SetValue("two",  new LNumber(2));
            Math(memory, "five", "two", Operator.Multiply, 10);
            Math(memory, "four", "two", Operator.Divide, 2);
            Math(memory, "five", "two", Operator.Add, 7);
            Math(memory, "five", "two", Operator.Subtract, 3);
            Math(memory, "five", "two", Operator.Modulus, 1);
        }


        [Test]
        public void Can_Do_Unary_Operations()
        {
            var memory = new Memory();
            memory.SetValue("one",   new LNumber(1));
            memory.SetValue("two",   new LNumber(2));
            memory.SetValue("three", new LNumber(3));
            memory.SetValue("four",  new LNumber(4));
            memory.SetValue("five",  new LNumber(5));
            memory.SetValue("six",   new LNumber(6));

            Unary(memory, "one", 0,   Operator.PlusPlus, 2);
            Unary(memory, "two", 2,   Operator.PlusEqual, 4);
            Unary(memory, "three", 0, Operator.MinusMinus, 2);
            Unary(memory, "four", 2,  Operator.MinusEqual, 2);
            Unary(memory, "five", 2,  Operator.MultEqual, 10);
            Unary(memory, "six", 2,   Operator.DivEqual, 3);
        }


        [Test]
        public void Can_Do_Math_Expressions_On_Constants_And_Variables()
        {
            var memory = new Memory();
            memory.SetValue("four", new LNumber(4));
            memory.SetValue("five", new LNumber(5));
            memory.SetValue("two",  new LNumber(2));            
            Math(memory, "five", 2, Operator.Multiply, 10);
            Math(memory, "four", 2, Operator.Divide, 2);
            Math(memory, "five", 2, Operator.Add, 7);
            Math(memory, "five", 2, Operator.Subtract, 3);
            Math(memory, "five", 2, Operator.Modulus, 1);
            Math(memory, 5, "two", Operator.Multiply, 10);
            Math(memory, 4, "two", Operator.Divide, 2);
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


        [Test]
        public void MemberAccessExpression()
        {
            // "result = user.Address.City";
            Tuple<string, Type, object, string, Type, object, List<Expr>> i = new Tuple<string, Type, object, string, Type, object, List<Expr>>("user", typeof(Person), new Person(), "result", typeof(string), "new york", new List<Expr>()
            {
                //new MemberAccessExpression("user", "Address"),
                //new MemberAccessExpression("City"),
            });
        }


        private void Compare(object left, object right, Operator op, bool expected)
        {
            // LESS THAN
            var exp = new CompareExpr(new ConstantExpr(left), op, new ConstantExpr(right));
            Assert.AreEqual(expected, exp.EvaluateAs<bool>());
        }


        private void Math(Memory memory, object left, object right, Operator op, double expected)
        {
            Expr expLeft = (left.GetType() == typeof(string))
                        ? (Expr)new VariableExpr(left.ToString())
                        : (Expr)new ConstantExpr(left);

            Expr expRight = (right.GetType() == typeof(string))
                         ? (Expr)new VariableExpr(right.ToString())
                         : (Expr)new ConstantExpr(right);
            var ctx = new Context() { Memory = memory };
            expLeft.Ctx = ctx;
            expRight.Ctx = ctx;
            var exp = new BinaryExpr(expLeft, op, expRight);
            Assert.AreEqual(expected, exp.EvaluateAs<double>());
        }


        private void Unary(Memory memory, string left, double inc, Operator op, double expected)
        {
            var ctx = new Context();
            ctx.Memory = memory;
            var exp = new UnaryExpr(left, inc, op, ctx);
            Assert.AreEqual(expected, exp.EvaluateAs<double>());
            Assert.AreEqual(expected, memory.GetAs<LNumber>(left).Value);
        }


        private void Assign(Memory memory, string name, object val, bool isConst, object expected)
        {
            var ctx = new Context();
            ctx.Memory = memory;
            Expr expr = isConst
                            ? (Expr)new ConstantExpr(val)
                            : (Expr)new VariableExpr(val.ToString());
            var exp = new AssignExpr(true, name, expr);
            expr.Ctx = ctx;
            exp.Ctx = ctx;
            exp.Evaluate();
            Assert.AreEqual(expected, memory.GetAs<LObject>(name).GetValue());
        }
    }
}
