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
using System.Drawing;


namespace ComLib.CaptchaSupport
{
    /// <summary>
    /// This interface should be implemented by classes that
    /// provide Captcha generation functionality.
    /// </summary>
    public interface ICaptchaGenerator
    {
        /// <summary>
        /// Generate a Captcha image based on supplied text.
        /// </summary>
        /// <param name="randomText"></param>
        /// <returns></returns>
        System.Drawing.Bitmap Generate(string randomText);


        /// <summary>
        /// Get/set the Captcha settings.
        /// </summary>
        CaptchaSettings Settings { get; set; }
    }


    /// <summary>
    /// This interface should be implemented by classes
    /// that aim to provide Captcha generation and
    /// verification functionality. In essence, this
    /// interface defines a Captcha generation provider.
    /// </summary>
    public interface ICaptcha : ICaptchaGenerator
    {
        /// <summary>
        /// Determines if the current Captcha is correct.
        /// </summary>
        /// <returns></returns>
        bool IsCorrect();


        /// <summary>
        /// Determines if the current Captcah is correct.
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        bool IsCorrect(string userInput);


        /// <summary>
        /// Determines if the current Captcha is correct.
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="encodedText"></param>
        /// <returns></returns>
        bool IsCorrect(string userInput, string encodedText);


        /// <summary>
        /// Generates a new Captcha image.
        /// </summary>
        /// <returns></returns>
        Bitmap Generate();


        /// <summary>
        /// Generates a new Captcha image.
        /// </summary>
        /// <returns></returns>
        Bitmap GenerateFromUrl();


        /// <summary>
        /// Returns random text.
        /// </summary>
        /// <returns></returns>
        string GetRandomText();


        /// <summary>
        /// Returns random encoded text.
        /// </summary>
        /// <returns></returns>
        string GetRandomTextEncoded();
    }


    /// <summary>
    /// This class provides a default Captcha implementation
    /// for web usage.
    /// </summary>
    public class Captcha
    {
        private static ICaptcha _captcha = new CaptchaWeb();


        /// <summary>
        /// Initialize captcha provider.
        /// </summary>
        /// <param name="captcha"></param>
        public static void Init(ICaptcha captcha)
        {
            _captcha = captcha;
        }


        /// <summary>
        /// Get random text.
        /// </summary>
        /// <returns></returns>
        public static string GetRandomText()
        {
            return _captcha.GetRandomText();
        }


        /// <summary>
        /// Get encoded random text.
        /// </summary>
        /// <returns></returns>
        public static string GetRandomTextEncoded()
        {
            return _captcha.GetRandomTextEncoded();
        }


        /// <summary>
        /// Determine if the current captcha is correct.
        /// </summary>
        /// <returns></returns>
        public static bool IsCorrect()
        {
            return _captcha.IsCorrect();
        }


        /// <summary>
        /// Determine if the current captcha is correct.
        /// </summary>
        /// <returns></returns>
        public static bool IsCorrect(string userInput)
        {
            return _captcha.IsCorrect(userInput);
        }


        /// <summary>
        /// Determine if the current captcha is correct.
        /// </summary>
        /// <returns></returns>
        public static bool IsCorrect(string userInput, string encodedInput)
        {
            return _captcha.IsCorrect(userInput, encodedInput);
        }


        /// <summary>
        /// Create a new Captcha image.
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap Generate()
        {
            return _captcha.Generate();
        }


        /// <summary>
        /// Create a new Captcha image.
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Bitmap GenerateFromUrl()
        {
            return _captcha.GenerateFromUrl();
        }

        
        /// <summary>
        /// Create a new captcha image using the random text supplied.
        /// </summary>
        /// <param name="randomText"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap Generate(string randomText)
        {
            return _captcha.Generate(randomText);
        }
    }
}
