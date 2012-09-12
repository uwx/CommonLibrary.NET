using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang;
using ComLib.Lang.Plugins;

namespace ComLib.Lang.Tests.Common
{

    public class CommonTestCases_System
    {
        /// <summary>
        /// Test cases for the typeof plugin
        /// </summary>
        public static TestCases While = new TestCases()
        {
            Name = "While Plugin",
            RequiredPlugins = new[] { typeof(WhilePlugin) },
            Positive = new List<Tuple<string, Type, object, string>>()
            { 
                // 1. parenthesis - no braces
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) result += 1"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) result += 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) result += 1\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) result += 1;\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) \r\n result += 1"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) \r\n result += 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) \r\n result += 1\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) \r\n result += 1;\r\n"),
            
                // 2. parenthesis - braces
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) { result += 1  }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) { result += 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) { result += 1\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) { result += 1;\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) \r\n{ \r\n result += 1 }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) \r\n{ \r\n result += 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) \r\n{ \r\n result += 1\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while( result < 3 ) \r\n{ \r\n result += 1;\r\n }\r\n"),
            
                // 3. no parenthesis - no braces
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 then result += 1"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 then result += 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 then result += 1\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 then result += 1;\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 \r\n result += 1"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 \r\n result += 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 \r\n result += 1\r\n"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 \r\n result += 1;\r\n"),
            
                // 4. no parenthesis - braces
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 { result += 1  }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 { result += 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 { result += 1\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 { result += 1;\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 \r\n{ \r\n result += 1 }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 \r\n{ \r\n result += 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 \r\n{ \r\n result += 1\r\n }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; while result < 3 \r\n{ \r\n result += 1;\r\n }\r\n")                       
            }
        };
    }
}
