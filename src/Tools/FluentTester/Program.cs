using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using ComLib.Lang;
using ComLib.Lang.Types;
using ComLib.Tools.FluentTester.Entities;
using ComLib.Tools.FluentTester.Helpers;
using ComLib.Tools.FluentTester;


namespace FluentTester
{
    public class FluentTesterRunner
    {
        static void Main(string[] args)
        {            
            try
            {
                bool success = Execute(args);
                // Success
                // Make font green print success to console.
                Console.ForegroundColor = success ? ConsoleColor.Green : ConsoleColor.Red;
                string text = success ? "SUCCESS" : "FAILURE(S)";
                Console.WriteLine(text);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // If error make font red and print error to console.
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
            Console.WriteLine("Completed... press any key to close window");
            Console.ReadKey();
        }



        private static bool Execute(string[] args)
        {
            bool success = true;
            string fileName = ConfigurationManager.AppSettings["filepath"];
            if (fileName == null || fileName == "") Console.WriteLine("No location stated in config file.");

            List<ScriptResult> resultSet = new List<ScriptResult>();
            List<Script> scripts = XmlHelper.MapScriptsFromXml(fileName);

            for (int i = 0; i < scripts.Count; i++)
            {
                var script = scripts[i];

                //Get contents of a fluent script file
                string fileContent = FluentHelper.GetFluentScriptFileContent(script.Name);
                
                // 1. Create instance of interpreter.
                var ip = FluentHelper.GetInterpreter();

                // 2. Check if emtpy file.
                if (!string.IsNullOrEmpty(fileContent))
                {
                    // 3. Execute script.
                    Console.Write("Executing : " + script.Name);
                    ip.Execute(fileContent);  

                    // 4. Check result.
                    ScriptResult result = new ScriptResult();
                    result.FilePath = script.Name;
                    result.Duration = ip.Result.Duration.TotalMilliseconds;
                    result.Succeed = ip.Result.Success;
                    if (!result.Succeed)
                    {
                        WriteScriptError(script.Name, ip.Result.Message);
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                    CheckExpectedResults(ip, fileContent, script, result);

                    if (!result.Succeed)
                    {
                        success = false;
                        WriteExpectedResultsFailedError();
                        // highlight
                    }

                    resultSet.Add(result);
                }
            }

            // Writes result in Xml file
            LogHelper.WriteInLogFile(resultSet);
            return success;
        }



        private static void CheckExpectedResults(Interpreter ip, string fileContent, Script script, ScriptResult result)
        {
            // Load the expected results from filecontent... if they are there.
            LoadExpectedResults(fileContent, script, result);
            
            // Check that each expected result is correct.
            for (int z = 0; z < script.ExceptedValues.Count; z++)
            {
                var resultData = new ExpectedData();
                var exceptedValue = script.ExceptedValues[z];
                // Result after executing a script
                
                object actualValue = ip.Memory.Get<object>(exceptedValue.Name);
                actualValue = ((LObject) actualValue).GetValue();

                // Check if the expected value matched
                resultData.Actual = actualValue.ToString();
                resultData.Expected = exceptedValue.Value;
                resultData.Name = exceptedValue.Name;
                resultData.IsMatched = FluentHelper.IsMatchingValue(actualValue, exceptedValue.DataType, exceptedValue.Value);
                
                // The script result is a failure if at least 1 of the expected results fails.
                if (resultData.IsMatched)
                {
                    //Console.WriteLine(exceptedValue.Name + " matched : " + actualValue.ToString() + " with : " + exceptedValue.Value);
                }
                else
                {
                    result.Succeed = false;
                    //Console.WriteLine(exceptedValue.Name + " did not match");
                }

                result.ExpectedResults.Add(resultData);
            }
        }


        private static void LoadExpectedResults(string fileContent, Script script, ScriptResult result)
        {
            // If config does not have <expected results> for script..
            // then look at the top of the script.
            if (script.ExceptedValues == null || script.ExceptedValues.Count == 0)
            {
                if (fileContent.StartsWith("/* @hasexpects = true"))
                {
                    var expectedValues = XmlHelper.GetExpectedResultsFromFile(fileContent);
                    script.ExceptedValues = expectedValues;
                }
            }
        }


        private static void WriteScriptError(string script, string error)
        {
            Console.ForegroundColor =ConsoleColor.Red;            
            Console.WriteLine(script + "failed with error: " + error);
            Console.ResetColor();
        }


        private static void WriteExpectedResultsFailedError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" failed expected results");
            Console.ResetColor();
        }
    }
}
