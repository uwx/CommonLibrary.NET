using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text.RegularExpressions;
using ComLib;
using ComLib.Logging;
using ComLib.Models;
using ComLib.Data;


namespace ComLib.CodeGeneration
{
    /// <summary>
    /// This class generates sql files for a model.
    /// </summary>
    public class CodeBuilderDb : CodeBuilderBase, ICodeBuilder    
    {
        private ConnectionInfo _conn;
        private bool _buildInstallScriptsOnly;


        /// <summary>
        /// Default setup.
        /// </summary>
        public CodeBuilderDb() 
        {
            _conn = ConnectionInfo.Default;
        }


        /// <summary>
        /// Initialize using connection.
        /// </summary>
        /// <param name="conn"></param>
        public CodeBuilderDb(ConnectionInfo conn) : this(false, conn)
        {
        }

        /// <summary>
        /// Initialize using connection.
        /// </summary>
        /// <param name="buildInstallScriptsOnly"></param>
        /// <param name="conn"></param>
        public CodeBuilderDb(bool buildInstallScriptsOnly, ConnectionInfo conn)
        {
            _conn = conn;
            _buildInstallScriptsOnly = buildInstallScriptsOnly;
        }


        /// <summary>
        /// Creates the models in the database.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public BoolMessageItem<ModelContainer> Process(ModelContext ctx)
        {
            var bufferTables = new StringBuilder();            
            var bufferProcs = new StringBuilder();
            var bufferBoth = new StringBuilder();
            var bufferDrop = new StringBuilder();
            DBSchema schema = new DBSchema(_conn);
            
            ctx.AllModels.Iterate(m => ctx.CanProcess(m, (model)=> model.GenerateTable), currentModel =>
            {                
                // Create the database table for all the models.
                List<Model> modelInheritanceChain = ctx.AllModels.InheritanceFor(currentModel.Name);
                Validate(ctx, currentModel);

                // Create table schema for model & create in database.
                DataTable modelTable = ConvertModelChainToTable(currentModel, modelInheritanceChain, ctx);
                string sqlTableSchema = string.Empty, sqlProcs = string.Empty, sqlModel = string.Empty;
                string sqlDrop = string.Empty, sqlDropProcs = string.Empty, sqlDropTable = string.Empty;

                // Build sql for 
                // 1. create table
                // 2. create procs
                // 3. create table & procs
                // 4. drop procs & table
                sqlDropTable = schema.GetDropTable(currentModel.TableName, true);
                var sqlDropTableNoGo = schema.GetDropTable(currentModel.TableName, false);
                sqlTableSchema = BuildTableSchema(ctx, modelInheritanceChain, currentModel, true);
                var sqlTableSchemaNoGo = BuildTableSchema(ctx, modelInheritanceChain, currentModel, false);
                List<string> sqlTable = new List<string>() { sqlDropTableNoGo, sqlTableSchemaNoGo };
                List<string> procNames = CodeFileHelper.GetProcNames(ctx.AllModels.Settings.ModelDbStoredProcTemplates);
                BoolMessageItem<List<string>> storedProcSql = CreateStoredProcs(ctx, currentModel, true);
                BoolMessageItem<List<string>> storedProcSqlNoGo = CreateStoredProcs(ctx, currentModel, false);
                if (storedProcSql.Success) storedProcSql.Item.ForEach(proc => sqlProcs += proc + Environment.NewLine);
                procNames.ForEach(procName => sqlDropProcs += schema.GetDropProc(currentModel.TableName, procName) + Environment.NewLine);

                sqlModel = sqlDropTable + Environment.NewLine + sqlTableSchema + Environment.NewLine + sqlProcs;
                sqlDrop = sqlDropProcs + sqlDropTable + Environment.NewLine;

                // Create in the database.
                ExecuteSqlInDb(sqlTable, ctx, currentModel);
                ExecuteSqlInDb(storedProcSqlNoGo.Item, ctx, currentModel);

                // Create model install file containing both the table/procs sql.
                CreateInstallSqlFile(ctx, currentModel, sqlModel);

                bufferTables.Append(sqlTable + Environment.NewLine);
                bufferProcs.Append(sqlProcs + Environment.NewLine + Environment.NewLine);
                bufferBoth.Append(sqlModel + Environment.NewLine);
                bufferDrop.Append(sqlDrop + Environment.NewLine);
            });
            
            // Create the files.
            string installPath = ctx.AllModels.Settings.ModelInstallLocation;                
            Try.CatchLog( () => File.WriteAllText(installPath + "_install_models_tables.sql", bufferTables.ToString()));
            Try.CatchLog(() => File.WriteAllText(installPath + "_install_models_tables.sql", bufferTables.ToString()));
            Try.CatchLog(() => File.WriteAllText(installPath + "_install_models_procs.sql", bufferProcs.ToString()));
            Try.CatchLog(() => File.WriteAllText(installPath + "_install_models_all.sql", bufferBoth.ToString()));
            Try.CatchLog(() => File.WriteAllText(installPath + "_uninstall_models.sql", bufferDrop.ToString()));
            return null;
        }


        private void Validate(ModelContext ctx, Model current)
        {
            List<Model> modelInheritanceChain = ctx.AllModels.InheritanceFor(current.Name);
            IErrors errors = new Errors();
            foreach (Model model in modelInheritanceChain)
            {
                foreach (var prop in model.Properties)
                    if (prop.DataType == typeof(string) && string.IsNullOrEmpty(prop.MaxLength) && prop.CreateColumn)
                        errors.Add("String length not specified for model : " + model.Name + "." + prop.Name);
            }
            if (errors.HasAny)
                throw new ArgumentException(errors.Message());
        }


        /// <summary>
        /// Create stored procedures
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="currentModel"></param>
        /// <param name="includeGo"></param>
        private BoolMessageItem<List<string>> CreateStoredProcs(ModelContext ctx, Model currentModel, bool includeGo)
        {
            string codeTemplatePath = ctx.AllModels.Settings.ModelDbStoredProcTemplates;
            string[] files = Directory.GetFiles(codeTemplatePath);
            if (files == null || files.Length == 0)
                return new BoolMessageItem<List<string>>(null, false, string.Empty);

            List<FileInfo> fileInfos = new List<FileInfo>();
            Dictionary<string, string> fileMap = new Dictionary<string, string>();

            files.ForEach(f => fileInfos.Add(new FileInfo(f)));

            StringBuilder buffer = new StringBuilder();
            List<string> procs = new List<string>();

            // Get each stored proc and do substitutions.
            foreach (FileInfo file in fileInfos)
            {
                string fileContent = File.ReadAllText(file.FullName);
                
                // Determine stored proc name.
                // 01234567890
                //    cde_ad]
                string nameCheck = @"CREATE PROCEDURE [dbo].[<%= model.TableName %>_";
                int ndxProcName = fileContent.IndexOf(nameCheck);
                ndxProcName = ndxProcName + nameCheck.Length;
                int nextBracket = fileContent.IndexOf("]", ndxProcName);
                string procName = fileContent.Substring(ndxProcName , (nextBracket - ndxProcName));

                fileContent = fileContent.Replace("<%= model.NameSpace %>", currentModel.NameSpace);
                fileContent = fileContent.Replace("<%= model.Name %>", currentModel.Name);
                fileContent = fileContent.Replace("<%= model.TableName %>", currentModel.TableName);

                string dropCommand = "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[<%= model.TableName %>_" + procName + "]') AND type in (N'P', N'PC'))" + Environment.NewLine
                    + "DROP PROCEDURE [dbo].[<%= model.TableName %>_" + procName + "]";
                
                if( includeGo)
                    dropCommand += Environment.NewLine + "go";

                dropCommand = dropCommand.Replace("<%= model.TableName %>", currentModel.TableName);

                procs.Add(dropCommand);
                procs.Add(fileContent);
                buffer.Append(fileContent + Environment.NewLine);
            }
            return new BoolMessageItem<List<string>>(procs, true, string.Empty);

        }


        /// <summary>
        /// Convert the model chain to database table.
        /// </summary>
        /// <param name="modelInheritanceChain">The list of models representing the inheritance chain, this includes the model
        /// being created. </param>
        /// <param name="modelToCreate">The model being created.</param>
        /// <param name="ctx"></param>
        public static DataTable ConvertModelChainToTable(Model modelToCreate, List<Model> modelInheritanceChain, ModelContext ctx)
        {
            DataTable table = new DataTable();
            List<DataColumn> primaryKeyColumns = new List<DataColumn>();

            // Add all the properties of each inherited model as columns in the table.
            foreach (Model model in modelInheritanceChain)
            {
                BuildTableColumns(model, table, primaryKeyColumns);
            }
            if (modelToCreate.ComposedOf != null && modelToCreate.ComposedOf.Count > 0)
            {
                // Add properties of each composed model as columns in the table.
                foreach (Composition compositionInfo in modelToCreate.ComposedOf)
                {
                    Model model = ctx.AllModels.ModelMap[compositionInfo.Name];
                    BuildTableColumns(model, table, primaryKeyColumns);
                }
            }
            table.PrimaryKey = primaryKeyColumns.ToArray();
            table.TableName = modelToCreate.TableName;
            return table;
        }


        /// <summary>
        /// Prefix to use for Table creation.
        /// </summary>
        public virtual string CreateTablePrefix(Model model)
        {
            return
            "SET ANSI_NULLS ON" + Environment.NewLine +
            //"GO" + Environment.NewLine +
            "SET QUOTED_IDENTIFIER ON" + Environment.NewLine +
            //"GO" + Environment.NewLine +
            "SET ANSI_PADDING ON" + Environment.NewLine;// +
            //"GO" + Environment.NewLine;
        }


        /// <summary>
        /// Prefix to use for Table creation.
        /// </summary>
        public virtual string CreateTableSuffix(Model model)
        {
            int clobPropertyCount = model.Properties.Count( (p) => p.DataType == typeof(StringClob));

            //var clobProperties = from p in model.Properties where p.DataType == typeof(StringClob) select p;
            string textImageOn = clobPropertyCount > 0 ? "TEXTIMAGE_ON [PRIMARY]" : string.Empty;

            string suffix = "," + Environment.NewLine +
            " CONSTRAINT [PK_" + model.TableName + "] PRIMARY KEY CLUSTERED " + Environment.NewLine +
            "( [Id] ASC" + Environment.NewLine +
            " )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" + Environment.NewLine +
            " ) ON [PRIMARY] " + textImageOn + Environment.NewLine;// +
            //"GO" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
            return suffix;
        }

        
        private string BuildTableSchema(ModelContext ctx, List<Model> modelInheritanceChain, Model modelToCreate, bool includeGo)
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append(CreateTablePrefix(modelToCreate));
            buffer.Append("CREATE TABLE [dbo].[" + modelToCreate.TableName + "]( " + Environment.NewLine);

            List<PropInfo> allProps = new List<PropInfo>();
            modelInheritanceChain.ForEach(m => allProps.AddRange(m.Properties));
            
            if( modelToCreate.ComposedOf != null)
                modelToCreate.ComposedOf.ForEach( c => allProps.AddRange(ctx.AllModels.ModelMap[c.Name].Properties));

            if (modelToCreate.Includes != null)
                modelToCreate.Includes.ForEach(m => allProps.AddRange(ctx.AllModels.ModelMap[m.Name].Properties));
            
            // Add all the properties of each model as columns in the table.
            for(int ndx = 0; ndx < allProps.Count; ndx++)
            {
                PropInfo prop = allProps[ndx];
                if (prop.CreateColumn)
                {
                    string columnInfo = string.Empty;

                    // First column.
                    if (ndx == 0 && !prop.IsKey)
                    {
                        columnInfo = BuildColumnInfo(prop);
                    }
                    else if (prop.IsKey)
                    {
                        columnInfo = BuildColumnInfoForKey(prop);
                        if (ndx != 0)
                            columnInfo = "," + Environment.NewLine + columnInfo;
                    }
                    else
                    {
                        columnInfo = "," + Environment.NewLine + BuildColumnInfo(prop);
                    }
                    buffer.Append(columnInfo);
                }
            }
            buffer.Append(CreateTableSuffix(modelToCreate));
            string sql = buffer.ToString();
            if(includeGo)
                sql += Environment.NewLine + "go";
            return sql;
        }


        /// <summary>
        /// Builds sql column ddl
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public virtual string BuildColumnInfo(PropInfo prop)
        {
            string columnInfo = "[{0}] [{1}]{2} {3}";
            string sqlType = TypeMap.Get<string>(TypeMap.SqlServer, prop.DataType.Name);
            string length = prop.DataType == typeof(string) ? "(" + prop.MaxLength + ")" : string.Empty;            
            string nullOption = prop.IsRequired ? "NOT NULL" : "NULL";
            columnInfo = string.Format(columnInfo, prop.ColumnName, sqlType, length, nullOption);
            return columnInfo;
        }


        /// <summary>
        /// Builds sql column identity ddl.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public virtual string BuildColumnInfoForKey(PropInfo prop)
        {
            // [Id] [bigint] IDENTITY(1,1) NOT NULL
            string columnInfo = "[{0}] [{1}] {2} {3}";
            string sqlType = TypeMap.Get<string>(TypeMap.SqlServer, prop.DataType.Name);
            string indentity = "IDENTITY(1,1)";
            columnInfo = string.Format(columnInfo, prop.ColumnName, sqlType, indentity, "NOT NULL");
            return columnInfo;
        }


        /// <summary>
        /// Create an install sql file specifically for creating the table for this model.
        /// The location of the file is obtained from the settings and the model name.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="currentModel"></param>
        /// <param name="sql"></param>
        public virtual void CreateInstallSqlFile(ModelContext ctx, Model currentModel, string sql)
        {
            string fileName = currentModel.InstallSqlFile;
            fileName = ctx.AllModels.Settings.ModelInstallLocation + "install_model_" + fileName;
            string error = string.Format("Error creating sql install file {0} for model {1}", fileName, currentModel.Name);
            Try.CatchLog(error, () => File.WriteAllText(fileName, sql));
        }


        /// <summary>
        /// Create table in the database.
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="ctx"></param>
        /// <param name="currentModel"></param>
        public virtual void ExecuteSqlInDb(List<string> sqls, ModelContext ctx, Model currentModel)
        {
            DbCreateType createType = ctx.AllModels.Settings.DbAction_Create;
            DBSchema helper = new DBSchema(_conn);
            string error = "Error executing sql for model : " + currentModel.Name + " table name : " + currentModel.TableName;

            Try.CatchLog( error, () =>
            {
                foreach (string sql in sqls)
                    helper.ExecuteNonQuery(sql, CommandType.Text, null);
            });
        }
       

        private static void BuildTableColumns(Model model, DataTable table, List<DataColumn> primaryKeyColumns)
        {
            foreach (PropInfo prop in model.Properties)
            {
                DataColumn column = new DataColumn();

                // Right now only handle simple datatypes.
                if (TypeMap.IsBasicNetType(prop.DataType))
                {
                    column.ColumnName = string.IsNullOrEmpty(prop.ColumnName) ? prop.Name : prop.ColumnName;
                    column.DataType = prop.DataType;
                    column.AllowDBNull = !prop.IsRequired;
                    column.Unique = prop.IsUnique;
                    if (prop.IsKey)
                    {
                        primaryKeyColumns.Add(column);
                    }
                    table.Columns.Add(column);
                }
            }
        }
    }


    /// <summary>
    /// This class generates row mapping and
    /// basic code for a model.
    /// </summary>
    public class CodeBuilderDomainDatabase
    {
        /// <summary>
        /// The tabs to use.
        /// </summary>
        public string Tab { get; set; }


        /// <summary>
        /// Get/set the model context.
        /// </summary>
        public ModelContext Context { get; set; }


        /// <summary>
        /// Builds the create params SQL.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelInheritanceChain">The model inheritance chain.</param>
        /// <returns></returns>
        public string BuildCreateParamsSql(Model model, List<Model> modelInheritanceChain)
        {
            List<string> names = new List<string>();
            var code = "\"insert into {0} ( {1} ) values ( {2} );\" + IdentityStatement;";

            this.Context.AllModels.Iterate(model.Name,
                    (minherit) => StoreDbParams(minherit, names),
                    (minclude, include) => StoreDbParams(minclude, names),
                    (mcompose, compose) => StoreDbParams(mcompose, names),
                    null, null);

            string cols = EnumerableExtensions.JoinDelimitedWithNewLine<string>(names, ", ", 6, "\"" + Environment.NewLine + Tab + " + \"", name => "[" + name + "]");
            string vals = EnumerableExtensions.JoinDelimitedWithNewLine<string>(names, ", ", 6, "\"" + Environment.NewLine + Tab + " + \"", name => "@" + name);

            string paramsCode = string.Format(code, model.TableName, cols, vals);
            return paramsCode;
        }


        /// <summary>
        /// Builds the update params SQL.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelInheritanceChain">The model inheritance chain.</param>
        /// <returns></returns>
        public string BuildUpdateParamsSql(Model model, List<Model> modelInheritanceChain)
        {
            string code = "\"update {0} set {1} where Id = \" + entity.Id";
            List<string> names = new List<string>();

            this.Context.AllModels.Iterate(model.Name,
                    (minherit) => StoreDbParams(minherit, names),
                    (minclude, include) => StoreDbParams(minclude, names),
                    (mcompose, compose) => StoreDbParams(mcompose, names), null, null);

            string cols = EnumerableExtensions.JoinDelimitedWithNewLine<string>(names, ", ", 6, "\"" + Environment.NewLine + Tab + " + \"", name => "[" + name + "] = @" + name);

            string paramsCode = string.Format(code, model.TableName, cols);
            return paramsCode;
        }


        /// <summary>
        /// Build the database parameters for Create/Update using parameterized sql.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelInheritanceChain"></param>
        /// <returns></returns>
        public string BuildDbParams(Model model, List<Model> modelInheritanceChain)
        {
            StringBuilder buffer = new StringBuilder();
            string code = string.Empty;
            string template = Tab + "dbparams.Add(BuildParam(\"@{0}\", SqlDbType.{1}, entity.{2}));";
            string template2 = Tab + "dbparams.Add(BuildParam(\"@{0}\", SqlDbType.{1}, entity.{composedName}.{2}));";
            string stringTemplate = Tab + "dbparams.Add(BuildParam(\"@{0}\", SqlDbType.{1}, string.IsNullOrEmpty(entity.{2}) ? \"\" : entity.{2}));";
            string stringTemplate2 = Tab + "dbparams.Add(BuildParam(\"@{0}\", SqlDbType.{1}, string.IsNullOrEmpty(entity.{composedName}.{2}) ? \"\" : entity.{composedName}.{2}));";

            this.Context.AllModels.Iterate(model.Name, 
                    (minherit) => AddDbParams(minherit, template, stringTemplate, buffer),
                    (minclude, include) => AddDbParams(minclude, template, stringTemplate, buffer),
                    (mcompose, compose) => AddDbParams(mcompose, template2.Replace("{composedName}", mcompose.Name), stringTemplate2.Replace("{composedName}", mcompose.Name), buffer),
                    null, null);
            
            string paramsCode = buffer.ToString();
            return paramsCode;
        }


        /// <summary>
        /// Build the entire row mapper.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelInheritanceChain"></param>
        /// <returns></returns>
        public string BuildRowMapper(Model model, List<Model> modelInheritanceChain)
        {
            // Job entity = Jobs.New();
            StringBuilder buffer = new StringBuilder();
            string code = string.Empty;

            this.Context.AllModels.Iterate(model.Name, 
                    (minherit) => buffer.Append(BuildRowMapperProperties("entity", model, minherit.Properties)),
                    (minclude, include) => buffer.Append(BuildRowMapperProperties("entity", model, minclude.Properties)),
                    (mcompose, compose) =>
                    {
                        code = string.Format("entity.{0} = new {1}();", compose.Name, compose.Name);
                        buffer.Append(code + Environment.NewLine);

                        // entity.Address
                        code = BuildRowMapperProperties("entity." + compose.Name, mcompose, mcompose.Properties);
                        buffer.Append(code);
                    }, null , null);


            string mappingCode = buffer.ToString();
            return mappingCode;
        }


        private void StoreDbParams(Model model, List<string> names)
        {
            model.Properties.ForEach(prop => { if (CanProcessProp(prop)) names.Add(prop.Name); });
        }


        private bool CanProcessProp(PropInfo prop)
        {
            if(string.Compare(prop.Name, "id", true) != 0 && prop.CreateColumn)
                return true; 
            return false;
        }


        private void AddDbParams(Model model, string template, string strTemplate, StringBuilder buffer)
        {
            model.Properties.ForEach(prop =>
            {
                if (CanProcessProp(prop))
                {
                    var sqlType = TypeMap.Get<object>(TypeMap.SqlClient, prop.DataType.Name).ToString();
                    string format = template;
                    if (prop.DataType == typeof(string) || prop.DataType == typeof(StringClob))
                    {
                        format = strTemplate;
                    }
                    buffer.Append(string.Format(format, prop.Name, sqlType, prop.Name) + Environment.NewLine);
                }
            });
        }


        /// <summary>
        /// Builds fetch for relational model objects.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <returns>Generated string.</returns>
        public string BuildRelationObjects(Model model)
        {
            // entity.User = GetOne<User>("where Id == " + 1);            
            var buffer = new StringBuilder();
            model.OneToOne.ForEach(rel =>
            {
                if (!string.IsNullOrEmpty(rel.Key))
                    buffer.Append(string.Format("entity.{0} = GetOne<{1}>(\"id = \" + entity.{2});" + Environment.NewLine, rel.ModelName, rel.ModelName, rel.Key));

                else if(!string.IsNullOrEmpty(rel.ForeignKey))
                    buffer.Append(string.Format("entity.{0} = GetOne<{1}>(\"{2} = \" + id);" + Environment.NewLine, rel.ModelName, rel.ModelName, rel.ForeignKey));
            });

            // entity.Comments = GetMany<Comment>("where refid == " + id);
            model.OneToMany.ForEach(rel =>
            {
                buffer.Append(string.Format("entity.{0}s = GetMany<{1}>(\"{2} = \" + id);" + Environment.NewLine, rel.ModelName, rel.ModelName, rel.ForeignKey));
            });
            string code = buffer.ToString();
            if (!string.IsNullOrEmpty(code))
            {
                code = "if (entity != null)" + Environment.NewLine
                     + Tab + " { " + Environment.NewLine 
                     + Tab + Tab + code + Environment.NewLine 
                     + Tab + " } " + Environment.NewLine;
            }
            return code;
        }


        /// <summary>
        /// Builds deletion logic for related objects.
        /// </summary>
        /// <param name="model">Model to use.</param>
        /// <returns>Generated string.</returns>
        public string BuildRelationObjectsDeletion(Model model)
        {
            // entity.User = GetOne<User>("where Id == " + 1);            
            var buffer = new StringBuilder();
            model.OneToOne.ForEach(rel =>
            {
                if (!string.IsNullOrEmpty(rel.Key))
                    buffer.Append(string.Format("entity.{0} = GetOne<{1}>(\"id = \" + entity.{2});" + Environment.NewLine, rel.ModelName, rel.ModelName, rel.Key));

                else if (!string.IsNullOrEmpty(rel.ForeignKey))
                    buffer.Append(string.Format("entity.{0} = GetOne<{1}>(\"{2} = \" + id);" + Environment.NewLine, rel.ModelName, rel.ModelName, rel.ForeignKey));
            });

            // entity.Comments = GetMany<Comment>("where refid == " + id);
            model.OneToMany.ForEach(rel =>
            {
                buffer.Append(string.Format("entity.{0}s = GetMany<{1}>(\"{2} = \" + id);" + Environment.NewLine, rel.ModelName, rel.ModelName, rel.ForeignKey));
            });
            string code = buffer.ToString();
            if (!string.IsNullOrEmpty(code))
            {
                code = "if (entity != null)" + Environment.NewLine
                     + Tab + " { " + Environment.NewLine
                     + Tab + Tab + code + Environment.NewLine
                     + Tab + " } " + Environment.NewLine;
            }
            return code;
        }


        /// <summary>
        /// Build row mapper.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="model"></param>
        /// <param name="allProps"></param>
        /// <returns></returns>
        public string BuildRowMapperProperties(string entityName, Model model, List<PropInfo> allProps)
        {
            // Job entity = Jobs.New();
            string code = string.Empty;
            StringBuilder buffer = new StringBuilder();
            buffer.Append(code);

            List<PropInfo> props = (from prop in allProps where prop.IsGetterOnly.Equals(false) && prop.CreateColumn.Equals(true) select prop).ToList();

            // Create mapping for each property
            foreach (PropInfo prop in props)
            {
                if (prop.DataType == typeof(string) || prop.DataType == typeof(StringClob))
                    code = BuildMappingCode(prop, entityName, "{0}.{1} = reader[\"{2}\"] == DBNull.Value ? string.Empty : reader[\"{3}\"].ToString();");
                
                else if (prop.DataType == typeof(int))
                    code = BuildMappingCode(prop, entityName, "{0}.{1} = reader[\"{2}\"] == DBNull.Value ? 0 : (int)reader[\"{3}\"];");

                else if (prop.DataType == typeof(bool))
                    code = BuildMappingCode(prop, entityName, "{0}.{1} = reader[\"{2}\"] == DBNull.Value ? false : (bool)reader[\"{3}\"];");

                else if (prop.DataType == typeof(DateTime))
                    code = BuildMappingCode(prop, entityName, "{0}.{1} = reader[\"{2}\"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader[\"{2}\"];");

                else if (prop.DataType == typeof(long))
                    code = BuildMappingCode(prop, entityName, "{0}.{1} = reader[\"{2}\"] == DBNull.Value ? 0 : (long)reader[\"{2}\"];");

                else if (prop.DataType == typeof(double))
                    code = BuildMappingCode(prop, entityName, "{0}.{1} = reader[\"{2}\"] == DBNull.Value ? 0 : Convert.ToDouble(reader[\"{2}\"]);");

                else if (prop.DataType == typeof(Image))
                {
                    code = BuildMappingCode(prop, entityName, "{0}.{1} = reader[\"{2}\"] == DBNull.Value ? new byte[] : (byte[])reader[\"{3}\"];");
                    code = code.Replace("new byte[] : ", "new byte[]{} : ");
                }
                
                buffer.Append(code + Environment.NewLine);
            }
            return buffer.ToString();
        }


        private string BuildMappingCode(PropInfo prop, string entityName, string codeLine)
        {
            // reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString();
            string code = string.Format(codeLine, entityName, prop.Name, prop.ColumnName, prop.ColumnName);
            return Tab + code;
        }
    }
}
