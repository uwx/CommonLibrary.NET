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
using System.Collections;
using System.Collections.Specialized;


namespace ComLib
{
    /// <summary> 
    /// Simple class to lookup stored configuration settings by key. 
    /// Also provides type conversion methods. 
    /// GetInt("PageSize"); 
    /// GetBool("IsEnabled"); 
    /// </summary> 
    public class ConfigSource : ConfigSection, IConfigSource
    {
        /// <summary>
        /// Default construction.
        /// </summary>
        public ConfigSource()
        {
            Init();
        }



        #region IConfigSource Members
        /// <summary>
        /// Event handler for when the underlying config source changed.
        /// </summary>
        public event EventHandler OnConfigSourceChanged;

        
        /// <summary>
        /// The source file path.
        /// </summary>
        public virtual string SourcePath
        {
            get { return string.Empty; }
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        public virtual void Init()
        {
        }


        /// <summary>
        /// Load from datasource.
        /// </summary>
        public virtual void Load()
        {
        }


        /// <summary>
        /// Save to the datasource.
        /// </summary>
        public virtual void Save()
        {
        }
        #endregion
    } 
}
