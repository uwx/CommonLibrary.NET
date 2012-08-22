using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;


using ComLib;
using ComLib.OrmLite;
using ComLib.Reflection;
using ComLib.Collections;
using ComLib.Models;



namespace ComLib.CodeGeneration
{
    /// <summary>
    /// Code Builder Domain Model.
    /// </summary>
    public class CodeBuilderDomain : CodeBuilderBase, ICodeBuilder
    {
        /// <summary>
        /// Get/set the model context.
        /// </summary>
        public ModelContext Context { get; set; }


        #region ICodeBuilder Members
        /// <summary>
        /// Builds the Domain model code which includes :
        /// 1. Entity.
        /// 2. ActiveRecord
        /// 3. Service 
        /// 4. Repository
        /// 5. Settings
        /// 6. Validator
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public virtual BoolMessageItem<ModelContainer> Process(ModelContext ctx)
        {
            string codeTemplatePath = ctx.AllModels.Settings.ModelCodeLocationTemplate;
            this.Context = ctx;
            Dictionary<string, CodeFile> fileMap = CodeBuilderUtils.GetFiles(ctx, string.Empty, codeTemplatePath);

            ctx.AllModels.Iterate(m => ctx.CanProcess(m, (model) => model.GenerateCode), currentModel =>
            {
                var modelChain = ctx.AllModels.InheritanceFor(currentModel.Name);
                fileMap = CodeFileHelper.GetFilteredDomainModelFiles(currentModel, fileMap);
                string generatedCodePath = (string)ctx.AllModels.Settings.ModelCodeLocation;
                generatedCodePath += "/" + currentModel.Name;
                IDictionary<string, string> subs = new Dictionary<string, string>();
                BuildSubstitutions(ctx, currentModel, modelChain, subs);

                foreach (string fileName in fileMap.Keys)
                {
                    string templateFile = codeTemplatePath + "/" + fileName;
                    string generated = generatedCodePath + "/" + fileMap[fileName].QualifiedName;
                    CodeFileHelper.Write(templateFile, generated, generatedCodePath, subs);
                }
            });

            return new BoolMessageItem<ModelContainer>(ctx.AllModels, true, string.Empty);
        }

        #endregion


        /// <summary>
        /// Build all the substutions.
        /// </summary>
        /// <param name="ctx">The entire context of all the models.</param>
        /// <param name="model"></param>
        /// <param name="modelChain">The inheritance chain of the model.</param>
        /// <param name="subs">The dictionary of substutions to populate.</param>
        public virtual void BuildSubstitutions(ModelContext ctx, Model model, List<Model> modelChain, IDictionary<string, string> subs)
        {
            OrmSqlStaticBuilder sql = OrmSqlStaticBuilderFactory.GetBuilder(ctx.AllModels.Settings.Connection.ProviderName);
            sql.Init(ctx, model.Name, model.TableName, 3);
            //Tuple2<string, string> massagersCode = BuildAllMassagers(model);

            subs["<%= model.NameSpace %>"] = model.NameSpace;
            subs["<%= model.ReferencedNameSpaces %>"] = BuildNameSpaces(model);
            subs["<%= model.Name %>"] = model.Name;
            subs["<%= model.Properties %>"] = BuildPropertiesForModel(model);
            subs["<%= model.ValidationCode %>"] = BuildValidation(model);
            subs["<%= model.RowMappingCode %>"] = BuildRowMapping(model);
            BuildDbParams(model, subs);
            subs["<%= model.GetRelations %>"] = BuildRelationFetch(model);
            subs["<%= model.EditRoles %>"] = BuildEditRoles(model);
            subs["<%= model.SqlInsert %>"] = sql.Insert();
            subs["<%= model.SqlUpdate %>"] = sql.Update();
        }


        /// <summary>
        /// Build the necessary referenced namespaces.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string BuildNameSpaces(Model model)
        {
            var buffer = new StringBuilder();

            // One to one relations
            model.OneToOne.ForEach(rel =>
            {
                var refModel = Context.AllModels.ModelMap[rel.ModelName];
                buffer.Append(string.Format("using {0};{1}", refModel.NameSpace, Environment.NewLine));
            });

            // One to Many relations
            model.OneToMany.ForEach(rel =>
            {
                var refModel = Context.AllModels.ModelMap[rel.ModelName];
                buffer.Append(string.Format("using {0};{1}", refModel.NameSpace, Environment.NewLine));
            });
            return buffer.ToString();
        }


        /// <summary>
        /// Build the validation code.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string BuildValidation(Model model)
        {
            if (!model.GenerateValidation)
            {
                return string.Empty;
            }
            CodeBuilderValidation validationBuilder = new CodeBuilderValidation();
            string validationCode = validationBuilder.BuildValidationForModel(Context, model);
            validationCode = GetValidationCode(validationCode);
            return validationCode;
        }


        private string GetValidationCode(string validationcode)
        {
            string code = @"protected override IValidator GetValidatorInternal()
            {
                var val = new EntityValidator((validationEvent) =>
                {
                    int initialErrorCount = validationEvent.Results.Count;
                    IValidationResults results = validationEvent.Results;            
                    <%= model.ValidationCode %>
                    return initialErrorCount == validationEvent.Results.Count;
                });
                return val;
            }";
            return code.Replace("<%= model.ValidationCode %>", validationcode);
        }


        /// <summary>
        /// Generates fetch for related objects.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <returns>Generated string.</returns>
        public string BuildRelationFetch(Model model)
        {
            var builder = new CodeBuilderDomainDatabase();
            builder.Tab = "\t\t\t";
            builder.Context = Context;
            var code = builder.BuildRelationObjects(model);
            return code;
        }


        /// <summary>
        /// Build the validation code.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string BuildRowMapping(Model model)
        {
            var builder = new CodeBuilderDomainDatabase();
            builder.Tab = "\t\t\t";
            builder.Context = Context;
            // Get list of models in the inheritance path for this model
            List<Model> modelChain = ModelUtils.GetModelInheritancePath(Context.AllModels, model.Name, true);

            var code = builder.BuildRowMapper(model, modelChain);
            return code;
        }


        /// <summary>
        /// Build the validation code.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="subs"></param>
        /// <returns></returns>
        public string BuildDbParams(Model model, IDictionary<string, string> subs)
        {
            var builder = new CodeBuilderDomainDatabase();
            builder.Tab = "\t\t\t";
            builder.Context = Context;
            // Get list of models in the inheritance path for this model
            List<Model> modelChain = ModelUtils.GetModelInheritancePath(Context.AllModels, model.Name, true);

            var code = builder.BuildDbParams(model, modelChain);
            var create = builder.BuildCreateParamsSql(model, modelChain);
            var update = builder.BuildUpdateParamsSql(model, modelChain);

            subs["<%= model.SqlDbParams %>"] = code;
            subs["<%= model.SqlDbParamsCreate %>"] = create;
            subs["<%= model.SqlDbParamsUpdate %>"] = update;

            return code;
        }


        /// <summary>
        /// Builds a comma delimited string of roles that can moderate
        /// instances of the model specified.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string BuildEditRoles(Model model)
        {
            if (model.ManagedBy == null || model.ManagedBy.Count == 0)
                return string.Empty;

            // Only 1 role.
            if (model.ManagedBy.Count == 1) return model.ManagedBy[0];

            // Build comma delimited list.
            string moderators = model.ManagedBy[0];

            for (int ndx = 1; ndx < model.ManagedBy.Count; ndx++)
            {
                moderators += "," + model.ManagedBy[ndx];
            }
            return moderators;
        }


        /// <summary>
        /// Build the properties.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual string BuildPropertiesForModel(Model model)
        {
            IncrementIndent(2);            
            StringBuilder buffer = new StringBuilder();

            // Build properties that belog directly to the model.
            BuildProperties(model, buffer);

            this.Context.AllModels.Iterate(model.Name, null,
                    (m, include) => BuildProperties(include.ModelRef, buffer),
                    (m, composition) => BuildProperty(composition.ModelRef, new PropInfo() { Name = composition.ModelRef.Name }, false, buffer),
                    (m, rel) => buffer.Append(BuildProperty(model, new PropInfo(rel.ModelName, null), false, rel.ModelName, false)),
                    (m, rel) => 
                    {
                        var propType = string.Format("IList<{0}>", rel.ModelName);
                        var propName = string.Format("{0}s", rel.ModelName);
                        buffer.Append(BuildProperty(model, new PropInfo(propName, null), false, propType, false));
                    }
            );

            DecrementIndent(2);
            string props = buffer.ToString();
            return props;
        }


        /// <summary>
        /// Build properties
        /// </summary>
        /// <param name="model"></param>
        /// <param name="buffer"></param>
        public void BuildProperties(Model model, StringBuilder buffer)
        {
            foreach (PropInfo prop in model.Properties)
            {
                if (prop.CreateCode) 
                    BuildProperty(model, prop, true, buffer);
            }
        }
      

        /// <summary>
        /// Build properties.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prop"></param>
        /// <param name="usePropType"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public void BuildProperty(Model model, PropInfo prop, bool usePropType, StringBuilder buffer)
        {
            BuildProperty(model, prop, usePropType, string.Empty, buffer); 
        }


        /// <summary>
        /// Build properties.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="prop"></param>
        /// <param name="usePropType"></param>
        /// <param name="type"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public void BuildProperty(Model model, PropInfo prop, bool usePropType, string type, StringBuilder buffer)
        {
            string code = BuildProperty(model, prop, usePropType, type, false);
            buffer.Append(code);
        }


        /// <summary>
        /// Builds code for a property.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <param name="prop">Property name.</param>
        /// <param name="usePropType">True to use a default get/set model,
        /// false to use a private variable with the default get/set model.</param>
        /// <param name="type">Type to generate.</param>
        /// <param name="initialize">If using a private variable, setting this to
        /// true will also generate code to generate a new instance of the variable.</param>
        /// <returns>Generated string.</returns>
        public string BuildProperty(Model model, PropInfo prop, bool usePropType, string type, bool initialize)
        {
            string indent = GetIndent();
            var buffer = new StringBuilder();
            string netType = string.Empty;
            if(usePropType && TypeMap.IsBasicNetType(prop.DataType))
                netType = TypeMap.Get<string>(TypeMap.NetFormatToCSharp, prop.DataType.Name);
            else 
                netType = string.IsNullOrEmpty(type) ? model.Name : type;
                
            if (usePropType)
            {
                buffer.Append(indent + "/// <summary>" + Environment.NewLine);
                buffer.Append(indent + "/// Get/Set " + prop.Name + Environment.NewLine);
                buffer.Append(indent + "/// </summary>" + Environment.NewLine);
                buffer.Append(indent + "public " + netType + " " + prop.Name + " { get; set; }" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
            }
            else
            {
                string name = prop.Name.ToLower()[0] + prop.Name.Substring(1);
                string privateVar = indent + "private " + netType + " _" + name;                   
    
                // private Address _address = new Address();
                if (initialize)
                    buffer.Append(privateVar + " = new " + netType + "();" + Environment.NewLine);                    
                else
                    buffer.Append(privateVar + ";" + Environment.NewLine);

                buffer.Append(indent + "/// <summary>" + Environment.NewLine);
                buffer.Append(indent + "/// Get/Set " + prop.Name + Environment.NewLine);
                buffer.Append(indent + "/// </summary>" + Environment.NewLine);
                buffer.Append(indent + "public " + netType + " " + prop.Name + Environment.NewLine
                    + indent + " { " + Environment.NewLine
                    + indent + " get { return _" + name + ";  }" + Environment.NewLine
                    + indent + " set { _" + name + " = value; }" + Environment.NewLine
                    + indent + " }" + Environment.NewLine + Environment.NewLine + Environment.NewLine);
            }
            return buffer.ToString();
        }
    }
}
