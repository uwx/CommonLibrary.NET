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
using System.Data;
using System.Data.Common;


namespace ComLib.Models
{
    /// <summary>
    /// This class provides utility methods for the <see cref="ComLib.Models"/> namespace.
    /// </summary>
    public class ModelUtils
    {
        /// <summary>
        /// Get the inheritance path of a model as list of models.
        /// </summary>
        /// <param name="container">The model container.</param>
        /// <param name="modelName">The model name.</param>
        /// <param name="sortOnProperties">True to sort the list on properties.</param>
        /// <returns>Inheritance path.</returns>
        public static List<Model> GetModelInheritancePath(ModelContainer container, string modelName, bool sortOnProperties)
        {
            List<Model> chain = GetModelInheritancePath(container, modelName);
            if (sortOnProperties)
                Sort(chain);

            return chain;
        }


        /// <summary>
        /// Get the inheritance path of a model as list of models.
        /// </summary>
        /// <param name="container">The model container.</param>
        /// <param name="modelName">The model name.</param>
        /// <returns>Inheritance path.</returns>
        public static List<Model> GetModelInheritancePath(ModelContainer container, string modelName)
        {
            Model currentModel = container.ModelMap[modelName];
            string inheritancePath = ConvertNestedToFlatInheritance(currentModel, container);
            
            // No parents?
            if( inheritancePath.IndexOf(",") < 0 )
            {
                return new List<Model>() { container.ModelMap[inheritancePath] };
            }

            // Delimited.
            List<Model> modelChain = new List<Model>();
            string[] parents = inheritancePath.Split(',');
            foreach (string parent in parents)
            {
                Model model = container.ModelMap[parent];
                modelChain.Add(model);
            }
            return modelChain;
        }


        /// <summary>
        /// Traverses the nodes inheritance path to build a single flat delimeted line of 
        /// inheritance paths.
        /// e.g. returns "Job,Post,EntityBase"
        /// </summary>
        /// <returns>Delimited line of inheritance paths.</returns>
        public static string ConvertNestedToFlatInheritance(Model model, ModelContainer container)
        {
            // Return name of environment provided if it doesn't have 
            // any inheritance chain.
            if (string.IsNullOrEmpty(model.Inherits))
                return model.Name;

            // Single parent.
            if (model.Inherits.IndexOf(",") < 0)
            {
                // Get the parent.
                Model parent = container.ModelMap[model.Inherits.Trim()];
                return model.Name + "," + ConvertNestedToFlatInheritance(parent, container);
            }

            // Multiple parents.
            string[] parents = model.Inherits.Split(',');
            string path = model.Name;
            foreach (string parent in parents)
            {
                Model parentModel = container.ModelMap[model.Inherits.Trim()];
                path += "," + ConvertNestedToFlatInheritance(parentModel, container);
            }
            return path;
        }


        /// <summary>
        /// Sort the model chain.
        /// </summary>
        /// <param name="modelChain">The model chain to sort.</param>
        public static void Sort(List<Model> modelChain)
        {
            modelChain.Sort(delegate(Model m1, Model m2) 
            {
                if (m1.PropertiesSortOrder > m2.PropertiesSortOrder)
                    return 1;
                if (m1.PropertiesSortOrder < m2.PropertiesSortOrder)
                    return -1;
                return 0;
            }
            );
        }
    }
}
