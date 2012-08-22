/*
 * Author: Nick Bitounis
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
using System.IO;
using System.Security.Cryptography;
using ComLib.Extensions;

namespace ComLib.Cryptography.DES
{
    /// <summary>
    /// This class implements Triple DES encrypt and decrypt methods
    /// that operate on hexadecimal data and keys. Data to be encrypted
    /// or decrypted should be exactly 16 hexadecimal characters.
    /// </summary>
    public class TripleDES
    {
        /// <summary>
        /// Encrypts hexadecimal data using a DES key.
        /// </summary>
        /// <param name="key">DES key to use with the encrypt operation.</param>
        /// <param name="data">Hexadecimal data to encrypt.</param>
        /// <returns>Encrypted hexadecimal data.</returns>
        public static string Encrypt (DESKey key, string data)
        {
            return DESEncrypt(key.ThirdKeyPart.ToString().HexToByteArray(),
                        DESDecrypt(key.SecondKeyPart.ToString().HexToByteArray(),
                            DESEncrypt(key.FirstKeyPart.ToString().HexToByteArray(), data.HexToByteArray()))
                            ).ByteArrayToHex();
        }


        /// <summary>
        /// Decrypts hexadecimal data using a DES key.
        /// </summary>
        /// <param name="key">DES key to use with the decrypt operation.</param>
        /// <param name="data">Hexadecimal data to decrypt.</param>
        /// <returns>Decrypted hexadecimal data.</returns>
        public static string Decrypt(DESKey key, string data)
        {
            return DESDecrypt(key.FirstKeyPart.ToString().HexToByteArray(),
                        DESEncrypt(key.SecondKeyPart.ToString().HexToByteArray(),
                            DESDecrypt(key.ThirdKeyPart.ToString().HexToByteArray(), data.HexToByteArray()))
                            ).ByteArrayToHex();
        }


        /// <summary>
        /// Encrypts hexadecimal data using a single-length DES key.
        /// </summary>
        /// <param name="key">DES key to use with the encrypt operation.</param>
        /// <param name="data">Hexadecimal data to encrypt.</param>
        /// <returns>Encrypted hexadecimal data.</returns>
        private static string DESEncrypt(DESKey key, string data)
        {
            return DESEncrypt(key.FirstKeyPart.ToString().HexToByteArray(), data.HexToByteArray()).ByteArrayToHex();
        }


        /// <summary>
        /// Decrypts hexadecimal data using a single-length DES key.
        /// </summary>
        /// <param name="key">DES key to use with the decrypt operation.</param>
        /// <param name="data">Hexadecimal data to decrypt.</param>
        /// <returns>Decrypted hexadecimal data.</returns>
        private static string DESDecrypt(DESKey key, String data)
        {
            return DESDecrypt(key.FirstKeyPart.ToString().HexToByteArray(), data.HexToByteArray()).ByteArrayToHex();
        }

        /// <summary>
        /// Encrypts 8 bytes of data with an 8-byte key, ECB mode, no padding, no IV.
        /// </summary>
        /// <param name="key">8-byte DES key to use in the encrypt operation.</param>
        /// <param name="data">8-byte data to encrypt.</param>
        /// <returns>Encrypted data.</returns>
        private static byte[] DESEncrypt(byte[] key, byte[] data)
        {
            System.Diagnostics.Debug.WriteLine("Encrypt [" + data.ByteArrayToHex() + "] using [" + key.ByteArrayToHex() + "] = [" + DESOperation(key, data, true).ByteArrayToHex() + "].");
            return DESOperation(key, data, true);
        }


        /// <summary>
        /// Decrypts 8 bytes of data with an 8-byte key, ECB mode, no padding, no IV.
        /// </summary>
        /// <param name="key">8-byte DES key to use in the decrypt operation.</param>
        /// <param name="data">8-byte data to decrypt.</param>
        /// <returns>Decrypted data.</returns>
        private static byte[] DESDecrypt(byte[] key, byte[] data)
        {
            System.Diagnostics.Debug.WriteLine("Decrypt [" + data.ByteArrayToHex() + "] using [" + key.ByteArrayToHex() + "] = [" + DESOperation(key, data, true).ByteArrayToHex() + "].");
            return DESOperation(key, data, false);
        }


        /// <summary>
        /// Encrypts or decrypts 8 bytes of data using an 8-byte key, ECB mode, no padding, no IV.
        /// </summary>
        /// <param name="key">8-byte DES key to use with the operation.</param>
        /// <param name="data">8-byte data to use with the operation.</param>
        /// <param name="encrypt">True to encrypt, false to decrypt.</param>
        /// <returns>Result of the operation.</returns>
        private static byte[] DESOperation(byte[] key, byte[] data, bool encrypt)
        {
            byte[] result = { 0, 0, 0, 0, 0, 0, 0, 0 };
            using (MemoryStream outStream = new MemoryStream(result))
            {
                using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
                {
                    desProvider.Mode = CipherMode.ECB;
                    desProvider.Key = key;
                    desProvider.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    desProvider.Padding = PaddingMode.None;

                    ICryptoTransform transform;
                    if (true == encrypt)
                        transform = desProvider.CreateEncryptor();
                    else
                        transform = desProvider.CreateDecryptor();

                    using (CryptoStream cs = new CryptoStream(outStream, transform, CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, 8);
                        cs.Close();
                    }
                }
            }
            return result;
        }

    }
}
