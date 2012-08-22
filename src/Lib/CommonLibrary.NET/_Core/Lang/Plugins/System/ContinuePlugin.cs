using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang;


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
    public class ContinuePlugin : StmtPlugin
    {
        private static string[] _tokens = new string[] { "continue" };


        /// <summary>
        /// Intialize.
        /// </summary>
        public ContinuePlugin()
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
            get { return "continue <statementterminator>"; }
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
                    "continue;",
                    "continue\r\n"
                };
            }
        }


        /// <summary>
        /// continue;
        /// </summary>
        /// <returns></returns>
        public override Stmt  Parse()
        {
            var stmt = new ContinueStmt();
            _tokenIt.Expect(Tokens.Continue);
            return stmt;
        }
    }



    /// <summary>
    /// For loop Expression data
    /// </summary>
    public class ContinueStmt : Stmt
    {
        /// <summary>
        /// Execute the statement.
        /// </summary>
        public override void DoExecute()
        {
            var loop = this.FindParent<ILoop>();
            if (loop == null) throw new LangException("syntax error", "unable to break, loop not found", string.Empty, 0);

            loop.Continue();
        }
    }
}
