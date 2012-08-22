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
    /// Cryptography interface to encrypt and decrypt strings.
    /// </summary>
    public interface ICrypto
    {
        /// <summary>
        /// Options for encryption.
        /// </summary>
        CryptoConfig Settings { get; }


        /// <summary>
        /// Encrypts a string.
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        string Encrypt(string plaintext);


        /// <summary>
        /// Decrypt the encrypted text.
        /// </summary>
        /// <param name="base64Text">The encrypted base64 text</param>
        /// <returns></returns>
        string Decrypt(string base64Text);


        /// <summary>
        /// Determine if encrypted text can be matched to unencrypted text.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="plainText"></param>
        /// <returns></returns>
        bool IsMatch(string encrypted, string plainText);
    }
}
