
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
using System.Security.Principal;


namespace ComLib.Authentication
{

    /// <summary>
    /// Interface for Authentication.
    /// </summary>
    public interface IAuth
    {
        /// <summary>
        /// Determines whether the user is authenticted.
        /// </summary>
        /// <returns></returns>
        bool IsAuthenticated();


        /// <summary>
        /// Return whether or not the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        bool IsGuest();


        /// <summary>
        /// User's Id - If applicable.
        /// </summary>
        int UserId { get; }


        /// <summary>
        /// The name of the current user.
        /// </summary>
        string UserName { get; }


        /// <summary>
        /// Provides just the username if the username contains
        /// the domain.
        /// e.g. returns "john" if username is "mydomain\john"
        /// </summary>
        string UserShortName { get; }


        /// <summary>
        /// Get the current user.
        /// </summary>
        IPrincipal User { get; }

        
        /// <summary>
        /// Get the user principal and cast it to an implementation class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userName"></param>
        /// <returns></returns>
        T GetUser<T>(string userName) where T : class, IPrincipal;


        /// <summary>
        /// Is User in the selected roles.
        /// </summary>
        /// <param name="rolesDelimited"></param>
        /// <returns></returns>
        bool IsUserInRoles(string rolesDelimited);


        /// <summary>
        /// Determine whether or not user is an admin.
        /// </summary>
        /// <returns></returns>
        bool IsAdmin();


        /// <summary>
        /// Determine if the logged in user is the same as the username supplied.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool IsUser(string username);


        /// <summary>
        /// Returns true if the logged in user is the same as the username supplied,
        /// or if the logged in user is an admin.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool IsUserOrAdmin(string username);

     
        /// <summary>
        /// Sign the user in.
        /// </summary>
        /// <param name="user"></param>
        void SignIn(IPrincipal user);


        /// <summary>
        /// Sign the user in via username.
        /// </summary>
        /// <param name="user"></param>
        void SignIn(string user);


        /// <summary>
        /// Sign the user in via username and remember the user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="rememberUser"></param>
        void SignIn(string user, bool rememberUser);


        /// <summary>
        /// Signout the user.
        /// </summary>
        void SignOut();
    }
}
