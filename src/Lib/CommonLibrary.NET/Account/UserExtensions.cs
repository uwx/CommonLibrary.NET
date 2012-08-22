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
using System.Text.RegularExpressions;
using System.Web.Security;

using ComLib;
using ComLib.Data;
using ComLib.Cryptography;
using ComLib.Authentication;

using ComLib.Entities;
using ComLib.ValidationSupport;


namespace ComLib.Account
{
    /// <summary>
    /// User entity extensions.
    /// 1. Setup the password encryption 
    /// 2. Handle lowering the case for username, email etc.
    /// </summary>
    public partial class User : ActiveRecordBaseEntity<User>
    {
        private static Func<string, string> _encryptionMethod = new Func<string, string>(Crypto.Encrypt);
        private static Func<string, string> _decryptionMethod = new Func<string, string>(Crypto.Decrypt);

        /// <summary>
        /// User password.
        /// </summary>
        protected string _password;


        /// <summary>
        /// User name.
        /// </summary>
        protected string _username;


        /// <summary>
        /// User name (lower case).
        /// </summary>
        protected string _usernameLowered;


        /// <summary>
        /// Full user name.
        /// </summary>
        protected string _usernameFull;


        /// <summary>
        /// User e-mail.
        /// </summary>
        protected string _email;


        /// <summary>
        /// User e-mail (lower case).
        /// </summary>
        protected string _emailLowered;

        
        #region Public static methods
        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        /// <returns>New instance of User.</returns>
        public static User New()
        {
            User entity = new User();
            entity.Settings = new UserSettings();
            return entity;
        }


        /// <summary>
        /// Initalize the internal encryptor lamda method.
        /// </summary>
        /// <param name="encryptor">Encryption function.</param>
        /// <param name="decryptor">Decryption function.</param>
        public static void InitEncryptor(Func<string, string> encryptor, Func<string, string> decryptor)
        {
            // TODO:NOT thread safe.
            _encryptionMethod = encryptor;
            _decryptionMethod = decryptor;
        }


        /// <summary>
        /// Encrypts using the initialized lamda encryption method.
        /// If it is not initialize, uses the default method.
        /// </summary>
        /// <param name="plainText">Clear text.</param>
        /// <returns>Encrypted text.</returns>
        public static string Encrypt(string plainText)
        {
            if (_encryptionMethod != null)
                return _encryptionMethod(plainText);

            return Crypto.Encrypt(plainText);
        }


        /// <summary>
        /// Encrypts using the initialized lamda encryption method.
        /// If it is not initialize, uses the default method.
        /// </summary>
        /// <param name="encrypted">Encrypted text.</param>
        /// <returns>Clear text.</returns>
        public static string Decrypt(string encrypted)
        {
            if (_decryptionMethod != null)
                return _decryptionMethod(encrypted);

            return Crypto.Decrypt(encrypted);
        }


        /// <summary>
        /// Provide a static method for verifying a user is registered/exists.
        /// </summary>
        /// <param name="username">Name of user.</param>
        /// <param name="password">User password.</param>
        /// <returns>Verification result.</returns>
        public static BoolMessageItem<User> VerifyUser(string username, string password)
        {
            UserService service = EntityRegistration.GetService<User>() as UserService;
            return service.VerifyUser(username, password);
        }


        /// <summary>
        /// Try logging in to server.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="password">User password.</param>
        /// <param name="rememberUser">True to remember the user.</param>
        /// <returns>Result of logon attempt.</returns>
        public static BoolMessage LogOn(string userName, string password, bool rememberUser)
        {
            UserService service = EntityRegistration.GetService<User>() as UserService;
            return service.LogOn(userName, password, rememberUser);
        }


        /// <summary>
        /// Register the user.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="email">E-mail of user.</param>
        /// <param name="password">User of password.</param>
        /// <returns>Result of user registration.</returns>
        public static BoolMessageItem<User> Create(string userName, string email, string password)
        {
            MembershipCreateStatus status = MembershipCreateStatus.ProviderError;
            var result = Create(userName, email, password, string.Empty, ref status);
            return result;
        }


        /// <summary>
        /// Register the user.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="email">E-mail of user.</param>
        /// <param name="password">User of password.</param>
        /// <param name="roles">Roles to assign to the user.</param>
        /// <returns>Result of user registration.</returns>
        public static BoolMessageItem<User> Create(string userName, string email, string password, string roles)
        {
            MembershipCreateStatus status = MembershipCreateStatus.ProviderError;
            var result = Create(userName, email, password, roles, ref status);
            return result;
        }


        /// <summary>
        /// Register the user.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="email">E-mail of user.</param>
        /// <param name="password">User of password.</param>
        /// <param name="roles">Roles to assign to the user.</param>
        /// <param name="status">Status of membership creation.</param>
        /// <returns>Result of user registration.</returns>
        public static BoolMessageItem<User> Create(string userName, string email, string password, string roles, ref MembershipCreateStatus status)
        {
            UserService service = EntityRegistration.GetService<User>() as UserService;
            return service.Create(userName, email, password, roles, ref status);
        }


        /// <summary>
        /// Change the current password.
        /// </summary>
        /// <param name="userName">username of the account for which the password is being changed.</param>
        /// <param name="currentPassword">Existing password on file.</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Result of password change.</returns>
        public static BoolMessage ChangePassword(string userName, string currentPassword, string newPassword)
        {
            UserService service = EntityRegistration.GetService<User>() as UserService;
            return service.ChangePassword(userName, currentPassword, newPassword);
        }


        /// <summary>
        /// Change the current email.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="newEmail">The new email.</param>
        /// <returns>Result of e-mail change.</returns>
        public static BoolMessage ChangeEmail(int userId, string newEmail)
        {
            UserService service = EntityRegistration.GetService<User>() as UserService;
            return service.ChangeEmail(userId, newEmail);
        }


        /// <summary>
        /// Lock out the specified user.
        /// </summary>
        /// <param name="userName">User to lock out.</param>
        /// <param name="lockOutReason">Reason for locking out.</param>
        public static void LockOut(string userName, string lockOutReason)
        {
            UserService service = EntityRegistration.GetService<User>() as UserService;
            service.LockOut(userName, lockOutReason);
        }


        /// <summary>
        /// UndoLock out the specified user.
        /// </summary>
        /// <param name="userName">User to undo lock out for.</param>
        /// <param name="comment">Reason for re-activating</param>
        public static void UndoLockOut(string userName, string comment)
        {
            UserService service = EntityRegistration.GetService<User>() as UserService;
            service.UndoLockOut(userName, comment);
        }


        /// <summary>
        /// Get the specified user by username.
        /// </summary>
        /// <param name="userName">User to get.</param>
        /// <returns>Instance of user corresponding to user name.</returns>
        public static User Get(string userName)
        {
            UserService service = EntityRegistration.GetService<User>() as UserService;
            return service.Get(userName);
        }


        /// <summary>
        /// Approve user.
        /// </summary>
        /// <param name="userName">User to approve.</param>
        /// <returns>Result of operation.</returns>
        public static BoolMessage Approve(string userName)
        {
            UserService service = EntityRegistration.GetService<User>() as UserService;
            return service.Approve(userName);
        }
        #endregion


        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public User() { }

        
        /// <summary>
        /// Initialize using username.
        /// </summary>
        /// <param name="username">Name of user.</param>
        /// <param name="password">User password.</param>
        /// <param name="email">E-mail of user.</param>
        /// <param name="isPlainPassword">True if it's a clear text password.</param>
        public User(string username, string password, string email, bool isPlainPassword)
            : this(username, password, email, isPlainPassword, string.Empty)
        {
        }


        /// <summary>
        /// Initialize using username.
        /// </summary>
        /// <param name="username">Name of user.</param>
        /// <param name="password">User password.</param>
        /// <param name="email">E-mail of user.</param>
        /// <param name="isPlainPassword">True if it's a clear text password.</param>
        /// <param name="roles">Roles to assign to the user.</param>
        public User(string username, string password, string email, bool isPlainPassword, string roles)
        {
            UserName = username;
            if (isPlainPassword)
                PasswordPlain = password;
            else
                Password = password;
            Email = email;
            Roles = roles;
        }
        #endregion


        #region Public Properties
        /// <summary>
        /// Sets a plain text password as encrypted.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }


        /// <summary>
        /// Set password using plaintext which gets encrypted.
        /// </summary>
        public string PasswordPlain
        {
            set { _password = User.Encrypt(value); }
        }


        /// <summary>
        /// Sets a plain text password as encrypted.
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; _usernameLowered = _username.ToLower(); }
        }


        /// <summary>
        /// Sets a plain text password as encrypted.
        /// </summary>
        public string UserNameLowered
        {
            get { return _usernameLowered; }
            set
            {
                if (string.Compare(_username, value, true) == 0)
                    _usernameLowered = value; 
            }
        }
        

        /// <summary>
        /// Sets a plain text password as encrypted.
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; _emailLowered = _email.ToLower(); }
        }


        /// <summary>
        /// Sets a plain text password as encrypted.
        /// </summary>
        public string EmailLowered
        {
            get { return _emailLowered; }
            set 
            {
                if (string.Compare(_email, value, true) != 0)
                    _emailLowered = value;
            }
        }
        #endregion   


        #region Validation
        /// <summary>
        /// Gets the validator.
        /// </summary>
        /// <returns>Validator used.</returns>
        protected override IValidator GetValidatorInternal()
        {
            return new UserValidator();
        }
        #endregion


        #region Life-Cycle Events
        /// <summary>
        /// Called one before creating a user in the datastore.
        /// </summary>
        /// <param name="ctx">Context.</param>
        /// <returns>Returns true.</returns>
        public override bool OnBeforeCreate(object ctx)
        {
            LastLockOutDate = DateTime.Today;
            LastLoginDate = DateTime.Today;
            LastPasswordChangedDate = DateTime.Today;
            LastPasswordResetDate = DateTime.Today;
            if (string.IsNullOrEmpty(CreateUser)) CreateUser = this.UserName;
            if (string.IsNullOrEmpty(UpdateUser)) UpdateUser = this.UserName;
            return true;
        }
        #endregion
    }



    /// <summary>
    /// User service extension to handle data massaging for the dates.
    /// </summary>
    public partial class UserService 
    {
        #region VerifyUser, Logon, ChangePassword, etc.
        /// <summary>
        /// Verify that this is a valid user.
        /// </summary>
        /// <param name="userName">"kishore"</param>
        /// <param name="password">"password"</param>
        /// <returns>Result of verification.</returns>
        public BoolMessageItem<User> VerifyUser(string userName, string password)
        {
            // Check inputs.
            if (string.IsNullOrEmpty(userName)) return new BoolMessageItem<User>(null, false, "Username not supplied.");
            if (string.IsNullOrEmpty(password)) return new BoolMessageItem<User>(null, false, "Password not supplied.");

            // Get the user.
            User user = Get(userName);

            // Check for matching records.
            if (user == null)
                return new BoolMessageItem<User>(null, false, "No accounts were found with username: " + userName);

            // Check password.
            string encryptedPassword = User.Encrypt(password);
            bool isPasswordMatch = string.Compare(encryptedPassword, user.Password, false) == 0;
            string message = isPasswordMatch ? string.Empty : "Password supplied does not match";
            
            return new BoolMessageItem<User>(user, isPasswordMatch, message);
        }


        /// <summary>
        /// Similar to VerifyUser except this updates the "LastLoginDate" if successful login.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="password">User password.</param>
        /// <param name="rememberUser">True to remember user.</param>
        /// <returns>Result of logon attempt.</returns>
        public BoolMessage LogOn(string userName, string password, bool rememberUser)
        {
            // Check inputs.
            BoolMessage inputCheck = Validation.AreNoneNull(new string[] { userName, password }, new string[] { "UserName", "Password" }, null, null);
            if (!inputCheck.Success) return inputCheck;

            BoolMessageItem<User> result = VerifyUser(userName, password);
            if (result.Success)
            {
                // An authentication provider should handle this.
                // Auth.SignIn(result.Item.UserName, rememberUser);
                
                // Update the last login date.
                result.Item.LastLoginDate = DateTime.Today;
                Update(result.Item);
            }

            return result;
        }


        /// <summary>
        /// Create the user and also return the membership status
        /// </summary>
        /// <param name="userName">kishore</param>
        /// <param name="email">kishore@abc.com</param>
        /// <param name="password">password</param>
        /// <param name="roles"></param>
        /// <param name="status">DuplicateUserName</param>
        /// <returns>Result of user creation.</returns>
        public virtual BoolMessageItem<User> Create(string userName, string email, string password, string roles, ref MembershipCreateStatus status)
        {
            User user = User.New();
            user.UserName = userName;
            user.IsApproved = true;
            user.Email = email;
            user.Roles = roles;
            BoolMessage result = Create(user, password, ref status);
            return new BoolMessageItem<User>(user, result.Success, string.Empty);
        }


        /// <summary>
        /// Create the user and also return the membership status
        /// </summary>
        /// <param name="user">kishore</param>
        /// <param name="password">password</param>
        /// <param name="status">DuplicateUserName</param>
        /// <returns>Result of user creation.</returns>
        public virtual BoolMessage Create(User user, string password, ref MembershipCreateStatus status)
        {
            UserSettings settings = (UserSettings)Settings;
            IValidationResults errors = new ValidationResults();
            status = ValidateCreation(user, settings, errors, password, password);

            // Previous call only validated.
            if (status != MembershipCreateStatus.Success)
            {
                return new BoolMessage(false, errors.Message());
            }

            // Encrypt the password before saving.
            user.PasswordPlain = password;

            // Create the user.
            Create(user);
            return new BoolMessage(!user.Errors.HasAny, user.Errors.Message());
        }


        /// <summary>
        /// Create the user.
        /// </summary>
        /// <param name="userName">"kishore"</param>
        /// <param name="email">"kishore@abc.com"</param>
        /// <param name="password">password</param>
        /// <param name="roles">Roles to assign to the user.</param>
        /// <returns>Result of user creation.</returns>
        public virtual BoolMessageItem<User> Create(string userName, string email, string password, string roles)
        {
            MembershipCreateStatus status = MembershipCreateStatus.Success;
            return Create(userName, email, password, roles, ref status);
        }


        /// <summary>
        /// Create a user.
        /// </summary>
        /// <param name="ctx">Context.</param>
        public override void Create(IActionContext ctx)
        {
            if (ctx.Item == null) 
            {
                base.Create(ctx);
                return;
            }

            User user = ctx.Item as User;
            UserSettings settings = Settings as UserSettings; 
            IValidationResults errors = new ValidationResults();
           
            var createStatus = ValidateCreation(user, settings, errors, user.Password, user.Password);
            if (createStatus != MembershipCreateStatus.Success)
            {
                user.Errors.Add("Unable to create user : " + createStatus);
                return;
            }
            base.Create(ctx);
        }


        /// <summary>
        /// Change the current password.
        /// </summary>
        /// <param name="userName">username of the account for which the password is being changed.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Result of password change operation.</returns>
        public virtual BoolMessage ChangePassword(string userName, string oldPassword, string newPassword)
        {
            // Check inputs.
            BoolMessage inputCheck = Validation.AreNoneNull(
                    new string[] { userName, oldPassword, newPassword },
                    new string[] { "UserName", "Current Password", "New Password" }, null, null);
            if (!inputCheck.Success) return new BoolMessage(false, inputCheck.Message);

            UserSettings settings = (UserSettings)Settings;

            // Check password 
            if (!string.IsNullOrEmpty(settings.PasswordRegEx) &&
                !Regex.IsMatch(newPassword, settings.PasswordRegEx))
                return new BoolMessage(false, "New password is invalid");
            
            // Check passwords match
            //if (string.Compare(newPassword, confirmPassword, false) != 0)
            //    return new BoolMessage(false, "Password and confirm password do not match.");
            
            // Check user account
            User user = Get(userName);
            if (user == null) 
                return new BoolMessage( false, "Unable to find account with matching username : " + userName);

            // Check that password supplied matches whats on file.
            string encryptedPassword = User.Encrypt(oldPassword);
            if (string.Compare(encryptedPassword, user.Password, false) != 0)
                return new BoolMessage(false, "Old password supplied does not match whats on file.");

            // Now change password.
            user.LastPasswordChangedDate = DateTime.Today;
            user.PasswordPlain = newPassword;
            Update(user);
            return BoolMessage.True;
        }


        /// <summary>
        /// Change the email address associated w/ the accountid.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="newEmail">The new email.</param>
        /// <returns>Result of e-mail change operation.</returns>
        public virtual BoolMessage ChangeEmail(int userId, string newEmail)
        {
            User user = Get(userId);
            user.Errors.Clear();
            return ChangeEmail(user, newEmail);
        }


        /// <summary>
        /// Changes the email associated w/ the user account.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="newEmail">The new email.</param>
        /// <returns>Result of e-mail change operation.</returns>
        public virtual BoolMessage ChangeEmail(User user, string newEmail)
        {
            if (user == null)
                throw new ArgumentNullException("User account must be supplied.");

            string error = null;
            // Check inputs.
            if (string.IsNullOrEmpty(newEmail))
            {
                error = "New email address must be supplied.";
                user.Errors.Add("Email", error);
                return new BoolMessage(false, error);
            }

            // Check email 
            if (!Validation.IsEmail(newEmail, false))
            {
                error = "New email is invalid";
                user.Errors.Add("Email", error);
                return new BoolMessage(false, error);
            }

            // Check email duplicates           
            IList<User> emailDups = this.Find(Query<User>.New().Where(u => u.EmailLowered).Is(newEmail.ToLower()));
            if (emailDups != null && emailDups.Count > 0)
            {
                error = "Email : " + newEmail + " is already taken.";
                user.Errors.Add("Email", error);
                return new BoolMessage(false, error);
            }
            // Now change email.
            user.Email = newEmail;
            Update(user);
            return new BoolMessage(!user.Errors.HasAny, user.Errors.Message());
        }


        /// <summary>
        /// Get a user by their name.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <returns>Instance of User corresponding to the user name.</returns>
        public User Get(string userName)
        {
            // Check user account
            User user = First(Query<User>.New().Where(e => e.UserNameLowered).Is(userName.ToLower()).Builder.BuildConditions(false));
            return user;
        }


        /// <summary>
        /// Lock out the user specified.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="lockOutReason">Reason for lockout.</param>
        public virtual void LockOut(string userName, string lockOutReason)
        {
            UserSettings settings = (UserSettings)Settings;
            User user = Get(userName);
            user.IsLockedOut = true;
            user.LastLockOutDate = DateTime.Today;
            user.LockOutReason = lockOutReason;
            Update(user);
        }


        /// <summary>
        /// Undo the lock out the user specified.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <param name="comment">Undo comment.</param>
        public virtual void UndoLockOut(string userName, string comment)
        {
            UserSettings settings = (UserSettings)Settings;
            User user = Get(userName);
            user.IsLockedOut = false;
            user.Comment = comment;
            Update(user);
        }


        /// <summary>
        /// Approve the user specified.
        /// </summary>
        /// <param name="userName">Name of user.</param>
        /// <returns>Result of approve operation.</returns>
        public virtual BoolMessage Approve(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return new BoolMessage(false, "Unable to retrive user with name : " + userName);

            User user = Get(userName);
            if (user == null) return new BoolMessage(false, "Unable to retrive user with name : " + userName);

            user.IsApproved = true;
            Update(user);
            return BoolMessage.True;
        }
        #endregion


        #region Validation methods
        /// <summary>
        /// Return a new instance of a validator. This is neccessary.
        /// </summary>
        /// <returns>Internal validator used.</returns>
        public override IEntityValidator GetValidator()
        {
            return new UserValidator();
        }


        /// <summary>
        /// Override the validation to handle check for existing accounts with same 
        /// username or email address.
        /// </summary>
        /// <param name="ctx">The action context.</param>
        /// <param name="entityAction">Action to perform.</param>
        /// <returns>Result of operation.</returns>
        protected override BoolMessage PerformValidation(IActionContext ctx, EntityAction entityAction)
        {
            if (!Settings.EnableValidation)
                return BoolMessage.True;

            bool validationResult = true;
            if (entityAction == EntityAction.Create || entityAction == EntityAction.Update)
            {
                // Validate the actual entity data members.
                IEntityValidator validator = this.GetValidator();
                validationResult = validator.Validate(ctx.Item, ctx.Errors);
                User entity = ctx.Item as User;
                
                if (entityAction == EntityAction.Update)
                {
                    ctx.Id = entity.Id;
                    User entityBeforeUpdate = Get(ctx);
                    if (string.Compare(entityBeforeUpdate.UserName, entity.UserName, true) != 0)
                        ctx.Errors.Add("Can not change username");
                }

                // Make sure password is encrypted.
                try { string decrypted = Crypto.Decrypt(entity.Password); }
                catch (Exception)
                {
                    ctx.Errors.Add("Password was not encrypted. Encrypt using method SetPassword(plainText);");
                    validationResult = false;
                }
            }

            // Now append all the errors.
            if (!validationResult)
            {
                if (ctx.CombineMessageErrors)
                {
                    string errorMessage = ctx.Errors.Message();
                    return new BoolMessage(false, errorMessage);
                }
            }
            return BoolMessage.True;
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// Validate passwords are same.
        /// </summary>
        /// <param name="password">User password.</param>
        /// <param name="passwordConfirmation">Second entry of password.</param>
        /// <returns>Validation result.</returns>
        public static BoolMessage ValidatePasswords(string password, string passwordConfirmation)
        {
            // Check password.
            if (string.Compare(password, passwordConfirmation, false) != 0)
                return new BoolMessage(false, "Password and Password Confirmation do not match.");
            
            return BoolMessage.True;
        }


        /// <summary>
        /// Performs validation for creation purposes. This includes checks for duplicate usernames/emails.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="errors">The errors.</param>
        /// <param name="password">The password.</param>
        /// <param name="confirmPassword">The confirm password.</param>
        /// <returns>Instance of membership creation status.</returns>
        public virtual MembershipCreateStatus ValidateCreation(User user, UserSettings settings, IValidationResults errors,
            string password, string confirmPassword)
        {
            MembershipCreateStatus status = MembershipCreateStatus.Success;

            // Check username
            if (!string.IsNullOrEmpty(settings.UserNameRegEx) 
                && !Regex.IsMatch(user.UserName, settings.UserNameRegEx))
            {
                status = MembershipCreateStatus.InvalidUserName;
                errors.Add("Invalid user name supplied.");
            }
            // Check email
            if (!Regex.IsMatch(user.Email, RegexPatterns.Email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                errors.Add("Invalid email supplied.");
            }
            // Check password 
            if (!string.IsNullOrEmpty(settings.PasswordRegEx)
                && !Regex.IsMatch(password, settings.PasswordRegEx))
            {
                status = MembershipCreateStatus.InvalidPassword;
                errors.Add("Invalid password supplied.");
            }
            // Check passwords match
            if (string.Compare(password, confirmPassword, false) != 0)
            {
                status = MembershipCreateStatus.InvalidEmail;
                errors.Add("Password and confirm password do not match.");
            }
            // Check username duplicates
            User userExisting = Get(user.UserName);
            if (userExisting != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                errors.Add("Username : " + user.UserName + " is already taken.");
            }            
            // Check email duplicates
            IList<User> emailDups = this.Find("EmailLowered = '" + user.EmailLowered + "'");
            if (emailDups != null && emailDups.Count > 0)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                errors.Add("Email : " + user.EmailLowered + " is already taken.");
            }
            return status;
        }
        #endregion
    }



    /// <summary>
    /// Create other settings for account services.
    /// </summary>
    public partial class UserSettings
    {
        /// <summary>
        /// Get/set the minimum password length.
        /// </summary>
        public int MinimumPasswordLength { get; set; }


        /// <summary>
        /// Get/set the minimum user name length.
        /// </summary>
        public int MinimumUserNameLength { get; set; }


        /// <summary>
        /// Get/set the minimum password login attempts.
        /// </summary>
        public int MinimumPasswordAttempts { get; set; }


        /// <summary>
        /// Get/set the regular expression to validate the user name.
        /// </summary>
        public string UserNameRegEx { get; set; }


        /// <summary>
        /// Get/set the regular expression to validate the password.
        /// </summary>
        public string PasswordRegEx { get; set; }


        /// <summary>
        /// Get/set the required confirmation e-mail flag.
        /// </summary>
        public bool RequireActivationViaConfirmationEmail { get; set; }


        /// <summary>
        /// Get/set the required security question flag.
        /// </summary>
        public bool RequireSecurityQuestion { get; set; }


        /// <summary>
        /// Get/set the enforce lockout flag.
        /// </summary>
        public bool EnforceLockoutOnIncorrectPasswords { get; set; }
    }
}
