using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang;


namespace ComLib.Lang.Extensions
{

    /* *************************************************************************
    <doc:example>	
    // Provides ability to check variable types and convert from one type to another.
        
    // 1. Supported types = "number", "yesno", "date", "time", "string"    
    // 2. Supported functions :
    // 3. You can replace the the "<type>" below with any of the supported types above.
     
    //  NAME             EXAMPLE                            RESULT
    //  is_<type>        is_number( 123 )                   true
    //  is_<type>        is_number( '123' )                 false
    //  is_<type>_like   is_number_like( '123' )            true
    //  to_<type>        to_number( '123' )                 123
    
    //  is_<type>        is_yesno( true )                   true
    //  is_<type>        is_yesno( 'true' )                 false
    //  is_<type>_like   is_yesno_like( 'true' )            true
    //  to_<type>        to_yesno( 'true' )                 true
    
    //  is_<type>        is_date( new Date(2012, 9, 10 )    true
    //  is_<type>        is_date( '9/10/2012' )             false
    //  is_<type>_like   is_date_like( '9/10/2012' )        true
    //  to_<type>        to_date( '9/10/2012' )             Date(2012, 9, 10)
    
    </doc:example>
    ***************************************************************************/

    /// <summary>
    /// Combinator for handling swapping of variable values. swap a and b.
    /// </summary>
    public class TypeOperationsPlugin : ExprPlugin
    {
        private IDictionary<string, string> _functionToTypeMap;


        /// <summary>
        /// Intialize.
        /// </summary>
        public TypeOperationsPlugin()
        {
            _hasStatementSupport = false;
            _canHandleExpression = true;
            _functionToTypeMap = new Dictionary<string, string>();
            var types = new string[] { "number", "yesno", "date", "time", "string" };
            var functionnames = new List<string>();

            // create all the supported functions: e.g. for "number" we have:
            // 1. "is_number"
            // 2. "is_number_like"
            // 3. "to_number"
            foreach (var type in types)
            {
                _functionToTypeMap["is_" + type] = type;
                _functionToTypeMap["is_" + type + "_like"] = type;
                _functionToTypeMap["to_" + type] = type;
            }
            _startTokens = _functionToTypeMap.Keys.ToArray();
        }


        /// <summary>
        /// The grammer for the function declaration
        /// </summary>
        public override string Grammer
        {
            get
            {
                return "typeof <expression>";
            }
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
                    "is_number( 123 )",
                    "is_number_like( '123' )",
                    "to_number( '123' )",
                };
            }
        }


        /// <summary>
        /// run step 123.
        /// </summary>
        /// <returns></returns>
        public override Expr Parse()
        {
            var functionName = _tokenIt.NextToken.Token.Text;
            
            // Move to next token. possibly a "("
            _tokenIt.Advance();
            var expectParenthesis = _tokenIt.NextToken.Token.Type == TokenTypes.LeftParenthesis;
            if (expectParenthesis)
                _tokenIt.Advance();

            // 1. Get the expression.
            var exp = _parser.ParseExpression(Terminators.ExpFlexibleEnd, true, false, true, true, false);            

            // 2. map the function name to the expression.
            var destinationType = _functionToTypeMap[functionName];

            // 3. determine if converting or just checking.
            var isConverting = functionName.StartsWith("to"); 
            if (expectParenthesis)
                _tokenIt.Expect(Tokens.RightParenthesis);

            return new TypeOperationsExpr(isConverting, destinationType, exp);
        }
    }



    /// <summary>
    /// Variable expression data
    /// </summary>
    public class TypeOperationsExpr : Expr
    {        
        private Expr _exp;
        private bool _isConversion;
        private string _destinationType;


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="exp">The expression value to round</param>
        public TypeOperationsExpr(bool isConversion, string destinationType, Expr exp)
        {
            _exp = exp;
            _isConversion = isConversion;
            _destinationType = destinationType;
        }


        /// <summary>
        /// Evaluate the type check/conversion operation.
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            if (_isConversion)
                return ConvertValue();
            return null;
        }


        /// <summary>
        /// Used for function calls like "is_number(123)"
        /// </summary>
        /// <returns></returns>
        private object CheckExplicitType()
        {
            var val = _exp.Evaluate();
            if (val == null)
                return LNull.Instance;

            var result = false;
            if (_destinationType == "string" && val.GetType() == typeof(string)   ) result = true;
            if (_destinationType == "number" && val.GetType() == typeof(double)   ) result = true;
            if (_destinationType == "bool"   && val.GetType() == typeof(bool)     ) result = true;
            if (_destinationType == "date"   && val.GetType() == typeof(DateTime) ) result = true;
            if (_destinationType == "time"   && val.GetType() == typeof(TimeSpan) ) result = true;
            if (_destinationType == "list"   && val.GetType() == typeof(LArray))    result = true;
            if (_destinationType == "map"    && val.GetType() == typeof(LMap))      result = true;
            return result;
        }


        /// <summary>
        /// Used for function calls like "to_number( '123' )";
        /// </summary>
        /// <returns></returns>
        private object ConvertValue()
        {
            var val = _exp.Evaluate();
            if (val == null)
                return LNull.Instance;
            var result = DoConvertValue(_destinationType, val);
            return result;
        }


        private object DoConvertValue(string destinationType, object val)
        {
            object result = null;
            if (destinationType == "string") result = Convert.ChangeType(val, typeof(string));
            if (destinationType == "number") result = Convert.ChangeType(val, typeof(double));
            if (destinationType == "bool")   result = Convert.ChangeType(val, typeof(bool));
            if (destinationType == "date")   result = Convert.ChangeType(val, typeof(DateTime));
            if (destinationType == "time")   result = Convert.ChangeType(val, typeof(TimeSpan));
            return result;
        }
    }
}
