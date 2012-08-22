using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace ComLib.Lang
{
    /* *************************************************************************
    <doc:example>	
    // Return plugin provides return values
    
    return false;
    </doc:example>
    ***************************************************************************/

    /// <summary>
    /// Plugin for throwing errors from the script.
    /// </summary>
    public class IdPlugin : StmtPlugin
    {
        private static string[] _tokens = new string[] { "$IdToken" };


        /// <summary>
        /// Intialize.
        /// </summary>
        public IdPlugin()
        {
            _startTokens = _tokens;
            _isSystemLevel = true;
            _supportsBlock = true;
            _precedence = 0;
        }


        /// <summary>
        /// The grammer for the function declaration
        /// </summary>
        public override string Grammer
        {
            get { return "<idtoken> ( ( '.' <idtoken> )* | ( '[' <expression> ']' )* | ( '(' <paramlist> ')' )*"; }
        }


        /// <summary>
        /// Examples
        /// </summary>
        public override string[] Examples
        {
            get
            {
                return new string[]
                {
                    "add( 1, 4 )",
                    "user.name = 'john'",
                    "users[0] = 'john'"
                };
            }
        }


        /// <summary>
        /// Whether or not this plugin can handle the current token.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public override bool CanHandle(Token current)
        {
            var ahead = _tokenIt.Peek();
            if (ahead.Token == Tokens.Dot || ahead.Token == Tokens.LeftParenthesis || ahead.Token == Tokens.LeftBracket)
                return true;
            if (ahead.Token == Tokens.Assignment)
                return true;
            return false;
        }


        /// <summary>
        /// Parses either the for or for x in statements.
        /// </summary>
        /// <returns></returns>
        public override Stmt Parse()
        {
            var tokenData = _tokenIt.NextToken;
            string name = _tokenIt.ExpectId(false);
            Expr exp = null;

            // Forward check for assignments on
            var aheadToken = _tokenIt.Peek();
            if (aheadToken.Token == Tokens.Dot || aheadToken.Token == Tokens.LeftBracket || aheadToken.Token == Tokens.LeftParenthesis)
            {
                exp = _parser.ParseIdExpression(name);
                if (exp is FunctionCallExpr)
                {
                    _tokenIt.Advance(1, false);
                    _tokenIt.ExpectEndOfStmt();
                    return new ExpressionStmt(exp);
                }
            }
            _tokenIt.Advance();
            var token = _tokenIt.NextToken.Token;
            if (token == Tokens.Assignment)
            {
                exp = exp ?? new VariableExpr(name);
                var varPlugin = new VarPlugin();
                varPlugin.Init(this._parser, _tokenIt);
                return varPlugin.Parse(exp);
            }
            else if (token == Tokens.Increment || token == Tokens.Decrement || token == Tokens.IncrementAdd ||
                     token == Tokens.IncrementDivide || token == Tokens.IncrementMultiply || token == Tokens.IncrementSubtract)  // ++ -- += *= /= -=
            {
                return _parser.ParseUnary(name);
            }
            throw _tokenIt.BuildSyntaxUnexpectedTokenException(tokenData);
        }
    }
}
