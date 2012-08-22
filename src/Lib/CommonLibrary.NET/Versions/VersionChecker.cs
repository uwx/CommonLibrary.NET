using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.IO;
using ComLib.Application;
using ComLib.Arguments;

namespace ComLib.VersionCheck
{
    public class VersionChecker : App
    {
        #region Private data
        private VersionContext _ctx;
        private DataTable _errors;
        private IDictionary<string, Func<VersionDefItem, BoolMessageItem<VersionDefItem>>> _handlerMap;
        private Action<BoolMessageItem<VersionDefItem>> _notifyCallback;
        private BoolMessageItem _checkResult;
        #endregion


        /// <summary>
        /// Create using arguments, version definition, config settings.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <param name="ctx">Version Definition context</param>
        /// <param name="checkResultHandler">Callback</param>
        public VersionChecker(Action<BoolMessageItem<VersionDefItem>> checkResultHandler)
        {
            _errors = new DataTable();
            _handlerMap = new Dictionary<string, Func<VersionDefItem, BoolMessageItem<VersionDefItem>>>();            
            _notifyCallback = checkResultHandler;
        }


        /// <summary>
        /// Check the result.
        /// </summary>
        public BoolMessageItem Result { get { return _checkResult; } }


        /// <summary>
        /// Get the list of arguments that are supported.
        /// Type sojara.commodities.exe --help to display all the options.
        /// </summary>
        /// <returns></returns>
        public override List<ArgAttribute> OptionsSupported
        {
            get
            {
                List<ArgAttribute> options = new List<ArgAttribute>()
                {           
                    new ArgAttribute("name",    "Name representing this check", typeof(string), true, "", "ComJane_1.1.3, LimMds_1.3"),
                    new ArgAttribute("definition", "Path to xml definition file", typeof(string), true, "", @"CommoditiesJane_1.1.3.xml"),
                    new ArgAttribute("email",    "The person/group to send email to.", typeof(string), false, "",  "kishore.reddy@credit-suisse.com"),
                    new ArgAttribute("filter",    "List of file extensions to include", typeof(string), false, "xml,dll", "xml,dll,config,exe"),
                    new ArgAttribute("pause",   false, "Pause sojara for debugging", typeof(bool), false, false, false, true, "true|false")                    
                };
                return options;
            }
        }


        /// <summary>
        /// Get examples of the sojara command line.
        /// </summary>
        /// <returns></returns>
        public override List<string> OptionsExamples
        {
            get
            {
                List<string> examples = new List<string>()
                {
                    @"VersionChecker.exe -name=CommoditiesJane_1.1.2 -definition=config\CommoditiesJane.xml -email=Gunjan.Modha@credit-suisse.com",
                    @"VersionChecker.exe -name=CommoditiesJane_1.1.3 -definition=CommoditiesJane.xml -email=badarinath.vinjamuri@credit-suisse.com"
                };
                return examples;
            }
        }


        /// <summary>
        /// Accept or reject the arguments supplied on the command line.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override BoolMessageItem<Args> AcceptArgs(string[] args)
        {            
            BoolMessageItem<Args> result = base.AcceptArgs(args, "-", "=");
            if (result.Success)
            {
                string definitionFile = result.Item.NamedArgs["definition"];
                if (!File.Exists(definitionFile))
                {
                    string error = string.Format("Definition file '{0}' does not exist.", definitionFile);
                    Console.WriteLine( Environment.NewLine + error );
                    return new BoolMessageItem<Args>(result.Item, false, error);
                }
                // Create context and load definition from file.
                _ctx = new VersionContext();                
                _ctx.Definition = VersionImport.Load<VersionDefinition>(result.Item.NamedArgs["definition"]);
                _ctx.Definition.Name = result.Item.NamedArgs["name"];
            }
            return result;
        }


        /// <summary>
        /// Initialize the check.
        /// </summary>
        public override void  Initialize()
        {
            _errors.Clear();
            _errors.TableName = "VersionCheckErrors";
            _handlerMap.Clear();
            _errors.Columns.Clear();
            _errors.Columns.Add("CheckType", typeof(string));
            _errors.Columns.Add("Path", typeof(string));
            _errors.Columns.Add("IsRequired", typeof(bool));
            _errors.Columns.Add("Success", typeof(bool));
            _errors.Columns.Add("Message", typeof(string));

            _handlerMap[typeof(VersionDefDir).Name] = new Func<VersionDefItem, BoolMessageItem<VersionDefItem>>(VersionHelper.CheckDir);
            _handlerMap[typeof(VersionDefDrive).Name] = new Func<VersionDefItem, BoolMessageItem<VersionDefItem>>(VersionHelper.CheckDrive);
            _handlerMap[typeof(VersionDefEnv).Name] = new Func<VersionDefItem, BoolMessageItem<VersionDefItem>>(VersionHelper.CheckEnv);
            _handlerMap[typeof(VersionDefFile).Name] = new Func<VersionDefItem, BoolMessageItem<VersionDefItem>>(VersionHelper.CheckFile);
            _handlerMap[typeof(VersionDefReg).Name] = new Func<VersionDefItem, BoolMessageItem<VersionDefItem>>(VersionHelper.CheckReg);
        }
        

        /// <summary>
        /// Execute the check.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            try
            {
                Initialize();
                bool passed = true;
                string message = string.Empty;
                for (int ndx = 0; ndx < _ctx.Definition.Components.Count; ndx++)
                {
                    VersionDefItem defItem = _ctx.Definition.Components[ndx];                    
                    var result = Check(defItem);
                    StoreResult(result);
                    if (!result.Success)
                    {
                        passed = false;
                        message = "One or more Version checks failed.";
                    }
                }
                _checkResult = new BoolMessageItem(null, passed, message);
                WriteDefinitionResult();
            }
            catch (Exception ex)
            {
                _checkResult = new BoolMessageItem(null, false, ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return _checkResult;
        }


        /// <summary>
        /// Check the definition.
        /// </summary>
        /// <param name="defItem"></param>
        /// <returns></returns>
        public virtual BoolMessageItem<VersionDefItem> Check(VersionDefItem defItem)
        {
            // Valid definition item.
            string key = defItem.GetType().Name;
            if(!_handlerMap.ContainsKey(key))
                return new BoolMessageItem<VersionDefItem>(defItem, false, "Unknown version definition item : " + defItem.ToString());

            // Get handler
            Func<VersionDefItem, BoolMessageItem<VersionDefItem>> handler = _handlerMap[key];

            // Check.
            BoolMessageItem<VersionDefItem> result = handler(defItem);

            // Notify subscribers.
            if(_notifyCallback != null) _notifyCallback(result);

            return result;
        }
        

        public override void Notify()
        {
            string emailTo = ConfigurationSettings.AppSettings["emailTo"];
            string emailFrom = ConfigurationSettings.AppSettings["emailFrom"];
            string subject = ConfigurationSettings.AppSettings["subject"];
            string pass = _checkResult.Success ? "Successful" : "Failure";

            if (string.IsNullOrEmpty(subject)) subject = _ctx.Definition.Name + " Version Check " + pass;            
            string emailEnabled = ConfigurationSettings.AppSettings["enableEmails"].ToLower().Trim();
            bool enableEmails = Convert.ToBoolean(emailEnabled);

            // Override from command line.
            if (_commandLineArgs.Length >= 2)
                emailTo = _parsedArgs.NamedArgs["email"];

            string failuresOnly = ConfigurationSettings.AppSettings["enableEmailsOnlyOnFailures"].ToLower().Trim();
            bool enableEmailOnlyOnFailure = Convert.ToBoolean(failuresOnly);

            if (enableEmails && !enableEmailOnlyOnFailure
                || enableEmails && enableEmailOnlyOnFailure && !_checkResult.Success)
            {
                string message = string.Format("Version Check {0} for {1}", pass, _ctx.Definition.Name);
                if (!_checkResult.Success)
                {
                    message += Environment.NewLine + _checkResult.Message;
                    message += Environment.NewLine + GetCheckFailuresText();
                }
                else
                {
                    message += Environment.NewLine + BuildDisplay(false, null);
                }
                EmailUtils.Send(emailTo, emailFrom, subject, message);
            }
        }


        /// <summary>
        /// Populate additional useful summary information.
        /// </summary>
        /// <param name="isStart"></param>
        /// <param name="summaryInfo"></param>
        /// <returns></returns>
        public override string BuildDisplay(bool isStart, System.Collections.IDictionary summaryInfo)
        {
            if (!isStart)
            {
                if (summaryInfo == null) summaryInfo = new OrderedDictionary();
                summaryInfo["Check Result"] = _checkResult.Success.ToString();
                summaryInfo["Check Name"] = _ctx.Definition.Name;
                summaryInfo["Check Def"] = _parsedArgs.NamedArgs["definition"];
            }
            string summary = base.BuildDisplay(isStart, summaryInfo);
            return summary;
        }


        public virtual void ShutDown()
        {
        }


        #region Private Helpers
        private void StoreResult(BoolMessageItem<VersionDefItem> result)
        {
            if (!result.Success)
            {
                DataRow row = _errors.NewRow();
                row["IsRequired"] = result.Item.IsRequired;
                row["Success"] = result.Success;
                row["CheckType"] = result.Item.GetType().Name;
                row["Path"] = result.Item.ToString();
                row["Message"] = result.Message;
                _errors.Rows.Add(row);
            }
        }


        private void WriteDefinitionResult()
        {
            try
            {
                string fileName = @"..\log\" + _ctx.Definition.Name.Replace(" ", "_") + "CheckResults.xml";

                // Log Errors.
                _errors.WriteXml(fileName);
            }
            catch(Exception){}
        }


        private string GetCheckFailuresXml()
        {
            StringWriter writer = new StringWriter();
            _errors.WriteXml(writer);
            string xml = writer.ToString();
            return xml;
        }


        private string GetCheckFailuresText()
        {
            StringBuilder buffer = new StringBuilder();

            foreach (DataRow row in _errors.Rows)
            {
                string required = row["IsRequired"].ToString().ToLower() == "false" ? "Optional" : "Required";
                string passed   = row["Success"].ToString().ToLower() == "false" ? "Failed" : "Passed";


                string result = string.Format("{0} - {1} - {2} - {3} - {4}", row["CheckType"].ToString(), 
                    passed, required, row["Path"].ToString(), row["Message"].ToString());

                buffer.Append(Environment.NewLine + Environment.NewLine + result);
            }
            string message = buffer.ToString();
            return message;
        }
        #endregion
    }
}
