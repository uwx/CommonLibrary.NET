using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using ComLib.Lang;
using ComLib.Lang.Plugins;


namespace ComLib.Lang.Tests.Unit
{
    /*
    [TestFixture]
    public class FluentCallExpressionTests
    {
        private List<Expr> ToVariableExpressions(params string[] args)
        {
            var exps = new List<Expr>();
            foreach (string arg in args)
                exps.Add(new VariableExpr(arg));
            return exps;
        }


        private void RegisterFunctions(Context ctx, string formatSeparater, bool isInternalFunc, string[] args)
        {
            var names = new List<string>();

            // Build up the list of function names.
            string name = args[0];
            names.Add(name);
            for (int ndx = 1; ndx < args.Length; ndx++)
            {

                if (formatSeparater == "none")
                    name += args[ndx];
                else
                    name += formatSeparater + args[ndx];

                names.Add(name);
            }
            // Now register the function in context
            foreach (var functionName in names)
            {
                if (isInternalFunc)
                    ctx.Functions.Register(functionName, new FunctionStmt());
                else
                    ctx.ExternalFunctions.Register(functionName, (fce) => fce.Name);
            }
        }



        [Test]
        public void Can_Check_For_Matching_Multiword_Function_Name_In_Internal_Script()
        {
            Context ctx = new Context();
            RegisterFunctions(ctx, " ", true, new string[]{ "order", "to", "buy", "shares"});
            var exps = ToVariableExpressions("order", "to", "buy", "shares");
            var result = FluentCallHelper.CheckMultiWordFunctionName(ctx, exps);

            Assert.IsTrue(result.Exists);
            Assert.AreEqual("order to buy shares", result.Name);
            Assert.AreEqual(MemberMode.FunctionScript, result.FunctionMode);
        }


        [Test]
        public void Can_Check_For_Matching_Multiword_Function_Name_To_Single_Word_Function_In_Internal_Script()
        {
            Context ctx = new Context();
            RegisterFunctions(ctx, "none", true, new string[] { "order", "To", "Buy", "Shares" });
            var exps = ToVariableExpressions("order", "to", "buy", "shares");
            var result = FluentCallHelper.CheckMultiWordFunctionName(ctx, exps);

            Assert.IsTrue(result.Exists);
            Assert.AreEqual("orderToBuyShares", result.Name);
            Assert.AreEqual(MemberMode.FunctionScript, result.FunctionMode);
        }


        [Test]
        public void Can_Check_For_Matching_Multiword_Function_Name_To_Single_Word_Underscore_Function_In_Internal_Script()
        {
            Context ctx = new Context();
            RegisterFunctions(ctx, "_", true, new string[] { "order", "To", "Buy", "Shares" });
            var exps = ToVariableExpressions("order", "to", "buy", "shares");
            var result = FluentCallHelper.CheckMultiWordFunctionName(ctx, exps);

            Assert.IsTrue(result.Exists);
            Assert.AreEqual("order_To_Buy_Shares", result.Name);
            Assert.AreEqual(MemberMode.FunctionScript, result.FunctionMode);
        }


        [Test]
        public void Can_Check_For_Matching_Multiword_Function_Name_In_External_Script()
        {
            Context ctx = new Context();
            RegisterFunctions(ctx, " ", false, new string[] { "order", "to", "buy", "shares" });
            var exps = ToVariableExpressions("order", "to", "buy", "shares");
            var result = FluentCallHelper.CheckMultiWordFunctionName(ctx, exps);

            Assert.IsTrue(result.Exists);
            Assert.AreEqual("order to buy shares", result.Name);
            Assert.AreEqual(MemberMode.FunctionExternal, result.FunctionMode);
        }
    }
    */
}
