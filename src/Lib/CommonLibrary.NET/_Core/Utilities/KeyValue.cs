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
    /// KeyValue pair. This is a class not a struct like the KeyValuePair.
    /// Unlike Tuple, this has Key/Value fields instead of properties that can be modified.
    /// Also,
    /// </summary>
    public class KeyValue<TKey, TValue>
    {
        /// <summary>
        /// The key.
        /// </summary>
        public TKey Key { get; set;}


        /// <summary>
        /// The value.
        /// </summary>
        public TValue Value { get; set; }


        /// <summary>
        /// Initialize
        /// </summary>
        public KeyValue()
        {
        }


        /// <summary>
        /// Initialize with supplied values.
        /// </summary>
        /// <param name="key">Value of key.</param>
        /// <param name="val">Value corresponding to key.</param>
        public KeyValue(TKey key, TValue val)
        {
            Key = key;
            Value = val;
        }


        /// <summary>
        /// Get string representation.
        /// </summary>
        /// <returns>String representation of this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", Key, Value);
        }
    }
}
