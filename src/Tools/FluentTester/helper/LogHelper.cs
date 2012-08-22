using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Tools.FluentTester.Entities;
using System.Configuration;

namespace ComLib.Tools.FluentTester.Helpers
{
    public static class LogHelper
    {
        /// <summary>
        /// Writes result in Xml file
        /// </summary>
        /// <param name="resultSet"></param>
        public static void WriteInLogFile(List<ScriptResult> resultSet)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(resultSet.GetType());

            System.IO.StreamWriter file = new System.IO.StreamWriter(ConfigurationManager.AppSettings["logfilepath"]);
            x.Serialize(file, resultSet);
            file.Close();
        }
    }
}
