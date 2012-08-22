using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;

using ComLib.Models;


namespace ComLib.CodeGeneration
{
    /// <summary>
    /// Base class for code builders.
    /// </summary>
    public class CodeBuilderBase
    {        
        /// <summary>
        /// Code ident level.
        /// </summary>
        protected int _indentLevel = 0;


        #region Indentation
        /// <summary>
        /// Gets the indentation level.
        /// </summary>
        /// <returns></returns>
        protected string GetIndent()
        {
            string indent = string.Empty;
            for (int ndx = 0; ndx < _indentLevel; ndx++)
            {
                indent += "\t";
            }
            return indent;
        }


        /// <summary>
        /// Increments the indentation by 1 level of spaces ( 4 ).
        /// </summary>
        protected void IncrementIndent()
        {
            _indentLevel++;
        }


        /// <summary>
        /// Increments the indentation by count number of spaces ( 4 )
        /// </summary>
        /// <param name="count"></param>
        protected void IncrementIndent(int count)
        {
            _indentLevel += count;
        }


        /// <summary>
        /// Decrements the indentation by 1 level of spaces ( 4 )
        /// </summary>
        protected void DecrementIndent()
        {
            _indentLevel--;
        }


        /// <summary>
        /// Decrements the indentation by count levels of spaces ( 4 ).
        /// </summary>
        /// <param name="count"></param>
        protected void DecrementIndent(int count)
        {
            _indentLevel -= count;
        }
        #endregion 
    }


    /// <summary>
    /// Handler for processing a specific model.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="currentModel"></param>
    /// <returns></returns>
    public delegate void ModelHandler( ModelContext ctx, Model currentModel);


    /// <summary>
    /// Handler for processing properties of the model.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="currentModel"></param>
    /// <param name="prop"></param>
    public delegate void PropertyHandler(ModelContext ctx, Model currentModel, PropInfo prop);


    /// <summary>
    /// Handler for processing an composite model.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="currentModel"></param>
    /// <param name="compositeModel"></param>
    public delegate void CompositionHandler(ModelContext ctx, Model currentModel, Model compositeModel);


    /// <summary>
    /// Handler for processing an included model.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="currentModel"></param>
    /// <param name="includedModel"></param>
    public delegate void IncludeHandler(ModelContext ctx, Model currentModel, Model includedModel);


    /// <summary>
    /// Handler for processing the UI for a model.
    /// </summary>
    /// <param name="ctx"></param>
    /// <param name="currentModel"></param>
    /// <param name="uiSpec"></param>
    /// <param name="prop"></param>
    public delegate void UIHandler(ModelContext ctx, Model currentModel, UISpec uiSpec, PropInfo prop);

}
