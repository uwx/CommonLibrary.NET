using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Tools.FluentTester.Entities;
using System.Xml;

namespace ComLib.Tools.FluentTester.Helpers
{
    public static class XmlHelper
    {
        /// <summary>
        /// Load xml using a file location
        /// </summary>
        /// <param name="xmlLoaction">File location</param>
        /// <returns></returns>
        public static List<Script> MapScriptsFromXml(string xmlLoaction)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlLoaction);
            return MapScriptsFromXml(doc);
        }


        public static List<ExceptedValue> GetExpectedResultsFromFile(string fileContent)
        {
            int ndxStart = fileContent.IndexOf("<expects>");
            int ndxEnd = fileContent.IndexOf("</expects>");
            string text = fileContent.Substring(ndxStart, (ndxEnd - ndxStart) + 10);
            var xml = new XmlDocument();
            xml.LoadXml(text);
            var doc = xml.DocumentElement;
            var expectedValues = new List<ExceptedValue>();

            for (int ndx = 0; ndx < doc.ChildNodes.Count; ndx++)
            {
                ExceptedValue ex = new ExceptedValue();
                XmlNode result = doc.ChildNodes[ndx];

                ex.Name = result.Attributes.GetNamedItem("name").Value;
                ex.DataType = result.Attributes.GetNamedItem("type").Value;
                ex.Value = result.Attributes.GetNamedItem("value").Value;
                ex.Name = ex.Name.Trim();
                expectedValues.Add(ex);
            }
            return expectedValues;
        }  


        /// <summary>
        /// Returns list of Script objects
        /// </summary>
        /// <param name="doc">Xml document</param>
        /// <returns>List of Script</returns>
        public static List<Script> MapScriptsFromXml(XmlDocument doc)
        {
            List<Script> scripts = new List<Script>();
            XmlNodeList scriptNodes = doc.GetElementsByTagName("script");

            for (int i = 0; i < scriptNodes.Count; i++)
            {
                Script script = new Script();
                XmlNode scriptNode = scriptNodes[i];
                script.Name = scriptNode.Attributes.GetNamedItem("path").Value;

                for (int y = 0; y < scriptNode.ChildNodes.Count; y++)
                {
                    ExceptedValue ex = new ExceptedValue();
                    XmlNode result = scriptNode.ChildNodes[y];                    
                    ex.Name = result.Attributes.GetNamedItem("name").Value;
                    ex.DataType = result.Attributes.GetNamedItem("type").Value;
                    ex.Value = result.Attributes.GetNamedItem("value").Value;
                    ex.Name = ex.Name.Trim();
                    script.ExceptedValues.Add(ex);
                }
                scripts.Add(script);
            }

            return scripts;
        }
    }
}
