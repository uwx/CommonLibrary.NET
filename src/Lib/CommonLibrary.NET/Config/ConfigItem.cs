using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using ComLib.Entities;
using ComLib.Data;
using ComLib.ValidationSupport;


namespace ComLib.Configuration
{
    /// <summary>
    /// An entity to represent the config item in a persistant store.
    /// </summary>
    public class ConfigItem : ActiveRecordBaseEntity<ConfigItem>
    {
        /// <summary>
        /// Application name. 
        /// To associate config settings from a different application.
        /// </summary>
        public string App { get; set; }


        /// <summary>
        /// Dev.config
        /// </summary> 
        public string Name { get; set; }


        /// <summary>
        /// AppSettings
        /// </summary>
        public string Section { get; set; }


        /// <summary>
        /// PageSize
        /// </summary>
        public string Key { get; set; }


        /// <summary>
        /// 15
        /// </summary>
        public string Val { get; set; }


        /// <summary>
        ///  Int | bool | double etc.
        /// </summary>
        public string ValType { get; set; }


        /// <summary>
        /// Create the config entry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="app">Application name.</param>
        /// <param name="name">Config name</param>
        /// <param name="section">Section name.</param>
        /// <param name="key">Key name</param>
        /// <param name="val">Value</param>
        public static void Create<T>(string app, string name, string section, string key, string val)
        {
            string type = typeof(T).Name;
            var item = new ConfigItem() { App = app, Name = name, Section = section, Key = key, Val = val, ValType = type };
            Create(item);
        }


        /// <summary>
        /// Create the config entry.
        /// </summary>
        /// <param name="app">Application name.</param>
        /// <param name="name">Config name</param>
        /// <param name="section">Section name.</param>
        /// <param name="key">Key name</param>
        /// <param name="val">Value</param>
        public static void Update(string app, string name, string section, string key, string val)
        {
            ConfigItem item = First(
                      "app = '" + DataUtils.Encode(app) + "'"
                    + "name = '" + DataUtils.Encode(name) + "'"
                    + "section = '" + DataUtils.Encode(section) + "'"
                    + "key = '" + DataUtils.Encode(key) + "'");
            item.Val = val;
            Update(item);
        }
    }



    /// <summary>
    /// Generic repository for persisting ConfigItem.
    /// </summary>
    public partial class ConfigItemRepository : RepositorySql<ConfigItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueries.NamedQueryRepository"/> class.
        /// </summary>
        public ConfigItemRepository() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="RepositorySql&lt;ConfigItem&gt;"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ConfigItemRepository(string connectionString)
            : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RepositorySql&lt;ConfigItem&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        public ConfigItemRepository(ConnectionInfo connectionInfo)
            : base(connectionInfo)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RepositorySql&lt;ConfigItem&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="db">The helper.</param>
        public ConfigItemRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new ConfigItemRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override ConfigItem Create(ConfigItem entity)
        {
            string sql = "insert into " + this.TableName + " ( CreateDate, UpdateDate, CreateUser, UpdateUser, UpdateComment" +
            ", App, [Name], [Section], [Key]" +
            ", [Val], ValType) " +
            "VALUES (" + "'" + entity.CreateDate.ToString("yyyy-MM-dd") + "'" + "," + "'" + entity.UpdateDate.ToString("yyyy-MM-dd") + "'" + "," + "'" + DataUtils.Encode(entity.CreateUser) + "'" + "," + "'" + DataUtils.Encode(entity.UpdateUser) + "'" + "," + "'" + DataUtils.Encode(entity.UpdateComment) + "'"
             + "," + "'" + DataUtils.Encode(entity.App) + "'" + "," + "'" + DataUtils.Encode(entity.Name) + "'"
             + "," + "'" + DataUtils.Encode(entity.Section) + "'" + "," + "'" + DataUtils.Encode(entity.Key) + "'" + "," + "'" + DataUtils.Encode(entity.Val) + "'" + "," + "'" + DataUtils.Encode(entity.ValType) + "'"
            + ");select scope_identity();";
            object result = _db.ExecuteScalarText(sql, null);
            entity.Id = Convert.ToInt32(result);
            return entity;
        }


        /// <summary>
        /// Update the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override ConfigItem Update(ConfigItem entity)
        {
            string sql = "update " + this.TableName + " set CreateDate = " + "'" + entity.CreateDate.ToString("yyyy-MM-dd") + "'" + ", UpdateDate = " + "'" + entity.UpdateDate.ToString("yyyy-MM-dd") + "'" + ", CreateUser = " + "'" + DataUtils.Encode(entity.CreateUser) + "'" + ", UpdateUser = " + "'" + DataUtils.Encode(entity.UpdateUser) + "'" + ", UpdateComment = " + "'" + DataUtils.Encode(entity.UpdateComment) + "'" + 
            ", App = " + "'" + DataUtils.Encode(entity.App) + "'" + ", [Name] = " + "'" + DataUtils.Encode(entity.Name) + "'" + ", Section = " + "'" + DataUtils.Encode(entity.Section) + "'" + ", [Key] = " + "'" + DataUtils.Encode(entity.Key) + "'" +
            ", Val = " + "'" + DataUtils.Encode(entity.Val) + "'" + ", ValType = " + "'" + DataUtils.Encode(entity.ValType) + "'" + " where Id = " + entity.Id; ;
            _db.ExecuteNonQueryText(sql, null);
            return entity;
        }


        /// <summary>
        /// RowMapper for ConfigItem.
        /// </summary>
        class ConfigItemRowMapper : EntityRowMapper<ConfigItem>, IEntityRowMapper<ConfigItem>
        {
            public override ConfigItem MapRow(IDataReader reader, int rowNumber)
            {
                ConfigItem entity = new ConfigItem();
                entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
                entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
                entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
                entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
                entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
                entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
                entity.App = reader["App"] == DBNull.Value ? string.Empty : reader["App"].ToString();
                entity.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
                entity.Section = reader["Section"] == DBNull.Value ? string.Empty : reader["Section"].ToString();
                entity.Key = reader["Key"] == DBNull.Value ? string.Empty : reader["Key"].ToString();
                entity.Val = reader["Val"] == DBNull.Value ? string.Empty : reader["Val"].ToString();
                entity.ValType = reader["ValType"] == DBNull.Value ? string.Empty : reader["ValType"].ToString();

                return entity;
            }
        }
    }



    /// <summary>
    /// Validator for ConfigItem
    /// </summary>
    public class ConfigItemValidator : EntityValidator
    {
        /// <summary>
        /// Validation method for the entity.
        /// </summary>
        /// <param name="validationEvent"></param>
        /// <returns></returns>
        protected override bool ValidateInternal(ValidationEvent validationEvent)
        {
            int initialErrorCount = validationEvent.Results.Count;
            IValidationResults results = validationEvent.Results;
            ConfigItem entity = (ConfigItem)validationEvent.Target;
            Validation.IsStringLengthMatch(entity.App, true, false, true, -1, 50, results, "App");
            Validation.IsStringLengthMatch(entity.Name, true, false, true, -1, 50, results, "Name");
            Validation.IsStringLengthMatch(entity.Section, true, false, true, -1, 20, results, "Section");
            Validation.IsStringLengthMatch(entity.Key, false, false, true, -1, 20, results, "Key");
            Validation.IsStringLengthMatch(entity.Val, false, false, false, -1, -1, results, "Val");
            Validation.IsStringLengthMatch(entity.ValType, true, false, true, -1, 20, results, "ValType");

            return initialErrorCount == validationEvent.Results.Count;
        }
    }
}
