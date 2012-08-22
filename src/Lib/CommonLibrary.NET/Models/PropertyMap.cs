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
    /// This class is used to hold information about a property.
    /// </summary>
    public class PropInfo
    {
        /// <summary>
        /// Default class constructor.
        /// </summary>
        public PropInfo()
        {         
        }

        
        /// <summary>
        /// Initialize using name of property and it's datatype.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="dataType">Property data type.</param>
        public PropInfo(string name, Type dataType)
        {
            Name = name;
            DataType = dataType;
        }


        /// <summary>
        /// Name of the property
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// DataType of the property.
        /// </summary>
        public Type DataType { get; set; }    


        /// <summary>
        /// Default value for this property.
        /// </summary>
        public object DefaultValue { get; set; }


        /// <summary>
        /// Regular expression to use for validations.
        /// </summary>
        public string RegEx { get; set; }


        /// <summary>
        /// Is Primary key.
        /// </summary>
        public bool IsKey { get; set; }


        /// <summary>
        /// If this is a required / not-null property.
        /// </summary>
        public bool IsRequired { get; set; }


        /// <summary>
        /// Indicate if this property is only a getter, no setter.
        /// </summary>
        public bool IsGetterOnly { get; set; }


        /// <summary>
        /// Indicate if min length should be checked.
        /// </summary>
        public bool CheckMinLength { get; set; }


        /// <summary>
        /// Indicate if max length should be checked.
        /// </summary>
        public bool CheckMaxLength { get; set; }


        private string _minLength = string.Empty;
        /// <summary>
        /// Maximum length for this property if string.
        /// </summary>
        public string MinLength
        {
            get { return _minLength; }
            set
            {
                if (DataType == typeof(string) && !string.IsNullOrEmpty(value))
                {
                    if (value == "-1")
                    {
                        CheckMinLength = false;
                        IsRequired = false;
                    }
                    else
                    {
                        CheckMinLength = true;
                        _minLength = value;
                        if (_minLength != "0")
                            IsRequired = true;
                    }
                }
            }
        }


        private string _maxLength = string.Empty;
        /// <summary>
        /// Maximum length for this property if string.
        /// </summary>
        public string MaxLength 
        {
            get { return _maxLength; }
            set
            {
                if (DataType == typeof(string) && !string.IsNullOrEmpty(value))
                {
                    if (value == "-1")
                        CheckMaxLength = false;
                    else
                    {
                        CheckMaxLength = true;
                        _maxLength = value;
                    }
                }
            }
        }


        /// <summary>
        /// Is unique.
        /// </summary>
        public bool IsUnique { get; set; }


        private string _columnName;
        /// <summary>
        /// Name if empty.
        /// </summary>
        public string ColumnName
        {
            get
            {
                if (string.IsNullOrEmpty(_columnName))
                    return Name;
                return _columnName;
            }
            set { _columnName = value; }
        }


        private bool _createCode = true;
        /// <summary>
        /// Whether or not to create this property in code.
        /// </summary>
        public bool CreateCode
        {
            get { return _createCode; }
            set { _createCode = value; }
        }


        private bool _createColumn = true;
        /// <summary>
        /// Whether or not to create this property in code.
        /// </summary>
        public bool CreateColumn
        {
            get { return _createColumn; }
            set { _createColumn = value; }
        }


        /// <summary>
        /// Indicates if the regular expression supplied is a constant.
        /// </summary>
        public bool IsRegExConst { get; set; }
    }



    /// <summary>
    /// Relationship to model.
    /// </summary>
    public class Relation
    {
        /// <summary>
        /// Initialize with the model name.
        /// </summary>
        /// <param name="modelName">Name of model.</param>
        public Relation(string modelName)
        {
            ModelName = modelName;
        }


        /// <summary>
        /// Get/set the model name.
        /// </summary>
        public string ModelName { get; set; }


        /// <summary>
        /// Get/set the foreign key.
        /// </summary>
        public string ForeignKey { get; set; }


        /// <summary>
        /// Get/set the key.
        /// </summary>
        public string Key { get; set; }
    }


    /// <summary>
    /// This class is used to store information about the
    /// UI to be generated.
    /// </summary>
    public class UISpec
    {
        /// <summary>
        /// Get/set whether to create edit user interface.
        /// </summary>
        public bool CreateEditUI { get; set; }


        /// <summary>
        /// Get/set whether to create summary user interface.
        /// </summary>
        public bool SummaryUI { get; set; }


        /// <summary>
        /// Get/set whether to create details user interface.
        /// </summary>
        public bool DetailsUI { get; set; }


        /// <summary>
        /// Get/set the property name.
        /// </summary>
        public string PropertyName { get; set; }


        /// <summary>
        /// Default class constructor.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="createEditUI">Edit UI creation flag.</param>
        /// <param name="summaryUI">Summary UI creation flag.</param>
        /// <param name="detailsUI">Details UI creation flag.</param>
        public UISpec(string propertyName, bool createEditUI, bool summaryUI, bool detailsUI)
        {
            PropertyName = propertyName;
            CreateEditUI = createEditUI;
            SummaryUI = summaryUI;
            DetailsUI = detailsUI;
        }
    }
}
