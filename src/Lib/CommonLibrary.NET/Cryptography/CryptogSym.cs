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
    /// Cryptography service to encrypt and decrypt strings.
    /// </summary>
    public class CryptoSym : ICrypto
	{
        /// <summary>
        /// Configuration settings for encryption/decryptiong.
        /// </summary>
        protected CryptoConfig _encryptionOptions;


        /// <summary>
        /// The underlying symmetric algorithm to use.
        /// </summary>
        protected SymmetricAlgorithm _algorithm;


        #region Constructors
        /// <summary>
        /// Default options
        /// </summary>
        public CryptoSym()
        {
            _encryptionOptions = new CryptoConfig();
            _algorithm = CryptographyUtils.CreateSymmAlgoTripleDes();
        }


        /// <summary>
        /// Initialize with the encryptionKey
        /// </summary>
        /// <param name="key"></param>
        public CryptoSym(string key)
        {
            _encryptionOptions = new CryptoConfig(true, key);
            _algorithm = CryptographyUtils.CreateSymmAlgoTripleDes();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoSym"/> class.
        /// </summary>
        /// <param name="key">The encryption key to use.</param>
        /// <param name="algorithm">The crypto algorithm to use</param>
        public CryptoSym(string key, SymmetricAlgorithm algorithm)
        {
            _encryptionOptions = new CryptoConfig(true, key);
            _algorithm = algorithm;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoSym"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="algorithm">The crypto algorithm to use</param>
        public CryptoSym(CryptoConfig options, SymmetricAlgorithm algorithm)
        {
            _encryptionOptions = options;
            _algorithm = algorithm;
        }
        #endregion


        /// <summary>
        /// Options for encryption.
        /// </summary>
        /// <value></value>
        public CryptoConfig Settings
        {
            get { return _encryptionOptions; }
        }


        /// <summary>
        /// Set the creator for the algorithm.
        /// </summary>
        /// <param name="algorithm"></param>
        public void SetAlgorithm(SymmetricAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }


		/// <summary>
		/// Encrypts the plaintext using an internal private key.
		/// </summary>
		/// <param name="plaintext">The text to encrypt.</param>
		/// <returns>An encrypted string in base64 format.</returns>
		public virtual string Encrypt( string plaintext )
		{
            if(!_encryptionOptions.Encrypt)
                return plaintext;

            string base64Text = CryptographyUtils.Encrypt(_algorithm, plaintext, _encryptionOptions.InternalKey);
			return base64Text;
		}


		/// <summary>
		/// Decrypts the base64key using an internal private key.
		/// </summary>
		/// <param name="base64Text">The encrypted string in base64 format.</param>
		/// <returns>The plaintext string.</returns>
        public virtual string Decrypt( string base64Text )
		{
            if(!_encryptionOptions.Encrypt)
                return base64Text;

            string plaintext = CryptographyUtils.Decrypt(_algorithm, base64Text, _encryptionOptions.InternalKey);
			return plaintext;
		}


        /// <summary>
        /// Determine if encrypted text can be matched to unencrypted text.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public bool IsMatch(string encrypted, string plainText)
        {
            string decrypted = Decrypt(encrypted);
            return string.Compare(decrypted, plainText, false) == 0;
        }
	}
}
