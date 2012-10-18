using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang.Types;
using NUnit.Framework;
using ComLib;
using ComLib.Lang;
using ComLib.Lang.Core;
using ComLib.Lang.Parsing;
using ComLib.Lang.AST;


namespace ComLib.Lang.Tests.Common
{
    public class LangTestsHelper
    {
        public static void Compare(object actual, object expected)
        {
            if (actual is LObject && actual != LObjects.Null)
                actual = ((LObject)actual).GetValue();

            if (actual is DateTime)
            {
                DateTime d1 = (DateTime)actual;
                DateTime d2 = (DateTime)expected;
                if ((d1.Hour > 0 || d1.Minute > 0 || d1.Second > 0 || d1.Millisecond > 0)
                     && (d2.Hour > 0 || d2.Minute > 0 || d2.Second > 0 || d2.Millisecond > 0))
                    Assert.AreEqual(d1, d2);
                else
                    Assert.AreEqual(d1.Date, d2);
            }
            else
                Assert.AreEqual(actual, expected);
        }


        public static void CompareType(object actual, object expected)
        {
            if (actual.GetType() == typeof(LString) && expected == typeof(string))
                return;
            if (actual.GetType() == typeof(LBool) && expected == typeof(bool))
                return;
            if (actual.GetType() == typeof(LNumber) && expected == typeof(double))
                return;
            if (actual.GetType() == typeof(LDate) && expected == typeof(DateTime))
                return;
            if (actual.GetType() == typeof(LTime) && expected == typeof(TimeSpan))
                return;
            Assert.Fail("Types do not match");
        }


        public static void Parse(List<Tuple<string, Type, object, string>> statements, bool execute = true, Action<Interpreter> initializer = null)
        {
            foreach (var stmt in statements)
            {

                Interpreter i = new Interpreter();
                if (initializer != null)
                    initializer(i);

                if (execute)
                {
                    i.Execute(stmt.Item4);
                    Assert.AreEqual(i.Memory[stmt.Item1], stmt.Item3);
                }
                else
                {
                    i.Parse(stmt.Item4);
                }
            }
        }


        public static void RunExpression(Memory memory, List<Tuple<Type, object, Expr>> expressions)
        {
            Context ctx = new Context();
            RunExpression2(ctx, memory, expressions);
        }


        public static void RunExpression2(Context ctx, Memory memory, List<Tuple<Type, object, Expr>> expressions)
        {
            ctx.Memory = memory;
            foreach (var exp in expressions)
            {   
                exp.Item3.Ctx = ctx;
                var result = exp.Item3.Evaluate();
                Assert.AreEqual(result, exp.Item2);
            }
        }
    }
}
