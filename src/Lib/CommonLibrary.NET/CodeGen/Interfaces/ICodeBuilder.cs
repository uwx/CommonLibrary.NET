using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib;
using ComLib.Models;


namespace ComLib.CodeGeneration
{
    /// <summary>
    /// Interface for a component that is part of a code-generator.
    /// </summary>
    public interface ICodeBuilder
    {
        /// <summary>
        /// Executes a specific part of a code-generation process.
        /// </summary>    
        BoolMessageItem<ModelContainer> Process(ModelContext ctx);
    }
}
