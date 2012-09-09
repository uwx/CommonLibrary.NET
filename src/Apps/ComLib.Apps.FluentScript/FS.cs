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
            _args = FSHelper.ParseArgs(args);
        }


        /// <summary>
        /// Validates the settings.
        /// </summary>
        /// <returns></returns>
        public BoolMsgItem Validate()
        {
            return FSHelper.Validate(_args);
        }


        /// <summary>
        /// Execute application.
        /// </summary>
        public BoolMsgItem  Execute()
        {
            var result = Validate();
            if (!result.Success)
                return result;

            // Get the script or template.
            var file = new FileInfo(_args.FilePath);
            var script = File.ReadAllText(file.FullName);
            var i = new Interpreter();
            i.Context.Settings.EnablePrinting = true;
            i.Context.Settings.EnableLogging = true;
            if (_args.IsTemplate)
            {
                var finalscript = Templater.Render(script);
                File.WriteAllText(_args.OutPutFolder + "\\" + file.Name, finalscript);

                // Interpret the rendered script.
                i.Execute(finalscript);
                string buffer = i.Memory.Get<string>("buffer");
                File.WriteAllText(_args.OutPutFolder + "\\" + file.Name.Replace(".js", ".html"), buffer);
            }
            // Convert to tokens.
            else if (_args.Tokenize)
            {
                // Interpret the rendered script.
                i.PrintTokens(file.FullName, _args.OutPutFolder + "\\" + file.Name.Replace(".js", ".tokens.txt"));
            }
            // Just execute
            else 
            {
                if (_args.PluginGroup == "sys")
                    i.Context.Plugins.RegisterAllSystem();
                else if(_args.PluginGroup == "all")
                    i.Context.Plugins.RegisterAll();

                i.Execute(script);
            }

            var exitCode = i.Result.Success ? 0 : 1;
            var finalResult = new BoolMsgItem(i.Result.Success, i.Result.Message, exitCode);
            return finalResult;
        }
    }
}
