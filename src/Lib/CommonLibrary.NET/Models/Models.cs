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
using ComLib.Data;


namespace ComLib.Models
{
    /// <summary>
    /// This structure represents the StringCLob type.
    /// </summary>
    public struct StringClob
    {
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <returns>String representation of this instance.</returns>
        public override string ToString()
        {
            return "StringClob";
        }
    }


    /// <summary>
    /// This structure represents the Image type.
    /// </summary>
    public struct Image
    {        
    }


    /// <summary>
    /// This structure represents a custom type.
    /// </summary>
    public class CustomType
    {
        /// <summary>
        /// Default class constructor.
        /// </summary>
        /// <param name="name">Name of custom type.</param>
        public CustomType(string name)
        {
            Name = name;
        }


        /// <summary>
        /// Get/set the name of the custom type.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <returns>String representation of this instance.</returns>
        public override string ToString()
        {
            return Name;
        }

    }


    /// <summary>
    /// This enumeration lists possible creation methods.
    /// </summary>
    public enum DbCreateType
    {
        /// <summary>
        /// Drop and create.
        /// </summary>
        DropCreate,


        /// <summary>
        /// Just create.
        /// </summary>
        Create,


        /// <summary>
        /// Update.
        /// </summary>
        Update
    }



    /// <summary>
    /// DomainModel representing class/table mappings.
    /// </summary>
    public class Model
    {
        private Relation _lastRelation;

        /// <summary>
        /// Default class constructor.
        /// </summary>
        public Model() : this(string.Empty)
        {
        }


        /// <summary>
        /// Creates a new instance with a given model name.
        /// </summary>
        /// <param name="name">The model name.</param>
        public Model(string name)
        {
            Name = name;
            this.Properties = new List<PropInfo>();
            this.Includes = new List<Include>();
            this.ComposedOf = new List<Composition>();
            this.OneToMany = new List<Relation>();
            this.OneToOne = new List<Relation>();
        }


        /// <summary>
        /// Get/set the model name.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Get/set the model table name.
        /// </summary>
        public string TableName { get; set; }


        /// <summary>
        /// Get/set the model namespace.
        /// </summary>
        public string NameSpace { get; set; }


        /// <summary>
        /// Get/set the model inheritance chain.
        /// </summary>
        public string Inherits { get; set; }


        /// <summary>
        /// Get/set whether to generate a table.
        /// </summary>
        public bool GenerateTable { get; set; }


        /// <summary>
        /// Get/set whether to generate or map.
        /// </summary>
        public bool GenerateOrMap { get; set; }
 

        /// <summary>
        /// Get/set whether to generate code.
        /// </summary>
        public bool GenerateCode { get; set; }


        /// <summary>
        /// Get/set whether to generate validations.
        /// </summary>
        public bool GenerateValidation { get; set; }


        /// <summary>
        /// Get/set whether to generate tests.
        /// </summary>
        public bool GenerateTests { get; set; }


        /// <summary>
        /// Get/set whether to generate user interfaces.
        /// </summary>
        public bool GenerateUI { get; set; }


        /// <summary>
        /// Get/set whether to generate a REST API.
        /// </summary>
        public bool GenerateRestApi { get; set; }


        /// <summary>
        /// Get/set whether to generate feeds.
        /// </summary>
        public bool GenerateFeeds { get; set; }


        /// <summary>
        /// Get/set whether this is a web UI.
        /// </summary>
        public bool IsWebUI { get; set; }


        /// <summary>
        /// Get/set the list of properties.
        /// </summary>
        public List<PropInfo> Properties { get; set; }


        /// <summary>
        /// Get/set the properties sort order.
        /// </summary>
        public int PropertiesSortOrder { get; set; }


        /// <summary>
        /// Get/set the exclude files.
        /// </summary>
        public string ExcludeFiles { get; set; }


        /// <summary>
        /// Get/set the composite flag.
        /// </summary>
        public bool IsComposite { get; set; }


        /// <summary>
        /// Get/set the install sql file.
        /// </summary>
        public string InstallSqlFile { get; set; }


        /// <summary>
        /// Get /set the repository type.
        /// </summary>
        public string RepositoryType { get; set; }


        /// <summary>
        /// List of names of model whose properties to include.
        /// </summary>
        public List<Include> Includes { get; set; }


        /// <summary>
        /// List of objects that compose this model.
        /// </summary>
        public List<Composition> ComposedOf { get; set; }


        /// <summary>
        /// Get/set the list of specifications of user interface generation.
        /// </summary>
        public List<UISpec> UI { get; set; }


        /// <summary>
        /// One-to-many relationships.
        /// </summary>
        public List<Relation> OneToMany { get; set; }


        /// <summary>
        /// One-to-many relationships.
        /// </summary>
        public List<Relation> OneToOne { get; set; }


        /// <summary>
        /// Validations to perform on entity.
        /// </summary>
        public List<ValidationItem> Validations { get; set; }


        // <summary>
        // List of data massage items to apply.
        // </summary>
        //public List<DataMassageItem> DataMassages { get; set; }


        /// <summary>
        /// List of roles that can manage ( delete ) instances of this model.
        /// </summary>
        /// <example>ManagedBy = new List(string){ "Owner", "Moderator", "Admin" }; </example>
        public List<string> ManagedBy { get; set; }


        /// <summary>
        /// List of properties that can be used to lookup up an entity.
        /// e.g. These should typically be the Id ( integer ) and "Name" ( string )
        /// </summary>
        /// <example>LookupOn = new List(string){ "Id", "Name" };</example>
        public List<string> LookupOn { get; set; }


        /// <summary>
        /// Assembly name.
        /// </summary>
        public string AssemblyName { get; set; }


        /// <summary>
        /// Additional settings to make it easy to add new settings dynamically.
        /// Also allows for inheritance.
        /// </summary>
        public Dictionary<string, string> Settings { get; set; }


        #region Fluent-Based
        /// <summary>
        /// Adds a property to the mode.
        /// </summary>
        /// <typeparam name="T">Type of property to add.</typeparam>
        /// <param name="name">Name of property to add.</param>
        /// <returns>The current instance of the model.</returns>
        public Model AddProperty<T>(string name)
        {
            PropInfo p = new PropInfo();
            p.DataType = typeof(T);
            p.Name = name;
            p.CreateCode = true;
            p.CreateColumn = true;
            this.Properties.Add(p);
            return this;
        }


        /// <summary>
        /// Sets the required property and returns this instance of model.
        /// </summary>
        public Model Required
        {
            get { this.Properties[this.Properties.Count - 1].IsRequired = true; return this; }
        }


        /// <summary>
        /// Sets the key property and returns this instance of model.
        /// </summary>
        public Model Key
        {
            get { this.Properties[this.Properties.Count - 1].IsKey = true; return this; }
        }


        /// <summary>
        /// Sets the maxlength property.
        /// </summary>
        /// <param name="max">Max length.</param>
        /// <returns>The current instance of the model.</returns>
        public Model MaxLength(string max)
        {
            this.Properties[this.Properties.Count - 1].MaxLength = max;
            return this;
        }


        /// <summary>
        /// Sets the default value.
        /// </summary>
        /// <param name="val">Default value.</param>
        /// <returns>The current instance of the model.</returns>
        public Model DefaultTo(object val)
        {
            this.Properties[this.Properties.Count - 1].DefaultValue = val;
            return this;
        }


        /// <summary>
        /// Sets the createcolumn property and returns this instance of model.
        /// </summary>
        public Model Persist
        {
            get { this.Properties[this.Properties.Count - 1].CreateColumn = true; return this; }
        }


        /// <summary>
        /// Sets the createcode property and returns this instance of model.
        /// </summary>
        public Model Code
        {
            get { this.Properties[this.Properties.Count - 1].CreateCode = true; return this; }
        }


        /// <summary>
        /// Clears the createcolumn property and returns this instance of model. 
        /// </summary>
        public Model NotPersisted
        {
            get { this.Properties[this.Properties.Count - 1].CreateColumn = false; return this; }
        }


        /// <summary>
        /// Clears the createcode property and returns this instance of model.
        /// </summary>
        public Model NoCode
        {
            get { this.Properties[this.Properties.Count - 1].CreateCode = false; return this; }
        }


        /// <summary>
        /// Sets the isgetteronly property and returns this instance of model.
        /// </summary>
        public Model GetterOnly
        {
            get { this.Properties[this.Properties.Count - 1].IsGetterOnly = true; return this; }
        }


        /// <summary>
        /// Sets the min and max lengths.
        /// </summary>
        /// <param name="min">Minimum length.</param>
        /// <param name="max">Maximum length.</param>
        /// <returns>The current instance of the model.</returns>
        public Model Range(string min, string max)
        {
            this.Properties[this.Properties.Count - 1].MinLength = min;
            this.Properties[this.Properties.Count - 1].MaxLength = max;
            return this;          
        }


        /// <summary>
        /// Determines that code and validations should be generated for this model.
        /// </summary>
        /// <returns>The current instance of the model.</returns>
        public Model BuildCode()
        {
            this.GenerateCode = true;
            this.GenerateValidation = true;
            return this;
        }


        /// <summary>
        /// Determines that no validation should be generated for this model.
        /// </summary>
        /// <returns>The current instance of the model.</returns>
        public Model NoValidation()
        {
            this.GenerateValidation = false;
            return this;
        }


        /// <summary>
        /// Sets the table name and determines that a table should be generated.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <returns>The current instance of the model.</returns>
        public Model BuildTable(string tableName)
        {
            this.TableName = tableName;
            this.GenerateTable = true;
            return this;
        }


        /// <summary>
        /// Automatically determines the install sql file name.
        /// </summary>
        /// <returns>The current instance of the model.</returns>
        public Model BuildInstallSqlFile()
        {
            this.InstallSqlFile = string.Format("{0}.sql", this.Name);
            return this;
        }


        /// <summary>
        /// Sets the namespace for the model.
        /// </summary>
        /// <param name="nameSpace">Namespace.</param>
        /// <returns>The current instance of the model.</returns>
        public Model NameSpaceIs(string nameSpace)
        {
            this.NameSpace = nameSpace;
            return this;
        }


        /// <summary>
        /// Sets the model from which this one inherits from.
        /// </summary>
        /// <param name="modelName">Name of model from which this inherits from.</param>
        /// <returns>The current instance of the model.</returns>
        public Model InheritsFrom(string modelName)
        {
            this.Inherits = modelName;
            return this;
        }


        /// <summary>
        /// Automatically generates the list of exclude files and the repository type
        /// for an active record entity.
        /// </summary>
        /// <returns>The current instance of the model.</returns>
        public Model BuildActiveRecordEntity()
        {
            this.ExcludeFiles = "ActiveRecord.cs,Service.cs,Validator.cs,Repository.cs,Settings.cs,Feeds.cs,ImportExport.cs,Serializer.cs";
            this.RepositoryType = "RepositorySql";
            return this;
        }


        /// <summary>
        /// Sets the regular expression pattern to be used by this instance
        /// and defines that the regular expression is a constant.
        /// </summary>
        /// <param name="regExPattern">Regular expression pattern.</param>
        /// <returns>The current instance of the model.</returns>
        public Model RegExConst(string regExPattern)
        {
            this.Properties[this.Properties.Count - 1].RegEx = regExPattern;
            this.Properties[this.Properties.Count - 1].IsRegExConst = true;
            return this;
        }


        /// <summary>
        /// Sets the regular expression patter to be used by this instance.
        /// </summary>
        /// <param name="regExPattern">Regular expression pattern.</param>
        /// <returns>The current instance of the model.</returns>
        public Model RegEx(string regExPattern)
        {
            this.Properties[this.Properties.Count-1].RegEx = regExPattern;
            return this;
        }


        /// <summary>
        /// Adds composition information to this instance.
        /// </summary>
        /// <param name="modelName">Name of model.</param>
        /// <returns>The current instance of the model.</returns>
        public Model HasComposition(string modelName)
        {
            this.ComposedOf.Add(new Composition(modelName));
            return this;
        }


        /// <summary>
        /// Adds a new include.
        /// </summary>
        /// <param name="modelName">Model name to include.</param>
        /// <returns>The current instance of the model.</returns>
        public Model HasInclude(string modelName)
        {
            this.Includes.Add(new Include(modelName));
            return this;
        }


        /// <summary>
        /// Adds a new one-to-one relation to another model.
        /// </summary>
        /// <param name="modelName">Name of model to add relation to.</param>
        /// <returns>The current instance of the model.</returns>
        public Model HasOne(string modelName)
        {
            Relation rel = new Relation(modelName);
            this.OneToOne.Add(rel);
            _lastRelation = rel;
            return this;
        }


        /// <summary>
        /// Adds a one-to-many relation to another model.
        /// </summary>
        /// <param name="modelName">Name of model to add relation to.</param>
        /// <returns>The current instance of the model.</returns>
        public Model HasMany(string modelName)
        {
            var rel = new Relation(modelName);
            this.OneToMany.Add(rel);
            _lastRelation = rel;
            return this;
        }


        /// <summary>
        /// Sets the relation key.
        /// </summary>
        /// <param name="key">Name of relation key.</param>
        /// <returns>The current instance of the model.</returns>
        public Model OnKey(string key)
        {
            _lastRelation.Key = key;
            return this;
        }


        /// <summary>
        /// Sets the foreign key. 
        /// </summary>
        /// <param name="key">Name of foreign key.</param>
        /// <returns>The current instance of the model.</returns>
        public Model OnForeignKey(string key)
        {
            _lastRelation.ForeignKey = key;
            return this;
        }


        /// <summary>
        /// Get this instance.
        /// </summary>
        public Model Mod { get { return this; } }
        #endregion
    }



    /// <summary>
    /// Composition information.
    /// </summary>
    public class Include
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="refModel">The name of the referenced model.</param>
        public Include(string refModel)
        {
            Name = refModel;
            GenerateOrMap = true;
            GenerateCode = true;
            GenerateUI = true;
        }


        /// <summary>
        /// Name of the model that in the <see cref="ModelContainer"/> that
        /// represents this composition.
        /// </summary>
        public string Name { get; set; }

        
        /// <summary>
        /// Get/set the generateormap flag.
        /// </summary>
        public bool GenerateOrMap { get; set; }


        /// <summary>
        /// Get/set whether to generate code.
        /// </summary>
        public bool GenerateCode { get; set; }


        /// <summary>
        /// Get/set whether to generate a user interface.
        /// </summary>
        public bool GenerateUI { get; set; }


        /// <summary>
        /// Get/set the model referenced.
        /// </summary>
        public Model ModelRef { get; set; }
    }



    /// <summary>
    /// Composition information.
    /// </summary>
    public class Composition : Include
    {
        /// <summary>
        /// Default class constructor.
        /// </summary>
        /// <param name="refModel">The referenced model.</param>
        public Composition(string refModel) : base(refModel) { }
    }


    /// <summary>
    /// Validation definition for a specific property.
    /// </summary>
    public class ValidationItem
    {
        /// <summary>
        /// Initialize the validator the property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="validator">The type validator.</param>
        public ValidationItem(string property, Type validator)
        {
            PropertyToValidate = property;
            PropertyValidator = validator;
        }


        /// <summary>
        /// The name of the property on the entity to validate.
        /// </summary>
        public string PropertyToValidate { get; set; }


        /// <summary>
        /// The datatype of the validator to use for validating this property.
        /// </summary>
        public Type PropertyValidator { get; set; }


        /// <summary>
        /// Whether or not the validator is instance based or can be statically called.
        /// </summary>
        public bool IsStatic { get; set; }
    }
}
