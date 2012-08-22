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
//</doc:using>
using ComLib.Application;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Errors extensions.
    /// </summary>
    public class Example_Errors : App
    {

        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
			//<doc:example>
            IErrors errors = new Errors();

            // 1. Add error message without a key.
            errors.Add("Required data file is not available");
            errors.Add("Web Service has timed out");

            // 2. Add errors associated w/ a specific key.
            errors.Add("Url", "Is Invalid");
            errors.Add("Stock", "Symbol is not valid for processing");

            // 3. Error count
            Console.WriteLine(errors.Count);

            // 4. Build an single error message that combines both the non-key and key based errors.
            Console.WriteLine(errors.Message());

            // 5. Indicates if there are any errors.
            Console.WriteLine(errors.HasAny);

            // 6. Iterate through all the key/value based errors.
            errors.Each((key, error) => Console.WriteLine("{0} : {1}", key, error));

            // 7. Iterate through all the key/value and non-key based errors.
            errors.EachFull(error => Console.WriteLine(error));

            // 8. Get the error on a specific key
            Console.WriteLine(errors.On("Url"));

            // 9. Copy errors over into another IError collection.
            var errors2 = new Errors();
            errors.CopyTo(errors2);

            // NOTE: Exposing the internal list/map is done for error collecting.
            // Though not currently implemented, this may return an IList that is Addable only
            // and an IDictionary<string, string> that is also addable only.
            // Because exposing the internal list isn't generally good.
            // 10. Get reference to internal error list.
            Console.WriteLine(errors.MessageList);

            // 11. Get reference to internal error map.
            Console.WriteLine(errors.MessageMap);

            // 12. Build the error message using <br/> instead of default Environment.NewLine
            Console.WriteLine(errors.Message("<br/>"));
			
			//</doc:example>
            return BoolMessageItem.True;
        }
    }
}
