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
    /// Class to iterate over models to create and provide callbacks at key points to 
    /// handle certain events related to model code generation.
    /// </summary>
    public class ModelIterator
    {
        #region Model Events
        /// <summary>
        /// Event to fire on model processing.
        /// </summary>
        public event Func<ModelContext, Model, bool> OnModelProcess;


        /// <summary>
        /// Event to fire when a model has been processed.
        /// </summary>
        public event Func<ModelContext, Model, bool> OnModelProcessCompleted;


        /// <summary>
        /// Event to fire when property should be processed for a specific model.
        /// </summary>
        public event Func<ModelContext, Model, PropInfo, bool> OnPropertyProcess;


        /// <summary>
        /// Event to fire when a composite model should be processed.
        /// </summary>
        public event Func<ModelContext, Model, Composition, bool> OnCompositeProcess;


        /// <summary>
        /// Event to fire when a included model should be processed.
        /// </summary>
        public event Func<ModelContext, Model, Include, bool> OnIncludeProcess;
        #endregion


        #region Filters
        /// <summary>
        /// Predicate to apply to model before processing it.
        /// </summary>
        public Func<Model, bool> FilterOnModel;


        /// <summary>
        /// Property filter.
        /// </summary>
        public Func<Model, PropInfo, bool> FilterOnProperty;
        #endregion



        /// <summary>
        /// Process the model using the callback lamdas.
        /// </summary>
        /// <param name="ctx">The full model context.</param>
        /// <param name="model">The model to process.</param>
        /// <param name="inheritanceChain">List with the inheritance chain.</param>
        /// <param name="inheritanceAction">Inheritance action.</param>
        /// <param name="includeAction">Include action.</param>
        /// <param name="compositionAction">Composition action.</param>
        public static void Process(ModelContext ctx, Model model, List<Model> inheritanceChain, Action<Model> inheritanceAction,
                            Action<Model, Include> includeAction, Action<Model, Composition> compositionAction)
        {
            // Build property mapping for each inherited model.
            foreach (Model inheritedModel in inheritanceChain)
                inheritanceAction(inheritedModel);

            // Build property mapping for each inherited model.
            if (model.Includes != null && model.Includes.Count > 0)
            {
                foreach (Include include in model.Includes)
                {
                    Model includedModel = ctx.AllModels.ModelMap[include.Name];
                    includeAction(includedModel, include);
                }
            }

            // Build property mapping for each Composed of model.
            if (model.ComposedOf != null && model.ComposedOf.Count > 0)
            {
                foreach (Composition composedModelName in model.ComposedOf)
                {
                    Model composedModel = ctx.AllModels.ModelMap[composedModelName.Name];
                    compositionAction(composedModel, composedModelName);
                }
            }
        }


        /// <summary>
        /// Process all the models, which pass the filter, one at a time.
        /// </summary>
        /// <param name="ctx">The model context.</param>
        public virtual void Process(ModelContext ctx)
        {
            foreach (Model currentModel in ctx.AllModels.AllModels)
            {
                // Pre condition.
                if (FilterOnModel(currentModel))
                {
                    ProcessModel(ctx, currentModel);
                }
            }
        }


        /// <summary>
        /// Process the model with the specified name.
        /// </summary>
        /// <param name="ctx">The model context.</param>
        /// <param name="modelName">The model name.</param>
        public virtual void Process(ModelContext ctx, string modelName)
        {
            Model current = ctx.AllModels.ModelMap[modelName];
            ProcessModel(ctx, current);
        }


        /// <summary>
        /// Proces given model
        /// </summary>
        /// <param name="ctx">The mode context.</param>
        /// <param name="currentModel">The current model.</param>
        public virtual void ProcessModel(ModelContext ctx, Model currentModel)
        {   
            // Notify.
            if (OnModelProcess != null)
                OnModelProcess(ctx, currentModel);

            // Create the database table for all the models.
            List<Model> modelChain = ModelUtils.GetModelInheritancePath(ctx.AllModels, currentModel.Name);

            // Sort the models to create the columns/properties in a specific order.
            // For the database, the inheritance chain doesn't really matter.
            ModelUtils.Sort(modelChain);

            foreach (Model model in modelChain)
            {
                // Build the entity properties.
                ProcessModelParts(ctx, model);
            }
            if (OnModelProcessCompleted != null)
                OnModelProcessCompleted(ctx, currentModel);               
        }


        /// <summary>
        /// Build the properties.
        /// </summary>
        /// <param name="ctx">The model context.</param>
        /// <param name="model">The model.</param>
        public virtual void ProcessModelParts(ModelContext ctx, Model model)
        {
            ProcessProperties(ctx, model);
            ProcessCompositions(ctx, model);
            ProcessIncludes(ctx, model);            
        }


        /// <summary>
        /// Process all the properties of the model.
        /// </summary>
        /// <param name="ctx">The model context.</param>
        /// <param name="model">The model.</param>
        public virtual void ProcessProperties(ModelContext ctx, Model model)
        {
            // Handle properties of model.
            foreach (PropInfo prop in model.Properties)
            {
                if (FilterOnProperty == null || (FilterOnProperty != null && FilterOnProperty(model, prop)))
                {
                    // Run event handler
                    if (this.OnPropertyProcess != null)
                        OnPropertyProcess(ctx, model, prop);
                }
            }
        }


        /// <summary>
        /// Process all the compositions of the model.
        /// </summary>
        /// <param name="ctx">The model context.</param>
        /// <param name="model">The model.</param>
        public virtual void ProcessCompositions(ModelContext ctx, Model model)
        {
            // Handle compositions
            if (model.ComposedOf != null && model.ComposedOf.Count > 0)
            {
                // Now build mapping for composed objects.
                foreach (Composition composite in model.ComposedOf)
                {
                    Model compositeModel = ctx.AllModels.ModelMap[composite.Name];
                    composite.ModelRef = compositeModel;
                    if (this.OnCompositeProcess != null)
                        OnCompositeProcess(ctx, model, composite);
                }
            }
        }


        /// <summary>
        /// Process all the includes of the model.
        /// </summary>
        /// <param name="ctx">The model context.</param>
        /// <param name="model">The model.</param>
        public virtual void ProcessIncludes(ModelContext ctx, Model model)
        {
            // Handle includes
            if (model.Includes != null && model.Includes.Count > 0)
            {
                // Now build mapping for composed objects.
                foreach (Include include in model.Includes)
                {
                    Model includedModel = ctx.AllModels.ModelMap[include.Name];
                    include.ModelRef = includedModel;
                    if (OnIncludeProcess != null)
                        OnIncludeProcess(ctx, model, include);
                }
            }
        }


        /// <summary>
        /// Get all the applicable properties after running the filter.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>List of applicable properties.</returns>
        public List<PropInfo> GetProperties(Model model)
        {
            List<PropInfo> props = new List<PropInfo>();
            // Handle properties of model.
            foreach (PropInfo prop in model.Properties)
            {
                if (FilterOnProperty == null || (FilterOnProperty != null && FilterOnProperty(model, prop)))
                {
                    props.Add(prop);
                }
            }
            return props;
        }
    }
}
