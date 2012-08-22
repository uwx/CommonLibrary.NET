using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Collections;
using ComLib.Arguments;
using ComLib.Application;
using ComLib.Lang;
using ComLib.Lang.Templating;


namespace ComLib.Apps.FluentSharp
{
    /// <summary>
    /// FluentSharp executeable.
    /// </summary>
    public class FS : App
    {
        private FSArgs _fsargs;


        /// <summary>
        /// Execute the program.
        /// </summary>
        /// <param name="args"></param>
        public static int Main(string[] args)
        {
            int result = Run(new FS(), args, "log,diagnostics").AsExitCode();
            return result;
        }


        /// <summary>
        /// Set the settings to indictate that 
        /// 1. Command line arguments are required
        /// 2. Command line args should be transferred to args reciever.
        /// </summary>
        public FS()
        {
            _fsargs = new FSArgs();
            Settings.ArgsReciever = _fsargs;
            Settings.ArgsRequired = true;
            Settings.ArgsAppliedToReciever = true;
        }


        public override void Init()
        {
            base.Init();
            var file = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Console.WriteLine(file.FullName);
            if (_args.Contains("logfolder"))
            {
                _fsargs.OutPutFolder = file.Directory.FullName;
            }
            if (_args.Contains("outputfolder"))
            {
                _fsargs.LogFolder = file.Directory.FullName;
            }
        }


        /// <summary>
        /// Execute application.
        /// </summary>
        public override BoolMessageItem  Execute()
        {
            // Get the script or template.
            FileInfo file = new FileInfo(_fsargs.FilePath);
            string script = File.ReadAllText(file.FullName);
            Interpreter i = new Interpreter();
            i.Context.Settings.EnablePrinting = true;
            i.Context.Settings.EnableLogging = true;
            if (_fsargs.IsTemplate)
            {
                var finalscript = Templater.Render(script);
                File.WriteAllText(_fsargs.OutPutFolder + "\\" + file.Name, finalscript);

                // Interpret the rendered script.
                i.Execute(finalscript);
                string buffer = i.Memory.Get<string>("buffer");
                File.WriteAllText(_fsargs.OutPutFolder + "\\" + file.Name.Replace(".js", ".html"), buffer);
            }
            // Convert to tokens.
            else if (_fsargs.Tokenize)
            {
                // Interpret the rendered script.
                i.PrintTokens(file.FullName, _fsargs.OutPutFolder + "\\" + file.Name.Replace(".js", ".tokens.txt"));
            }
            // Just execute
            else 
            {
                i.Context.Plugins.RegisterAll();
                i.Execute(script);
            }
            _result = new BoolMessageItem(1, true, string.Empty);
            return _result;
        }


        /// <summary>
        /// Just showing how to override and populate any other sub-stitution
        /// values for the template.
        /// </summary>
        /// <param name="msg"></param>
        public override void Notify(IDictionary msg)
        {  
        }
    }
}
