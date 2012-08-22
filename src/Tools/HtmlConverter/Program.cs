using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;


namespace ComLib.TextConverter
{

    /// </summary>
    public class HtmlConverter
    {
        private static List<TestScenario> _examples;


        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                InitScenarios();

                // Run tests on the conversion of each scenario.
                foreach (var example in _examples)
                {
                    var svc = new TextConversionService();
                    svc.RegisterAll();
                    var converted = svc.Convert(example.Source);

                    // Compare converted to expected.
                }
                Console.Write("Done.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error has occurred, please see the program output and the stack trace " +
                                  "to determine the cause of the error.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(ex.ToString());
            }
        }


        private static void InitScenarios()
        {
            _examples = new List<TestScenario>()
            {
                new TestScenario("! Heading 1", "<h1>Heading 1</h1>"),
                new TestScenario("!! Heading 2", "<h2>Heading 1</h2>"),
                new TestScenario("$ item 1", "<ul><li>item 1</li></ul>"),
                new TestScenario("*bold*", "<strong>bold</strong>"),
                new TestScenario("_italics_", "<i>italics</i>"),
                new TestScenario("+underline+", "<u>italics</u>"),
                new TestScenario("~strikethrough~", "<s>italics</s>"),
                new TestScenario("^superscript^", "<sup>superscript</sup>"),
                new TestScenario(",subscript,", "<sub>subscript</sub>"),
            };           
        }


        class TestScenario
        {
            public TestScenario(string source, string expected)
            {
                Source = source;
                Expected = expected;
            }


            public string Source;
            public string Expected;   
        }
    }
}
