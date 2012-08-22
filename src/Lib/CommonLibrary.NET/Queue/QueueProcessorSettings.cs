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
using System.Threading;


namespace ComLib.Queue
{
    /// <summary>
    /// Settings for the processing queue.
    /// </summary>
    public class QueueProcessorSettings
    {
        /// <summary>
        /// Gets or sets the number to process per dequeue.
        /// </summary>
        /// <value>The number to process per dequeue.</value>
        public int NumberToProcessPerDequeue { get; set; }
    }
}
