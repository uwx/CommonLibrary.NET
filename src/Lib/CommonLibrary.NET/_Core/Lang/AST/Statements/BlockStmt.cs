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
    /// For loop Expression data
    /// </summary>
    public class BlockStmt : Stmt
    {
        /// <summary>
        /// List of statements
        /// </summary>
        protected List<Stmt> _statements = new List<Stmt>();


        /// <summary>
        /// Public access to statments.
        /// </summary>
        public List<Stmt> Statements
        {
            get { return _statements; }
        }


        /// <summary>
        /// Executes the statement.
        /// </summary>
        public override void Execute()
        {
            if (this.Ctx != null && this.Ctx.Callbacks.HasAny)
            {
                Ctx.Callbacks.Notify("statement-on-before-execute", this, this);
                ExecuteBlock();
                Ctx.Callbacks.Notify("statement-on-after-execute", this, this);
                return;
            }
            ExecuteBlock();
        }


        /// <summary>
        /// Execute the statements.
        /// </summary>
        public override void DoExecute()
        {
            LangHelper.Execute(this._statements, this.Parent);
        }


        /// <summary>
        /// Executes the block with callback/template methods.
        /// </summary>
        protected virtual void ExecuteBlock()
        {
            try
            {
                OnBlockEnter();
                DoExecute();
            }
            finally
            {
                OnBlockExit();
            }
        }


        /// <summary>
        /// On enter of the block.
        /// </summary>
        protected virtual void OnBlockEnter()
        {
            this.Ctx.Memory.Push();
        }


        /// <summary>
        /// On exit of the block.
        /// </summary>
        protected virtual void OnBlockExit()
        {
            this.Ctx.Memory.Pop();
        }


        /// <summary>
        /// String representation
        /// </summary>
        /// <param name="tab">Tab to use for nested statements in blocks</param>
        /// <param name="incrementTab">Whether or not to add another tab</param>
        /// <param name="includeNewLine">Whether or not to include a new line.</param>
        /// <returns></returns>
        public override string AsString(string tab = "", bool incrementTab = false,  bool includeNewLine = true)
        {
            string info = base.AsString(tab, incrementTab);

            // Empty statements?
            if (_statements == null || _statements.Count == 0) return info;

            var buffer = new StringBuilder();

            // Now iterate over all the statements in the block
            foreach (var stmt in _statements)
            {
                buffer.Append(stmt.AsString(tab, true));
            }

            var result = info + buffer.ToString();
            if (includeNewLine) result += Environment.NewLine;

            return result;
        }
    }
}
