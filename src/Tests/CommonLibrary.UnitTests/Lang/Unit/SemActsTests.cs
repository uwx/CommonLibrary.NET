using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using ComLib.Lang;
using ComLib.Lang.Core;
using ComLib.Lang.AST;
using ComLib.Lang.Parsing;
using ComLib.Lang.Plugins;

namespace ComLib.Lang.Tests.Unit
{
    [TestFixture]
    public class SemActs_Tests
    {

        [Test]
        public void Can_Validate_Division_By_Zero()
        {
            var symScope = new Symbols();
            symScope.DefineVariable("result");
            symScope.DefineVariable("a");

            var semacts = new SemActs();
            var a = new VariableExpr("a");
            a.Ref = new ScriptRef("", 1, 1);
            a.SymScope = symScope.Current;
            
            var zero = new ConstantExpr(0);
            zero.Ref = new ScriptRef("", 1, 5);
            zero.SymScope = symScope.Current;

            var divExpr = new BinaryExpr(a, Operator.Divide, zero);
            divExpr.SymScope = symScope.Current;
            var assignExpr = new AssignExpr(true, new VariableExpr("result"), divExpr);
            assignExpr.SymScope = symScope.Current;
            var stmts = new List<Expr>();
            stmts.Add(assignExpr);

            bool success = semacts.Validate(stmts);
            var results = semacts.Results;
            Assert.IsFalse(success);
            Assert.IsFalse(results.Success);
            Assert.IsTrue(results.HasResults);
            Assert.AreEqual(results.Results.Count, 1);
        }


        [Test]
        public void Can_Validate_Variable_Does_Not_Exist()
        {
            var symScope = new Symbols();
            symScope.DefineVariable("result");
            
            var semacts = new SemActs();
            var a = new VariableExpr("a");
            a.Ref = new ScriptRef("", 1, 1);
            a.SymScope = symScope.Current;
            
            var zero = new ConstantExpr(2);
            zero.Ref = new ScriptRef("", 1, 5);
            zero.SymScope = symScope.Current;

            var divExpr = new BinaryExpr(a, Operator.Divide, zero);
            divExpr.SymScope = symScope.Current;

            var assignExpr = new AssignExpr(true, new VariableExpr("result"), divExpr);
            assignExpr.SymScope = symScope.Current;
            var stmts = new List<Expr>();
            stmts.Add(assignExpr);

            bool success = semacts.Validate(stmts);
            var results = semacts.Results;
            Assert.IsFalse(success);
            Assert.IsFalse(results.Success);
            Assert.IsTrue(results.HasResults);
            Assert.AreEqual(results.Results.Count, 1);
        }
    }
}