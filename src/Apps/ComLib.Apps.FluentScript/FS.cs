using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using ComLib.Lang;
using ComLib.Lang.Templating;


namespace ComLib.Apps.FluentSharp
{
    /// <summary>
    /// FluentSharp executeable.
    /// </summary>
    public class FS 
    {
        private FSArgs _args;
        private string[] _cmdArgs;


        /// <summary>
        /// Execute the program.
        /// </summary>
        /// <param name="args"></param>
        public static int Main(string[] args)
        {
            var fs = new FS(args);
            var result = fs.Execute();
            return (int)result.Item;
        }


        /// <summary>
        /// Set the settings to indictate that 
        /// 1. Command line arguments are required
        /// 2. Command line args should be transferred to args reciever.
        /// </summary>
        public FS(string[] args)
        {
            _cmdArgs = args;
        }


        /// <summary>
        /// Execute application.
        /// </summary>
        public BoolMsgItem  Execute()
        {
            // Check if asking for help.
            if (IsHelp())
            {
                var sample = new FSArgs();
                Console.Write(sample.ToFullHelpText());
                return new BoolMsgItem(true, string.Empty, 0);
            }

            // 1. Load settings from config file
            LoadSettings();

            // 2. Override settings from command line arguments
            OverrideSettings();

            // 3. Setup defaults
            ApplyDefaultSettings();

            // 4. Validate the settings
            var result = ValidateSettings();
            if (!result.Success)
            {
                HandleValidationFailure(result);
                return new BoolMsgItem(false, result.Message, 1);
            }

            // 5. Create interpreter and run code.
            var i = CreateInterpreter();

            // 6. Execute the code
            ExecuteCode(i);            

            var exitCode = i.Result.Success ? 0 : 1;
            var finalResult = new BoolMsgItem(i.Result.Success, i.Result.Message, exitCode);
            return finalResult;
        }


        /// <summary>
        /// Load the settings.
        /// </summary>
        /// <returns></returns>
        private void LoadSettings()
        {
            _args = FSHelper.LoadSettings();
        }


        /// <summary>
        /// Override the settings from config with command line settings
        /// </summary>
        /// <returns></returns>
        private void OverrideSettings()
        {
            FSHelper.ParseArgs(_args, _cmdArgs);
        }


        /// <summary>
        /// Validates the settings.
        /// </summary>
        /// <returns></returns>
        private BoolMsgItem ValidateSettings()
        {
            return FSHelper.Validate(_args);
        }


        private void ApplyDefaultSettings()
        {
            if (_args.LogFolder == "logfiles")
                Directory.CreateDirectory(_args.LogFolder);
            if (_args.OutPutFolder == "outputfiles")
                Directory.CreateDirectory(_args.OutPutFolder);
        }


        private Interpreter CreateInterpreter()
        {
            var i = new Interpreter();

            // 1. Printing/logging.
            i.Context.Settings.EnablePrinting = true;
            i.Context.Settings.EnableLogging = true;

            // 2. What plugins to register?
            if (_args.PluginGroup == "sys")
                i.Context.Plugins.RegisterAllSystem();
            else if (_args.PluginGroup == "all")
                i.Context.Plugins.RegisterAll();

            return i;
        }


        private void HandleValidationFailure(BoolMsgItem result)
        {
            var orig = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Unable to execute. Some settings/inputs have errors:");
            Console.WriteLine(result.Message);
            Console.WriteLine("Type 'fs help' for more information");
            Console.ResetColor();
        }


        private void ExecuteCode(Interpreter i)
        {
            // 4. Setup environment - e.g folders.
            // Get the script or template.
            var file = new FileInfo(_args.FilePath);
            var script = File.ReadAllText(file.FullName);

            
            // CASE 1: Template file ( e.g. like asp.net syntax with fluentscript in between <% %>
            if (_args.IsTemplate)
            {
                var finalscript = Templater.Render(script);
                File.WriteAllText(_args.OutPutFolder + "\\" + file.Name, finalscript);

                // Interpret the rendered script.
                i.Execute(finalscript);
                string buffer = i.Memory.Get<string>("buffer");
                File.WriteAllText(_args.OutPutFolder + "\\" + file.Name.Replace(".js", ".html"), buffer);
            }
            // CASE 2: Just want to get the tokens from the script.
            else if (_args.Tokenize)
            {
                // Interpret the rendered script.
                i.PrintTokens(file.FullName, _args.OutPutFolder + "\\" + file.Name.Replace(".js", ".tokens.txt"));
            }
            // 3. CASE 3: Execute the script.
            else
            {
                Console.WriteLine();
                i.Execute(script);
                Console.WriteLine();
            }
        }


        private bool IsHelp()
        {
            if (_cmdArgs.Length == 0) return false;
            var first = _cmdArgs[0].ToLower();

            if (first == "?" || first.Contains("help"))
                return true;
            return false;
        }
    }
}
