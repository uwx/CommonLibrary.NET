using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;


using ComLib;
using ComLib.Lang;
using ComLib.Lang.Extensions;
using ComLib.Tests;

namespace ComLib.Lang.Tests.Common
{
    public class ScriptTestsBase
    {
        protected static new Tuple<string, Type, object, string> TestCase(string resultVarName, Type resultType, object resultValue, string script)
        {
            return new Tuple<string, Type, object, string>
                (resultVarName, resultType, resultValue, script);
        }

        protected void ExpectError(ILangPlugin plugin, string script, string messageErrorPart)
        {
            // Check errors.
            var it = new Interpreter();
            it.Context.Plugins.Register(plugin);
            it.Execute(script);
            Assert.IsFalse(it.Result.Success);
            Assert.IsTrue(it.Result.Message.Contains(messageErrorPart));
        }


        protected void ExpectError(Tuple<string, string, string> scenario)
        {
            var scenarios = new List<Tuple<string, string, string>>();
            scenarios.Add(scenario);
            ExpectErrors(scenarios);
        }


        protected void ExpectErrors(List<Tuple<string, string, string>> scenarios)
        {
            var i = new Interpreter();
            i.Context.Types.Register(typeof(Person), () => new Person());

            for (int ndx = 0; ndx < scenarios.Count; ndx++)
            {
                var scenario = scenarios[ndx];
                Console.WriteLine(scenario.Item3);
                i.Execute(scenario.Item3);
                Assert.IsFalse(i.Result.Success);
                Assert.IsNotNull(i.Result.Ex);
                Assert.IsTrue(i.Result.Message.StartsWith(scenario.Item1));
                if (scenario.Item2 != null)
                {
                    Assert.IsTrue(i.Result.Message.Contains(scenario.Item2));
                }
            }
        }


        /// <summary>
        /// Parses / executes a list of statements.
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="execute"></param>
        /// <param name="initializer"></param>
        /// <param name="replaceSemicolonsWithNewLines"></param>
        protected void RunTests(TestCases testCases, TestType testType,
                bool execute = true, Action<Interpreter> initializer = null,
                bool replaceSemicolonsWithNewLines = false, Action onNewScript = null)
        {
            var statements = testCases.Positive;
            Parse(statements, execute, (i) =>
            {
                if (initializer != null)
                    initializer(i);
                else
                {
                    //if (testCases.SetupPlugins != null && testCases.SetupPlugins.Length > 0)
                    //{
                    //    for (int ndx = 0; ndx < testCases.SetupPlugins.Length; ndx++)
                    //    {
                    //        var plugin = testCases.SetupPlugins[ndx];
                    //        i.Context.Plugins.Register((ISetupPlugin)plugin);
                    //    }
                    //}

                    if (testType == TestType.Component)
                        InitComponentTests(i, testCases);
                    if (testType == TestType.Integration)
                        InitIntegrationTests(i, testCases);
                }
            }, replaceSemicolonsWithNewLines, onNewScript);
        }


        /// <summary>
        /// Parses / executes a list of statements.
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="execute"></param>
        /// <param name="initializer"></param>
        /// <param name="replaceSemicolonsWithNewLines"></param>
        protected void ExpectErrors(TestCases testCases, TestType testType,
                bool execute = true, Action<Interpreter> initializer = null,
                bool replaceSemicolonsWithNewLines = false, Action onNewScript = null)
        {
            var statements = testCases.Positive;
            for(int ndx = 0; ndx < testCases.Failures.Count; ndx++)
            {
                var scenario = testCases.Failures[ndx];
                var i = new Interpreter();
                if (initializer != null)
                    initializer(i);
                else
                {
                    if (testType == TestType.Component)
                        InitComponentTests(i, testCases);
                    if (testType == TestType.Integration)
                        InitIntegrationTests(i, testCases);
                }
                Console.WriteLine(scenario.Item3);
                i.Execute(scenario.Item3);
                Assert.IsFalse(i.Result.Success);
                Assert.IsNotNull(i.Result.Ex);
                Assert.IsTrue(i.Result.Message.StartsWith(scenario.Item1));
                if (scenario.Item2 != null)
                {
                    Assert.IsTrue(i.Result.Message.Contains(scenario.Item2));
                }
            }
        }


        /// <summary>
        /// Parses / executes a list of statements.
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="execute"></param>
        /// <param name="initializer"></param>
        /// <param name="replaceSemicolonsWithNewLines"></param>
        protected void InitComponentTests(Interpreter i, TestCases testCases)
        {
            if (testCases.RequiredTypes != null && testCases.RequiredTypes.Length > 0)
                testCases.RequiredTypes.ForEach(type => i.Context.Types.Register(type, null));

            if (testCases.RequiredPlugins != null && testCases.RequiredPlugins.Length > 0)
                testCases.RequiredPlugins.ForEach( pluginType => i.Context.Plugins.RegisterCustomByType(pluginType));            
        }


        /// <summary>
        /// Parses / executes a list of statements.
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="execute"></param>
        /// <param name="initializer"></param>
        /// <param name="replaceSemicolonsWithNewLines"></param>
        protected void InitIntegrationTests(Interpreter i, TestCases testCases)
        {
            if (testCases.RequiredTypes != null && testCases.RequiredTypes.Length > 0)
                testCases.RequiredTypes.ForEach(type => i.Context.Types.Register(type, null));
            
            i.Context.Plugins.RegisterAllCustom();
        }


        /// <summary>
        /// Parses / executes a list of statements.
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="execute"></param>
        /// <param name="initializer"></param>
        /// <param name="replaceSemicolonsWithNewLines"></param>
        protected void Parse(Tuple<string, Type, object, string> statement,
                bool execute = true, Action<Interpreter> initializer = null,
                bool replaceSemicolonsWithNewLines = false, Action onNewScript = null)
        {
            var statements = new List<Tuple<string, Type, object, string>>();
            statements.Add(statement);
            Parse(statements, execute, initializer, replaceSemicolonsWithNewLines, onNewScript);
        }


        /// <summary>
        /// Parses / executes a list of statements.
        /// </summary>
        /// <param name="statements"></param>
        /// <param name="execute"></param>
        /// <param name="initializer"></param>
        /// <param name="replaceSemicolonsWithNewLines"></param>
        protected void Parse(List<Tuple<string, Type, object, string>> statements, 
                bool execute = true, Action<Interpreter> initializer = null, 
                bool replaceSemicolonsWithNewLines = false, Action onNewScript = null )
        {
            for (int ndx = 0; ndx < statements.Count; ndx++)
            {
                var stmt = statements[ndx];
                Interpreter i = new Interpreter();
                if (initializer != null)
                    initializer(i);
                
                if (execute)
                {
                    Console.WriteLine();
                    Console.Write(stmt.Item4);
                    i.Execute(stmt.Item4);
                    if (!i.Result.Success && !string.IsNullOrEmpty(i.Result.Message))
                        Console.WriteLine("\r\n\r\nERROR : " + i.Result.Message);

                    if (stmt.Item1 != null)
                    {
                        object obj = i.Memory[stmt.Item1];
                        Compare(obj, stmt.Item3); 
                    }
                    if (replaceSemicolonsWithNewLines)
                    {
                        var newText = stmt.Item4.Replace(";", Environment.NewLine);
                        if(onNewScript != null) 
                            onNewScript();
                        
                        i.Execute(newText);

                        // For print statements no check for any result.
                        if (stmt.Item1 != null)
                        {
                            object obj = i.Memory[stmt.Item1];
                            Compare(obj, stmt.Item3); 
                        }
                    }
                }
                else
                {
                    i.Parse(stmt.Item4);
                }                
            }
        }


        private void Compare(object actual, object expected)
        {
            if (actual is DateTime)
            {
                DateTime d1 = (DateTime)actual;
                DateTime d2 = (DateTime)expected;
                if ( ( d1.Hour > 0 || d1.Minute > 0 || d1.Second > 0 || d1.Millisecond > 0 )
                     && ( d2.Hour > 0 || d2.Minute > 0 || d2.Second > 0 || d2.Millisecond > 0 ))
                    Assert.AreEqual(d1, d2);
                else
                    Assert.AreEqual(d1.Date, d2);
            }
            else
                Assert.AreEqual(actual, expected);
        }


        protected void ParseFuncCalls(List<Tuple<string, int, Type, object, string>> statements)
        {
            for (int ndx = 0; ndx < statements.Count; ndx++)
            {
                var stmt = statements[ndx];
                Interpreter i = new Interpreter();
                object result = null;

                string funcCallTxt = stmt.Item5;

                // Handle calls to "user.create".
                i.SetFunctionCallback(stmt.Item1, (exp) =>
                {
                    // 1. Check number of parameters match
                    Assert.AreEqual(exp.ParamList.Count, stmt.Item2);

                    // 2. Check name of func
                    Assert.AreEqual(exp.Name, stmt.Item1);

                    if (stmt.Item2 > 0)
                    {
                        // 3. return the type
                        result = exp.ParamList[Convert.ToInt32(exp.ParamList[0])];
                        return result;
                    }
                    result = 1;
                    return result;
                });

                i.Execute(funcCallTxt);

                // 4. Check return value
                Assert.AreEqual(result, stmt.Item4);
            }
        }


        protected void RunTestCases(List<Tuple<string, Type, object, string>> testcases)
        {
            for (int ndx = 0; ndx < testcases.Count; ndx++)
            {
                var test = testcases[ndx];
                Interpreter i = new Interpreter();
                i.Context.Types.Register(typeof(Person), null);
                Console.WriteLine(test.Item4);
                i.Execute(test.Item4);

                // 1. Check type of result is correct
                Assert.AreEqual(i.Memory.Get<object>(test.Item1).GetType(), test.Item2);

                // 2. Check that value is correct
                object actualValue = i.Memory.Get<object>(test.Item1);
                if (test.Item3 is DateTime)
                {
                    var actualDate = (DateTime)actualValue;
                    actualDate = actualDate.Date;
                    Assert.AreEqual(test.Item3, actualDate);
                }
                else
                    Assert.AreEqual(test.Item3, actualValue);
            }
        }
    }


}
