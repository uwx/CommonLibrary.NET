using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.VersionCheck
{
    public class Versions
    {
        /// <summary>
        /// Check the system using the definition / settings provided.
        /// </summary>
        /// <param name="def"></param>
        /// <param name="config"></param>
        /// <param name="checkResultHandler"></param>
        /// <returns></returns>
        public static BoolMessage Check(string[] args, VersionDef def, VersionConfig config, 
            Action<BoolMessageItem<VersionBase>> checkResultHandler)
        {
            var checker = new VersionCheckerApp(args, def, config, checkResultHandler);
            checker.Init();
            BoolMessageItem result = checker.Execute();
            checker.ShutDown();
            return result;
        }
    }
}
