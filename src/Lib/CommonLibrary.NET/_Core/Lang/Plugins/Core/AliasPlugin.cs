using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang;


namespace ComLib.Lang.Extensions
{

    /* *************************************************************************
    <doc:example>	
    // Alias plugin is a base class for other plugin that want to register aliases
    // for tokens. e.g using "set" to actually represent "var" or using
    // "and" to represent &&
    
    </doc:example>
    ***************************************************************************/

    /// <summary>
    /// Combinator for handling days of the week.
    /// </summary>
    public class AliasTokenPlugin : TokenPlugin
    {
        private IDictionary<string, Token> _map;


        /// <summary>
        /// Initialize
        /// </summary>
        public AliasTokenPlugin(string alias, Token replacement)
        {
            _map = new Dictionary<string, Token>();
            _tokens = new string[] { alias };
            _canHandleToken = true;
            Register(alias, replacement);
        }        


        /// <summary>
        /// The grammer for the function declaration
        /// </summary>
        public override string Grammer
        {
            get
            {
                return "<alias> -> <replacement>";
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
                    "see examples for <alias>"
                };
            }
        }


        /// <summary>
        /// Register an alias.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="replacement"></param>
        public void Register(string text, Token replacement)
        {
            _map[text] = replacement;
        }


        /// <summary>
        /// Whether or not this plugin is a match for the token supplied.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public override bool CanHandle(Token current)
        {
            return _map.ContainsKey(current.Text);
        }


        /// <summary>
        /// Peeks at the token.
        /// </summary>
        /// <returns></returns>
        public override Token Peek()
        {
            var token = _map[_tokenIt.NextToken.Token.Text];
            return token;
        }


        /// <summary>
        /// Parses the day expression.
        /// Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
        /// </summary>
        /// <returns></returns>
        public override Token Parse()
        {
            var token = _map[_tokenIt.NextToken.Token.Text];
            return token;
        }


        /// <summary>
        /// Parse the expression with parameters for moving the token iterator forward first
        /// </summary>
        /// <param name="advanceFirst">Whether or not to move the token iterator forward first</param>
        /// <param name="advanceCount">How many tokens to move the token iterator forward by</param>
        /// <returns></returns>
        public override Token Parse(bool advanceFirst, int advanceCount)
        {
            if (advanceFirst)
                _tokenIt.Advance(advanceCount);

            var token = _map[_tokenIt.NextToken.Token.Text];
            return token;
        }
    }
}
