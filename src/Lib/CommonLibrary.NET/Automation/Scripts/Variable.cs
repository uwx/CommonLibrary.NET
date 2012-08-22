using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Automation
{
    /// <summary>
    /// Represents a variable in the script
    /// </summary>
    public class Variable
    {
        /// <summary>
        /// Name of the variable.
        /// </summary>
        public string Name;
        

        /// <summary>
        /// Value of the variable
        /// </summary>
        public object Value;


        /// <summary>
        /// Type of the variable
        /// </summary>
        public Type DataType;
    }
}
