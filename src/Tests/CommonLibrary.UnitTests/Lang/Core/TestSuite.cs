using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Tests.Common
{
    public class LangTestSuite
    {
        private List<Tuple<string, Type, object, string>> _testcases = new List<Tuple<string, Type, object, string>>();


        /// <summary>
        /// Build up a test case
        /// </summary>
        /// <param name="resultVarName"></param>
        /// <param name="resultType"></param>
        /// <param name="resultValue"></param>
        /// <param name="script"></param>
        public LangTestSuite TestCase(string resultVarName, Type resultType, object resultValue, string script)
        {
            var test = new Tuple<string, Type, object, string>(resultVarName, resultType, resultValue, script);
            _testcases.Add(test);
            return this;
        }


        /// <summary>
        /// Get the tests.
        /// </summary>
        public List<Tuple<string, Type, object, string>> Tests { get { return _testcases;  } }
    }
}
