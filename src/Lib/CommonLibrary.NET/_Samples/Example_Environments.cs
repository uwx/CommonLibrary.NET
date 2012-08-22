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
using ComLib.Environments;
//</doc:using>
using ComLib.Application;
using ComLib.Logging;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Environments namespace.
    /// </summary>
    public class Example_Environments : App
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Example_Environments()
        {
        }

		
		//<doc:example>
        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            //// Setup notification on Environment changed.
            Env.OnChange += (sender, args) => Console.Write("New Env: " + Env.Name);

            // Use 1: Set to "prod"(PRODUCTION) with default available envs as "prod,uat,qa,dev".
            Envs.Set("prod");
            PrintEnvironment();

            // Use 2: Set to "qa"(QA) with default available envs as "prod,qa,dev".
            Envs.Set("qa", "prod,qa,dev");
            PrintEnvironment();

            // Use 3: Set to "dev"(DEVELOPMENT) with default available envs as "prod,uat,qa,dev".
            //        Also set the "dev" env RefPath to "dev.config".
            Envs.Set("dev", "prod,uat,qa,dev", "dev.config");
            PrintEnvironment();

            // Use 4: Env Set up & Configuration File Setup WITH-OUT Inheritance
            //        - Set env to "ny.prod"(PRODUCTION) with default available envs as "ny.prod,london.prod,qa,dev".
            //        - The "ny.prod" environment name is set to environment type of "prod" via format "<envName>:<envType>" e.g. "myprod:prod"
            //        - Also set the "ny.prod" env RefPath to use 3 config files "ny.prod.config,london.prod.config,dev.config. ( For config file inheritance ).
            Envs.Set("ny.prod", "ny.prod:prod,london.prod:prod,qa,dev", "ny.prod.config,london.prod.config,qa.config,dev.config");
            PrintEnvironment();

            // Use 5: Env Set up & Configuration File Setup WITH-OUT Inheritance but WITH config file distribution.
            //        - Also set the "ny.prod" env RefPath to use 1 config files "ny.prod.config,london.prod.config,dev.config.
            Envs.Set("ny.prod", "ny.prod:prod,london.prod:prod,qa,dev", "ny.prod.config,london.prod.config,qa.config,dev.config", true, false);
            PrintEnvironment();

            // Use 5: Env Set up & Configuration File Setup WITH Inheritance
            //        - Also set the "ny.prod" env RefPath to use 1 config files "ny.prod.config,london.prod.config,dev.config.
            Envs.Set("ny.prod", "ny.prod:prod,london.prod:prod,qa,dev", "ny.prod.config,london.prod.config,qa.config,dev.config", true, true);
            PrintEnvironment();

            // Use 5: Set up the environment using Built objects. 
            Envs.Set("Dev2", GetSampleEvironments());
            PrintEnvironment();


            // Use 6: Change the environment from the last one ("Dev2").
            Env.Change("Qa");
            PrintEnvironment();
            return BoolMessageItem.True;
        }        


        /// <summary>
        /// This builds a datastructure of all the environments supported
        /// and the links to the config files for each environment 
        /// and how they are inherited.
        /// 
        /// THIS CAN BE LOADED FROM AN XML, JSON, YAML, INI file or whatever.
        /// </summary>
        /// <returns></returns>
        public static List<EnvItem> GetSampleEvironments()
        {
            List<EnvItem> envs = new List<EnvItem>()
            {
                new EnvItem(){ Name = "Dev",    RefPath ="dev.config",    InheritsDeeply = true,  EnvType = EnvType.Dev,       Inherits = "" },
                new EnvItem(){ Name = "Dev2",   RefPath ="dev2.config",   InheritsDeeply = true,  EnvType = EnvType.Dev,       Inherits = "" },
                new EnvItem(){ Name = "Qa",     RefPath ="qa.config",     InheritsDeeply = true,  EnvType = EnvType.Qa,        Inherits = "Dev" },
                new EnvItem(){ Name = "Prod",   RefPath ="prod.config",   InheritsDeeply = true,  EnvType = EnvType.Prod,      Inherits = "Qa" },
                new EnvItem(){ Name = "Custom", RefPath ="custom.config", InheritsDeeply = true,  EnvType = EnvType.MixedProd, Inherits = "Prod,Dev2" }
            };
            return envs;
        }


        private void PrintEnvironment()
        {
            Logger.Info("====================================================");
            Logger.Info("ENVIRONMENTS ");
            Logger.Info("Environment name: " + Env.Name);
            Logger.Info("Environment type: " + Env.EnvType);
            Logger.Info("Environments #  : " + Env.Count);
            Logger.Info("Environment inherits: " + Env.Inherits);
            Logger.Info("Environment file: " + Env.RefPath);
            Logger.Info("Environment IsProd: " + Env.IsProd);
            Logger.Info("Environment IsQa: " + Env.IsQa);
            Logger.Info("Environment IsDev: " + Env.IsDev);
            Logger.Info(Environment.NewLine);
        }
		//</doc:example>
    }
}
