using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang
{
    /// <summary>
    /// Base class for statements.
    /// </summary>
    public class Stmt : AstNode
    {
        /// <summary>
        /// Parent of this statement
        /// </summary>
        public Stmt Parent { get; set; }


        /// <summary>
        /// The symbol scope associated w/ this instance.
        /// </summary>
        public ISymbols SymScope { get; set; }


        /// <summary>
        /// Whether or not this statement can be executed immediately at parsing time
        /// e.g. useful for function declaration statements among other future planned features.
        /// </summary>
        public bool IsImmediatelyExecutable { get; set; }


        /// <summary>
        /// Internal method to wrap statement executions around the callbacks.
        /// </summary>
        public virtual void DoExecute()
        {            
        }


        /// <summary>
        /// Executes the statement.
        /// </summary>
        public virtual void Execute()
        {           
            if (this.Ctx != null && this.Ctx.Callbacks.HasAny)
            {
                Ctx.Callbacks.Notify("statement-on-before-execute", this, this);
                DoExecute();
                Ctx.Callbacks.Notify("statement-on-after-execute", this, this);
                return;
            }
            DoExecute();
        }


        /// <summary>
        /// Finds the first parent that is of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T FindParent<T>() where T : class
        {
            T found = default(T);
            Stmt current = Parent;
            while (current != null)
            {
                if (current is T)
                {
                    found = current as T;
                    break;
                }
                current = Parent.Parent;
            }
            return found;
        }


        /// <summary>
        /// String representation of statement.
        /// </summary>
        /// <param name="tab">Tab to use</param>
        /// <param name="incrementTab">Whether or not to add another tab</param>
        /// <param name="includeNewLine">Whether or not to include a new line.</param>
        /// <returns></returns>
        public virtual string AsString(string tab = "", bool incrementTab = false, bool includeNewLine = true)
        {   
            var stmtType = this.GetType().Name.Replace("Stmt", "");
            string info = string.Format("{0}, {1}, {2} ", stmtType, Ref.Line, Ref.CharPos);

            if (incrementTab)
                tab = tab + "\t";

            var result = tab + info;
            if (includeNewLine) result += Environment.NewLine;

            return result;
        }
    }
}
