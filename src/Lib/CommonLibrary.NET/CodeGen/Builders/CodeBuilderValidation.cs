using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;

using ComLib;

using ComLib.Models;


namespace ComLib.CodeGeneration
{
    /// <summary>
    /// Builds the validation for the model, this includes it's properties and
    /// it's composite objects.
    /// </summary>
    public class CodeBuilderValidation : CodeBuilderBase, ICodeBuilder
    {
        
        #region ICodeBuilder Members
        /// <summary>
        /// Create the ORM mappings in xml file.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public BoolMessageItem<ModelContainer> Process(ModelContext ctx)
        {
            // Not implemented for now since the main builder ( DomainBuilder will call this per model
            // and therefore hook into the method BuildValidationForModel.
            return new BoolMessageItem<ModelContainer>(ctx.AllModels, true, string.Empty);
        }

        #endregion


        /// <summary>
        /// Build the properties.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string BuildValidationForModel(ModelContext ctx, Model model)
        {           
            StringBuilder buffer = new StringBuilder();
            string conversionLine = string.Format("{0} entity = ({1})validationEvent.Target;", model.Name, model.Name);
            //buffer.Append("var val = new Validator( (valEvent) =>" + Environment.NewLine);
            //buffer.Append("{" + Environment.NewLine);
            IncrementIndent(4);
            buffer.Append(conversionLine + Environment.NewLine);
            foreach (PropInfo prop in model.Properties)
            {
                if (prop.CreateCode)
                {
                    // Build mapping for model.
                    BuildValidationForProperty(model, prop, buffer);
                }
            }
            if (model.ComposedOf != null && model.ComposedOf.Count > 0)
            {
                // Now build mapping for composed objects.
                foreach (Composition composite in model.ComposedOf)
                {
                    Model compositeModel = ctx.AllModels.ModelMap[composite.Name];
                    // BuildValidationForProperty(compositeModel, new PropInfo() { Name = compositeModel.Name }, buffer);
                }
            }
            // Now add custom validations.
            if (model.Validations != null && model.Validations.Count > 0)
            {
                // Build ValidationUtils.ValidateAndCollect(new LocationRule(entity.Address), results);
                foreach (ValidationItem validation in model.Validations)
                {
                    string objectValidationCode = "ValidationUtils.ValidateAndCollect(new " + validation.PropertyValidator.Name;
                    objectValidationCode += "(entity." + validation.PropertyToValidate + "), results);" + Environment.NewLine;
                    buffer.Append(GetIndent() + objectValidationCode);
                }
            }
            DecrementIndent(4);
            //buffer.Append("});" + Environment.NewLine);
            //buffer.Append("return val.IsValid;" + Environment.NewLine);
            string props = buffer.ToString();
            return props;
        }
        
        /// <summary>
        /// Builds the validation code for all entire object, taking into account
        /// all the validations for each property.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prop"></param>
        /// <param name="buffer"></param>
        public void BuildValidationForProperty(Model model, PropInfo prop, StringBuilder buffer)
        {
            // Validate basic types, int, string, datetime, double.
            if (TypeMap.IsBasicNetType(prop.DataType))
            {
                string shortTypeName = TypeMap.Get<string>(TypeMap.NetFormatToCSharp, prop.DataType.Name);
                if (shortTypeName == "string" && !string.IsNullOrEmpty(prop.RegEx))
                    BuildValidationForStringRegExProperty(model, prop, buffer);
                else if (shortTypeName == "string" && string.IsNullOrEmpty(prop.RegEx))
                    BuildValidationForStringProperty(model, prop, buffer);
                else if (shortTypeName == "int")
                    BuildValidationForIntProperty(model, prop, buffer);
                else if (shortTypeName == "DateTime")
                    BuildValidationForDateTimeProperty(model, prop, buffer);
            }
        }


        /// <summary>
        /// Build validation for string properties.
        /// This inclues checking for null and length of the string.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prop"></param>
        /// <param name="buffer"></param>
        public void BuildValidationForStringProperty(Model model, PropInfo prop, StringBuilder buffer)
        {
            PropStringVals vals = new PropStringVals(model, prop) { MethodName = "IsStringLengthMatch" };
            string validationCode = "Validation.{0}({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8} );" + Environment.NewLine;
            string codeLine = string.Format(validationCode, vals.MethodName, vals.PropName, vals.AllowNull, vals.CheckMinLength, vals.CheckMaxLength, vals.Min, vals.Max, "results", '"' + prop.Name + '"');
            buffer.Append(GetIndent() + codeLine);
        }


        /// <summary>
        /// Build validation for int properties.
        /// This inclues checking for null and length of the string.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prop"></param>
        /// <param name="buffer"></param>
        public void BuildValidationForIntProperty(Model model, PropInfo prop, StringBuilder buffer)
        {
            PropStringVals vals = new PropStringVals(model, prop) { MethodName = "IsNumericWithinRange" };
            string validationCode = "Validation.{0}({1}, {2}, {3}, {4}, {5}, {6}, {7});" + Environment.NewLine;
            string codeLine = string.Format(validationCode, vals.MethodName, vals.PropName, vals.CheckMinLength, vals.CheckMaxLength, vals.Min, vals.Max, "results", '"' + prop.Name + '"');
            buffer.Append(GetIndent() + codeLine);
        }


        /// <summary>
        /// Build validation for datetime properties.
        /// This inclues checking for null and length of the string.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prop"></param>
        /// <param name="buffer"></param>        
        public void BuildValidationForDateTimeProperty(Model model, PropInfo prop, StringBuilder buffer)
        {
            PropStringVals vals = new PropStringVals(model, prop) { MethodName = "IsDateWithinRange" };
            string validationCode = "Validation.{0}({1}, {2}, {3}, {4}, {5}, {6}, {7} );" + Environment.NewLine;
            string codeLine = string.Format(validationCode, vals.MethodName, vals.PropName, vals.CheckMinLength, vals.CheckMaxLength, vals.Min, vals.Max, "results", '"' + prop.Name + '"');
            buffer.Append(GetIndent() + codeLine);
        }


        /// <summary>
        /// Build validation for int properties.
        /// This inclues checking for null and length of the string.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prop"></param>
        /// <param name="buffer"></param>        
        public void BuildValidationForStringRegExProperty(Model model, PropInfo prop, StringBuilder buffer)
        {
            PropStringVals vals = new PropStringVals(model, prop) { AllowNull = "false", MethodName = "IsStringRegExMatch" };
            string validationCode = "Validation.{0}({1}, {2}, {3}, {4}, {5} );" + Environment.NewLine;
            string regexPattern = prop.IsRegExConst ? prop.RegEx : "@\"" + prop.RegEx + "\"";
            string codeLine = string.Format(validationCode, vals.MethodName, vals.PropName, vals.AllowNull, regexPattern, "results", '"' + prop.Name + '"');
            buffer.Append(GetIndent() + codeLine);
        }


        class PropStringVals
        {
            public string AllowNull;
            public string CheckMinLength;
            public string CheckMaxLength;
            public string Min;
            public string Max;
            public string PropName;
            public string MethodName;

            public PropStringVals(Model model, PropInfo prop)
            {
                AllowNull = prop.IsRequired ? "false" : "true";
                CheckMinLength = prop.CheckMinLength ? "true" : "false";
                CheckMaxLength = prop.CheckMaxLength ? "true" : "false";
                PropName = "entity." + prop.Name;

                if (prop.DataType == typeof(DateTime))
                {
                    Min = prop.CheckMinLength ? "DateTime.Parse(" + prop.MinLength + ");" : "DateTime.MinValue";
                    Max = prop.CheckMaxLength ? "DateTime.Parse(" + prop.MaxLength + ");" : "DateTime.MaxValue";
                }
                else
                {
                    Min = GetLength(prop.CheckMinLength, prop.MinLength, "-1");
                    Max = GetLength(prop.CheckMaxLength, prop.MaxLength, "-1");
                }
            }

            private string GetLength(bool checkLen, string len, string inactiveValue)
            {
                if (!checkLen) return inactiveValue;
                if (string.IsNullOrEmpty(len) || len == "-1") return inactiveValue;

                return len;
            }
        }
    }
}
