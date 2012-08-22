using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using ComLib.Lang;

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

            var zero = new ConstantExpr(0);
            zero.Ref = new ScriptRef("", 1, 5);

            var divExpr = new BinaryExpr(a, Operator.Divide, zero);
            var assignStmt = new AssignStmt(true, new VariableExpr("result"), divExpr);
            assignStmt.SymScope = symScope.Current;
            var stmts = new List<Stmt>();
            stmts.Add(assignStmt);

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
            var semacts = new SemActs();
            var a = new VariableExpr("a");
            a.Ref = new ScriptRef("", 1, 1);

            var zero = new ConstantExpr(2);
            zero.Ref = new ScriptRef("", 1, 5);

            var symScope = new Symbols();
            symScope.DefineVariable("result");
            var divExpr = new BinaryExpr(a, Operator.Divide, zero);
            var assignStmt = new AssignStmt(true, new VariableExpr("result"), divExpr);
            assignStmt.SymScope = symScope.Current;
            var stmts = new List<Stmt>();
            stmts.Add(assignStmt);

            bool success = semacts.Validate(stmts);
            var results = semacts.Results;
            Assert.IsFalse(success);
            Assert.IsFalse(results.Success);
            Assert.IsTrue(results.HasResults);
            Assert.AreEqual(results.Results.Count, 1);
        }
    }
}