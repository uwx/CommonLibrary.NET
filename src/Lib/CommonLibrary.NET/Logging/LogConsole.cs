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
using System.Text;


namespace ComLib.Logging
{
    /// <summary>
    /// This is the default extremely simple ( Console ) logger for the static class Logger.
    /// This means that the Logger does NOT have to be initialized with a provider.
    /// You can just use it immediately.
    /// </summary>
    public class LogConsole : LogBase, ILog
    {
        //private bool _useColorCoding = false;
        private static object _colorSync = new object();
       

        /// <summary>
        /// Default constructor. Not associated with any class/type.
        /// </summary>
        public LogConsole() : base(typeof(LogConsole).FullName) 
        { 
        }


        /// <summary>
        /// Constructor with name.
        /// </summary>
        public LogConsole(string name) : this(name, false)
        {
        }


        /// <summary>
        /// Constructor with name.
        /// </summary>
        public LogConsole(string name, bool useColorCoding) : base(name)
        {
        }


        /// <summary>
        /// This is the only method REQUIRED to be implemented.
        /// </summary>
        /// <param name="logEvent">Event to log.</param>
        public override void Log(LogEvent logEvent)
        {            
            if (!string.IsNullOrEmpty(logEvent.FinalMessage))
                Console.WriteLine(logEvent.FinalMessage);
            else
            {
                string message = BuildMessage(logEvent);
                Console.WriteLine(message);
            }
        }
    }
}
