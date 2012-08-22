using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Lang.Docs;


namespace ComLib.Lang
{
    /// <summary>
    /// Meta data about function.
    /// </summary>
    public class FunctionMetaData
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public FunctionMetaData()
        {
        }


        /// <summary>
        /// Initailize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argNames"></param>
        public FunctionMetaData(string name, List<string> argNames)
        {
            Init(name, argNames);
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argNames"></param>
        public void Init(string name, List<string> argNames)
        {
            this.Name = name;
            if (argNames != null && argNames.Count > 0)
            {
                this.Arguments = new List<Arg>();
                this.ArgumentNames = new Dictionary<string, string>();
                this.ArgumentsLookup = new Dictionary<string, Arg>();
                for(int ndx = 0; ndx < argNames.Count; ndx++)
                {
                    var argName = argNames[ndx];
                    var arg = new Arg() { Name = argName };
                    arg.Index = ndx;
                    this.Arguments.Add(arg);
                    this.ArgumentsLookup[argName] = arg;
                    this.ArgumentNames[argName] = argName;
                }
            }
        }


        /// <summary>
        /// Function declaration
        /// </summary>
        public string Name;


        /// <summary>
        /// The doc tags for this function.
        /// </summary>
        public DocTags Doc;


        /// <summary>
        /// The aliases for the function name.
        /// </summary>
        public List<string> Aliases;


        /// <summary>
        /// Lookup for all the arguments.
        /// </summary>
        public IDictionary<string, Arg> ArgumentsLookup;


        /// <summary>
        /// Lookup for all the arguments names
        /// </summary>
        public IDictionary<string, string> ArgumentNames;


        /// <summary>
        /// Names of the parameters
        /// </summary>
        public List<Arg> Arguments;


        /// <summary>
        /// Whether or not this function can be used as a suffix for a single parameter
        /// e.g. 3 hours
        /// </summary>
        public bool IsSuffixable { get; set; }


        /// <summary>
        /// Whether or not this function supports a wild card.
        /// </summary>
        public bool HasWildCard { get; set; }


        /// <summary>
        /// The version of the function. defaulted to 1.0.0.0.
        /// This to enable having multiple versions of a function ( future feature )
        /// </summary>
        public Version Version { get; set; }


        /// <summary>
        /// The return type of the function.
        /// </summary>
        public Type ReturnType { get; set; }


        /// <summary>
        /// Total arguments
        /// </summary>
        public int TotalArgs
        {
            get { return Arguments == null ? 0 : Arguments.Count; }
        }
    }
}
