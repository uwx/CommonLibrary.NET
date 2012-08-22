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
    public class WhilePlugin : StmtBlockPlugin
    {
        private static string[] _tokens = new string[] { "while" };


        /// <summary>
        /// Intialize.
        /// </summary>
        public WhilePlugin()
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
            get { return "while ( ( <expression> then <statementblock> ) | ( '(' <expression> ')' <statementblock> ) )"; }
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
                    "while count < 20 then print( 'hi' );",
                    "while count < 20 then { print( 'hi' ); }",
                    "while ( count < 20 )   print( 'hi' );",
                    "while ( count < 20 ) { print( 'hi' ); }",
                };
            }
        }


        /// <summary>
        /// Parses either the for or for x in statements.
        /// </summary>
        /// <returns></returns>
        public override Stmt Parse()
        {
            WhileStmt stmt = new WhileStmt();

            // While ( condition expression )
            _tokenIt.Expect(Tokens.While);
            ParseConditionalBlock(stmt);
            return stmt;
        }
    }



    /// <summary>
    /// For loop Expression data
    /// </summary>
    public class WhileStmt : ConditionalBlockStmt, ILoop
    {
        /// <summary>
        /// Whether or not the break the loop
        /// </summary>
        protected bool _breakLoop;


        /// <summary>
        /// Whether or not to continue the loop
        /// </summary>
        protected bool _continueLoop;


        /// <summary>
        /// Whether or not to continue running the loop
        /// </summary>
        protected bool _continueRunning;


        /// <summary>
        /// Create new instance/
        /// </summary>
        public WhileStmt() : base(null, null) { }


        /// <summary>
        /// Create new instance with condition
        /// </summary>
        /// <param name="condition"></param>
        public WhileStmt(Expr condition)
            : base(condition, null)
        {
            InitBoundary(true, "}");
        }


        /// <summary>
        /// Execute
        /// </summary>
        public override void DoExecute()
        {
            _continueRunning = true;
            _breakLoop = false;
            _continueLoop = false;
            _continueRunning = Condition.EvaluateAs<bool>();

            while (_continueRunning)
            {
                if (_statements != null && _statements.Count > 0)
                {
                    foreach (var stmt in _statements)
                    {
                        stmt.Execute();

                        Ctx.Limits.CheckLoop(this);

                        // If Break statment executed.
                        if (_breakLoop)
                        {
                            _continueRunning = false;
                            break;
                        }
                        // Continue statement.
                        else if (_continueLoop)
                            break;
                    }
                }
                else break;

                // Break loop here.
                if (_continueRunning == false)
                    break;

                _continueRunning = Condition.EvaluateAs<bool>();
            }
        }


        /// <summary>
        /// Break loop
        /// </summary>
        public void Break()
        {
            _breakLoop = true;
        }


        /// <summary>
        /// Continue loop
        /// </summary>
        public void Continue()
        {
            _continueLoop = true;
        }
    }    
}
