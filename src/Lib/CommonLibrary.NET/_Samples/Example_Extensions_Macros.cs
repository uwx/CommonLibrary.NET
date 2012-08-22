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
using ComLib.Macros;
using ComLib.Application;
using ComLib.Extensions;


namespace ComLib.Samples
{
    /// <summary>
    /// Example of Macro component to interpret macros or command in text, much like wordpress shortcodes.
    /// </summary>
    public class Example_Extensions_Macros : App
    {
        private string _macroSampleText1 = @"Local news for $[today format=""MM//dd//yyyy""/] with NY Times.";
        private string _macroSampleText2 = @"Saying $[helloworld language=""en""]Some more greeting here.[/helloworld]";

        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            // 1. Macro service loads classes with attribute "MacroAttribute" and of type IMacro.
            var macros = new MacroService();

            // 2. Register Option 1: Load macro extensions from dll "CommonLibrary"            
            macros.Load("CommonLibrary");

            // 3. Register Option 2: Programmactically with macro attributes.       
            macros.Register(new MacroAttribute(){ Name = "helloworld" },
                            new MacroParameterAttribute[] { new MacroParameterAttribute() { Name = "language" } },
                           (tag) => "hello world using tag: " + tag.Name + ", " + tag.InnerContent );

            // 3. Register Option 3: Programmactically with simple api.
            macros.Register("helloworld2", "", "Simple hello world macro", (tag) => "hello world 2 : "); 
            
            
            // 4. Now build the content by processing the macros
            var content = macros.BuildContent(_macroSampleText1);

            // 5. Render content that has a lamda based macro in it.
            // NOTE: Render content that has multiple macros involves the same method call.
            var content2 = macros.BuildContent(_macroSampleText2);
            
            
            return BoolMessageItem.True;
        }
    }



    [Macro(Name = "today", DisplayName = "Simple Date Macro 1", Description = "Simple Date Macro 1")]
    [MacroParameter(Name = "format", DataType = typeof(string), Description = "How to format the date")]
    class SimpleDateMacro : IMacro
    {
        /// <summary>
        /// Render the widget
        /// </summary>
        /// <returns></returns>
        public string Process(Tag tag)
        {
            string format = tag.Attributes.GetOrDefault<string>("format", "mm/dd/yyyy");
            return DateTime.Today.ToString(format);
        }
    }
}
