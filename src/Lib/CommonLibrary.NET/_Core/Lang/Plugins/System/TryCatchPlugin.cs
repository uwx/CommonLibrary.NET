using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang.Helpers;

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
    public class TryCatchPlugin : StmtBlockPlugin
    {
        private static string[] _tokens = new string[] { "try" };


        /// <summary>
        /// Intialize.
        /// </summary>
        public TryCatchPlugin()
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
            get { return "try <statementblock> catch '(' <id> ')' <statementblock>"; }
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
                    "try { add(1, 2); } catch( err ) { print( err.name ); }"
                };
            }
        }


        /// <summary>
        /// try/catch.
        /// </summary>
        /// <returns></returns>
        public override Stmt  Parse()
        {
            var stmt = new TryCatchStmt();
            var statements = new List<Stmt>();

            _tokenIt.Expect(Tokens.Try);
            ParseBlock(stmt);
            _tokenIt.ExpectMany(Tokens.Catch, Tokens.LeftParenthesis);
            stmt.ErrorName = _tokenIt.ExpectId();
            _tokenIt.Expect(Tokens.RightParenthesis);
            ParseBlock(stmt.Catch);
            stmt.Ctx = Ctx;
            stmt.Catch.Ctx = Ctx;
            return stmt;
        }
    }



    /// <summary>
    /// For loop Expression data
    /// </summary>
    public class TryCatchStmt : BlockStmt
    {
        /// <summary>
        /// Create new instance
        /// </summary>
        public TryCatchStmt()
        {
            InitBoundary(true, "}");
            Catch = new BlockStmt();
        }


        /// <summary>
        /// Name for the error in the catch clause.
        /// </summary>
        public string ErrorName;


        /// <summary>
        /// Else statement.
        /// </summary>
        public BlockStmt Catch;


        /// <summary>
        /// Disable management of memory scope by baseclass
        /// </summary>
        protected override void OnBlockEnter()
        {
            //base.OnBlockEnter();
        }


        /// <summary>
        /// Disable management of memory scope by baseclass
        /// </summary>
        protected override void OnBlockExit()
        {
            //base.OnBlockExit();
        }


        /// <summary>
        /// Execute
        /// </summary>
        public override void DoExecute()
        {
            bool tryScopePopped = false;
            bool catchScopePopped = false;
            try
            {
                Ctx.Memory.Push();
                LangHelper.Execute(_statements, this);
                Ctx.Memory.Pop();
                tryScopePopped = true;
            }
            // Force the langlimit excpetion to propegate 
            // do not allow to flow through to the catch all "Exception ex".
            catch (LangLimitException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Ctx.Limits.CheckExceptions(this);

                // Pop the try scope.
                if (!tryScopePopped) Ctx.Memory.Pop();

                // Push the scope in the catch block
                Ctx.Memory.Push();
                Ctx.Memory.SetValue(ErrorName, LError.FromException(ex));

                // Run statements in catch block.
                if (Catch != null && Catch.Statements.Count > 0)
                    LangHelper.Execute(Catch.Statements, Catch);

                // Pop the catch scope.
                Ctx.Memory.Pop();
                catchScopePopped = true;
            }
            finally
            {
                // Pop the catch scope in case there was an error.
                if (!catchScopePopped) Ctx.Memory.Remove(ErrorName);
            }
        }
    }    
}
