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

namespace ComLib.Models
{
    /// <summary>
    /// This class holds contextual information pertaining to the generation process.
    /// </summary>
    public class ModelContext
    {
        private ModelContainer _container;
        private IDictionary<string, string> _includeModels;


        /// <summary>
        /// Default construction.
        /// </summary>
        public ModelContext()
        {
            _includeModels = new Dictionary<string, string>();
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="container">Collection of models.</param>
        public ModelContext(ModelContainer container)
        {
            _container = container;
            _includeModels = new Dictionary<string, string>();
            Init();
        }


        /// <summary>
        /// Get / Set the container.
        /// </summary>
        public ModelContainer AllModels
        {
            get { return _container; }
            set { _container = value; Init(); }
        }


        /// <summary>
        /// Names of models to be included in the processing.
        /// </summary>
        public IDictionary<string, string> IncludeModels
        {
            get { return _includeModels; }
        }



        /// <summary>
        /// Initialize by storing all the model chain inheritance paths for each model.
        /// </summary>
        public void Init()
        {
            AllModels.Init();
        }


        /// <summary>
        /// Determines if a model can be processed.
        /// </summary>
        /// <param name="model">Mode to check.</param>
        /// <param name="predicate2">Checking function.</param>
        /// <returns>True if the model can be processed.</returns>
        public bool CanProcess(Model model, Func<Model, bool> predicate2)
        {
            bool pass = predicate2(model);
            if (pass && IncludeModels.Count == 0)
                return true;

            if (pass && IncludeModels.ContainsKey(model.Name))
                return true;

            return false;
        }
    }
}
