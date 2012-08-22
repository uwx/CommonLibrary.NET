
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
using System.Security.Cryptography;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;


namespace ComLib.CaptchaSupport
{
    
    /// <summary>
    /// Generates random text.
    /// </summary>
    public class RandomTextGenerator : IRandomTextGenerator
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomTextGenerator"/> class.
        /// </summary>
        public RandomTextGenerator()
        {
            Settings = new RandomTextGeneratorSettings();
            Settings.Length = 5;
            Settings.AllowedChars = "ABCDEFGHJKLMNPRSTUVWXY3456789";
        }


        #region IRandomTextGenerator Members
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public RandomTextGeneratorSettings Settings { get; set; }


        /// <summary>
        /// Generate the random text.
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            Byte[] randomBytes = new Byte[Settings.Length];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            char[] chars = new char[Settings.Length];
            int allowedCharCount = Settings.AllowedChars.Length;
            string allowedChars = Settings.AllowedChars;

            for (int i = 0; i < Settings.Length; i++)
            {
                chars[i] = allowedChars[(int)randomBytes[i] % allowedCharCount];
            }

            return new string(chars);
        }
        #endregion
    }
}
