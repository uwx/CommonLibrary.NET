using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ComLib.Tools.FluentTester.Entities
{
    [Serializable]
    public class ExpectedData
    {
        [XmlAttribute]
        public bool IsMatched { get; set; }


        [XmlAttribute]
        public string Name { get; set; }


        [XmlAttribute]
        public string Expected { get; set; }


        [XmlAttribute]
        public string Actual { get; set; }
    }
}
