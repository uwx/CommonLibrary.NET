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
using ComLib.Caching;
//</doc:using>
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example of Cache component.
    /// </summary>
    public class Example_Cache : App
    {
		/// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            // Using default ASP.NET Cache.
            //<doc:example>        
            Cacher.Insert("my_site", "http://www.ufc.com");
            Cacher.Insert("my_show", "ufc: ultimate fighting");
            Cacher.Insert("my_place", "bahamas", 360, true);
            Cacher.Get<string>("my_framework", 30, () => "commonlibrary.net");

            Console.WriteLine("====================================================");
            Console.WriteLine("CACHE ");
            Console.WriteLine("Obtained from cache : '" + Cacher.Get<string>("my_site") + "'");
            Console.WriteLine("Contains cache for 'my_show' : " + Cacher.Contains("my_show"));
            Console.WriteLine(Environment.NewLine);
			
			//</doc:example>
			return BoolMessageItem.True;
        }		
		

        /// <summary>
        /// Clear the cache on shutdown of the application.
        /// </summary>
        public override void ShutDown()
        {
            Cacher.Clear();
        }
    }
}
