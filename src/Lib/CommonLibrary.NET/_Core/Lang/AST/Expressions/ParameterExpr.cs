﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.Types;
using ComLib.Lang.Helpers;
// </lang:using>

namespace ComLib.Lang.AST
{        
    /// <summary>
    /// Function call expression data.
    /// </summary>
    public class ParameterExpr : Expr, IParameterExpression
    {
        /// <summary>
        /// Metadata about the parameters.
        /// </summary>
        protected FunctionMetaData _fmeta;


        /// <summary>
        /// Function call expression
        /// </summary>
        public ParameterExpr()
        {
            this.Init(null);
        }


        /// <summary>
        /// Initailizes with function metadata.
        /// </summary>
        /// <param name="meta"></param>
        public void Init(FunctionMetaData meta)        
        {
            _fmeta = meta;
            ParamList = new List<object>();
            ParamListExpressions = new List<Expr>();
        }


        /// <summary>
        /// List of expressions.
        /// </summary>
        public List<Expr> ParamListExpressions { get; set; }


        /// <summary>
        /// List of arguments.
        /// </summary>
        public List<object> ParamList { get; set; }


        /// <summary>
        /// Resolves the parameters.
        /// </summary>
        protected void ResolveParams()
        {
            FunctionHelper.ResolveParametersForScriptFunction(_fmeta, this.ParamListExpressions, this.ParamList);
        }


        /// <summary>
        /// Gets a parameter value if available or the default value otherwise. Index out of bounds will throw an error.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected object GetParamValue(int index, bool allowDefaultValue, object defaultValue)
        {
            bool hasParams = this.ParamList != null && this.ParamList.Count > 0;
            if (!hasParams && !allowDefaultValue)
                throw BuildRunTimeException("No parameters available for custom function plugin : " + this._fmeta.Name);

            if (hasParams && this.ParamList.Count <= index)
                throw BuildRunTimeException("Unexpected parameter retrieval attempted in custom function plugin : " + this._fmeta.Name);
            
            if (!hasParams && allowDefaultValue) 
                return defaultValue;

            return this.ParamList[index];
        }
    }
}
