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
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using ComLib.Cryptography;


namespace ComLib.Web.Services.GravatarSupport
{
    /// <summary>
    /// Rating to use for the gravatar
    /// </summary>
    public enum Rating { 
        /// <summary>
        /// All ages.
        /// </summary>
        g, 
        
        
        /// <summary>
        /// Parental guidance suggested.
        /// </summary>
        pg, 
        
        
        /// <summary>
        /// Restricted.
        /// </summary>
        r, 
        
        
        /// <summary>
        /// No one under age of consent.
        /// </summary>
        x };
    

    /// <summary>
    /// Icon Type to use for the gravatar
    /// </summary>
    public enum IconType { 
        /// <summary>
        /// No type.
        /// </summary>
        none, 
        
        
        /// <summary>
        /// Visual representation of unique value.
        /// </summary>
        identicon, 
        
        
        /// <summary>
        /// Unique monster avatar.
        /// </summary>
        monsterid, 
        
        
        /// <summary>
        /// Generated avatar.
        /// </summary>
        wavatar };
    

    /// <summary>
    /// Twitter class for getting tweets
    /// </summary>
    public class Gravatar
    {
        private string URL_WITH_ICON    = "http://www.gravatar.com/avatar/{0}{1}?d={2}&s={3}&r={4}";
        private string URL_WITHOUT_ICON = "http://www.gravatar.com/avatar/{0}{1}?s={2}&r={3}";


        /// <summary>
        /// Initializes a new instance of the <see cref="Gravatar"/> class.
        /// </summary>
        public Gravatar()
        {
            Size = 80;
            Rating = Rating.g;
            IconType = IconType.none;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Gravatar"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="size">The size.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="iconType">Type of the icon.</param>
        /// <param name="imageExtension">The extension of the image.</param>
        public Gravatar(string email, int size, Rating rating, IconType iconType, string imageExtension)
        {
            Init(email, size, rating, iconType, imageExtension);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Gravatar"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="size">The size.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="iconType">Type of the icon.</param>
        /// <param name="imageExtension">The extension of the image.</param>
        public Gravatar(string email, int size, string rating, string iconType, string imageExtension)
        {
            Rating rated = string.IsNullOrEmpty(rating) ? Rating.g : (Rating)Enum.Parse(typeof(Rating), rating, true);
            IconType icon = string.IsNullOrEmpty(iconType) ? IconType.none : (IconType)Enum.Parse(typeof(IconType), iconType, true);
            Init(email, size, rated, icon, imageExtension);
        }


        /// <summary>
        /// Inits the variables with the values supplied.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="size">The size.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="iconType">Type of the icon.</param>
        /// <param name="imageExtension">The extension of the image.</param>
        public void Init(string email, int size, Rating rating, IconType iconType, string imageExtension)
        {
            Email = email;
            Size = size;
            Rating = rating;
            IconType = iconType;
            ImageExtension = imageExtension;
        }


        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }


        /// <summary>
        /// extension used for image. e.g. jpg or png if applicable.
        /// </summary>
        public string ImageExtension { get; set; }


        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; set; }


        /// <summary>
        /// Rating can either be g, pg, r, x
        /// </summary>
        public Rating Rating { get; set; }


        /// <summary>
        /// Gets or sets the type of the icon.
        /// </summary>
        /// <value>The type of the icon.</value>
        public IconType IconType { get; set; }


        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url
        {
            get
            {
                // Validate the size.
                if (Size <= 0) Size = 80;

                // 1. Hash the email
                string emailHash = Crypto.ToMD5Hash(Email.ToLower());
                emailHash = emailHash.ToLower();
               
                // 2. Format the url
                string finalUrl = string.Empty;
                if (IconType != IconType.none)
                {
                    finalUrl = string.Format(URL_WITH_ICON, emailHash, ImageExtension, IconType.ToString(), Size, Rating.ToString());
                }
                else
                {
                    finalUrl = string.Format(URL_WITHOUT_ICON, emailHash, ImageExtension, Size, Rating.ToString());
                }
                return finalUrl;
            }
        }
    }
}
