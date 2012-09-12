using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
