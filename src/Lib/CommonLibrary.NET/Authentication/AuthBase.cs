using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;


namespace ComLib.Authentication
{
    /// <summary>
    /// This is the base class from which all authentication implementations 
    /// must derive from (currently inherited by <see cref="ComLib.Authentication.AuthWeb"/> and
    /// <see cref="ComLib.Authentication.AuthWin"/>).
    /// </summary>
    public abstract class AuthBase
    {
        /// <summary>
        /// Defines the role name for administrators.
        /// </summary>
        protected string _adminRoleName = "Administrators";


        /// <summary>
        /// The name of the current user.
        /// </summary>
        public abstract string UserName { get; }


        /// <summary>
        /// Provides just the username if the username contains
        /// the domain.
        /// e.g. returns "john" if username is "mydomain\john"
        /// </summary>
        public abstract string UserShortName { get; }


        /// <summary>
        /// Get the current user.
        /// </summary>
        public abstract IPrincipal User { get; }


        /// <summary>
        /// The user id.
        /// </summary>
        public int UserId
        {
            get
            {
                var user = User;
                if (user != null && user is UserPrincipal)
                    return ((UserPrincipal)user).UserId;

                return -1;
            }
        }


        /// <summary>
        /// Determine if the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsAuthenticated();


        /// <summary>
        /// Return whether or not the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsGuest()
        {
            return !IsAuthenticated();
        }


        /// <summary>
        /// Determine if currently logged in user is an administrator.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAdmin()
        {
            if (!IsAuthenticated()) return false;

            return IsUserInRoles(_adminRoleName);
        }


        /// <summary>
        /// Determine if the logged in user is the same as the username supplied.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUser(string username)
        {
            // Check for empty usershort name.
            string shortName = UserShortName;
            if (string.IsNullOrEmpty(shortName))
                return false;
            return string.Compare(username, shortName, true) == 0;
        }


        /// <summary>
        /// Returns true if the logged in user is the same as the username supplied,
        /// or if the logged in user is an admin.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsUserOrAdmin(string username)
        {
            return IsUser(username) || IsAdmin();
        }


        /// <summary>
        /// Is User in the selected roles.
        /// </summary>
        /// <param name="rolesDelimited"></param>
        /// <returns></returns>
        public abstract bool IsUserInRoles(string rolesDelimited);
    }
}
