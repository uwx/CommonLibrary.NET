
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
using System.Security.Principal;


namespace ComLib.Authentication
{

    /// <summary>
    /// Custom Identity class
    /// </summary>
    public class UserIdentity : IIdentity
    {
        private string _userName;
        private string _authenticationType;
        private bool _isAuthenticated = false;
        private int _id;


        /// <summary>
        /// Create new instance using default initialization,
        /// authenticated = false, username = string.Empty
        /// </summary>
        public UserIdentity() { }


        /// <summary>
        /// Create new instance using supplied values.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="authenticationType"></param>
        /// <param name="isAuthenticated"></param>
        public UserIdentity(int id, string userName, string authenticationType, bool isAuthenticated)
        {
            _id = id;
            _isAuthenticated = isAuthenticated;
            _userName = userName;
            _authenticationType = authenticationType;
        }


        #region IIdentity Members
        /// <summary>
        /// Get the authentication type.
        /// </summary>
        public string AuthenticationType
        {
            get { return _authenticationType; }
        }


        /// <summary>
        /// Indicates if user is authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
        }


        /// <summary>
        /// Return the username.
        /// </summary>
        public string Name
        {
            get { return _userName; }
        }


        /// <summary>
        /// Get the user id.
        /// </summary>
        public int UserId
        {
            get { return _id; }
        }
        #endregion
    }
}
