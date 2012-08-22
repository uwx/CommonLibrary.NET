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
using ComLib.Configuration;
//</doc:using>
using ComLib.Application;
using ComLib.IO;
using ComLib.Entities;
using ComLib.Account;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Configuration namespace.
    /// </summary>
    public class Example_Config : App
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Example_Config()
        {
        }


        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            // Loading a single config file.
            SingleFile();

            // Loading 2 config files ( inheritance based configuration ).
            Inheritance();

            return BoolMessageItem.True;
        }


		//<doc:example>		
        /// <summary>
        /// Using a single file for configuration.
        /// </summary>
        public void SingleFile()
        {           
            // Initialize the static configuration with the provider.
            Config.Init(new IniDocument("dev.config", GetSampleContents("blogdev"), false));

            Console.WriteLine("Config Name           :  " + Config.Name);
            Console.WriteLine("Config Source         :  " + Config.SourcePath);
            Console.WriteLine("Global.Category       :  " + Config.Get<string>("Global", "Category"));
            Console.WriteLine("Global.Days           :  " + Config.Get<int>("Global", "Days"));
            Console.WriteLine("Class.Title           :  " + Config.Get<string>("Class", "Title"));
            Console.WriteLine("Class.Date            :  " + Config.Get<DateTime>("Class", "Date"));
            Console.WriteLine("Class.IsOnline        :  " + Config.Get<bool>("Class", "LastFtpDate"));
            Console.WriteLine("Class.Cost            :  " + Config.Current["Class", "Cost"]);
            Console.WriteLine(Environment.NewLine);
        }


        /// <summary>
        /// This example shows how the prod.config configsource can be merged
        /// with the dev.config to "inherit" all it's settings.
        /// </summary>
        public void Inheritance()
        {            
            // List of files.
            // prod.config inherits from dev.config.
            var configs = new List<IConfigSource>()
            {
                new IniDocument("prod.config", GetSampleContents("prod"), false),
                new IniDocument("dev.config", GetSampleContents("dev"), false)
            };

            // Intialize w/ the provider.
            Config.Init(new ConfigSourceMulti(configs));

            // NOTE: 
            // 1. Global.AppName inherited from dev.config.
            // 2. DB.desc comes from prod.config.
            // 3. DB.server from prod.config overriden with same value from dev.config.
            Console.WriteLine("Config Name           :  " + Config.Name);
            Console.WriteLine("Config Source         :  " + Config.SourcePath);
            Console.WriteLine("Global.AppName        :  " + Config.Get<string>("Global", "AppName"));
            Console.WriteLine("Global.Env            :  " + Config.Get<string>("Global", "Env"));
            Console.WriteLine("DB.server             :  " + Config.Get<string>("DB", "server"));
            Console.WriteLine("DB.user               :  " + Config.Get<string>("DB", "user"));
            Console.WriteLine("DB.password           :  " + Config.Get<string>("DB", "password"));
            Console.WriteLine("DB.port               :  " + Config.Current["DB", "port"]);
            Console.WriteLine("DB.desc               :  " + Config.Current["DB", "desc"]);
        }
		//</doc:example>
		

        private string GetSampleContents(string env)
        {
            // This is just an example of loading configuration data from a string.
            // This could be from a file.
            // - INI files ( for most practical purposes I've found are more than enough )
            // - Also JSON documents could be used instead of XML.            
            // This is equivalent to a java .props file.
            string dev = "[Global]" + Environment.NewLine
                             + "AppName:CommonLibary.App1" + Environment.NewLine
                             + "Env: Dev" + Environment.NewLine

                             + "[DB]" + Environment.NewLine
                             + "server: dev.server01" + Environment.NewLine
                             + "user: devuser1" + Environment.NewLine
                             + "password: pass123" + Environment.NewLine
                             + "port:8081" + Environment.NewLine;

            // This is equivalent to a java .props file.
            string prod = "[Global]" + Environment.NewLine
                             + "Env: Prod" + Environment.NewLine

                             + "[DB]" + Environment.NewLine
                             + "server: prod_srv" + Environment.NewLine
                             + "user: readonly" + Environment.NewLine
                             + "password: ro9999 " + Environment.NewLine
                             + "desc: primary prod server" + Environment.NewLine;

            // This is equivalent to a java .props file.
            string blogDev = "[Global]" + Environment.NewLine
                             + "Category: Art,Drawing" + Environment.NewLine
                             + "Days: 2" + Environment.NewLine

                             + "[Class]" + Environment.NewLine
                             + "Title: Build a website" + Environment.NewLine
                             + "Date: 9/20/2010" + Environment.NewLine
                             + "IsOnline: false" + Environment.NewLine
                             + "Cost: 450.50" + Environment.NewLine;

            if (env == "dev") return dev;
            if (env == "prod") return prod;
            return blogDev;
        }
    }
}
