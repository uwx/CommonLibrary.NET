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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using ComLib.Entities;


namespace <%= model.NameSpace %>
{
    /// <summary>
    /// Active record functionality for <%= model.Name %>
    /// </summary>
    public partial class <%= model.Name %>s : ActiveRecord<<%= model.Name %>> 
    { 
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static <%= model.Name %> New()
        {
            <%= model.Name %> entity = new <%= model.Name %>();
            entity.Validator = new <%= model.Name %>Validator();
            entity.Settings = new <%= model.Name %>Settings();
            return entity;
        }
    }
}
