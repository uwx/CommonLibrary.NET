using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang
{
    /// <summary>
    /// A combinator to extend the parser
    /// </summary>
    public class StmtPlugin2 : ExprPluginBase, IStmtPlugin2
    {
        /// <summary>
        /// Whether or not this statement supports a terminator like semicolon ;.
        /// </summary>
        protected bool _supportsTerminator;


        /// <summary>
        /// Whether or not this statement supports a block
        /// </summary>
        protected bool _supportsBlock;


        /// <summary>
        /// Whether or not this is a system level plugin.
        /// </summary>
        protected bool _isSystemLevel;
        

        /// <summary>
        /// Initialize
        /// </summary>
        public StmtPlugin2()
        {
            _hasStatementSupport = true;
        }


        /// <summary>
        /// Whether or not the statement plugin support a terminator such as ; or new line.
        /// </summary>
        public bool SupportsTerminator { get { return _supportsTerminator; } }


        /// <summary>
        /// Whether or not the statement plugin supports a block such as { ... } which can hold multipl statements.
        /// </summary>
        public bool SupportsBlock { get { return _supportsBlock; } }


        /// <summary>
        /// Whether or not this a system level plugin.
        /// </summary>
        public bool IsSystemLevel { get { return _isSystemLevel; } }


        /// <summary>
        /// The context of the script.
        /// </summary>
        public Context Ctx { get; set; }


        /// <summary>
        /// Parses a statement using only the token information available.
        /// </summary>
        /// <returns></returns>
        public virtual Stmt Parse()
        {
            return null;
        }


        /// <summary>
        /// Parses a statement using contextual information supplied.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual Stmt Parse(object context)
        {
            return null;
        }


        /// <summary>
        /// Whether or not this parser can handle the supplied token.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public override bool CanHandle(Token current)
        {
            return true;
        }
    }
}
