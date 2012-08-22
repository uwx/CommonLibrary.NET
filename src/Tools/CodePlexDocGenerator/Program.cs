using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;

using CodePlexDocGenerator.Lib;

namespace CodePlexDocGenerator
{
    /// <summary>
    /// This program generates the Codeplex wiki examples page using the 
    /// examples of the CommonLibrary.Net project.
    /// 
    /// To do that, the program utilizes a template file which defines the 
    /// format of an example. In addition to the template file, the program
    /// reads the _Samples\_Examples.xml file in order to get more directives
    /// about its processing.
    /// 
    /// This template file contains wiki markup code as needed and may also 
    /// contain the following replacement strings:
    /// 
    /// ${componentname}: Substituted with the example name.
    /// ${desc}: Substituted by the example description, as that is extracted
    ///          from the _Examples.xml file.
    /// ${examplecode}: Substituted with the actual example code.
    /// ${filepath}: Substituted with the path to the example source code file.
    /// ${dategenerated}: Substituted with the current date.
    /// ${comlibversion}: Substituted with the CommonLibrary application version.
    /// ${coresource}: Substituted with the core source file, as that is
    ///                located in the _Examples.xml file.
    /// ${changset}: Substituted with the change set number.
    /// ${sourcecontrolfileid}: Substituted with the source control file id.
    /// 
    /// The program can be executed with or without arguments. If it's executed
    /// without arguments, the program will prompt the user for its arguments.
    /// Otherwise, the following four arguments are expected in the order shown
    /// below:
    /// 
    /// * Changeset number.
    /// * Name of template file.
    /// * Path to CommonLibrary source code.
    /// * Wiki output file.
    /// 
    /// </summary>
    class Program
    {
        /// <summary>
        /// Default template location.
        /// </summary>
        public static string defaultTemplate = "..\\..\\template.txt";


        /// <summary>
        /// Default location of CommonLibrary source code.
        /// </summary>
        public static string defaultComLib = "..\\..\\..\\..\\Lib\\CommonLibrary.Net";


        /// <summary>
        /// Default wiki output file.
        /// </summary>
        public static string defaultWiki = "wiki.txt";


        /// <summary>
        /// Default ignore doc behavior.
        /// </summary>
        public static string defaultIgnoreDoc = "true";


        /// <summary>
        /// Prompts for interactive parameter prompting.
        /// </summary>
        public static string[] prompts = {"Changset number [no default]: ",
                                          "Template file [default " + defaultTemplate + "]: ",
                                          "Path to ComLib source [default " + defaultComLib + "]: ",
                                          "Wiki output file [default " + defaultWiki + "]: ",
                                          "Path to the xml documentation instruction file [default " + defaultXmlDoc + "]: "
                                         };


        /// <summary>
        /// Path to the xml file representing the documentation instructions.
        /// </summary>
        public static string defaultXmlDoc = "\\_Samples\\_Examples.xml";


        /// <summary>
        /// Path to _Examples.xml with details about wiki creation.
        /// </summary>
        public static string pathToXML = "\\_Samples\\_Examples.xml";


        /// <summary>
        /// Relative path to assemblyversion.cs file.
        /// </summary>
        public static string pathToAssembly = "\\properties\\assemblyversion.cs";


        /// <summary>
        /// Relative path to the samples folder.
        /// </summary>
        public static string pathToExamples = "\\_Samples";


        /// <summary>
        /// Substitution string for the component name.
        /// </summary>
        public static string substComponent = "${componentname}";


        /// <summary>
        /// The number of the number
        /// </summary>
        public static string componentNumber = "${componentnumber}";


        /// <summary>
        /// Substitution string for the description.
        /// </summary>
        public static string substDesc = "${desc}";


        /// <summary>
        /// Substitution string for the example code.
        /// </summary>
        public static string substExampleCode = "${examplecode}";


        /// <summary>
        /// Substitution string for the component name.
        /// </summary>
        public static string substFilePath = "${filepath}";


        /// <summary>
        /// Substitution string for the generation date.
        /// </summary>
        public static string substDateGenerated = "${dategenerated}";


        /// <summary>
        /// Substitution string for the CommonLibrary assembly version.
        /// </summary>
        public static string substComlibVersion = "${comlibversion}";


        /// <summary>
        /// Substitution string for the core source file.
        /// </summary>
        public static string substCoreSource = "${coresource}";


        /// <summary>
        /// Substitution string for the changset number.
        /// </summary>
        public static string substChangset = "${changeset}";


        /// <summary>
        /// Substitution string for the file id.
        /// </summary>
        public static string substFileId = "${sourcecontrolfileid}";

        /// <summary>
        /// Doc directive for start using section.
        /// </summary>
        public static string docStartUsing = "//<doc:using>";


        /// <summary>
        /// Doc directive for end using section.
        /// </summary>
        public static string docEndUsing = "//</doc:using>";


        /// <summary>
        /// Doc directive for start execute section.
        /// </summary>
        public static string docStartExecute = "<doc:example>";


        /// <summary>
        /// Doc directive for end execute section.
        /// </summary>
        public static string docEndExecute = "</doc:example>";


        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                // Get the parameters.
                var settings = DocSettings.LoadFromConfig();
                if(ConfigurationManager.AppSettings["useCmdline"] == "true")
                    settings = GetParameters(args);

                // Read the xml.
                SortedList<int, Component> lst = ReadXML(settings.ComLib, settings.XML);

                // Read the template file.
                Console.WriteLine("Reading the template file...");
                string templateContents = GetFileContents(settings.Template);

                // Get the comlib version and the names of the example files.
                Console.WriteLine("Extracting the assembly version...");
                string comlibversion = GetComLibVersion(settings.ComLib + pathToAssembly);

                // Create the content for each one of the examples.
                Console.WriteLine("Reading the example source code files...");
                StringBuilder sb = new StringBuilder();
                foreach (int key in lst.Keys)
                {
                    Component comp = lst[key];
                    int compNum = key + 1;
                    if (comp.WikiEnabled)
                    {
                        string exampleFile = settings.ComLib + settings.Examples + "\\" + comp.ExampleFile;
                        if (WikiStyle.Xml_Markers == comp.Style)
                            sb.Append(Substitute(exampleFile, comp.GetDescription(), GetFileContents(exampleFile), 
                                                 templateContents, comlibversion,
                                                 comp.Name, comp.FileReference,
                                                 settings.ChangeSet, comp.FileId, false, compNum));
                        else if (WikiStyle.Full_File == comp.Style)
                            sb.Append(Substitute(exampleFile, comp.GetDescription(), GetFileContents(exampleFile), 
                                                 templateContents, comlibversion, 
                                                 comp.Name, comp.FileReference,
                                                 settings.ChangeSet, comp.FileId, true, compNum));
                        else
                            sb.Append(Substitute(exampleFile, comp.GetDescription(), "Example not available", 
                                                 templateContents, comlibversion, 
                                                 comp.Name, comp.FileReference,
                                                 settings.ChangeSet, comp.FileId, false, compNum));
                    }
                }

                // Write it out to the output file.
                Console.WriteLine("Writing the wiki output file...");
                using (StreamWriter sw = new StreamWriter(settings.DocFile, false, System.Text.Encoding.Default))
                {
                    sw.Write(sb.ToString());
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


        /// <summary>
        /// Reads the xml file with instructions to generate the wiki output.
        /// </summary>
        /// <param name="comlibPath">Path to ComLib source.</param>
        /// <param name="docXmlPath">Path to the document xml instructions file</param>
        /// <returns></returns>
        public static SortedList<int, Component> ReadXML(string comlibPath, string docXmlPath)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(comlibPath + docXmlPath);

            SortedList<int, Component> lst = new SortedList<int, Component>();
            foreach (DataRow dr in ds.Tables["component"].Rows)
            {
                Component comp = new Component(dr);
                lst.Add(comp.component_id, comp);
            }

            foreach (DataRow dr in ds.Tables["fileref"].Rows)
            {
                int compid = Convert.ToInt32(dr["component_id"]);
                lst[compid].FileReference = Convert.ToString(dr["name"]);
            }

            foreach (DataRow dr in ds.Tables["example"].Rows)
            {
                int compid = Convert.ToInt32(dr["component_id"]);
                lst[compid].WikiEnabled = Convert.ToBoolean(dr["wikienabled"]);
                WikiStyle wk = (WikiStyle)Enum.Parse(typeof(WikiStyle), Convert.ToString(dr["wikistyle"]), true);
                lst[compid].Style = wk;
                lst[compid].ExampleFile = Convert.ToString(dr["example_text"]);
                if (dr["fileid"] != DBNull.Value)
                    lst[compid].FileId = Convert.ToString(dr["fileid"]);
                else
                    lst[compid].FileId = string.Empty;
            }
            return lst;
        }

        /// <summary>
        /// Given the template contents, this method performs the
        /// substitutions necessary to create the final output for
        /// a single example.
        /// </summary>
        /// <param name="example">Location of example file.</param>
        /// <param name="descr">Description for example.</param>
        /// <param name="contents">File contents.</param>
        /// <param name="template">Template contents.</param>
        /// <param name="comlibversion">Version of ComLib.</param>
        /// <param name="componentName">Name of component.</param>
        /// <param name="coreSource">Core source file.</param>
        /// <param name="ignoreDoc">True to ignore (and remove) doc directives.</param>
        /// <returns>Wikified example.</returns>
        private static string Substitute(string example, string descr, string contents, 
                                         string template, string comlibversion, 
                                         string componentName, string coreSource, 
                                         string changset, string fileid,
                                         bool ignoreDoc, int compNum)
        {
            if (!ignoreDoc)
            {
                contents = RemoveExtraWhitespace(contents);
                contents = RemoveUnwantedUsingDirectives(contents);
                contents = RemovedUnwantedCode(contents);
            }

            // Strip doc directives anyway.
            contents = StripLineWithContent(contents, docStartExecute);
            contents = StripLineWithContent(contents, docEndExecute);
            contents = StripLineWithContent(contents, docStartUsing);
            contents = StripLineWithContent(contents, docEndUsing);
            char[] trim = {'\r', '\n'};
            contents = contents.Trim(trim);

            string content = template.Replace(substComponent, componentName);
            content = content.Replace(componentNumber, compNum.ToString());
            content = content.Replace(substDesc, descr);
            content = content.Replace(substCoreSource, coreSource);
            content = content.Replace(substExampleCode, contents);
            content = content.Replace(substChangset, changset);
            content = content.Replace(substFileId, fileid);
            // Apply {" and "} to escape wiki italics.
            content = content.Replace(substFilePath, "{\"" + example.Replace(example.Substring(0, example.LastIndexOf("\\")), "<Samples_Dir>\"}"));
            content = content.Replace(substDateGenerated, DateTime.Now.ToShortDateString());
            content = content.Replace(substComlibVersion, comlibversion);

            return content;
        }


        /// <summary>
        /// Removes extra whitespace from examples code.
        /// </summary>
        /// <param name="contents">Original example contents.</param>
        /// <returns></returns>
        /// <remarks>This is ugly.</remarks>
        private static string RemoveExtraWhitespace(string contents)
        {
            int startPos, endPos;
            startPos = contents.IndexOf(docStartExecute);
            endPos = contents.IndexOf(docEndExecute);
            if ((startPos < 0) || (endPos <= startPos))
                return contents;

            string[] separator = { "\r\n" };
            string[] lines = contents.Substring(startPos + docStartExecute.Length, endPos - startPos - docStartExecute.Length).Split(separator, StringSplitOptions.None);
            int minWhitespace = int.MaxValue;
            for (int i = 0; i < lines.GetUpperBound(0); i++)
            {
                lines[i] = lines[i].Replace("\t", "    ");
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    int curWhiteSpace = lines[i].Length - lines[i].TrimStart().Length;
                    if (minWhitespace > curWhiteSpace)
                        minWhitespace = curWhiteSpace;
                }
            }
            
            if (minWhitespace > 0)
            {
                StringBuilder newContent = new StringBuilder();
                for (int i = 0; i < lines.GetUpperBound(0); i++)
                    if (!string.IsNullOrEmpty(lines[i]))
                        newContent.Append(lines[i].Substring(minWhitespace) + "\r\n");
                    else
                        newContent.Append("\r\n");

                 contents = contents.Substring(0, startPos + docStartExecute.Length) + newContent.ToString() + contents.Substring(endPos);
            }

            return contents;
        }


        /// <summary>
        /// Removes all code that is not wrapped
        /// in our own doc execute directive.
        /// </summary>
        /// <param name="contents">Contents of code.</param>
        /// <returns>Code with removed original code.</returns>
        private static string RemovedUnwantedCode(string contents)
        {
            string[] splitChars = { "\r\n" };
            string[] split = contents.Split(splitChars, StringSplitOptions.None);
            bool foundDoc = false;
            bool inCode = false;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= split.GetUpperBound(0); i++)
            {
                if (split[i].StartsWith("namespace "))
                    inCode = true;

                if (!inCode)
                    sb.AppendLine(split[i]);
                else
                {
                    if (split[i].Contains(docStartExecute))
                        foundDoc = true;
                    else if (split[i].Contains(docEndExecute))
                        foundDoc = false;

                    if (foundDoc)
                        sb.AppendLine(split[i]);
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// Remove using directives that are not wrapped
        /// in our own doc directives.
        /// </summary>
        /// <param name="contents">Contents of code.</param>
        /// <returns>Code with removed using directives.</returns>
        private static string RemoveUnwantedUsingDirectives(string contents)
        {
            string[] splitChars = {"\r\n"};
            string[] split = contents.Split(splitChars, StringSplitOptions.None);
            bool foundDoc = false;
            for (int i = 0; i <= split.GetUpperBound(0); i++)
            {
                if (split[i].Contains(docStartUsing))
                    foundDoc = true;
                else if (split[i].Contains(docEndUsing))
                    foundDoc = false;
                if (!foundDoc && split[i].StartsWith("using "))
                    contents = StripLineWithContent(contents, split[i]);
            }
            return contents;
        }


        /// <summary>
        /// Removes a line from a string.
        /// </summary>
        /// <param name="contents">Original content.</param>
        /// <param name="exclude">Content of line to remove, if found.</param>
        /// <returns>String with removed lines.</returns>
        private static string StripLineWithContent(string contents, string exclude)
        {
            int pos;
            while ((pos = contents.IndexOf(exclude)) != -1)
            {
                int prevPos = pos > 1 ? pos -2 : 0;
                while ((prevPos > 0) && (contents.Substring(prevPos, 2) != "\r\n"))
                    prevPos -= 2;
                contents = contents.Replace(contents.Substring(prevPos, pos - prevPos + exclude.Length), "");
            }
            return contents;
        }


        /// <summary>
        /// Returns the comlib version.
        /// </summary>
        /// <param name="pathToAssemblyVersion">Path to ComLib assembly version.</param>
        /// <returns>String with ComLib assembly version.</returns>
        private static string GetComLibVersion(string pathToAssemblyVersion)
        {
            string asmContents = GetFileContents(pathToAssemblyVersion);
            return asmContents.Substring(asmContents.IndexOf("(\"") + 2, 7);
        }


        /// <summary>
        /// Returns the contents of a text file.
        /// </summary>
        /// <param name="pathToFile">Path to file to read.</param>
        /// <returns>File contents.</returns>
        private static string GetFileContents(string pathToFile)
        {
            using (StreamReader sr = new StreamReader(pathToFile, System.Text.Encoding.Default))
            {
                return sr.ReadToEnd();
            }
        }


        /// <summary>
        /// Returns the parameters from the command line arguments
        /// or from the console by prompting the user.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        /// <param name="changset">Changset number.</param>
        /// <param name="templateFile">Path to template file.</param>
        /// <param name="comlibPath">ComLib version.</param>
        /// <param name="wikiOutput">Output file for wiki text.</param>
        private static DocSettings GetParameters(string[] args)
        {
            var settings = new DocSettings();

            // Do we have exactly 3 arguments?
            if (args.GetLength(0) == 5)
            {
                // Yes, get the parameters from the command line arguments.
                settings.ChangeSet = args[0];
                settings.Template = args[1];
                settings.ComLib = args[2];
                settings.DocFile = args[3];
                settings.XML = args[4];
            }
            else
            {
                // No, prompt the user.
                settings.ChangeSet = GetParameter(prompts[0], string.Empty);
                settings.Template = GetParameter(prompts[1], defaultTemplate);
                settings.ComLib = GetParameter(prompts[2], defaultComLib);
                settings.DocFile = GetParameter(prompts[3], defaultWiki);
                settings.XML = GetParameter(prompts[4], defaultXmlDoc);
            }

            // Make sure the comlib path does not end with a \ char.
            if (settings.ComLib.EndsWith("\\"))
                settings.ComLib = settings.ComLib.Substring(0, settings.ComLib.Length - 1);
            
            return settings;
        }


        /// <summary>
        /// Prompts the user for a parameter and returns it.
        /// </summary>
        /// <param name="prompt">Prompt to the user.</param>
        /// <param name="defaultResp">Default response if enter is pressed.</param>
        /// <returns>User (or default) response.</returns>
        private static string GetParameter (string prompt, string defaultResp)
        {
            string rsp = string.Empty;
            do
            {
                Console.Write(prompt);
                rsp = Console.ReadLine();
                if (string.IsNullOrEmpty(rsp) && defaultResp != string.Empty)
                    rsp = defaultResp;
            }
            while (rsp == string.Empty);
            return rsp;
        }
    }
}
