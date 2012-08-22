/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Benchmarks
{
    /// <summary>
    /// Light-weight class to provide benchmarking support to measure time used to run code.
    /// </summary>
    public class Benchmark
    {
        private static BenchmarkService _service = new BenchmarkService();


        /// <summary>
        /// Run a simple benchmark with the supplied action and get the result.
        /// </summary>
        /// <param name="action">The action to benchmark</param>
        public static BenchmarkResult Run(Action action)
        {
            return Report(string.Empty, string.Empty, null, action);
        }


        /// <summary>
        /// Run a simple benchmark with the supplied action and get the result.
        /// </summary>
        /// <param name="name">The name of the action to benchmark.</param>
        /// <param name="action">The action to benchmark</param>
        public static BenchmarkResult Run(string name, Action action)
        {
            return Report(name, string.Empty, null, action);
        }


        /// <summary>
        /// Run a simple benchmark with the supplied action and get the result.
        /// </summary>
        /// <param name="name">The name of the action to benchmark</param>
        /// <param name="message">A message representing the action to run benchmark against.</param>
        /// <param name="action"></param>
        public static BenchmarkResult Run(string name, string message, Action action)
        {
            return Report(name, message, null, action);
        }


        /// <summary>
        /// Run a simple benchmark with the supplied action and call the logger action supplied.
        /// </summary>
        /// <param name="name">The name of the action to benchmark</param>
        /// <param name="message">A message associated w/ the action to benchmark</param>
        /// <param name="logger">The callback method for logging purposes.</param>
        /// <param name="action">The action to benchmark.</param>
        public static BenchmarkResult Report(string name, string message, Action<BenchmarkResult> logger, Action action)
        {
            BenchmarkService service = _service == null ? new BenchmarkService() : _service;
            return service.Report(name, message, logger, action);                
        }


        /// <summary>
        /// Run a simple benchmark with the supplied action and call the logger action supplied.
        /// </summary>
        /// <param name="name">The name of the action to benchmark</param>
        /// <param name="message">A message associated w/ the action to benchmark</param>
        /// <param name="logger">The callback method for logging purposes.</param>
        /// <param name="action">The action to benchmark.</param>
        public static void Get(string name, string message, Action<BenchmarkResult> logger, Action<BenchmarkService> action)
        {
            BenchmarkService service = new BenchmarkService(name, message, logger);
            action(service);
        }
    }
}
