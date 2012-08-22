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
using System.Web.Security;


namespace ComLib.Account
{
    /// <summary>
    /// This interface should be implemented by
    /// classes that provide a user account service.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Approve user.
        /// </summary>
        /// <param name="userName">User to approve.</param>
        /// <returns>Result of operation.</returns>
        BoolMessage Approve(string userName);


        /// <summary>
        /// Create the user and also return the membership status
        /// </summary>
        /// <param name="user">kishore</param>
        /// <param name="password">password</param>
        /// <param name="status">DuplicateUserName</param>
        /// <returns>Result of user creation.</returns>
        BoolMessage Create(User user, string password, ref MembershipCreateStatus status);


        /// <summary>
        /// Register the user.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="email">E-mail of user.</param>
        /// <param name="password">User of password.</param>
        /// <param name="roles">Roles to assign to the user.</param>
        /// <returns>Result of user registration.</returns>
        BoolMessageItem<User> Create(string userName, string email, string password, string roles);


        /// <summary>
        /// Register the user.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="email">E-mail of user.</param>
        /// <param name="password">User of password.</param>
        /// <param name="roles">Roles to assign to the user.</param>
        /// <param name="status">Status of membership creation.</param>
        /// <returns>Result of user registration.</returns>
        BoolMessageItem<User> Create(string userName, string email, string password, string roles, ref MembershipCreateStatus status);


        /// <summary>
        /// Try logging in to server.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="password">User password.</param>
        /// <param name="rememberUser">True to remember the user.</param>
        /// <returns>Result of logon attempt.</returns>
        BoolMessage LogOn(string userName, string password, bool rememberUser);


        /// <summary>
        /// Verify that this is a valid user.
        /// </summary>
        /// <param name="userName">"kishore"</param>
        /// <param name="password">"password"</param>
        /// <returns>Result of verification.</returns>
        BoolMessageItem<User> VerifyUser(string userName, string password);


        /// <summary>
        /// Get the specified user by username.
        /// </summary>
        /// <param name="userName">User to get.</param>
        /// <returns>Instance of user corresponding to user name.</returns>
        User Get(string userName);


        /// <summary>
        /// Change the current password.
        /// </summary>
        /// <param name="userName">username of the account for which the password is being changed.</param>
        /// <param name="currentPassword">Existing password on file.</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Result of password change.</returns>
        BoolMessage ChangePassword(string userName, string currentPassword, string newPassword);


        /// <summary>
        /// Lock out the specified user.
        /// </summary>
        /// <param name="userName">User to lock out.</param>
        /// <param name="lockOutReason">Reason for locking out.</param>
        void LockOut(string userName, string lockOutReason);


        /// <summary>
        /// UndoLock out the specified user.
        /// </summary>
        /// <param name="userName">User to undo lock out for.</param>
        /// <param name="comment">Reason for re-activating</param>
        void UndoLockOut(string userName, string comment);
    }
}
