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

namespace ComLib
{
    /// <summary>
    /// This class contains methods to help with enumrating items.
    /// </summary>
    public class EnumerableHelper
    {
        /// <summary>
        /// Calls the action by supplying the start and end index.
        /// </summary>
        /// <param name="itemCount">Number of items.</param>
        /// <param name="cols">Number of columns.</param>
        /// <param name="action">Action to call for each item.</param>
        public static void ForEachByCols(int itemCount, int cols, Action<int, int> action)
        {
            if (itemCount == 0)
                return;

            if (itemCount <= cols)
            {
                action(0, itemCount - 1);
                return;
            }

            int startNdx = 0;
            while (startNdx < itemCount)
            {
                // 1. startNdx = 0 .. endNdx = 2
                // 2. startNdx = 3 .. endNdx = 5
                int endNdx = startNdx + (cols - 1);
                if (endNdx >= itemCount)
                    endNdx = itemCount - 1;

                action(startNdx, endNdx);
                startNdx = endNdx + 1;
            }
        }
    }
}
