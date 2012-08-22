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
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace CommonLibrary
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    public interface IKeyValueParserValidating<TKey, TVal> : IKeyValueParser<TKey, TVal>
    {
        ITokenLookup TokenLookup { get; }        
    }


    /// <summary>
    /// Key value parser
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    public interface IKeyValueParser<TKey, TVal>
    {
        KeyValueParserSettings Settings { get; }

        IList<KeyValueData<TKey, TVal>> Parse(string textToParse, IList<string> errors);
        IList<KeyValueData<TKey, TVal>> ParseFile(string filePath, IList<string> errors);
        IList<KeyValueData<TKey, TVal>> ParseLineWithMultiPairs(string text);        
        KeyValueData<TKey, TVal> ParseLine(string text);
    }

    

    /// <summary>
    /// Class to store the key/values
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    public class KeyValueData<TKey, TVal>
    {
        private TKey _key;
        private TVal _val;
        private bool _isValidKey;
        private static KeyValueData<TKey, TVal> _null;


        static KeyValueData()
        {
            TKey defaultKey = default(TKey);
            TVal defaultVal = default(TVal);

            _null = new KeyValueData<TKey, TVal>(defaultKey, defaultVal);
        }


        /// <summary>
        /// Get the null / empty object.
        /// </summary>
        public static KeyValueData<TKey, TVal> Empty
        {
            get { return _null; }
        }


        /// <summary>
        /// Initialize the data with a valid value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="isValueValid"></param>
        public KeyValueData(TKey key, TVal val)
        {
            _key = key;
            _val = val;
            _isValidKey = true;
        }


        /// <summary>
        /// Initialize the data.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="isValueValid"></param>
        public KeyValueData(TKey key, TVal val, bool isValidKey)
        {
            _key = key;
            _val = val;
            _isValidKey = isValidKey;
        }


        /// <summary>
        /// The key.
        /// </summary>
        public TKey Key
        {
            get { return _key; }
        }


        /// <summary>
        /// The value.
        /// </summary>
        public TVal Value
        {
            get { return _val; }
        }


        /// <summary>
        /// Is the value valid.
        /// </summary>
        public bool IsValidKey
        {
            get { return _isValidKey; }
        }


        internal void SetKey(TKey key)
        {
            _key = key;
        }


        internal void SetIsValidKey(bool isValid)
        {
            _isValidKey = isValid;
        }
    }



    /// <summary>
    /// Settings for the parser.
    /// These settings only apply if the ITokenLookup is
    /// 
    /// 1. Case insensitive.
    /// 2. Ignores whitespace.
    /// </summary>
    public class KeyValueParserSettings
    {
        private bool _trimWhiteSpaceFromKeys = false;
        private bool _convertToLowerCase = false;
        private bool _supportMultiPairsOnSingleLine = true;
        private string _keyValuePairDelimter = ":";
        private string _multiPairDelimeter = ",";
        private string _multiPairKeyValueDelimeter = "=";


        /// <summary>
        /// Trim the white space from keys.
        /// </summary>
        public bool TrimWhiteSpaceFromKeys
        {
            get { return _trimWhiteSpaceFromKeys; }
            set { _trimWhiteSpaceFromKeys = value; }
        }


        /// <summary>
        /// Convert the keys to lowercase.
        /// </summary>
        public bool ConvertKeysToLowerCase
        {
            get { return _convertToLowerCase; }
            set { _convertToLowerCase = value; }
        }


        /// <summary>
        /// Delimeter for a single keyvalue pair. "e.g." "="
        /// 
        /// key = "firstname" 
        /// value = "kishore"
        /// delimeter = "="
        /// </summary>
        /// <example>firstname = kishore</example>
        public string KeyValuePairDelimeter
        {
            get { return _keyValuePairDelimter; }
            set { _keyValuePairDelimter = value; }
        }


        /// <summary>
        /// Delimeter for multiple keyValue pairs on single line.
        /// </summary>
        /// <example>firstname=kishore,lastname=reddy,isadmin=1</example>
        public string MultiPairDelimeter
        {
            get { return _multiPairDelimeter; }
            set { _multiPairDelimeter = value; }
        }


        public string MultiPairKeyValueDelimeter
        {
            get { return _multiPairKeyValueDelimeter; }
            set { _multiPairKeyValueDelimeter = value; }
        }


        public bool SupportMultiPairsOnSingleLine
        {
            get { return _supportMultiPairsOnSingleLine; }
            set { _supportMultiPairsOnSingleLine = value; }
        }
    }



    /// <summary>
    /// Validating key value parser that validates the Tokens.
    /// </summary>
    public class KeyValueValidatingStringParser : KeyValueStringParser, IKeyValueParserValidating<string, string>
    {
        private ITokenLookup _tokenLookup;

        /// <summary>
        /// Initialize the validating parser with the list of valid tokens.
        /// </summary>
        /// <param name="tokenLookUp"></param>
        public KeyValueValidatingStringParser(ITokenLookup tokenLookUp)
        {
            _tokenLookup = tokenLookUp;
        }


        /// <summary>
        /// The token lookup.
        /// </summary>
        public ITokenLookup TokenLookup
        {
            get { return _tokenLookup; }
        }


        /// <summary>
        /// Parsese the key/value text and then validates the key against the 
        /// token lookup. 
        /// Also sets the key based on the settings of the token if valid.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        protected override KeyValueData<string, string> InternalParseLine(string text, string delimeter, bool applySettings)
        {
            KeyValueData<string, string> pair = base.InternalParseLine(text, delimeter, false);

            // Only massage the key and validate it if there is a key.
            if (string.IsNullOrEmpty(pair.Key)) { return pair; }
            
            string key = pair.Key;

            // Convert to lowercase if token lookup is not case sensitive.
            // Trim white space if it's not important.
            if ( !_tokenLookup.IsCaseSensitive && _settings.ConvertKeysToLowerCase) { key = key.ToLower(); }
            if ( !_tokenLookup.IsWhiteSpaceSensitive && _settings.TrimWhiteSpaceFromKeys ) { key = key.Trim(); }

            bool isValid = _tokenLookup.IsValid(key);

            // Set the new key, and whether it's valid.            
            pair.SetIsValidKey(isValid);

            // Only set the new key if valid.
            pair.SetKey(key); 

            return pair;
        }
    }


    /// <summary>
    /// String based keyvalue parser
    /// </summary>
    public class KeyValueStringParser : IKeyValueParser<string, string>
    {
        protected KeyValueParserSettings _settings = new KeyValueParserSettings();
        

        /// <summary>
        /// Initialize the key value parser with default settings.
        /// </summary>
        public KeyValueStringParser()
        {
            _settings.KeyValuePairDelimeter = ":";
            _settings.MultiPairDelimeter = ",";
            _settings.MultiPairKeyValueDelimeter = "=";
            _settings.ConvertKeysToLowerCase = false;
            _settings.TrimWhiteSpaceFromKeys = false;
        }        


        #region IKeyValueParser<TKey,TVal> Members

        /// <summary>
        /// Get / set the settings.
        /// </summary>
        public KeyValueParserSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }


        /// <summary>
        /// Parses the text checking for multiple lines.
        /// </summary>
        /// <param name="textToParse"></param>
        /// <returns></returns>
        public virtual IList<KeyValueData<string, string>> Parse(string text, IList<string> errors)
        {
            ValidationResults result = Validate(errors, false, text, string.Empty);
            if (!result.IsValid) { return null; }

            StringReader reader = new StringReader(result.Text);
            string currentLine = reader.ReadLine();
            IList<KeyValueData<string, string>> keyValueList = new List<KeyValueData<string, string>>();

            // More to read.
            while (currentLine != null)
            {
                KeyValueData<string, string> pair = ParseLine(currentLine);
                keyValueList.Add(pair);
                currentLine = reader.ReadLine();
            }
            return keyValueList;
        }     


        /// <summary>
        /// Parse the file containing the key-value pairs.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public virtual IList<KeyValueData<string, string>> ParseFile(string filePath, IList<string> errors)
        {
            ValidationResults result = Validate(errors, true, string.Empty, filePath);
            if (!result.IsValid) { return null; }

            // Parse normally.
            return Parse(result.Text, errors);
        }
        

        /// <summary>
        ///  Parses multiple pairs on a single line.
        /// e.g. Location : Country = US, State = New York, City = Queens, Zip = 11375
        /// </summary>
        /// <param name="singleLineText"></param>
        /// <returns></returns>        
        public virtual IList<KeyValueData<string, string>> ParseLineWithMultiPairs(string line)
        {
            // list of key value pairs.
            IList<KeyValueData<string, string>> keyValuePairs = new List<KeyValueData<string, string>>();
            KeyValueData<string, string> keyValuePair = ParseLine(line);

            // Empty ?
            if (keyValuePair == KeyValueData<string, string>.Empty)
            {
                keyValuePairs.Add(keyValuePair);
                return keyValuePairs;
            }

            // If there are not multi-pairs
            if (string.IsNullOrEmpty(keyValuePair.Value)
                 || keyValuePair.Value.IndexOf(_settings.MultiPairDelimeter) == -1)
            {
                keyValuePair = InternalParseLine(keyValuePair.Value, _settings.MultiPairKeyValueDelimeter, true);
                keyValuePairs.Add(keyValuePair);
                return keyValuePairs;
            }

            // Multiple pairs.            
            string[] pairs = keyValuePair.Value.Split(new string[] { _settings.MultiPairDelimeter }, StringSplitOptions.None);
            foreach (string pair in pairs)
            {
                KeyValueData<string, string> keyValue = InternalParseLine(pair, _settings.MultiPairKeyValueDelimeter, true);              
                keyValuePairs.Add(keyValue);
            }
            return keyValuePairs;
        }
        

        /// <summary>
        /// !_tokenLookup.IsCaseSensitive && _settings.ConvertKeysToLowerCase
        /// !_tokenLookup.IsWhiteSpaceSensitive && _settings.TrimWhiteSpaceFromKeys
        /// </summary>
        /// <param name="textToParse"></param>
        /// <param name="delimeter"></param>
        /// <param name="trimKeys"></param>
        /// <param name="getLowerCaseKeys"></param>
        /// <returns></returns>
        public virtual KeyValueData<string, string> ParseLine(string text)
        {
            KeyValueData<string, string> pair = InternalParseLine(text, _settings.KeyValuePairDelimeter, true);
            return pair;            
        }
        #endregion


        protected virtual KeyValueData<string, string> InternalParseLine( string text, string delimeter, bool applySettings)
        {
            // Check for emtpy string
            if (string.IsNullOrEmpty(text)) { return KeyValueData<string, string>.Empty; }
            
            // Now check to see if there is no delmiter or at the beggining.
            int ndxKeyValueSeparator = text.IndexOf(delimeter);
            if (ndxKeyValueSeparator <= 0)
            {
                return new KeyValueData<string, string>(string.Empty, text, true);
            }

            string key = text.Substring(0, ndxKeyValueSeparator);
            string value = text.Substring(ndxKeyValueSeparator + 1);

            KeyValueData<string, string> pair = new KeyValueData<string, string>(key, value, true);
                        
            if (applySettings)
            {
                if (_settings.ConvertKeysToLowerCase) { key = key.ToLower(); }
                if (_settings.TrimWhiteSpaceFromKeys) { key = key.Trim(); }

                pair.SetKey(key);
            }
            return pair;
        }


        /// <summary>
        /// Validates the text or path of the file provided to 
        /// make sure that either the
        /// 1. File exists and contains data.
        /// 2. Text is not empty.
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="isFile"></param>
        /// <param name="text"></param>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        protected ValidationResults Validate(IList<string> errors, bool isFile, string text, string pathToFile)
        {
            // Empty text supplied.
            if (!isFile)
            {
                if (string.IsNullOrEmpty(text))
                {
                    errors.Add("Text supplied containing classes is empty.");
                    return new ValidationResults(false, string.Empty);
                }
                else
                {
                    return new ValidationResults(true, text);
                }
            }

            // File but it doesn't exist.
            if (isFile && !File.Exists(pathToFile))
            {
                errors.Add("File containing classes does not exist.");
                return new ValidationResults(false, string.Empty);
            }

            // Empty text in file ?
            text = File.ReadAllText(pathToFile);
            if (string.IsNullOrEmpty(text))
            {
                errors.Add("File containing classes is empty.");
                return new ValidationResults(false, string.Empty);
            }
            return new ValidationResults(true, text);
        }


        /// <summary>
        /// Validation results.
        /// </summary>
        public class ValidationResults
        {
            public string Text;
            public bool IsValid;


            public ValidationResults(bool isValid, string text)
            {
                Text = text;
                IsValid = isValid;
            }
        }
    }       
}
