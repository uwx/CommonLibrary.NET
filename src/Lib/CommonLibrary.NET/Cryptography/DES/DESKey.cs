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
using ComLib.Extensions;

namespace ComLib.Cryptography.DES
{
    /// <summary>
    /// This class is used to represent a hexadecimal single, double or 
    /// triple length DES key.
    /// </summary>
    public class DESKey
    {
        /// <summary>
        /// First part of key.
        /// </summary>
        private string _keyA;


        /// <summary>
        /// Second part of key.
        /// </summary>
        private string _keyB;
        

        /// <summary>
        /// Third part of key.
        /// </summary>
        private string _keyC;


        /// <summary>
        /// Key length.
        /// </summary>
        private DesKeyType _keyType;


        /// <summary>
        /// Returns the first key part.
        /// </summary>
        public String FirstKeyPart
        {
            get { return _keyA; }
        }


        /// <summary>
        /// Returns the second key part.
        /// </summary>
        public String SecondKeyPart
        {
            get { return _keyB; }
        }


        /// <summary>
        /// Returns the third key part.
        /// </summary>
        public String ThirdKeyPart
        {
            get { return _keyC; }
        }


        /// <summary>
        /// Creates a new random DES key.
        /// </summary>
        /// <param name="type">The DES key type (single, double or triple length).</param>
        public DESKey (DesKeyType type)
        {
            switch (type)
            {
                case DesKeyType.SingleLength:
                    Init(GetRandomHex());
                    break;
                case DesKeyType.DoubleLength:
                    Init(GetRandomHex() + GetRandomHex());
                    break;
                default:
                    Init(GetRandomHex() + GetRandomHex() + GetRandomHex());
                    break;
            }
        }


        /// <summary>
        /// Creates a new DES key from hexadecimal characters.
        /// </summary>
        /// <param name="text">The hexadecimal representation of the DES key.</param>
        public DESKey(string text)
        {
            Init(text);
        }


        /// <summary>
        /// Initializes with a hexadecimal DES key.
        /// </summary>
        /// <param name="text">Hexadecimal representation of the DES key.</param>
        private void Init (string text)
        {
            Guard.IsTrue(text.IsHex(), "Key is not hexadecimal.");
            Guard.IsOneOfSupplied<int>(text.Length, new List<int>() { 16, 32, 48 }, "Key must be 16, 32 or 48 hexadecimal characters");

            text = text.ToUpper();
            switch (text.Length)
            {
                case 16:
                    _keyA = text;
                    _keyB = _keyA;
                    _keyC = _keyA;
                    _keyType = DesKeyType.SingleLength;
                    break;
                case 32:
                    _keyA = text.Substring(0, 16);
                    _keyB = text.Substring(16, 16);
                    _keyC = _keyA;
                    _keyType = DesKeyType.DoubleLength;
                    break;
                default:
                    _keyA = text.Substring(0, 16);
                    _keyB = text.Substring(16, 16);
                    _keyC = text.Substring(32, 16);
                    _keyType = DesKeyType.TripleLength;
                    break;
            }
        }

        /// <summary>
        /// Returns the current DES key in hexadecimal form.
        /// </summary>
        /// <returns>A string representation of the DES key.</returns>
        public override string ToString()
        {
            if (DesKeyType.SingleLength == _keyType)
                return _keyA;
            else
                if (DesKeyType.DoubleLength == _keyType)
                    return _keyA + _keyB;
                else
                    return (_keyA + _keyB + _keyC);
        }


        /// <summary>
        /// Creates a random single-length hexadecimal key.
        /// </summary>
        /// <returns>A single-length hexadecimal key.</returns>
        private string GetRandomHex()
        {
            Random rnd = new Random();
            long randomNumber = rnd.Next() + rnd.Next() * 256;
            return randomNumber.ToString().DecimalToHex();
        }
    }
}
