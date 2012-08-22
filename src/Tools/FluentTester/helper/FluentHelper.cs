using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang;
using System.IO;
using System.Xml;
using ComLib.Tools.FluentTester.Samples;


namespace ComLib.Tools.FluentTester.Helpers
{
    public static class FluentHelper
    {
        /// <summary>
        /// Starts Interpereter
        /// </summary>
        /// <returns></returns>
        public static Interpreter GetInterpreter()
        {
            // how to run fluent script interpreter
            var i = new Interpreter();

            // Register all plugins e.g. "Date", "Day" "Time" etc.
            i.Context.Plugins.RegisterAll();
            i.Context.Types.Register(typeof(Person), () => new Person());
            return i;
        }


        /// <summary>
        /// Get relative and absolute file location and then
        /// Get content of a fluent script file
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public static string GetFluentScriptFileContent(string scriptName)
        {
            if (String.IsNullOrEmpty(scriptName)) return null;

            // Get relative and absolute file location
            string fileRelativeLocation = @"..\.." + scriptName;
            string fileAbsoluteLocation = Path.GetFullPath(fileRelativeLocation);

            // Get content of a fluent script file
            return System.IO.File.ReadAllText(fileAbsoluteLocation);
        }


        public static bool IsMatchingValue(object actual, string type, string expectedText)
        {
            object expected = expectedText;
            if (type == "bool") expected = Convert.ToBoolean(expectedText);
            else if (type == "number")
            {
                actual = Convert.ToDouble(actual);
                expected = Convert.ToDouble(expectedText);
            }
            else if (type == "datetime")
            {
                expectedText = expectedText.Replace("${month}", DateTime.Today.Month.ToString());
                expectedText = expectedText.Replace("${day}", DateTime.Today.Day.ToString());
                expectedText = expectedText.Replace("${year}", DateTime.Today.Year.ToString());
                expected = Convert.ToDateTime(expectedText);
            }
            else if (type == "time") expected = TimeSpan.Parse(expectedText);
            else if (type == "dayofweek")
            {
                expected = Enum.Parse(typeof(DayOfWeek), expectedText);
            }
            bool isMatch = actual.Equals(expected);
            return isMatch;

        }
              
    }
}
