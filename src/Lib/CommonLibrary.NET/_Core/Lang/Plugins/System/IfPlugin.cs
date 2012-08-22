using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using ComLib.Lang.Helpers;

namespace ComLib.Lang
{
    /// <summary>
    /// Plugin for throwing errors from the script.
    /// </summary>
    public class IfPlugin : StmtBlockPlugin
    {
        private static string[] _tokens = new string[] { "if" };


        /// <summary>
        /// Intialize.
        /// </summary>
        public IfPlugin()
        {
            _startTokens = _tokens;
            _isSystemLevel = true;
            _supportsBlock = true;
        }


        /// <summary>
        /// The grammer for the function declaration
        /// </summary>
        public override string Grammer
        {
            get { return "if ( ( <expression> then <statementblock> ) | ( '(' <expression> ')' <statementblock> ) )"; }
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
                    "if true then print hi",
                    "if ( total > 10 ) print hi",
                    "if ( isActive && age >= 21  ) { print('fluent'); print('script'); }"
                };
            }
        }


        /// <summary>
        /// return value;
        /// </summary>
        /// <returns></returns>
        public override Stmt Parse()
        {            
            IfStmt stmt = new IfStmt();
            var statements = new List<Stmt>();

            // While ( condition expression )
            _tokenIt.Expect(Tokens.If);

            // Parse the if
            ParseConditionalBlock(stmt);

            // Handle "else if" and/or else
            if (_tokenIt.NextToken.Token == Tokens.Else)
            {
                // _tokenIt.NextToken = "else"
                _tokenIt.Advance();

                // What's after else? 
                // 1. "if"      = else if statement
                // 2. "{"       = multi  line else
                // 3. "nothing" = single line else
                // Peek 2nd token for else if.
                var token = _tokenIt.NextToken;
                if (_tokenIt.NextToken.Token == Tokens.If)
                {
                    stmt.Else = Parse() as BlockStmt;
                }
                else // Multi-line or single line else
                {
                    var elseStmt = new BlockStmt();
                    ParseBlock(elseStmt);
                    elseStmt.Ctx = Ctx;
                    stmt.Else = elseStmt;
                    _parser.SetScriptPosition(stmt.Else, token);
                }
            }
            return stmt;
        }
    }



    /// <summary>
    /// For loop Expression data
    /// </summary>
    public class IfStmt : ConditionalBlockStmt
    {
        /// <summary>
        /// Create new instance
        /// </summary>
        public IfStmt() : base(null, null) { }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="condition"></param>
        public IfStmt(Expr condition)
            : base(condition, null)
        {
            InitBoundary(true, "}");            
        }


        /// <summary>
        /// Else statement.
        /// </summary>
        public BlockStmt Else;



        /// <summary>
        /// Execute
        /// </summary>
        public override void DoExecute()
        {
            // Case 1: If is true
            if (Condition.EvaluateAs<bool>())
            {
                LangHelper.Execute(_statements, this);
            }
            // Case 2: Else available to execute
            else if (Else != null)
            {
                Else.Execute();
            }
        }
    }    
}
