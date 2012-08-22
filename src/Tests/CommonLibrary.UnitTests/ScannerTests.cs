using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;


using ComLib;

namespace CommonLibrary.Tests
{

    [TestFixture]
    public class ScannerTests
    {
        public Scanner Create()
        {
            LexSettings settings = new LexSettings();
            Scanner reader = new Scanner("abcd1234'	a \\b   k", '\\', settings.QuotesChars, settings.WhiteSpaceChars);
            return reader;
        }


        [Test]
        public void CanParse()
        {
            Scanner reader = Create();
            char current = reader.ReadChar();
            Assert.AreEqual('a', current);
            Assert.AreEqual('b', reader.PeekChar());
            Assert.AreEqual("bc", reader.PeekChars(2));
            Assert.AreEqual("bcd", reader.ReadChars(3));
            Assert.AreEqual("1234", reader.ReadChars(4));
            reader.ReadChar();
            Assert.IsTrue(reader.IsToken());
            Assert.AreEqual('4', reader.PreviousChar);
            reader.ReadBackChar();
            Assert.AreEqual('4', reader.CurrentChar);
            reader.ReadChar();
            reader.ReadChar();
            Assert.IsTrue(reader.IsWhiteSpace());
            reader.ReadChars(2);
            Assert.IsTrue(reader.IsWhiteSpace());
            reader.ReadChar();
            Assert.IsTrue(reader.IsEscape());
            reader.ConsumeWhiteSpace();
            Assert.AreEqual(reader.CurrentChar, '\\');
            reader.ReadChar();
            Assert.AreEqual(reader.CurrentChar, 'b');
            reader.ConsumeWhiteSpace();
            Assert.AreEqual(reader.CurrentChar, 'b');
            reader.ConsumeWhiteSpace(true);
            Assert.AreEqual(reader.CurrentChar, 'k');
        }


        [Test]
        public void CanParseNumber()
        {
            var tokens = new List<Tuple<int, string, string, string>>()
            {
                new Tuple<int, string, string, string>(3, " ",   "123",    "123"      ),  
                new Tuple<int, string, string, string>(3, " ",   "123",    "123 "     ),
                new Tuple<int, string, string, string>(4, " ",  "+123",    "+123"     ),  
                new Tuple<int, string, string, string>(4, " ",  "+123",    "+123 "    ),
                new Tuple<int, string, string, string>(4, " ",  "-123",    "-123"     ),  
                new Tuple<int, string, string, string>(4, " ",  "-123",    "-123 "    ),
                new Tuple<int, string, string, string>(3, "<",   "123",    "123<"     ),
                new Tuple<int, string, string, string>(3, "<",   "123",    "123< "    ),
                new Tuple<int, string, string, string>(4, "<",  "+123",    "+123<"    ),
                new Tuple<int, string, string, string>(4, "<",  "+123",    "+123< "   ),
                new Tuple<int, string, string, string>(4, "<",  "-123",    "-123<"    ),
                new Tuple<int, string, string, string>(4, "<",  "-123",    "-123< "   ),
                new Tuple<int, string, string, string>(6, " ",   "123.45", "123.45"   ),  
                new Tuple<int, string, string, string>(6, " ",   "123.45", "123.45 "  ),
                new Tuple<int, string, string, string>(7, " ",  "+123.45", "+123.45"  ),  
                new Tuple<int, string, string, string>(7, " ",  "+123.45", "+123.45 " ),
                new Tuple<int, string, string, string>(7, " ",  "-123.45", "-123.45"  ),  
                new Tuple<int, string, string, string>(7, " ",  "-123.45", "-123.45 " ),
                new Tuple<int, string, string, string>(6, "<",   "123.45", "123.45<"  ),  
                new Tuple<int, string, string, string>(6, "<",   "123.45", "123.45< " ),
                new Tuple<int, string, string, string>(7, "<",  "+123.45", "+123.45<" ),  
                new Tuple<int, string, string, string>(7, "<",  "+123.45", "+123.45< "),
                new Tuple<int, string, string, string>(7, "<",  "-123.45", "-123.45<" ), 
                new Tuple<int, string, string, string>(7, "<",  "-123.45", "-123.45< ")
            };
            CheckTokens(tokens, (scanner, text) => scanner.ReadNumber(false));
            CheckTokens(tokens, (scanner, text) => scanner.ReadNumber(false, false), true);
        }


        [Test]
        public void CanParseId()
        {
            var tokens = new List<Tuple<int, string, string, string>>()
            {
                new Tuple<int, string, string, string>(6 , " ", "common",     "common"),     
                new Tuple<int, string, string, string>(6 , " ", "common",     "common "),
                new Tuple<int, string, string, string>(6 , ">", "common",     "common>"),
                new Tuple<int, string, string, string>(9 , " ", "common123",  "common123"),
                new Tuple<int, string, string, string>(9,  " ", "common123",  "common123 "),
                new Tuple<int, string, string, string>(9,  ">", "common123",  "common123>"),
                new Tuple<int, string, string, string>(10, " ", "_common123", "_common123"), 
                new Tuple<int, string, string, string>(10, " ", "_common123", "_common123 "), 
                new Tuple<int, string, string, string>(10, ">", "_common123", "_common123>"),
                new Tuple<int, string, string, string>(10, " ", "$common123", "$common123"), 
                new Tuple<int, string, string, string>(10, " ", "$common123", "$common123 "), 
                new Tuple<int, string, string, string>(10, ">", "$common123", "$common123>"),
                new Tuple<int, string, string, string>(8 , " ", "_$123com",   "_$123com"), 
                new Tuple<int, string, string, string>(8 , " ", "_$123com",   "_$123com "), 
                new Tuple<int, string, string, string>(8 , ">", "_$123com",   "_$123com>"),
                new Tuple<int, string, string, string>(3 , ".", "com",        "com.mon"),  
                new Tuple<int, string, string, string>(3 , ".", "com",        "com.mon "),  
                new Tuple<int, string, string, string>(3 , ".", "com",        "com.mon>"),
                new Tuple<int, string, string, string>(4 , ".", "_com",       "_com.mon"), 
                new Tuple<int, string, string, string>(4 , ".", "_com",       "_com.mon "), 
                new Tuple<int, string, string, string>(4 , ".", "_com",       "_com.mon>" ),
                new Tuple<int, string, string, string>(4 , ".", "$com",       "$com.mon"), 
                new Tuple<int, string, string, string>(4 , ".", "$com",       "$com.mon "), 
                new Tuple<int, string, string, string>(4 , ".", "$com",       "$com.mon>" ),
                new Tuple<int, string, string, string>(4 , ".", "$com",       "$com.mon(>" )
            };
            CheckTokens(tokens, (scanner, text) => scanner.ReadId(false));
            CheckTokens(tokens, (scanner, text) => scanner.ReadId(false, false), true);
        }


        [Test]
        public void CanParseString()
        {
            var tokens = new List<Tuple<int, string, string, string>>()
            {
                new Tuple<int, string, string, string>(6 , " ", "test",     "'test'"),
                new Tuple<int, string, string, string>(6 , " ", "test",     "'test' "),
                new Tuple<int, string, string, string>(6 , ";", "test",     "'test';"),
                new Tuple<int, string, string, string>(8 , ";", "te'st",     "'te\\'st';"),
                new Tuple<int, string, string, string>(6 , " ", "test",     "\"test\""),
                new Tuple<int, string, string, string>(6 , " ", "test",     "\"test\" "),
                new Tuple<int, string, string, string>(6 , ";", "test",     "\"test\";"),
                new Tuple<int, string, string, string>(8 , ";", "te\"st",     "\"te\\\"st\";")
            };
            CheckTokens(tokens, (scanner, text) => scanner.ReadString(text[0]));
            CheckTokens(tokens, (scanner, text) => scanner.ReadString(text[0], setPosAfterToken: false), true);
        }


        [Test]
        public void CanParseUntilChars()
        {
            var tokens = new List<Tuple<int, string, string, string>>()
            {
                new Tuple<int, string, string, string>(72, " ", "abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?", "/*abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?*/"),
                new Tuple<int, string, string, string>(72, " ", "abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?", "/*abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?*/  "),
                new Tuple<int, string, string, string>(72, "v", "abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?", "/*abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?*/var"),
                new Tuple<int, string, string, string>(72, "n", "abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?", "/*abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?*/name")

            };
            CheckTokens(tokens, (scanner, text) =>
            {
                scanner.ReadChar();
                scanner.ReadChar();
                return scanner.ReadUntilChars(false, '*', '/');
            });
            CheckTokens(tokens, (scanner, text) =>
            {
                scanner.ReadChar();
                scanner.ReadChar();
                return scanner.ReadUntilChars(false, '*', '/', false);
            }, true, 3);  
        }


        [Test]
        public void CanParseLines()
        {
            var tokens = new List<Tuple<int, string, string, string>>()
            {
                new Tuple<int, string, string, string>(72, " ", "abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?", "//abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?\r\n"),
                new Tuple<int, string, string, string>(72, " ", "abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?", "//abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?\r\n  "),
                new Tuple<int, string, string, string>(72, "v", "abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?", "//abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?\r\nvar"),
                new Tuple<int, string, string, string>(72, "n", "abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?", "//abcdefghijklmnopqrstuvwxyz1234567890`~!@#$%^&*()_+-=[]\\{}|;':\",./<>?\r\nname")

            };
            CheckTokens(tokens, (scanner, text) =>
            {
                scanner.ReadChar();
                scanner.ReadChar();
                return scanner.ReadLine(false);
            });
            CheckTokens(tokens, (scanner, text) =>
            {
                scanner.ReadChar();
                scanner.ReadChar();
                return scanner.ReadLine(false, false);
            }, true);
        }


        [Test]
        public void CanConsumeSpace()
        {
            var tokens = new List<Tuple<int, string, string, string>>()
            {
                new Tuple<int, string, string, string>(1 , "v",  string.Empty,    " var"),
                new Tuple<int, string, string, string>(2 , "v",  string.Empty,    "  var"),
                new Tuple<int, string, string, string>(3 , "v",  string.Empty,    "   var"),
                new Tuple<int, string, string, string>(1 , "$",  string.Empty,    " $"),
                new Tuple<int, string, string, string>(2 , "$",  string.Empty,    "  $"),
                new Tuple<int, string, string, string>(3 , "$",  string.Empty,    "   $")
            };
            CheckTokens(tokens, (scanner, text) => 
            { 
                scanner.ConsumeWhiteSpace(false);
                return new ScanTokenResult(true, string.Empty);
            });
            CheckTokens(tokens, (scanner, text) =>
            {
                scanner.ConsumeWhiteSpace(false, false);
                return new ScanTokenResult(true, string.Empty);
            }, true);
        }


        private void CheckTokens(List<Tuple<int, string, string, string>> tokens, Func<Scanner, string, ScanTokenResult> scanCall, bool subtractFromExpectedPosition = false, int subtractCount = 1)
        {
            foreach (var pair in tokens)
            {
                var expectedPos = pair.Item1;
                var expectedCurrChar = pair.Item2;
                var expectedText = pair.Item3;
                var text = pair.Item4;
                var scanner = new Scanner(text);
                scanner.ReadChar();
                var result = scanCall(scanner, text);

                Assert.IsTrue(result.Success);
                Assert.AreEqual(expectedText, result.Text);
                if (subtractFromExpectedPosition)
                {
                    Assert.AreEqual(expectedPos - subtractCount, scanner.Position);
                    Assert.AreEqual(text[scanner.Position], scanner.CurrentChar);
                }
                else
                {
                    Assert.AreEqual(expectedPos, scanner.Position);
                    Assert.AreEqual(expectedCurrChar, scanner.CurrentChar.ToString());
                }
            }
        }
    }
}
