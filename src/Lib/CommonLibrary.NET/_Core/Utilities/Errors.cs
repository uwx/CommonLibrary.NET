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
using System.Linq;
using System.Text;

namespace ComLib
{
    /// <summary>
    /// Interface for storing messages by both list and key/value.
    /// </summary>
    public interface IMessages
    {
        /// <summary>
        /// Add a message
        /// </summary>
        /// <param name="message">Message to add.</param>
        void Add(string message);


        /// <summary>
        /// Add a message by key/value pair.
        /// </summary>
        /// <param name="key">Key to message.</param>
        /// <param name="message">Message to add.</param>
        void Add(string key, string message);

        
        /// <summary>
        /// Clear all the messages.
        /// </summary>
        void Clear();


        /// <summary>
        /// Copy all the messages to the instance supplied.
        /// </summary>
        /// <param name="messages">Messages to copy.</param>
        void CopyTo(IMessages messages);


        /// <summary>
        /// Get count of all the messages.
        /// </summary>
        int Count { get; }


        /// <summary>
        /// Iterate through all the key/value messages.
        /// </summary>
        /// <param name="callback">Callback to call for each key/value pair.</param>
        void Each(Action<string, string> callback);


        /// <summary>
        /// Iterate through all the messages.
        /// </summary>
        /// <param name="callback">Callback to call for each message.</param>
        void EachFull(Action<string> callback);        


        /// <summary>
        /// Builds a single message of all the messages separated by newline.
        /// </summary>
        /// <returns>String with all messages.</returns>
        string Message();

        
        /// <summary>
        /// Builds a single message of all the messages separated by separator supplied.
        /// </summary>
        /// <param name="separator">Separator to use.</param>
        /// <returns>String with all messages separated.</returns>
        string Message(string separator);

        
        /// <summary>
        /// Whether or not there are any messages in this instance.
        /// </summary>
        bool HasAny { get; }


        /// <summary>
        /// Gets the message associated w/ the specified key.
        /// </summary>
        /// <param name="key">Key to message.</param>
        /// <returns>Message associated with key.</returns>
        string On(string key);


        /// <summary>
        /// Get all the messages.
        /// </summary>
        /// <returns>List of messages.</returns>
        IList<string> On();


        /// <summary>
        /// Internal list of non-key/value based messages.
        /// </summary>
        IList<string> MessageList { get; set; }


        /// <summary>
        /// Internal map of the key/value messages.
        /// </summary>
        IDictionary<string, string> MessageMap { get; set; }
    }



    /// <summary>
    /// Interface for message storage for errors.
    /// </summary>
    public interface IErrors : IMessages
    {
        /// <summary>
        /// Get/set the list of messages.
        /// </summary>
        [Obsolete("Use MessageList")]
        IList<string> ErrorList { get; set; }

        /// <summary>
        /// Get/set the list of mappings.
        /// </summary>
        [Obsolete("Use MessageList")]
        IDictionary<string, string> ErrorMap { get; set; }
    }


    /// <summary>
    /// Class to store messages by key/value and by a simple list of messages.
    /// </summary>
    public class Messages : IMessages
    {
        /// <summary>
        /// The internal map of key/value based messages.
        /// </summary>
        protected IDictionary<string, string> _messageMap;


        /// <summary>
        /// Internal list of messages.
        /// </summary>
        protected IList<string> _messageList;


        /// <summary>
        /// Adds an error associated with the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="error">The error.</param>
        public void Add(string key, string error)
        {
            if (_messageMap == null) _messageMap = new Dictionary<string, string>();

            // No key? add to list.
            if (string.IsNullOrEmpty(key))
                Add(error);
            else
                _messageMap[key] = error;
        }


        /// <summary>
        /// Adds the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        public void Add(string error)
        {
            if (_messageList == null) _messageList = new List<string>();
            _messageList.Add(error);
        }


        /// <summary>
        /// Iterates over the error map and calls the callback
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void Each( Action<string, string> callback)
        {
            if (_messageMap == null) return;

            foreach (KeyValuePair<string, string> pair in _messageMap)
            {
                callback(pair.Key, pair.Value);
            }
        }


        /// <summary>
        /// Iterates over the error map and error list and calls the callback.
        /// Errormap entries are combined.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void EachFull( Action<string> callback)
        {
            if (_messageMap != null)
            {
                foreach (KeyValuePair<string, string> pair in _messageMap)
                {
                    string combined = pair.Key + " " + pair.Value;
                    callback(combined);
                }
            }

            if (_messageList != null)
            {
                foreach (string error in _messageList)
                    callback(error);
            }
        }


        /// <summary>
        /// Builds a full error message of error map and list using NewLine as a separator between errors.
        /// </summary>
        /// <returns>Full error message.</returns>
        public string Message()
        {
            return Message(Environment.NewLine);
        }


        /// <summary>
        /// Builds a complete error message using the supplied separator for each error.
        /// </summary>
        /// <param name="separator">Separator to use.</param>
        /// <returns>Full error message with lines separated.</returns>
        public string Message(string separator)
        {
            StringBuilder buffer = new StringBuilder();
            if (_messageList != null)
            {
                foreach (string error in _messageList)
                    buffer.Append(error + separator);
            } 
            
            if (_messageMap != null)
            {
                foreach (KeyValuePair<string, string> pair in _messageMap)
                {
                    string combined = pair.Key + " " + pair.Value;
                    buffer.Append(combined + separator);
                }
            }
            
            return buffer.ToString();
        }


        /// <summary>
        /// Gets the number of errors.
        /// </summary>
        /// <value>The number of errors.</value>
        public int Count 
        {
            get 
            { 
                int errorCount = 0;
                if (_messageList != null) errorCount += _messageList.Count;
                if (_messageMap != null) errorCount += _messageMap.Count;

                return errorCount;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance has any errors.
        /// </summary>
        /// <value><c>true</c> if this instance has any errors; otherwise, <c>false</c>.</value>
        public bool HasAny
        {
            get { return Count > 0; }
        }


        /// <summary>
        /// Clears all the errors.
        /// </summary>
        public void Clear()
        {
            if (_messageMap != null) _messageMap.Clear();
            if (_messageList != null) _messageList.Clear();
        }


        /// <summary>
        /// Gets the error on the specified error key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Message corresponding to the key.</returns>
        public string On(string key)
        {
            if (_messageMap != null && _messageMap.ContainsKey(key))
                return _messageMap[key];

            return string.Empty;
        }


        /// <summary>
        /// Gets all the errors
        /// </summary>
        /// <returns>List of all errors.</returns>
        public IList<string> On()
        {
            if (_messageList == null) return null;

            // ? Should return a read-only list here?
            return _messageList;
        }


        /// <summary>
        /// Gets or sets the error list.
        /// </summary>
        /// <value>The error list.</value>
        public IList<string> MessageList
        {
            get { return _messageList; }
            set { _messageList = value; }
        }


        /// <summary>
        /// Gets or sets the error map.
        /// </summary>
        /// <value>The error map.</value>
        public IDictionary<string, string> MessageMap
        {
            get { return _messageMap; }
            set { _messageMap = value; }
        }


        /// <summary>
        /// Copies all messages from this instance over to the supplied instance.
        /// </summary>
        /// <param name="messages">The messages.</param>
        public void CopyTo(IMessages messages)
        {
            if (messages == null) return;

            if (_messageList != null)
                foreach (string error in _messageList)
                    messages.Add(error);

            if (_messageMap != null)
                foreach (KeyValuePair<string, string> pair in _messageMap)
                    messages.Add(pair.Key, pair.Value);
        }
    }



    /// <summary>
    /// A message storage class for storing errors by both simple string and by key/value string pairs.
    /// </summary>
    public class Errors : Messages, IErrors
    {
        /// <summary>
        /// Gets or sets the error list.
        /// </summary>
        /// <value>The error list.</value>
        [Obsolete("Use MessageList")]
        public IList<string> ErrorList
        {
            get { return _messageList; }
            set { _messageList = value; }
        }


        /// <summary>
        /// Gets or sets the error map.
        /// </summary>
        /// <value>The error map.</value>
        [Obsolete("Use MessageMap")]
        public IDictionary<string, string> ErrorMap
        {
            get { return _messageMap; }
            set { _messageMap = value; }
        }
    }
}
