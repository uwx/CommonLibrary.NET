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

using ComLib;
using ComLib.Entities;
using ComLib.ValidationSupport;


namespace <%= model.NameSpace %>
{
    /// <summary>
    /// Validator for <%= model.Name %>
    /// </summary>
    public partial class <%= model.Name %>Validator : EntityValidator<<%= model.Name %>>
    {
        /// <summary>
        /// Validation method for the entity.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="useTarget">if set to <c>true</c> [use target].</param>
        /// <param name="results">The results.</param>
        /// <returns></returns>
        protected override bool ValidateInternal(ValidationEvent validationEvent)
        {
            int initialErrorCount = validationEvent.Results.Count;
            IValidationResults results = validationEvent.Results;            
            <%= model.ValidationCode %>
            return initialErrorCount == validationEvent.Results.Count;
        }
    }
}
