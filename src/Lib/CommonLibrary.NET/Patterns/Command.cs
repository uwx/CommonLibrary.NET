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
using ComLib;


namespace ComLib.Patterns
{
    /// <summary>
    /// Command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute
        /// </summary>
        /// <returns></returns>
        bool Execute();        
    }



    /// <summary>
    /// Command with contextual method signature.
    /// </summary>
    public interface ICommandContextual
    {
        /// <summary>
        /// Execute with contextual information and return bool/message.
        /// </summary>
        /// <param name="context">Action context.</param>
        /// <returns>Result of the operation.</returns>
        BoolMessageItem Execute(IActionContext context);
    }
}
