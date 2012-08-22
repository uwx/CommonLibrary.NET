using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ComLib.Tools.FluentTester.Entities
{
    [Serializable]
    public class ScriptResult
    {
        public ScriptResult()
        {
            ExpectedResults = new List<ExpectedData>();
        }

    
        [XmlAttribute]
        public string FilePath { get; set; }


        [XmlAttribute]
        public bool Succeed { get; set; }


        [XmlAttribute]
        public double Duration { get; set; }


        public List<ExpectedData> ExpectedResults { get; set; }        
    }
}
