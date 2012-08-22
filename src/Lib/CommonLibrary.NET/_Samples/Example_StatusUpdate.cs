using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Security.Principal;


using ComLib;
using ComLib.Entities;
using ComLib.StatusUpdater;
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example of ActiveRecord Initialization/Configuration.
    /// </summary>
    public class Example_StatusUpdate : App
    {

        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            StatusUpdates.Update("startup", "starting", "application startup", DateTime.Now, DateTime.Now);
            StatusUpdates.Update("startup", "completed", "application startup", DateTime.Now, DateTime.Now);
            StatusUpdates.Update("executing", "executing", "app execution", DateTime.Now, DateTime.Now);

            return BoolMessageItem.True;
        }
    }
}
