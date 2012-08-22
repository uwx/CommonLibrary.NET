using System;
using System.Collections.Generic;
using System.Text;

using ComLib;
using ComLib.Automation;
using NUnit.Framework;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class InterpreterTests
    {
        [Test]
        public void CanCheckForLoop()
        {
            var interpreter = new Interpreter(new Scope());
            
            // Valid formats
            Assert.IsTrue(interpreter.IsForLoop("for(var ndx = 0; ndx < 10; ndx++ )"));
            Assert.IsTrue(interpreter.IsForLoop("for( var ndx = 0; ndx > total; ndx++ ){"));
            Assert.IsTrue(interpreter.IsForLoop("for (var ndx = 0; ndx >= 10; ndx++ )"));
            Assert.IsTrue(interpreter.IsForLoop("for ( var ndx = 0 ; ndx <= 10; ndx+=2 )"));
            Assert.IsTrue(interpreter.IsForLoop("for(var ndx = 0; ndx < 10; ndx++ )"));

            // InValid formats
            Assert.IsFalse(interpreter.IsForLoop("for var ndx = 0; ndx < 10; ndx++ )"));
            Assert.IsFalse(interpreter.IsForLoop("for( var ndx = 0 ndx < 10; ndx++ ){"));
            Assert.IsFalse(interpreter.IsForLoop("for (var ndx = 0; ndx  10; ndx++ )"));
            Assert.IsFalse(interpreter.IsForLoop("for ( var ndx = 0 ; ndx <= 10;  )"));
            Assert.IsFalse(interpreter.IsForLoop("for(var ndx = 0; ndx < 10; ndx++ "));
        }


        [Test]
        public void CanTokenizeForLoop()
        {
            var interpreter = new Interpreter(new Scope());
            
            // Valid formats
            CheckFor(interpreter.TokenizeForLoop("for ( var ndx = 0 ; ndx <= 10; ndx++)", 1), "ndx", "0", "<=", "10", "+=", "1");
            CheckFor(interpreter.TokenizeForLoop("for ( var i = 2 ; i < 20; i+=2)", 1),       "i", "2", "<", "20", "+=", "2");
            CheckFor(interpreter.TokenizeForLoop("for ( var i = 10 ; i > 0; i--)", 1),        "i", "10", ">", "0", "-=", "1");
            CheckFor(interpreter.TokenizeForLoop("for ( var i = 12 ; i >= 4; i-=2)", 1),      "i", "12", ">=", "4", "-=", "2");
        }


        private static void CheckFor(Interpreter.ForLoopExpression exp, string varname, string startExp, string checkOp, string checkExp, string incrementOp, string incrementExp)
        {
            Assert.AreEqual(exp.Variable, varname);
            Assert.AreEqual(exp.StartExpression, startExp);
            Assert.AreEqual(exp.CheckOp, checkOp);
            Assert.AreEqual(exp.CheckExpression, checkExp);
            Assert.AreEqual(exp.IncrementOp, incrementOp);
            Assert.AreEqual(exp.IncrementExpression, incrementExp);
        }
    }
}
