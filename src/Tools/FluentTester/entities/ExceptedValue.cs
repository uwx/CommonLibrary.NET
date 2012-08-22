using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Tools.FluentTester.Entities
{
    public class ExceptedValue
    {
        /// <summary>
        /// The name of the variable.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// The type of the variable to check.
        /// </summary>
        public string DataType { get; set; }


        /// <summary>
        /// The expected value of the variable.
        /// </summary>
        public string Value { get; set; } 
    }
}
