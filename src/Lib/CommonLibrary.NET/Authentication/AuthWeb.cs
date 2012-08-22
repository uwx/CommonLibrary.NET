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
using System.Web;
using System.Security.Principal;
using System.Web.Security;




namespace ComLib.Authentication
{
    /// <summary>
    /// <see cref="IAuth"/> implementation to provide Authentication service using the web based User(principal) object exposed in the context.Current.User object.
    /// </summary>
    public class AuthWeb : AuthBase, IAuth
    {
        private Func<string, IPrincipal> _userAuthenticationService = null;


        /// <summary>
        /// Default constructor.
        /// </summary>
        public AuthWeb() { }


        /// <summary>
        /// Initialize with the admin role name.
        /// </summary>
        /// <param name="adminRoleName"></param>
        public AuthWeb(string adminRoleName)
        {
            _adminRoleName = adminRoleName;
        }


        /// <summary>
        /// Initialize with the admin role name.
        /// </summary>
        /// <param name="adminRoleName"></param>
        /// <param name="userAuth"></param>
        public AuthWeb(string adminRoleName, Func<string, IPrincipal> userAuth)
        {
            _adminRoleName = adminRoleName;
            _userAuthenticationService = userAuth;
        }


        /// <summary>
        /// Get the current user.
        /// </summary>
        public override IPrincipal User
        {
            get { return HttpContext.Current.User; }
        }


        /// <summary>
        /// Get the user data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userName"></param>
        /// <returns></returns>
        public T GetUser<T>(string userName) where T : class, IPrincipal
        {
            if (_userAuthenticationService == null)
            {
               throw new ArgumentException("UserAuthenticationService is not initialized.");
            }
            
            return _userAuthenticationService(userName) as T;
        }


        /// <summary>
        /// The name of the current user.
        /// </summary>
        public override string UserName
        {
            get 
            {
                if (!IsAuthenticated())
                    return string.Empty;

                return HttpContext.Current.User.Identity.Name; 
            }
        }


        /// <summary>
        /// Provides just the username if the username contains
        /// the domain.
        /// e.g. returns "john" if username is "mydomain\john"
        /// </summary>
        public override string UserShortName 
        {
            get
            {
                if (!IsAuthenticated())
                    return string.Empty;

                string fullName = HttpContext.Current.User.Identity.Name;
                int ndxSlash = fullName.LastIndexOf(@"\");
                if (ndxSlash == -1)
                    ndxSlash = fullName.LastIndexOf("/");

                if (ndxSlash == -1)
                    return fullName;

                return fullName.Substring(ndxSlash + 1);
            }
        }


        /// <summary>
        /// Determine if the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        public override bool IsAuthenticated()
        {
            if (HttpContext.Current == null) return false;
            if (HttpContext.Current.User == null) return false;

            return HttpContext.Current.User.Identity.IsAuthenticated;
        }      


        /// <summary>
        /// Determine if currently logged in user is an administrator.
        /// </summary>
        /// <returns></returns>
        public override bool IsAdmin()
        {
            if (! IsAuthenticated()) return false;
            if (Auth.User is UserPrincipal)
                return ((UserPrincipal)User).IsInRole(_adminRoleName);

            return Roles.IsUserInRole(_adminRoleName);
        }


        /// <summary>
        /// Is User in the selected roles.
        /// </summary>
        /// <param name="rolesDelimited"></param>
        /// <returns></returns>
        public override bool IsUserInRoles(string rolesDelimited)
        {
            if (string.IsNullOrEmpty(rolesDelimited)) return true;            
            if (!IsAuthenticated() ) return false;
            if (Auth.User is UserPrincipal)
                return ((UserPrincipal)User).IsInRole(rolesDelimited);

            return Roles.IsUserInRole(rolesDelimited);
        }


        /// <summary>
        /// Sign the user in.
        /// </summary>
        /// <param name="user"></param>
        public void SignIn(IPrincipal user)
        {
            FormsAuthentication.SetAuthCookie(user.Identity.Name, true);
        }


        /// <summary>
        /// Sign the user in.
        /// </summary>
        /// <param name="user"></param>
        public void SignIn(string user)
        {
            FormsAuthentication.SetAuthCookie(user, true);
        }


        /// <summary>
        /// Sign the user in via username.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rememberUser"></param>
        public void SignIn(string user, bool rememberUser)
        {
            FormsAuthentication.SetAuthCookie(user, rememberUser);
        }


        /// <summary>
        /// Signout the user.
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
