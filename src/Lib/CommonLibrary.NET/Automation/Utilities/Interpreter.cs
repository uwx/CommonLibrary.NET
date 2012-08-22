using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace ComLib.Automation
{
    /// <summary>
    /// Very, Very light version of Javascript - JSLite. Non-Lexical parser.
    /// Features include:
    /// 1. var declarations
    /// 2. if conditions
    /// 3. while loops
    /// 4. for loops
    /// 5. func calls
    /// </summary>
    public class Interpreter
    {
        private const string RegexCommentSingleLine = @"\s*\\\\\s{1}\*";
        private const string RegexVariable = @"\s*var\s*(?<name>[\w]+)\s*=\s*(?<value>.+)$";
        private const string RegexForLoop = @"\s*for\s*\(\s*var\s+(?<name>\w+)\s*=\s*(?<start>[^;]+);\s*(?<checkname>\w+)\s*(?<checkop>[^\s]+)\s*(?<checkvalue>[^;]+);\s*(?<incrementname>\w+)\s*(?<incrementvalue>[^\)]+)\)\{?";
        private const string RegexFuncCall = @"\s*(?<funcname>[\$_\w\.]+)\(";

        private Scope _scope;        
        //private Func<JsLiteArgs,bool> _visitor;


        /// <summary>
        /// Initialize interpreter with scope.
        /// </summary>
        /// <param name="scope">Scope of the script.</param>
        public Interpreter(Scope scope)
        {
            _scope = scope;            
        }


        /// <summary>
        /// Whether or not the line supplied is a for loop.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool IsForLoop(string line)
        {
            var match = Regex.Match(line, RegexForLoop);
            if (!match.Success) return false;

            // Check the checkop
            var checkOp = match.Groups["checkop"].Value;
            if (checkOp != "<" && checkOp != "<=" && checkOp != ">" && checkOp != ">=")
                return false;

            return true;
        }


        /// <summary>
        /// Parse variable into tuple of key/value pair.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public VariableExpr TokenizeVariable(string line, string pattern)
        {
            var match = Regex.Match(line, pattern);
            if (!match.Success)
                throw new InvalidOperationException("Invalid format of variable : " + line + ". Must be var name = <value> | '<value>' | \"<value>\"");

            var name = match.Groups["name"].Value;
            var val = match.Groups["value"].Value;
            return ParseVariable(name, val);
        }
        
        

        private VariableExpr ParseVariable(string name, string val)
        {
            var type = typeof(int);
            
            val.Trim();
            if (val.EndsWith(";")) val = val.Substring(0, val.Length - 1);

            object objVal = val;

            // 1. String
            if (val.StartsWith("'") || val.StartsWith("\""))
            {
                objVal = val.Substring(1, val.Length - 2);
                type = typeof(string);
            }
            // 2. Bool
            else if (string.Compare(val, "true", true) == 0 || string.Compare(val, "false", true) == 0)
            {
                type = typeof(bool);
                objVal = bool.Parse(val);
            }
            else if (Regex.IsMatch(val, RegexPatterns.Integer))
            {
                type = typeof(int);
                objVal = Convert.ToInt32(val);
            }
            // 4. Number
            else if (Regex.IsMatch(val, RegexPatterns.Numeric))
            {
                type = typeof(double);
                objVal = double.Parse(val);
            }
            // 5. function call.
            else if (val.Contains("("))
            {
                type = typeof(Action);
            }            
            

            return new VariableExpr() { Name = name, Value = objVal, DataType = type };
        }


        /// <summary>
        /// Tokenizes the for loop into a forloop expression;
        /// </summary>
        /// <param name="line">The text line representing the loop</param>
        /// <param name="lineNumber">The line number of the for loop</param>
        /// <returns></returns>
        public ForLoopExpression TokenizeForLoop(string line, int lineNumber)
        {
            var match = Regex.Match(line, RegexForLoop);
            var forloop = new ForLoopExpression();
            forloop.Variable = match.Groups["name"].Value;
            forloop.StartExpression = match.Groups["start"].Value.Trim();
            forloop.CheckOp = match.Groups["checkop"].Value.Trim();
            forloop.CheckExpression = match.Groups["checkvalue"].Value.Trim();
            forloop.IncrementOp = match.Groups["incrementvalue"].Value.Trim();
            string checkName = match.Groups["checkname"].Value.Trim();
            string incrementName = match.Groups["incrementname"].Value.Trim();
            
            // Confirm that name of variable is same 
            if (string.Compare(forloop.Variable, checkName, true) != 0)
                throw new ArgumentException("Invalid for loop : variable " + forloop.Variable + " does not match : " + checkName);

            if (string.Compare(forloop.Variable, incrementName, true) != 0)
                throw new ArgumentException("Invalid for loop : variable " + forloop.Variable + " does not match : " + incrementName);

            // Normalize
            if (forloop.CheckOp == "less_than")
                forloop.CheckOp = "<";
            else if (forloop.CheckOp == "less_than_equal")
                forloop.CheckOp = "<=";

            // Convert ++ to +=1, -- to -=1
            if (forloop.IncrementOp == "++")
            {
                forloop.IncrementOp = "+=";
                forloop.IncrementExpression = "1";
            }
            else if (forloop.IncrementOp == "--")
            {
                forloop.IncrementOp = "-=";
                forloop.IncrementExpression = "1";
            }
            else if (forloop.IncrementOp.Contains("+=") || forloop.IncrementOp.Contains("-="))
            {
                int ndxOfEqual = forloop.IncrementOp.IndexOf("=");                
                string incOp = forloop.IncrementOp.Substring(0, ndxOfEqual + 1);
                forloop.IncrementExpression = forloop.IncrementOp.Substring(ndxOfEqual + 1).Trim();
                forloop.IncrementOp = incOp;
            }
            else if (!forloop.IncrementOp.Contains("+=") &&
                     !forloop.IncrementOp.Contains("-="))
                throw new ArgumentException("Invalid increment value supplied in for loop : " + forloop.IncrementOp + " at line : " + lineNumber);

            return forloop;
        }


        /// <summary>
        /// Tokenizes a function call
        /// </summary>
        /// <param name="line">Text representing the function call.</param>
        /// <param name="linenumber">Current line number</param>
        /// <returns></returns>
        public FunctionCallExpr TokenizeFunctionCall(string line, int linenumber)
        {
            line = line.Trim();
            int ndxLeftParen = line.IndexOf("(");
            if (!line.EndsWith(");")) throw new ArgumentException("Function call : " + line + ", does not end with );");
            if (ndxLeftParen == -1) throw new ArgumentException("Function call : " + line + ", does not start with (");
            
            // Reader
            Scanner reader = new Scanner(line);
            reader.ReadChar();
            // 1. Get function name.
            var funcName = reader.ReadTokenUntil('(', '"', false, true, false, false, true);
            var paramMap = new Dictionary<string, object>();
            var paramList = new List<object>();

            // 2. Empty list?
            char currentChar = reader.CurrentChar;
            if (currentChar == ')') return new FunctionCallExpr() { Name = funcName, ParamMap = new Dictionary<string, object>() };
            char[] endToken = new []{' ',',',')'};

            // Keep reading.
            bool isJsonParam = false;
            int paramIndex = 0;
            while (!reader.IsEnded())
            {   
                string param = string.Empty;

                // White space?
                if (currentChar == ' ') reader.ConsumeWhiteSpace(false, false);
                else if (currentChar == '{') isJsonParam = true;
                else if (currentChar == '}') isJsonParam = false;
                else if (currentChar == ')' || currentChar == ';') break;
                else if (currentChar == ',') { }
                else if (currentChar == '\'' || currentChar == '\"')
                {
                    param = reader.ReadToken(currentChar, '\\', false, true, true, false);
                    paramMap[paramIndex.ToString()] = param;
                    paramList.Add(param);
                    paramIndex++;
                }
                else
                {
                    if (isJsonParam)
                    {
                        var pair = ParsePair(reader);
                        var varExp = ParseVariable(pair.Key, pair.Value);
                        paramMap[pair.Key] = varExp.Value;
                    }
                    else
                    {
                        param = reader.ReadTokenUntil(endToken);
                        var varExp = ParseVariable("temp", param);
                        paramMap[paramIndex.ToString()] = varExp.Value;
                        paramList.Add(varExp.Value);
                        paramIndex++;
                    }
                }

                // Next char.
                currentChar = reader.ReadChar();
            }
            return new FunctionCallExpr() { Name = funcName, ParamMap = paramMap, ParamList = paramList};
        }


        private KeyValuePair<string, string> ParsePair(Scanner reader)
        {
            var name = reader.ReadTokenUntil(':', '\\', false, true, false, false, true);
            var val = string.Empty;
            reader.ConsumeWhiteSpace();
            var nextChar = reader.CurrentChar;
            if (nextChar == '\'' || nextChar == '"')
                val = reader.ReadToken(nextChar, '\\', false, true, true, false);
            else
                val = reader.ReadTokenUntil(new char[]{',', '}'});

            return new KeyValuePair<string, string>(name.Trim(), val.Trim());
        }



        /// <summary>
        /// Function call expression data.
        /// </summary>
        public class FunctionCallExpr
        {
            /// <summary>
            /// Name of the function.
            /// </summary>
            public string Name;


            /// <summary>
            /// List of arguments.
            /// </summary>
            public List<object> ParamList;


            /// <summary>
            /// Arguments to the function.
            /// </summary>
            public IDictionary ParamMap;
        }



        /// <summary>
        /// Variable expression data
        /// </summary>
        public class VariableExpr
        {
            /// <summary>
            /// Name of the variable.
            /// </summary>
            public string Name;


            /// <summary>
            /// Datatype of the variable.
            /// </summary>
            public Type DataType;


            /// <summary>
            /// Value of the variable.
            /// </summary>
            public object Value;
        }



        /// <summary>
        /// For loop Expression data
        /// </summary>
        public class ForLoopExpression
        {
            /// <summary>
            /// Name of the variable
            /// </summary>
            public string Variable;


            /// <summary>
            /// Expression representing the start value.
            /// </summary>
            public string StartExpression;


            /// <summary>
            /// Operator for condition check in loop ndx >= 4
            /// </summary>
            public string CheckOp;


            /// <summary>
            /// Expression representing the bound value ndx > = 4 
            /// </summary>
            public string CheckExpression;


            /// <summary>
            /// The operator to increment by ++ or +=
            /// </summary>
            public string IncrementOp;


            /// <summary>
            /// The expression if not ++ but += then the number after +=
            /// </summary>
            public string IncrementExpression;
        }
    }
    /*
     

Evaluate(Expression right, Operator op, Expression left)
{
	double result = 0;
	if( op == "*" )
	{
		result = left.Value * right.Value;
	}
	else if( op == "/" )
	{
		result = left.Value / right.Value;
	}
	else if( op == "+" )
	{
		result = left.Value + right.Value;
	}
	else if( op == "-" )
	{
		result = left.Value - right.Value;
	}
	else if( op == "%" )
	{
		result = left.Value * right.Value;
	}	
}


Evaluate(Expression left, Operator op, Expression right)
{
	bool result = false;
	if( op == "<" )
	{
		result = left.Value < right.Value;
	}
	else if( op == "<=" )
	{
		result = left.Value <= right.Value;
	}
	else if( op == ">" )
	{
		result = left.Value > right.Value;
	}
	else if( op == ">=" )
	{
		result = left.Value >= right.Value;
	}
	else if( op == "==" )
	{
		result = left.Value == right.Value;
	}
	else if( op == "!=" )
	{
		result = left.Value != right.Value;
	}
	return result;
}


Evaluate(Expression left, Operator op, Expression right)
{
	bool result = false;
	if( op == "&&" )
	{
		result = left.Value && right.Value;
	}
	else if (op == "||")
	{
		result = left.Value || right.Value;
	}
	return result;
} 
    */
}
