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
    /// Interface for a persistance queue repository.
    /// </summary>
    /// <typeparam name="T">Type of items to store in repository.</typeparam>
    public interface IQueueRepository<T>
    {
        /// <summary>
        /// Saves the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        void Save(IList<T> items);


        /// <summary>
        /// Loads all.
        /// </summary>
        /// <returns>List of all items.</returns>
        IList<T> LoadAll();

        
        /// <summary>
        /// Loads the batch.
        /// </summary>
        /// <returns>List of all items.</returns>
        IList<T> LoadBatch();
    }
}
