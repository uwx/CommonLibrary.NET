using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang
{
    /// <summary>
    /// Abstract syntax tree node.
    /// </summary>
    public class AstNode
    {
        /// <summary>
        /// Reference to the script.
        /// </summary>
        public ScriptRef Ref;


        /// <summary>
        /// Context information of the script.
        /// </summary>
        public Context Ctx;


        /// <summary>
        /// Whether or not this expression supports using parenthesis "( )" e.g. function calls.
        /// </summary>
        protected bool _supportsBoundary;


        /// <summary>
        /// The text representing the end of the boundary. e.g. ), }, ]
        /// </summary>
        protected string _boundaryText;


        /// <summary>
        /// Whether or not this expression supports using parenthesis "( )" e.g. function calls.
        /// </summary>
        public bool SupportsBoundary { get { return _supportsBoundary; } set { _supportsBoundary = value; } }


        /// <summary>
        /// The character used for the end of the boundry e.g. ) ] }
        /// </summary>
        public string BoundaryText { get { return _boundaryText; } set { _boundaryText = value; } }


        /// <summary>
        /// Initialize the boundary information.
        /// </summary>
        /// <param name="supportsBoundary"></param>
        /// <param name="boundaryText"></param>
        public virtual void InitBoundary(bool supportsBoundary, string boundaryText)
        {
            _supportsBoundary = supportsBoundary;
            _boundaryText = boundaryText;
        }


        /// <summary>
        /// Returns the fully qualified name of this node.
        /// </summary>
        /// <returns></returns>
        public virtual string ToQualifiedName()
        {
            return string.Empty;
        }
    }
}
