using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using ComLib.Lang;
using ComLib.Lang.Core;
using ComLib.Lang.Types;

namespace ComLib.Lang.Tests.Unit
{
    [TestFixture]
    public class Symbols_Global_Tests
    {

        [Test]
        public void Can_Define_Variable()
        {
            var syms = new SymbolsGlobal();
            syms.DefineVariable("a");

            Assert.IsTrue(syms.Contains("a"));
        }


        [Test]
        public void Can_Define_Constant()
        {
            var syms = new SymbolsGlobal();
            syms.DefineConstant("MIN", 10);
            Assert.IsTrue(syms.Contains("MIN"));
            Assert.IsTrue(syms.GetSymbol("MIN").Category == SymbolConstants.Const);
            Assert.AreEqual(syms.GetSymbol<SymbolTypeConst>("MIN").Value, 10);
        }


        [Test]
        public void Can_Get_Var_Symbol()
        {
            var syms = new SymbolsGlobal();
            syms.DefineVariable("a");
            var sym = syms.GetSymbol("a");

            Assert.AreEqual(sym.Name, "a");
            Assert.AreEqual(sym.DataType, typeof(object));
            Assert.AreEqual(sym.Category, "var");
        }


        [Test]
        public void Can_Get_Function_Symbol()
        {
            var syms = new SymbolsGlobal();
            syms.DefineFunction("add", 2, new string[]{ "a", "b"}, typeof(object));
            var sym = syms.GetSymbol<SymbolTypeFunc>("add");

            throw new NotImplementedException();
            Assert.AreEqual(sym.Name, "add");
            //Assert.AreEqual(sym.DataType, typeof(LFunction));
            Assert.AreEqual(sym.Category, "func");
            Assert.AreEqual(sym.Meta.TotalArgs, 2);
            Assert.AreEqual(sym.Meta.Arguments[0].Name, "a");
            Assert.AreEqual(sym.Meta.Arguments[1].Name, "b");
            Assert.AreEqual(sym.Meta.ReturnType, typeof(object));
        }
    }



    [TestFixture]
    public class Symbols_Function_With_Global_Tests
    {
        private ISymbols GetNested()
        {
            var symg = new SymbolsGlobal();
            symg.DefineVariable("a");
            symg.DefineVariable("c");
            symg.DefineFunction("add", 2, new string[] { "a", "b" }, typeof(object));            

            var symn = new SymbolsFunction("add", symg);
            symn.DefineVariable("a");
            symn.DefineVariable("b");
            return symn;
        }


        [Test]
        public void Is_Nesting_Of_Variables_Configured()
        {
            var syms = GetNested();

            Assert.AreEqual(syms.Name, "add");
            Assert.IsNotNull(syms.ParentScope);
            Assert.AreEqual(syms.ParentScope.Name, "global");
        }


        [Test]
        public void Can_Define_Variable()
        {
            var syms = GetNested();
            syms.DefineVariable("d");

            Assert.IsTrue(syms.Contains("d"));  
            Assert.IsFalse(syms.ParentScope.Contains(("d")));
        }


        [Test]
        public void Can_Check_For_Nested_Variables()
        {
            var syms = GetNested();            
            Assert.IsTrue(syms.Contains("a"));
            Assert.IsTrue(syms.ParentScope.Contains("a"));
            Assert.IsTrue(syms.Contains("c"));
        }
    }



    [TestFixture]
    public class Symbols_Tests
    {
        [Test]
        public void Can_Define_Vars()
        {
            var syms = new Symbols();
            syms.DefineVariable("a");
            syms.DefineVariable("c");
            syms.Global.DefineFunction("add", 2, new string[] { "a", "b" }, typeof(object)); 
            syms.Push(new SymbolsFunction("add"), true);
            syms.DefineVariable("d");

            Assert.IsFalse(syms.Global.Contains("d"));
            Assert.IsTrue(syms.Current.Contains("d"));
            Assert.IsTrue(syms.Contains("d"));
            Assert.IsTrue(syms.Global.Contains("a"));
            Assert.IsTrue(syms.Contains("a"));
        }


        [Test]
        public void Can_Have_Block_Inside_Function()
        {
            var syms = new Symbols();
            syms.DefineVariable("a");
            syms.Global.DefineFunction("add", 2, new string[] { "a", "b" }, typeof(object));

            // func
            var sf = new SymbolsFunction("add", syms.Global);
            syms.Push(sf, true);

            // block in func
            var sb = new SymbolsNested("block", sf);
            syms.Push(sb, true);

            // put variable c in block in function
            syms.DefineVariable("c");

            Assert.IsFalse(syms.Global.Contains("c"));
            Assert.IsFalse(sf.Contains("c"));
            Assert.IsTrue(sb.Contains("c"));
            Assert.IsTrue(syms.Contains("c"));
            Assert.IsTrue(syms.Current.Contains("c"));
        }
    }
}