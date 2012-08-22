using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;

//<doc:using>
using ComLib;
using ComLib.Diagnostics;
//</doc:using>
using ComLib.Entities;
using ComLib.Account;
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Diagnostics namespace.
    /// </summary>
    public class Example_Diagnostics : App
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Example_Diagnostics()
        {
        }


        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
			//<doc:example>
            // 1. Write out the machine information and loaded dlls.
            Diagnostics.Diagnostics.WriteInfo("MachineInfo,AppDomain", "Diagnostics_MachineInfo_DllsLoaded.txt");

            // 2. Write out the environment variables.
            Diagnostics.Diagnostics.WriteInfo("Env_System,Env_User", "Diagnostics_EnvironmentVars.txt");

			//</doc:example>
            Console.WriteLine("Wrote diagnostic data to Diagnostics_MachineInfo_DllsLoaded.txt and Diagnostics_EnvironmentVars.txt");
            return BoolMessageItem.True;
        }
    }
}
