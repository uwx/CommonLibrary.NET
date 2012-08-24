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

namespace ComLib.Lang.Tests.Unit
{

    [TestFixture]
    public class Lexer_Tests
    {
        [Test]
        public void Can_Read_Word()
        {
            CheckRead("var age = 20;", 1, "var", 0, 1, 1, 3);
        }


        [Test]
        public void Can_Read_String()
        {
            CheckRead("var name = 'fluent';", 7, "fluent", 11, 1, 12, 19);
        }


        [Test]
        public void Can_Read_String_Without_Interpolation()
        {
            CheckRead("var name = 'fluent #{d}';", 7, "fluent #{d}", 11, 1, 12, 24);
        }


        [Test]
        public void Can_Read_Single_Line_Comment()
        {
            CheckRead("var id; // comment to new line\r\n", 6, " comment to new line", 8, 1, 9, 30);
        }


        [Test]
        public void Can_Read_Multi_Line_Comment()
        {
            CheckRead("var id; /* first\r\nsecond\r\nthird*/ var id;", 6, " first\r\nsecond\r\nthird", 8, 1, 9, 33);
        }        


        [Test]
        public void Can_Read_Operator()
        {
            CheckRead("if num <= 10 ", 5, "<=", 7, 1, 8, 9);
        }


        [Test]
        public void Can_Read_Interpolated_Tokens()
        {
            CheckRead("var a = true; var age = 32; var text = \"#{a} - #{age} + 1\";", 25, "#{a} - #{age} + 1", 39, 1, 40, 58);
        }


        [Test]
        public void Can_Read_Symbol()
        {
            CheckRead("var age = 20;", 8, ";", 12, 1, 13, 13);
        }


        [Test]
        public void Can_Read_Number()
        {
            CheckRead("var age = 20;", 7, "20", 10, 1, 11, 12);
        }

        
        [Test]
        public void Can_Tokenize_Var_On_Single_Line()
        {
            var lex = new Lexer("var age = 20;  name =  'fls';");
            Console.WriteLine(  "var age = 20;  name =  'fls';");
            var tokens = lex.Tokenize();

            CheckToken(tokens, 0, 0,  Tokens.Var,           TokenKind.Keyword,          "var",      1, 1, 0);
            CheckToken(tokens, 1, 1,  null,                 TokenKind.Ident,            "age",      1, 5, 4);
            CheckToken(tokens, 2, 2,  Tokens.Assignment,    TokenKind.Symbol,           "=",        1, 9, 8);
            CheckToken(tokens, 3, 3,  null,                 TokenKind.LiteralNumber,    "20",       1, 11, 10);
            CheckToken(tokens, 4, 4,  Tokens.Semicolon,     TokenKind.Symbol,           ";",        1, 13, 12); 
        }


        [Test]
        public void Can_Tokenize_Var_With_Single_Line_Comment()
        {            
            var lex = new Lexer("var age = 20; // this is a comment\r\n name =  'fls';");
            Console.WriteLine(  "var age = 20; // this is a comment\r\n name =  'fls';");
            var tokens = lex.Tokenize();

            CheckToken(tokens, 0, 0, Tokens.Var,        TokenKind.Keyword,          "var", 1, 1, 0);
            CheckToken(tokens, 1, 1, null,              TokenKind.Ident,            "age", 1, 5, 4);
            CheckToken(tokens, 2, 2, Tokens.Assignment, TokenKind.Symbol,           "=", 1, 9, 8);
            CheckToken(tokens, 3, 3, null,              TokenKind.LiteralNumber,    "20", 1, 11, 10);
            CheckToken(tokens, 4, 4, Tokens.Semicolon,  TokenKind.Symbol,           ";", 1, 13, 12);
            CheckToken(tokens, 5, 5, null,              TokenKind.Comment,          " this is a comment", 1, 15, 14);
            CheckToken(tokens, 6, 6, Tokens.NewLine,    TokenKind.LiteralOther,     "newline", 1, 35, 34);

            CheckToken(tokens, 7, 7, null,              TokenKind.Ident,            "name", 2, 2, 37);
            CheckToken(tokens, 8, 8, Tokens.Assignment, TokenKind.Symbol,           "=", 2, 7, 42);
            CheckToken(tokens, 9, 9, null,              TokenKind.LiteralString,    "fls", 2, 10, 45);
            CheckToken(tokens, 10, 10, Tokens.Semicolon,TokenKind.Symbol,           ";", 2, 15, 50);
        }


        [Test]
        public void Can_Tokenize_Var_With_Multi_Line_Comment()
        {
            
            var lex = new Lexer("var age = 20; \r\n/* line 1\r\n line 2 */\r\n name =  'fls';");
            Console.WriteLine(  "var age = 20; \r\n/* line 1\r\n line 2 */\r\n name =  'fls';");
            var tokens = lex.Tokenize();

            CheckToken(tokens, 0, 0, Tokens.Var,          TokenKind.Keyword,        "var", 1, 1, 0);
            CheckToken(tokens, 1, 1, null,                TokenKind.Ident,          "age", 1, 5, 4);
            CheckToken(tokens, 2, 2, Tokens.Assignment,   TokenKind.Symbol,         "=", 1, 9, 8);
            CheckToken(tokens, 3, 3, null,                TokenKind.LiteralNumber,         "20", 1, 11, 10);
            CheckToken(tokens, 4, 4, Tokens.Semicolon,    TokenKind.Symbol,         ";", 1, 13, 12);

            CheckToken(tokens, 5,  5,  Tokens.NewLine,    TokenKind.LiteralOther,   "newline", 1, 15, 14);
            CheckToken(tokens, 6,  6,  null,              TokenKind.Comment,        " line 1\r\n line 2 ", 2, 1, 16);
            CheckToken(tokens, 7,  7,  Tokens.NewLine,    TokenKind.LiteralOther,   "newline", 3, 11, 37);

            CheckToken(tokens, 8,  8,  null,              TokenKind.Ident,          "name", 4, 2, 40);
            CheckToken(tokens, 9,  9,  Tokens.Assignment, TokenKind.Symbol,         "=", 4, 7, 45);
            CheckToken(tokens, 10, 10, null,              TokenKind.LiteralString,  "fls", 4, 10, 48);
            CheckToken(tokens, 11, 11, Tokens.Semicolon,  TokenKind.Symbol,         ";", 4, 15, 53);
        }


        [Test]
        public void Can_Tokenize_Var_On_Multi_Line()
        {
            var lex = new Lexer("var age = 20;  name =  'fls';\r\n  isActive   = true;");
            Console.WriteLine("var age = 20;  name =  'fls';\r\n  isActive   = true;");
            var tokens = lex.Tokenize();

            CheckToken(tokens, 0, 0, Tokens.Var, TokenKind.Keyword, "var", 1, 1, 0);
            CheckToken(tokens, 1, 1, null, TokenKind.Ident, "age", 1, 5, 4);
            CheckToken(tokens, 2, 2, Tokens.Assignment, TokenKind.Symbol, "=", 1, 9, 8);
            CheckToken(tokens, 3, 3, null, TokenKind.LiteralNumber, "20", 1, 11, 10);
            CheckToken(tokens, 4, 4, Tokens.Semicolon, TokenKind.Symbol, ";", 1, 13, 12);

            CheckToken(tokens, 5, 5, null, TokenKind.Ident, "name", 1, 16, 15);
            CheckToken(tokens, 6, 6, Tokens.Assignment, TokenKind.Symbol, "=", 1, 21, 20);
            CheckToken(tokens, 7, 7, null, TokenKind.LiteralString, "fls", 1, 24, 23);
            CheckToken(tokens, 8, 8, Tokens.Semicolon, TokenKind.Symbol, ";", 1, 29, 28);
            CheckToken(tokens, 9, 9, Tokens.NewLine, TokenKind.LiteralOther, "newline", 1, 30, 29);

            CheckToken(tokens, 10, 10, null, TokenKind.Ident, "isActive", 2, 3, 33);
            CheckToken(tokens, 11, 11, Tokens.Assignment, TokenKind.Symbol, "=", 2, 14, 44);
            CheckToken(tokens, 12, 12, null, TokenKind.LiteralBool, "true", 2, 16, 46);
            CheckToken(tokens, 13, 13, Tokens.Semicolon, TokenKind.Symbol, ";", 2, 20, 50);
        }


        [Test]
        public void Can_Tokenize_Var_On_Multi_Line_With_Interpolated_Tokens()
        {
            var lex = new Lexer("var age = 20;  name =  \"before #{age} after\";\r\n  isActive   = true;");
            Console.WriteLine(  "var age = 20;  name =  \"before #{age} after\";\r\n  isActive   = true;");
            var tokens = lex.Tokenize();

            CheckToken(tokens, 0, 0, Tokens.Var,            TokenKind.Keyword,   "var", 1, 1, 0);
            CheckToken(tokens, 1, 1, null,                  TokenKind.Ident,        "age", 1, 5, 4);
            CheckToken(tokens, 2, 2, Tokens.Assignment,     TokenKind.Symbol,    "=", 1, 9, 8);
            CheckToken(tokens, 3, 3, null,                  TokenKind.LiteralNumber,    "20", 1, 11, 10);
            CheckToken(tokens, 4, 4, Tokens.Semicolon,      TokenKind.Symbol,    ";", 1, 13, 12);

            CheckToken(tokens, 5, 5, null,                  TokenKind.Ident,        "name", 1, 16, 15);
            CheckToken(tokens, 6, 6, Tokens.Assignment,     TokenKind.Symbol,    "=", 1, 21, 20);
            CheckToken(tokens, 7, 7, null,                  TokenKind.Multi,  "before #{age} after", 1, 24, 23);
            CheckToken(tokens, 8, 8, Tokens.Semicolon,      TokenKind.Symbol,    ";", 1, 45, 44);
            CheckToken(tokens, 9, 9, Tokens.NewLine,        TokenKind.LiteralOther,   "newline", 1, 46, 45);

            CheckToken(tokens, 10, 10, null,                TokenKind.Ident,        "isActive", 2, 3, 49);
            CheckToken(tokens, 11, 11, Tokens.Assignment,   TokenKind.Symbol,    "=", 2, 14, 60);
            CheckToken(tokens, 12, 12, null,                TokenKind.LiteralBool,   "true", 2, 16, 62);
            CheckToken(tokens, 13, 13, Tokens.Semicolon,    TokenKind.Symbol,    ";", 2, 20, 66);
        }



        private void CheckToken(List<TokenData> tokens, int index, int tokenIndex, Token token, int tokenKind, string text, int lineNum, int lineCharPos, int charPos)
        {
            if (token != null)
                Assert.AreEqual(tokens[index].Token, token);

            if (tokenKind != 0)
                Assert.AreEqual(tokens[index].Token.Kind, tokenKind);

            if (!string.IsNullOrEmpty(text))
                Assert.AreEqual(tokens[index].Token.Text, text);

            Assert.AreEqual(tokens[index].Index, tokenIndex);
            Assert.AreEqual(tokens[index].Line, lineNum);
            Assert.AreEqual(tokens[index].LineCharPos, lineCharPos);
            Assert.AreEqual(tokens[index].Pos,  charPos);
        }


        private void CheckRead(string text, int numTokensToRead, string expectedText, int tokenPos, int tokenLine, int tokenCharPos, int lexerPos)
        {
            var lex = new Lexer(text);
            TokenData token = null;
            for (var count = 1; count <= numTokensToRead; count++)
            {
                token = lex.NextToken();
            }

            Assert.AreEqual(token.Token.Text, expectedText);
            Assert.AreEqual(token.Pos, tokenPos);
            Assert.AreEqual(token.LineCharPos, tokenCharPos);
            Assert.AreEqual(lex.State.Pos, lexerPos);
        }
    }
}
