using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;

//<doc:using>
using ComLib.Benchmarks;
//</doc:using>
using ComLib.Application;


namespace ComLib.Samples
{
	//<doc:execute>
    /// <summary>
    /// Example for the Benchmark namespace.
    /// </summary>
    public class Example_Benchmark : App
    {
		/// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem  Execute()
        {
			//<doc:example>        
            // Example 1: Get a benchmark by running an action.
            BenchmarkResult result = Benchmark.Run(() => Console.WriteLine("Get benchmark using run method."));

            // Example 2: Get a benchmark by running a named action with a message.
            var result2 = Benchmark.Run("Example 2", "Running named benchmark", () => Console.WriteLine("Running example 2"));

            // Example 3: Run an action and report the benchmark data.
            Benchmark.Report("Example 3",  "Reporting benchmark", (res) => Console.WriteLine(res.ToString()), () => Console.WriteLine("Running example 3"));
            Benchmark.Report("Example 3b", "Reporting benchmark", null, () => Console.WriteLine("Running example 3b"));

            // Example 4: Get instance of Benchmark service and run multiple reports on it.
            Benchmark.Get("Example 4", "testing", null, (bm) =>
            {
                bm.Run("4a", "testing", () => Console.WriteLine("Running 4a"));
                bm.Run("4b", "testing", () => Console.WriteLine("Running 4b"));
                bm.Run("4c", "testing", () => Console.WriteLine("Running 4c"));
            });
            
            // Example 5: Get instace of benchmark service manually.
            var bm2 = new BenchmarkService("Example 5", "manually instance of bm service", null);
            bm2.Run(() => Console.WriteLine("Running example 5"));

			//</doc:example>
            return BoolMessageItem.True;
        }
		
    }
}
