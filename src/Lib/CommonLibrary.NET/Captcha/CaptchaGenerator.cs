
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
using System.Web;

using ComLib;
using ComLib.Cryptography;


namespace ComLib.CaptchaSupport
{
    /// <summary>
    /// This class implements a Captcha generator 
    /// for web usage.
    /// </summary>
    public class CaptchaWeb : CaptchaGenerator, ICaptcha
    {
        private string _hiddenFieldName;
        private string _userInputFieldName;
        private string _urlParam;


        /// <summary>
        /// Initialize.
        /// </summary>
        public CaptchaWeb()
            : this("CaptchaGeneratedText", "CaptchaUserInput", "CaptchaText")
        {
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="hiddenFieldName"></param>
        /// <param name="userInputTextFieldName"></param>
        /// <param name="urlParam"></param>
        public CaptchaWeb(string hiddenFieldName, string userInputTextFieldName, string urlParam)
        {
            _hiddenFieldName = hiddenFieldName;
            _userInputFieldName = userInputTextFieldName;
            _urlParam = urlParam;
        }


        /// <summary>
        /// Generate a new Bitmap.
        /// </summary>
        /// <returns></returns>
        public Bitmap Generate()
        {
            string text = GetRandomText();
            return Generate(text);
        }


        /// <summary>
        /// Generate a new BitMap using the encoded random text supplied in the url.
        /// </summary>
        /// <returns></returns>
        public Bitmap GenerateFromUrl()
        {
            string encodedRandomText = HttpContext.Current.Request.Params[_urlParam];
            string randomText = Crypto.Decrypt(encodedRandomText);
            return Generate(randomText);
        }


        /// <summary>
        /// Get random text.
        /// </summary>
        /// <returns></returns>
        public string GetRandomText()
        {
            IRandomTextGenerator random = new RandomTextGenerator();
            random.Settings.Length = Settings.NumChars;
            string text = random.Generate();
            return text;
        }


        /// <summary>
        /// Get the random encoded text.
        /// </summary>
        /// <returns></returns>
        public string GetRandomTextEncoded()
        {
            string text = GetRandomText();
            string encoded = Crypto.Encrypt(text);
            return encoded;
        }


        /// <summary>
        /// Determine whether the captca image is correct based on the 
        /// 1. user input text
        /// 2. hidden encoded captcha text.
        /// </summary>
        /// <returns></returns>
        public bool IsCorrect()
        {
            //HttpContext.Current.Cache.Insert("", "", null, 
            // Get the form user input.
            string userInput = HttpContext.Current.Request.Params[_userInputFieldName];
            return IsCorrect(userInput);
        }


        /// <summary>
        /// Determine whether the captca image is correct based on the 
        /// 1. user input text
        /// 2. hidden encoded captcha text.
        /// </summary>
        /// <returns></returns>
        public bool IsCorrect(string userInput)
        {
            string encodedText = HttpContext.Current.Request.Params[_hiddenFieldName];
            string captchText = Crypto.Decrypt(encodedText);
            bool isMatch = string.Compare(captchText, userInput, !Settings.IsCaseSensitive) == 0;
            return isMatch;
        }


        /// <summary>
        /// Determine whether the captca image is correct based on the 
        /// 1. user input text
        /// 2. hidden encoded captcha text.
        /// </summary>
        /// <returns></returns>
        public bool IsCorrect(string userInput, string encodedText)
        {
            string captchText = Crypto.Decrypt(encodedText);
            bool isMatch = string.Compare(captchText, userInput, !Settings.IsCaseSensitive) == 0;
            return isMatch;
        }
    }



    /// <summary>
    /// Generates an Captcha image.
    /// Credit: http://www.brainjar.com/dotNet/CaptchaImage/
    /// </summary>
    public class CaptchaGenerator : ICaptchaGenerator
    {
        private Random _random = new Random();
        

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaGenerator"/> class.
        /// </summary>
        public CaptchaGenerator()
        {
            Settings = new CaptchaSettings();
            Settings.Width = 200;
            Settings.Height = 50;
            Settings.Font = "Arial";
            Settings.NumChars = 6;
        }


        #region ICaptchaImageGenerator Members
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public CaptchaSettings Settings { get; set; }



        /// <summary>
        /// Generates the bitmap using a random text using the random text generator.
        /// </summary>
        /// <returns></returns>
        public Bitmap Generate(string randomText)
        {
            // Check.
            Guard.IsTrue(Settings.Width > 0, "Width of captcha must be greater than 0");
            Guard.IsTrue(Settings.Height > 0, "Height of captch must be greater than 0");
            
            // Get the settings to local variables.
            int minWidth = Settings.Width;
            int minHeight = Settings.Height;
            string fontName = Settings.Font;

            // Create instance of bitmap object
            Bitmap bitmap = new Bitmap(minWidth, minHeight, PixelFormat.Format32bppArgb);

            // Create instance of graphics object
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectangle = new Rectangle(0, 0, minWidth, minHeight);

            // Fill the background in a light gray pattern
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.DiagonalCross, Color.LightGray, Color.White);
            graphics.FillRectangle(hatchBrush, rectangle);

            // At this point need to figure out fontsize.
            SizeF sizeF;
            float fontSize = rectangle.Height + 1;
            Font font;            
            do	
            {
                // Adjust font size since it needs to fit on screen.
                fontSize--;
                font = new Font(fontName, fontSize, FontStyle.Bold);
                sizeF = graphics.MeasureString(randomText, font);
            } while ( sizeF.Width > rectangle.Width );

            // Format the text
            StringFormat objStringFormat = new StringFormat();
            objStringFormat.Alignment = StringAlignment.Center;
            objStringFormat.LineAlignment = StringAlignment.Center;

            // Create a path using the text and randomly warp it
            GraphicsPath objGraphicsPath = new GraphicsPath();
            objGraphicsPath.AddString(randomText, font.FontFamily, (int)font.Style, font.Size, rectangle, objStringFormat);
            float flV = 4F;

            // Create a parallelogram for the text to draw into
            PointF[] arrPoints =
			{
				new PointF(_random.Next(rectangle.Width) / flV, _random.Next(rectangle.Height) / flV),
				new PointF(rectangle.Width - _random.Next(rectangle.Width) / flV, _random.Next(rectangle.Height) / flV),
				new PointF(_random.Next(rectangle.Width) / flV, rectangle.Height - _random.Next(rectangle.Height) / flV),
				new PointF(rectangle.Width - _random.Next(rectangle.Width) / flV, rectangle.Height - _random.Next(rectangle.Height) / flV)
			};

            // Create the warped parallelogram for the text
            Matrix objMatrix = new Matrix();
            objMatrix.Translate(0F, 0F);
            objGraphicsPath.Warp(arrPoints, rectangle, objMatrix, WarpMode.Perspective, 0F);

            // Add text
            hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.DarkGray, Color.Black);
            graphics.FillPath(hatchBrush, objGraphicsPath);

            // Add noise
            int intMax = Math.Max(rectangle.Width, rectangle.Height);
            int total = (int)(rectangle.Width * rectangle.Height / 30F);

            for (int i = 0; i < total; i++)
            {
                int x = _random.Next(rectangle.Width);
                int y = _random.Next(rectangle.Height);
                int w = _random.Next(intMax / 15);
                int h = _random.Next(intMax / 70);
                graphics.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Dispose the graphics objects.
            font.Dispose();
            hatchBrush.Dispose();
            graphics.Dispose();
            
            return bitmap;
        }
        #endregion
    }
}
