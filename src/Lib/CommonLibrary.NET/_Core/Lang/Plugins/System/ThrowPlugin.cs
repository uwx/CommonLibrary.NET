using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang;


namespace ComLib.Lang
{

    /* *************************************************************************
    <doc:example>	
    // Throw plugin provides throwing of errors from the script.
    
    throw 'user name is required';
    </doc:example>
    ***************************************************************************/

    /// <summary>
    /// Plugin for throwing errors from the script.
    /// </summary>
    public class ThrowPlugin : StmtPlugin
    {
        private static string[] _tokens = new string[] { "throw" };


        /// <summary>
        /// Intialize.
        /// </summary>
        public ThrowPlugin()
        {
            _startTokens = _tokens;
            _isSystemLevel = true;
            _supportsTerminator = true;
        }


        /// <summary>
        /// The grammer for the function declaration
        /// </summary>
        public override string Grammer
        {
            get { return "throw <expression> <statementterminator>"; }
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
                    "throw 'invalid amount';",
                    "throw 300\r\n"
                };
            }
        }


        /// <summary>
        /// throw error;
        /// </summary>
        /// <returns></returns>
        public override Stmt  Parse()
        {
             _tokenIt.Expect(Tokens.Throw);
            var exp = _parser.ParseExpression(Terminators.ExpStatementEnd, passNewLine: true);
            return new ThrowStmt() { Exp = exp };
        }
    }



    /// <summary>
    /// For loop Expression data
    /// </summary>
    public class ThrowStmt : Stmt
    {
        /// <summary>
        /// Create new instance
        /// </summary>
        public ThrowStmt()
        {
        }


        /// <summary>
        /// Name for the error in the catch clause.
        /// </summary>
        public Expr Exp;


        /// <summary>
        /// Execute
        /// </summary>
        public override void DoExecute()
        {
            object message = null;
            if (Exp != null)
                message = Exp.Evaluate();

            throw new LangException("TypeError", message.ToString(), this.Ref.ScriptName, this.Ref.Line);
        }
    }
}
