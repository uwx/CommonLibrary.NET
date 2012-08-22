using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace ComLib.Automation
{
    /// <summary>
    /// Constants used when making callbacks while interpreting code
    /// </summary>
    public enum JsLiteToken
    {        
        /// <summary>
        /// FuncCall indicator.
        /// </summary>
        FuncCall,


        /// <summary>
        /// For loop indicator
        /// </summary>
        ForLoop,


        /// <summary>
        /// Variable declaration indicator.
        /// </summary>
        Var,


        /// <summary>
        /// Comment indicator.
        /// </summary>
        CommentLineSingle,

        
        /// <summary>
        /// Multi-line comment.
        /// </summary>
        CommentLineMultiple,


        /// <summary>
        /// Multi-line comment.
        /// </summary>
        CommentLineMultipleEnd,


        /// <summary>
        /// If statement.
        /// </summary>
        If,


        /// <summary>
        /// Empty line.
        /// </summary>
        EmptyLine,


        /// <summary>
        /// Other line
        /// </summary>
        Other
    }



    /// <summary>
    /// Arguments used in callback methods
    /// </summary>
    public class JsLiteArgs
    {
        /// <summary>
        /// The current token type.
        /// </summary>
        public JsLiteToken TokenType;

        /// <summary>
        /// Name of function called or statement type "var" | "for" | "helloworld" func
        /// </summary>
        public string Name;


        /// <summary>
        /// Current line number being handled.
        /// </summary>
        public int LineNumber;


        /// <summary>
        /// Context data
        /// </summary>
        public object Context;


        /// <summary>
        /// List of parameters to func call.
        /// </summary>
        public List<object> ParamList;

        
        /// <summary>
        /// List of named parameters to func call.
        /// </summary>
        public IDictionary ParamMap;
    }



    /// <summary>
    /// Very, Very light version of Javascript - JSLite. Non-Lexical parser.
    /// Features include:
    /// 1. var declarations
    /// 2. if conditions
    /// 3. while loops
    /// 4. for loops
    /// 5. func calls
    /// </summary>
    public class JsLite
    {
        private const string RegexCommentSingleLine = @"\s*\\\\\s{1}\*";
        private const string RegexVariable = @"\s*var\s*(?<name>[\w]+)\s*=\s*(?<value>.+)$";
        private const string RegexForLoop = @"\s*for\s*\(\s*var\s+(?<name>\w+)\s*=\s*(?<start>[^;]+);\s*(?<checkname>\w+)\s*(?<checkcondition>[^\s]+)\s*(?<checkvalue>[^;]+);\s*(?<incrementname>\w+)\s*(?<incrementval>[^\)]+)\)\{?";
        private const string RegexFuncCall = @"\s*(?<funcname>[\$_\w\.]+)\(";

        private Scope _scope;
        private string _script;
        private IList<string> _lines;
        private int _currentLineIndex = 0;
        private string _currentLine;
        private Interpreter _interpreter;
        private Func<JsLiteArgs,bool> _visitor;


        /// <summary>
        /// Initialize interpreter with scope.
        /// </summary>
        /// <param name="scope">Scope of the script.</param>
        /// <param name="callback">Callback to handle various statements.</param>
        public JsLite(Scope scope, Func<JsLiteArgs,bool> callback)
        {
            _scope = scope;
            _visitor = callback;
            _interpreter = new Interpreter(_scope);
        }


        /// <summary>
        /// Interprets the script.
        /// </summary>
        public void Interpret(string script)
        {
            // Init
            _script = script;
            _lines = StringHelper.ReadLines(script);

            // Each line
            for (int ndx = 0; ndx < _lines.Count; ndx++)
            {
                // Read next line.
                JsLiteToken tokenType = ReadLine(ndx);

                if ( tokenType == JsLiteToken.CommentLineSingle || tokenType == JsLiteToken.EmptyLine) continue;
                else if (tokenType == JsLiteToken.CommentLineMultiple) ndx = ProcessMultiLineComment(ndx);
                else if (tokenType == JsLiteToken.FuncCall) ProcessFuncCall();
                else if (tokenType == JsLiteToken.Var) ProcessVariable();
                else if (tokenType == JsLiteToken.ForLoop) ProcessForLoop();
            }
        }


        #region Process Statements
        /// <summary>
        /// Process a variable.
        /// </summary>
        private void ProcessVariable()
        {
            var exp = _interpreter.TokenizeVariable(_currentLine, RegexVariable);
            _visitor(new JsLiteArgs() { LineNumber = _currentLineIndex, TokenType = JsLiteToken.Var, Name = exp.Name, Context = exp.Value });
        }


        /// <summary>
        /// Process a function call.
        /// </summary>
        private void ProcessFuncCall()
        {
            var exp = _interpreter.TokenizeFunctionCall(_currentLine, _currentLineIndex);
            bool proceed = _visitor(new JsLiteArgs { LineNumber = _currentLineIndex, TokenType = JsLiteToken.FuncCall, Name = exp.Name, ParamMap = exp.ParamMap, ParamList = exp.ParamList });
            if (!proceed) return;
        }


        /// <summary>
        /// Process a variable.
        /// </summary>
        private void ProcessForLoop()
        {
            var line = _currentLine;
        }


        /// <summary>
        /// Skips over the multi-line index.
        /// </summary>
        /// <param name="lineIndex"></param>
        /// <returns></returns>
        private int ProcessMultiLineComment(int lineIndex)
        {
            // Go to next line.
            lineIndex++;
            while (lineIndex < _lines.Count)
            {
                var nextLineToken = ReadLine(lineIndex);
                if (nextLineToken == JsLiteToken.CommentLineMultipleEnd)
                {
                    break;
                }
                lineIndex++;
            }
            return lineIndex;
        }
        #endregion


        private JsLiteToken ReadLine(int ndx)
        {
            _currentLineIndex = ndx;
            _currentLine = _lines[ndx];
            _currentLine = _currentLine.Trim();
            if (string.IsNullOrWhiteSpace(_currentLine)) return JsLiteToken.EmptyLine;
            if (_currentLine.StartsWith("//")) return JsLiteToken.CommentLineSingle;
            if (_currentLine.StartsWith("/* ")) return JsLiteToken.CommentLineMultiple;
            if (_currentLine.EndsWith("*/")) return JsLiteToken.CommentLineMultipleEnd;
            if (_currentLine.StartsWith("for")) return JsLiteToken.ForLoop;
            if (_currentLine.StartsWith("var")) return JsLiteToken.Var;
            if (_currentLine.StartsWith("if")) return JsLiteToken.If;
            if (Regex.IsMatch(_currentLine, RegexFuncCall)) return JsLiteToken.FuncCall;

            return JsLiteToken.Other;
        }
    }
}
