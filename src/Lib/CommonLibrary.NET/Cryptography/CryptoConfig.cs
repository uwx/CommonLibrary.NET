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
using System.Security.Cryptography;
using System.Text; 
using System.Configuration;

namespace ComLib.Cryptography
{

    /// <summary>
    /// Settings for the encryption.
    /// </summary>
    public class CryptoConfig
    {
        private bool _encrypt = true;
        private string _internalKey = "keyphrase";

        
        /// <summary>
        /// encryption option
        /// </summary>
        public CryptoConfig()
        {
        }


        /// <summary>
        /// encryption options
        /// </summary>
        /// <param name="encrypt"></param>
        /// <param name="key"></param>
        public CryptoConfig(bool encrypt, string key)
        {
            _encrypt = encrypt;
            _internalKey = key;
        }


        /// <summary>
        /// Whether or not to encrypt;
        /// Primarily used for unit testing.
        /// Default is to encrypt.
        /// </summary>
        public bool Encrypt
        {
            get { return _encrypt; }
        }


        /// <summary>
        /// Key used to encrypt a word.
        /// </summary>
        public string InternalKey
        {
            get { return _internalKey; }
        }
    }
}
