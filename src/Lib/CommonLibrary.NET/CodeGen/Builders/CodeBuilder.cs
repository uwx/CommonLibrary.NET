using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib;
using ComLib.Models;


namespace ComLib.CodeGeneration
{
    /// <summary>
    /// This class provides methods that call 
    /// all the builders relevant to code generation.
    /// </summary>
    public class CodeBuilder
    {
        //private ModelContext _context;

        
        /// <summary>
        /// Process.
        /// </summary>
        /// <param name="ctx">The model context containing all the models.</param>
        /// <returns></returns>
        public static BoolMessageItem<ModelContainer> Process(ModelContext ctx)
        {
            IList<ICodeBuilder> builders = new List<ICodeBuilder>()
            {
                new CodeBuilderDb(ctx.AllModels.Settings.Connection),
                new CodeBuilderDomain()
            };
            return Process(ctx, builders);
        }


        /// <summary>
        /// Process.
        /// </summary>
        /// <param name="ctx">The model context containing all the models.</param>
        /// <param name="builders">The builders.</param>
        /// <returns></returns>
        public static BoolMessageItem<ModelContainer> Process(ModelContext ctx, IList<ICodeBuilder> builders)
        {
            foreach (ICodeBuilder builder in builders)
            {
                builder.Process(ctx);
            }
            return new BoolMessageItem<ModelContainer>(null, false, string.Empty);
        }


        /// <summary>
        /// Process.
        /// </summary>
        /// <param name="ctx">The model context containing all the models.</param>
        /// <param name="populator">The populator.</param>
        /// <param name="modelsToInclude">The names of the models to process. This is used a include filter.</param>
        /// <returns></returns>
        public static BoolMessageItem<ModelContainer> Process(ModelContext ctx, Action<IList<ICodeBuilder>> populator, IDictionary<string, string> modelsToInclude)
        {
            // Reset the models to include.
            ctx.IncludeModels.Clear();
            foreach (var entry in modelsToInclude)
                ctx.IncludeModels[entry.Key] = entry.Key;

            IList<ICodeBuilder> builders = new List<ICodeBuilder>();
            populator(builders);
            foreach (ICodeBuilder builder in builders)
            {
                builder.Process(ctx);
            }
            return new BoolMessageItem<ModelContainer>(null, false, string.Empty);
        }


        /// <summary>
        /// Builds the install files.
        /// </summary>
        /// <param name="ctx">The model context containing all the models.</param>
        /// <param name="models">The name of the model to process. (optional)</param>
        public static void CreateInstallFiles(ModelContext ctx, params string[] models)
        {
            var filteredModels = GetModelNamesToProcess(models);
            Process(ctx, builders => builders.Add(new CodeBuilderDb(true, ctx.AllModels.Settings.Connection)), filteredModels);
        }


        /// <summary>
        /// Builds the database.
        /// </summary>
        /// <param name="ctx">The model context containing all the models.</param>
        /// <param name="models">The name of the model to process. (optional)</param>
        public static void CreateDatabase(ModelContext ctx, params string[] models)
        {
            var filteredModels = GetModelNamesToProcess(models);
            Process(ctx, builders => builders.Add(new CodeBuilderDb(ctx.AllModels.Settings.Connection)), filteredModels);
        }


        /// <summary>
        /// Builds the code.
        /// </summary>
        /// <param name="ctx">The model context containing all the models.</param>
        /// <param name="models">The name of the model to process. (optional)</param>
        public static void CreateCode(ModelContext ctx, params string[] models)
        {
            var filteredModels = GetModelNamesToProcess(models);
            Process(ctx, builders => builders.Add(new CodeBuilderDomain()), filteredModels);
        }


        /// <summary>
        /// Builds all.
        /// </summary>
        /// <param name="ctx">The model context containing all the models.</param>
        /// <param name="models">The name of the model to process. (optional)</param>
        public static void CreateAll(ModelContext ctx, params string[] models)
        {
            var filteredModels = GetModelNamesToProcess(models);
            Process(ctx, builders =>
                {
                    builders.Add(new CodeBuilderDb(ctx.AllModels.Settings.Connection));
                    builders.Add(new CodeBuilderDomain());
                }, filteredModels);
        }


        /// <summary>
        /// Get the models names as a dictionary.
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private static IDictionary<string, string> GetModelNamesToProcess(string[] models)
        {
            if (models == null || models.Length == 0)
                return new Dictionary<string, string>();

            return models.ToDictionary();
        }
    }
}
