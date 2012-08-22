/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections;
using ComLib;
using ComLib.Application;
using ComLib.Arguments;
using ComLib.Logging;
using ComLib.Environments;
using ComLib.EmailSupport;
using ComLib.Configuration;


namespace ComLib.Apps.StockMarketApp
{

    /// <summary>
    /// Full sample application using the CommonLibrary.NET framework.
    /// </summary>
    public class StockMarketApplication : App
    {
        /// <summary>
        /// Run the application via it's interface ( Init - Execute - Shutdown )
        /// using the static Run utility method.
        /// </summary>
        /// <param name="args">command line arguments.
        /// e.g. -env:Prod,Dev -date:${today-1} -config:config\prod.config,config\dev.config -source:Reuters 10</param>
        static int Main(string[] args)
        {
            Args.InitServices((textargs) => ComLib.LexArgs.ParseList(textargs), (arg) => ComLib.Subs.Substitutor.Substitute(arg));
            int result = Run(new StockMarketApplication(), args, "log,diagnostics,email").AsExitCode();
            return result;
        }


        /// <summary>
        /// Set the settings to indictate that 
        /// 1. Command line arguments are required
        /// 2. Command line args should be transferred to args reciever.
        /// </summary>
        public StockMarketApplication()
        {
            Settings.ArgsReciever = new StockMarketAppArgs();
            Settings.ArgsRequired = true;
            Settings.ArgsAppliedToReciever = true;
        }


        /// <summary>
        /// INHERITANCE
        /// 1. -env:Prod -date:${today}   -config:prod.config,dev.config -log:{env}.log -source:Bloomberg 10
        /// 2. -env:Prod -date:${today-1} -config:prod.config,dev.config -log:{env}.log -source:Reuters 10
        /// </summary>
        public override void Init()
        {
            // base.Init(). Does everything below.
            // 1. Initialize the environment
            // 2. Append the file logger.
            // 3. Initialize config inheritance.
            // 4. Set the config, logger, emailer instances on the application.
            base.Init();
        }


        /// <summary>
        /// Dislay the startup / end information.
        /// </summary>
        /// <remarks>Just overriding to show how to extend the summary display with 
        /// some additional key/values.
        /// </remarks>
        /// <param name="isStart"></param>
        public override void Display(bool isStart, IDictionary summaryInfo)
        {
            // The base method displays important information such as starttime, endtime, duration, etc.
            // This just show to to add extra summary information.
            summaryInfo["RunType"] = "batch_mode";
            base.Display(isStart, summaryInfo);
        }


        /// <summary>
        /// Execute the core logic of the application.
        /// </summary>
        /// <remarks>Note this does not need to be inside of a try-catch 
        /// if using the ApplicationDecorator.</remarks>
        public override BoolMessageItem  Execute()
        {
            Log.Info("Executing application : " + Conf.Get<string>("Global", "ApplicationName"), null, null);                        
            _result = BoolMessageItem.True;            
            return _result;
        }


        /// <summary>
        /// Shutdown the application.
        /// </summary>
        public override void ShutDown()
        {
            Log.Info("Shutting down", null, null);
        }


        /// <summary>
        /// Just showing how to override and populate any other sub-stitution
        /// values for the template.
        /// </summary>
        /// <param name="msg"></param>
        public override void Notify(IDictionary msg)
        {
            // Allow the base class to send the email.
            msg["application"] = Conf.Get<string>("Application", "name");
            msg["subject"] = _result.Success ? "StockMarketApp Successful" : "StockMarketApp FAILED";
            msg["body"] = _result.Success ? "Success" : _result.Message;               
            base.Notify(msg);
        }
    }
}
