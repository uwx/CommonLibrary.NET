using System;

using ComLib;
using ComLib.Application;
using ComLib.Web.ScriptsSupport;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the ScriptsSupport namespace.
    /// </summary>
    public class Example_Scripts : App
    {

        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            bool useRealHttpContext = false;
            // Add 2 groups ("locations") to the scripts.
            Scripts.AddLocation("header", useRealHttpContext);
            Scripts.AddLocation("footer", useRealHttpContext);
            
            // 1. Register a css style sheet.
            Scripts.AddCss("default_theme", "/themes/default/theme.css");

            // 2. Register a javascript file
            Scripts.AddJavascript("comlib.js", "/scripts/core/comlib.js");

            // 3. Register a javascript file in a specific location group "header"
            Scripts.AddJavascript("comlib.js", "/scripts/core/comlib.js", "header");

            // 4. Get access to a location.
            ScriptsHolder header = Scripts.For("header");
            
            // 5. Generate html for the default group "footer".
            Scripts.ToHtml();

            // 6. Generate html for the registered styles/scripts in a specific group.
            var html = Scripts.ToHtml("header");

            return BoolMessageItem.True;
        }
    }
}
