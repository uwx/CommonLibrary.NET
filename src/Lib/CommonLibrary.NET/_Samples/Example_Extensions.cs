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
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example of Extensions component to load classes using attributes
    /// and create instances automatically.
    /// </summary>
    public class Example_Extensions : App
    {   
        //<doc:execute>
        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            // 1. ExtensionService loads classes with "Extension" attribute w/ interface IBlogWidget
            var exts = new ExtensionService<ExtensionAttribute, IBlogWidget>();

            // 2. Load extensions/plugins from "CommonLibrary"            
            exts.Load("CommonLibrary");

            // 3. Get instance of extension with name "BlogWidget_1".
            IBlogWidget widget = exts.Create("BlogWidget_1");
            var content = widget.Render();

            return BoolMessageItem.True;
        }
        //</doc:execute>
    }



    //<doc:exampleref>
    /// <summary>
    /// interface for the custom widgets
    /// </summary>
    interface IBlogWidget
    {
        string Render();
    }



    [Extension(Name = "BlogWidget_1", DisplayName = "Blog widget 1", Description = "Blog widget 1")]
    class SimpleBlogWidget : IBlogWidget
    {
        /// <summary>
        /// Render the widget
        /// </summary>
        /// <returns></returns>
        public string Render()
        {
            return "This is blog widget 1";
        }
    }
}
