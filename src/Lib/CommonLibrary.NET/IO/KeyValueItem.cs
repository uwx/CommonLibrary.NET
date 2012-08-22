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

namespace ComLib.IO
{
    /// <summary>
    /// Class to store the key/values
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    public class KeyValueItem<TKey, TVal> : ICloneable
    {
        private TKey _key;
        private TVal _val;
        private static KeyValueItem<TKey, TVal> _empty;


        static KeyValueItem()
        {
            TKey defaultKey = default(TKey);
            TVal defaultVal = default(TVal);

            _empty = new KeyValueItem<TKey, TVal>(defaultKey, defaultVal);
        }


        /// <summary>
        /// Get the null / empty object.
        /// </summary>
        public static KeyValueItem<TKey, TVal> Empty
        {
            get { return _empty; }
        }


        /// <summary>
        /// Initialize the data with a valid value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="isValueValid"></param>
        public KeyValueItem(TKey key, TVal val)
        {
            _key = key;
            _val = val;
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
        /// Set the key. 
        /// </summary>
        /// <param name="key"></param>
        internal void SetKey(TKey key)
        {
            _key = key;
        }


        /// <summary>
        /// Set the key. 
        /// </summary>
        /// <param name="key"></param>
        internal void SetValue(TVal val)
        {
            _val = val;
        }

        #region ICloneable Members
        /// <summary>
        /// Create shallow copy.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
