using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;

//<doc:using>
using ComLib;
using ComLib.Entities;
using ComLib.Account;
//</doc:using>
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Account namespace.
    /// </summary>
    public class Example_Account : App
    {       		
        /// <summary>
        /// Run the sample application.
        /// </summary>		
        public override BoolMessageItem Execute()
        {
            //<doc:example>		   
            // 1. Create.
            User account = new User("kishore", "password", "kishore@abc.com", true);
            User.Create(account);

            // 2. ChangePassword.
            User.ChangePassword("kishore", "password", "password2");

            // 3. Approve the user.
            User.Approve("kishore");

            // 4. Verify username and password.
            User.VerifyUser("kishore", "password2");

            // 5. Logon the user. Only updates the "LastLoginDate" in his account.
            User.LogOn("kishore", "password2", false);

            // 6. Lock out the user.
            User.LockOut("kishore", "spamming users");

            // 7. Undo the lockout.
            User.UndoLockOut("kishore", "false alarm");
			
			//</doc:example>
            return BoolMessageItem.True;
        }
		

        /// <summary>
        /// Configure the ActiveRecord for Accounts
        /// NOTES: - All service objects can be constructed with a repository, validator, settings object.            
        ///        - All service objects can be constructed with a fake repository ( in memory ) for testing.
        /// </summary>
        /// <param name="context"></param>
        public override void Init(object context)
        {
            base.Init(context);
			//<doc:setup>
            // Use fake repo for testing, but the real repo has same interface.
            IRepository<User> repository = new RepositoryInMemory<User>();
            var settings = new UserSettings() { UserNameRegEx = @"[a-zA-Z1-9\._]{3,15}", PasswordRegEx = "[a-zA-Z1-9]{5,15}" };

            // User initialization must use the userservice, and uservalidator.
            User.Init(() => new UserService(), () => repository, () => new UserValidator(), new UserSettings(), false, null);
			//</doc:setup>
        }        
    }
}
