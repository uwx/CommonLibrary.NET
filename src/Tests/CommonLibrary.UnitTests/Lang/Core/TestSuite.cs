using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Tests.Common
{
    /// <summary>
    /// Used to store a collection of testcases.
    /// </summary>
    public class LangTestSuite
    {
        private List<Tuple<string, Type, object, string>> _tests = new List<Tuple<string, Type, object, string>>();


        /// <summary>
        /// Adds a test case to the collection.
        /// </summary>
        /// <param name="resultVarName">The name of the variable containing the resulting calculation/value of the script.</param>
        /// <param name="resultType">The type of the result variable</param>
        /// <param name="resultValue">The expected result value</param>
        /// <param name="script">The script to execute that will create the result variable</param>
        /// <returns></returns>
        public LangTestSuite TestCase(string resultVarName, Type resultType, object resultValue, string script)
        {
            var test = new Tuple<string, Type, object, string>(resultVarName, resultType, resultValue, script);
            _tests.Add(test);
            return this;
        }


        /// <summary>
        /// Get all the tests in this collection.
        /// </summary>
        public List<Tuple<string, Type, object, string>> Tests { get { return _tests; } }
    }
}
