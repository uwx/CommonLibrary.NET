using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Tools.FluentTester.Entities
{
    public class Script
    {
        public Script()
        {
            ExceptedValues = new List<ExceptedValue>();
        }


        public string Name { get; set; }


        public List<ExceptedValue> ExceptedValues { get; set; }
    }
}
