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
    public class BreakPlugin : StmtPlugin
    {
        private static string[] _tokens = new string[] { "break" };


        /// <summary>
        /// Intialize.
        /// </summary>
        public BreakPlugin()
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
            get { return "break <statementterminator>"; }
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
                    "break;",
                    "break\r\n"
                };
            }
        }


        /// <summary>
        /// break;
        /// </summary>
        /// <returns></returns>
        public override Stmt  Parse()
        {
            var stmt = new BreakStmt();
            _tokenIt.Expect(Tokens.Break);
            return stmt;
        }
    }



    /// <summary>
    /// For loop Expression data
    /// </summary>
    public class BreakStmt : Stmt
    {
        /// <summary>
        /// Execute the statement.
        /// </summary>
        public override void DoExecute()
        {
            var loop = this.FindParent<ILoop>();
            if (loop == null) throw new LangException("syntax error", "unable to break, loop not found", string.Empty, 0);

            loop.Break();
        }
    }
}
