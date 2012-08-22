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

namespace ComLib.Cryptography.DES
{
    /// <summary>
    /// Used to determine the length of a DES key.
    /// </summary>
    public enum DesKeyType { 
        /// <summary>
        /// Single-length DES key (16 hexadecimal characters, 8 bytes).
        /// </summary>
        SingleLength,

        /// <summary>
        /// Double-length DES key (32 hexadecimal characters, 16 bytes).
        /// </summary>
        DoubleLength,

        /// <summary>
        /// Triple-length DES key (48 hexadecimal characters, 24 bytes).
        /// </summary>
        TripleLength
    };

}
