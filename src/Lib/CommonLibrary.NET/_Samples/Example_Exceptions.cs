using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;

using ComLib.Entities;
using ComLib.Account;
using ComLib;
using ComLib.Application;
using ComLib.Exceptions;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Exceptions namespace.
    /// </summary>
    public class Example_Exceptions : App
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Example_Exceptions()
        {
        }


        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            ErrorManager.Register("myErrorManager", false, new CustomExceptionHandler("myErrorManager"));
            Console.WriteLine("====================================================");
            Console.WriteLine("EXCEPTION HANDLING ");

            try
            {
                throw new ArgumentException("exception handling testing");
            }
            catch (Exception ex)
            {
                // Option 1. Use default error handler.
                ErrorManager.Handle("Default error handling.", ex);

                // Option 2. Use custom named error handler "myErrorManager"
                ErrorManager.Handle("Example with custom NAMED error handler.", ex, "myErrorManager");
            }
            Console.WriteLine(Environment.NewLine);
            return BoolMessageItem.True;
        }


        class CustomExceptionHandler : ErrorManagerBase
        {
            public CustomExceptionHandler(string name)
            {
                _name = name;
            }


            protected override void InternalHandle(object error, Exception exception, IErrors errors, object[] arguments)
            {
                Logging.Logger.Error("Custom exception handler");
                Logging.Logger.Error(error, exception, arguments);   
            }
        }
    }
}
