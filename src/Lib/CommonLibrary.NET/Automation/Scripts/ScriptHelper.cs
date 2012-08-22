using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ComLib;


namespace ComLib.Automation
{
    /// <summary>
    /// Contains helper methods for script
    /// </summary>
    class ScriptHelper
    {
        /// <summary>
        /// Tries to load the content as an xml document.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal static BoolMessageItem<XmlDocument> LoadXml(string content)
        {
            XmlDocument doc = new XmlDocument();
            bool success = true;
            string message = string.Empty;
            try
            {
                doc.LoadXml(content);
            }
            catch (Exception ex)
            {
                success = false;
                message = "Unable to load xml : " + ex.Message;
            }
            return new BoolMessageItem<XmlDocument>(doc, success, message);
        }
    }
}
