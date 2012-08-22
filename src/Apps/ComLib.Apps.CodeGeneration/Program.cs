using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using ComLib;
using ComLib.Data;
using ComLib.Entities;
using ComLib.Extensions;
using ComLib.Application;
using ComLib.Arguments;
using ComLib.Logging;
using ComLib.Environments;
using ComLib.EmailSupport;
using ComLib.Configuration;
using ComLib.IO;
using ComLib.CodeGeneration;
using ComLib.Models;
using ComLib.LocationSupport;
//using ComLib.WebModules.Products;
//using ComLib.WebModules.Events;

namespace CommonLibrary.CodeGeneration
{
    /// <summary>
    /// Full sample application using the CommonLibrary.NET framework.
    /// </summary>
    public class CodeGeneratorApplication : App
    {
        /// <summary>
        /// Run the application via it's interface ( Init - Execute - Shutdown )
        /// using the static Run utility method.
        /// </summary>
        /// <param name="args">command line arguments.
        /// e.g. -env:Prod,Dev -date:${today-1} -config:config\prod.config,config\dev.config -source:Reuters 10</param>
        static int Main(string[] args)
        {
            int result = Run(new CodeGeneratorApplication(), args, false, "log,diagnostics").AsExitCode();
            return result;
        }


        public override BoolMessageItem Execute(object context)
        {
            ConnectionInfo conn = new ConnectionInfo("Server=kishore_pc1;Database=testdb1;User=testuser1;Password=password;", "System.Data.SqlClient");
            SampleModels models = new SampleModels(conn);
            var ctx = models.GetModelContext();

            // 1. MAKE SURE TO SET THE FOLDER PATHS IN THE SAMPLE MODELS.            
            // This only builds all the code, sql install scripts, repository for Model "Event" in the sample models.
            ComLib.CodeGeneration.CodeBuilder.CreateAll(ctx, "Event");

            // This builds all the code, sql install scripts, repository for ALL MODELS in the Sample Models.
            //ComLib.CodeGeneration.CodeBuilder.CreateAll(ctx);
            
            return BoolMessageItem.True;
        }
    }
}
