/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace CommonLibrary
{
    public class TokenLookupSettings
    {
        /// <summary>
        /// Whether or not the token lookup is case sensitive.
        /// </summary>
        public bool IsCaseSensitive { get; set; }


        /// <summary>
        /// Whether or not the token lookup is space sensitive.
        /// </summary>
        public bool IsSpaceSensitive { get; set; }
    }



    /// <summary>
    /// Token lookup.
    /// </summary>
    public interface ITokenLookup
    {
        /// <summary>
        /// Whether or not the token specified is a valid token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsValid(string token);


        /// <summary>
        /// Get the formatted token name given a raw token.
        /// Given a token that contains capital letters and spaces, returns the formatted
        /// token if implementation ignores the spaces and capital letters.
        /// </summary>
        /// <param name="token">Raw token Case and space sensitive.</param>
        /// <returns></returns>
        string this[string rawToken] { get; }


        /// <summary>
        /// Return list of valid formatted tokens based on the settings.
        /// </summary>
        ReadOnlyCollection<string> Tokens { get; }


        /// <summary>
        /// Get the token lookup settings.
        /// </summary>
        TokenLookupSettings Settings { get; }


        bool IsCaseSensitive { get; }
        bool IsWhiteSpaceSensitive { get; }
    }



    /// <summary>
    /// Lookup class for tokens.
    /// </summary>
    public class TokenLookup : ITokenLookup
    {
        private IDictionary<string, string> _validTokens;
        private bool _isCaseSensitive;
        private bool _ignoreWhiteSpace = true;
        private TokenLookupSettings _settings;
        private IList<string> _formattedValidTokens;


        /// <summary>
        /// Initialize the token lookup
        /// </summary>
        /// <param name="validTokens"></param>
        /// <param name="isCaseSensitive"></param>
        public TokenLookup(string[] validTokens, bool isCaseSensitive, bool ignoreWhiteSpace)
        {
            _validTokens = new Dictionary<string, string>();
            _settings = new TokenLookupSettings();
            _settings.IsCaseSensitive = isCaseSensitive;
            _settings.IsSpaceSensitive = !ignoreWhiteSpace;            

            // Store the tokens.
            Init(validTokens);
        }


        /// <summary>
        /// Return list of read-only valid tokens.
        /// </summary>
        public ReadOnlyCollection<string> Tokens
        {
            get { return new ReadOnlyCollection<string>(_formattedValidTokens); }
        }


        /// <summary>
        /// Get the settings.
        /// </summary>
        public TokenLookupSettings Settings
        {
            get { return _settings; }
        }


        /// <summary>
        /// Determine if valid key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsValid(string token)
        {
            // Validate
            if (string.IsNullOrEmpty(token)) { return false; }

            token = GetValidTokenFormat(token);

            return _validTokens.ContainsKey(token);
        }


        /// <summary>
        /// Is case sensitive.
        /// </summary>
        public bool IsCaseSensitive
        {
            get { return _settings.IsCaseSensitive; }
        }


        /// <summary>
        /// Is white space sensitive
        /// </summary>
        public bool IsWhiteSpaceSensitive
        {
            get { return _settings.IsSpaceSensitive; }
        }


        /// <summary>
        /// returns the original token in proper case 
        /// if valid, empty string otherwise.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string this[string token]
        {
            get
            {
                // Validate.
                if( string.IsNullOrEmpty( token ) ) { return string.Empty; }

                // Get the right format.
                token = GetValidTokenFormat(token);
                
                if (!_validTokens.ContainsKey(token)) { return string.Empty; }

                // Exists.
                return _validTokens[token];
            }
        }


        /// <summary>
        /// Store the tokens.
        /// </summary>
        /// <param name="validTokens"></param>
        private void Init(string[] validTokens)
        {
            // Store the tokens in the hashtable.
            foreach (string token in validTokens)
            {
                string tokenKey = token;
                tokenKey = GetValidTokenFormat(tokenKey);
                _validTokens.Add(tokenKey, token);
                _formattedValidTokens.Add(tokenKey);
            }
        }


        private string GetValidTokenFormat(string token)
        {
            if (!_isCaseSensitive) { token = token.ToLower(); }
            if (_ignoreWhiteSpace) { token = token.Trim(); }

            return token;
        }
    }
}
