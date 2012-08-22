using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Security.Principal;

//<doc:using>
using ComLib;
using ComLib.Authentication;
//</doc:using>
using ComLib.Entities;
using ComLib.Account;
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Authentication namespace.
    /// </summary>
    public class Example_Authentication : App
    {
		/// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {            
			//<doc:example>        
            // 1. Using default authentication ( WINDOWS )
            // NOTE: Known bug, need to figure out how to determine if admin on windows.
            Console.WriteLine("====================================================");
            Console.WriteLine("AUTHENTICATION ");
            Console.WriteLine("Is authenticated : " + Auth.IsAuthenticated());
            Console.WriteLine("Is guest         : " + Auth.IsGuest());
            Console.WriteLine("Is admin         : " + Auth.IsAdmin());
            Console.WriteLine("UserName         : " + Auth.UserName);
            Console.WriteLine("UserNameShort    : " + Auth.UserShortName);
            Console.WriteLine(Environment.NewLine);

            // 2. Use a FAKE authentication ( useful for UNIT-TESTING )
            UserPrincipal userForUnitTest = new UserPrincipal(2, "kdog", "admin", new UserIdentity(2, "kdog", "custom", false));
            Auth.Init(new AuthWin("admin", userForUnitTest));
            Console.WriteLine("Is authenticated : " + Auth.IsAuthenticated());
            Console.WriteLine("Is guest         : " + Auth.IsGuest());
            Console.WriteLine("Is admin         : " + Auth.IsAdmin()); 
            Console.WriteLine("UserName         : " + Auth.UserName);
            Console.WriteLine("UserNameShort    : " + Auth.UserShortName);
            
            // 3. Using ASP.NET authentication ( via HttpContext.Current.User ).
            // The lambda is used for getting an IPrincipal given a username
            // who may not be the HttpContext.Current.User
            Auth.Init(new AuthWeb("admin", (username) => GetUser(username)));
            // Can not use this obviously.

            // Reset to windows.
            Auth.Init(new AuthWin());
			
			//</doc:example>        
            return BoolMessageItem.True;
        }
		

        /// <summary>
        /// Get user data given the username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private IPrincipal GetUser(string userName)
        {
            UserPrincipal sampleUser = new UserPrincipal(2, userName, "poweruser", new UserIdentity(2, userName, "poweruser", false));
            return sampleUser;
        }
    }
}
