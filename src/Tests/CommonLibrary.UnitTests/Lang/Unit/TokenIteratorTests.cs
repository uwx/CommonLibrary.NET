using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;


using ComLib;
using ComLib.Lang;
using ComLib.Tests;
using ComLib.Lang.Tests.Common;

namespace ComLib.Lang.Tests.Unit
{
    [TestFixture]
    public class TokenIteratorTests
    {
        private string GetCode()
        {
            return "var data = [ 'john', 100, true ]; \r\n function inc( num ) { return num + 1; } var result = inc(userinfo[1]);";
        }


        private TokenIterator GetTokenIterator(int llK, string code)
        {
            var lexer = new Lexer(code);
            var tk = new TokenIterator();
            tk.Init((llk) => lexer.GetTokenBatch(llk), llK, (pos) => lexer.ResetPos(pos));
            return tk;
        }


        [Test]
        public void Can_Without_LLK_Init()
        {
            var code = GetCode();
            var lexer = new Lexer(code);
            var tokens = lexer.Tokenize();
            var tk = new TokenIterator();
            tk.Init(tokens, 1, 0);

            Assert.AreEqual(tk.LLK, -1);
            Assert.AreEqual(tk.TokenList.Count, 36);
            Assert.AreEqual(tk.CurrentIndex, -1);
            Assert.IsNull(tk.NextToken);
            Assert.AreEqual(tk.CurrentBatchIndex, -1);

            tk.Advance();
            Assert.AreEqual(tk.CurrentIndex, 0);
            Assert.AreEqual(tk.CurrentBatchIndex, 0);
            Assert.AreEqual(tk.NextToken.Token, Tokens.Var);
        }


        [Test]
        public void Can_With_LLK_Init()
        {
            var code = GetCode();
            var lexer = new Lexer(code);
            var tk = GetTokenIterator(4, code);

            Assert.AreEqual(tk.LLK, 4);
            Assert.AreEqual(tk.TokenList.Count, 8);
            Assert.AreEqual(tk.CurrentIndex, -1);
            Assert.IsNull(tk.NextToken);
            Assert.AreEqual(tk.CurrentBatchIndex, -1);
        }


        [Test]
        public void Can_With_LLK_Advance()
        {
            var code = GetCode();
            var tk = GetTokenIterator(4, code);

            tk.Advance();
            Assert.AreEqual(tk.CurrentIndex, 0);
            Assert.AreEqual(tk.CurrentBatchIndex, 0);
            Assert.AreEqual(tk.NextToken.Token, Tokens.Var);
        }


        [Test]
        public void Can_With_LLK_Advance_To_MidPoint()
        {
            var code = GetCode();
            var tk = GetTokenIterator(4, code);

            tk.Advance();
            tk.Advance();
            tk.Advance();
            tk.Advance();
            Assert.AreEqual(tk.NextToken.Token, Tokens.LeftBracket);
        }


        [Test]
        public void Can_With_LLK_Advance_To_NextBatch()
        {
            var code = GetCode();
            var tk = GetTokenIterator(4, code);


            tk.Advance();
            tk.Advance();
            tk.Advance();
            tk.Advance();

            // Note(s): 
            // 1. At this point midpoint is at token 4, index 3 = '['
            // 2. The literal 'john' token is right now at token 5, index 4
            // 3. Advanceing one more time will fetch the next batch
            // 4. When the next batch is fetched, the tokens will be shifted left in the list
            // 5. When the tokens are shifted left, the literal 'john' is now token 1, index 0
            Assert.AreEqual(tk.TokenList[4].Token.Text, "john");
            tk.Advance();
            Assert.AreEqual(tk.TokenList[0].Token.Text, "john");
            Assert.AreEqual(tk.NextToken.Token.Text, "john");
        }


        [Test]
        public void Can_With_LLK_Advance_N_Count()
        {
            var code = GetCode();
            var tk = GetTokenIterator(4, code);


            tk.Advance();
            Assert.AreEqual(tk.NextToken.Token, Tokens.Var);
            tk.Advance(3);
            Assert.AreEqual(tk.NextToken.Token, Tokens.LeftBracket);
            Assert.AreEqual(tk.TokenList[3].Token, Tokens.LeftBracket);
        }


        [Test]
        public void Can_With_LLK_Advance_Once_Past_Many_Batches()
        {
            var code = GetCode();
            var tk = GetTokenIterator(4, code);

            tk.Advance(33);
            Assert.AreEqual(tk.NextToken.Token, Tokens.RightBracket);
        }


        [Test]
        public void Can_With_LLK_Advance_To_End_Token()
        {
            var code = GetCode();
            var tk = GetTokenIterator(4, code);

            tk.Advance(34);
            tk.Advance();
            tk.Advance();
            Assert.AreEqual(tk.NextToken.Token, Tokens.EndToken);
            Assert.AreEqual(tk.CurrentIndex, 3);
            tk.Advance();
            Assert.AreEqual(tk.CurrentIndex, 3);
            Assert.AreEqual(tk.NextToken.Token, Tokens.EndToken);
        }


        [Test]
        public void Can_With_LLK_Peek()
        {
            var code = GetCode();
            var tk = GetTokenIterator(4, code);

            tk.Advance(3);
            var p = tk.Peek();
            Assert.AreEqual(tk.NextToken.Token, Tokens.Assignment);
            Assert.AreEqual(p.Token, Tokens.LeftBracket);
        }
    }
}
