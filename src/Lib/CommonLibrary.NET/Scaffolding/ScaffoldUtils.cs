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
using System.Collections;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;

using ComLib;
using ComLib.Reflection;


namespace ComLib.Scaffolding
{
    public class ScaffoldUtils
    {
        private NameValueCollection _params;


        /// <summary>
        /// Initialize with the User-Interface parameters.
        /// </summary>
        /// <param name="uiParams"></param>
        public ScaffoldUtils(NameValueCollection uiParams)
        {
            _params = uiParams;
        }


        /// <summary> 
        /// Create a table containing : 
        /// 1. Name of the property 
        /// 2. Input control to have user put in value for the property 
        /// 3. Type of the property. 
        /// </summary> 
        /// <param name="entityType"></param> 
        /// <param name="clientId"></param> 
        /// <returns></returns> 
        public Table BuildEntityUITable(string entityName, string clientId, bool loadEntityValues, object entityObject, StringDictionary propertiesToExclude)
        {
            IList<PropertyInfo> properties = ReflectionUtils.GetProperties(propertiesToExclude, Type.GetType(entityName));
            Table table = new Table();
            table.ID = ScaffoldConstants.EntityUIContainerName;
            for (int ndx = 0; ndx < properties.Count; ndx++)
            {
                PropertyInfo property = properties[ndx];
                object entityValue = string.Empty;
                if (loadEntityValues)
                {
                    entityValue = ReflectionUtils.GetPropertyValueSafely(entityObject, property);
                }

                // The controller will provide all the writable properties. 
                //if (property.CanWrite) 
                {
                    TableRow row = CreatePropertyRow(property, clientId, loadEntityValues, entityValue);
                    table.Rows.Add(row);
                }
            }
            return table;
        }


        /// <summary>
        /// Get the business entityies properties values from the dynamic UI.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public IList<KeyValuePair<string, string>> GetValuesFromUI(string entityName, string clientId, StringDictionary propsToExclude)
        {
            IList<PropertyInfo> properties = ReflectionUtils.GetProperties(propsToExclude, Type.GetType(entityName));
            IList<KeyValuePair<string, string>> propValues = new List<KeyValuePair<string, string>>();

            // Get the property values entered by the user. 
            foreach (PropertyInfo property in properties)
            {
                // Get id of the control.
                string id = GetControlId(clientId, property.Name);

                // Add to the list of property values.
                if (_params[id] != null)
                {
                    string val = _params[id] as string;
                    KeyValuePair<string, string> propVal = new KeyValuePair<string, string>(property.Name, val);
                    propValues.Add(propVal);
                }
            }
            return propValues;
        }


        #region Private methods
        /// <summary> 
        /// Gets the value of the property that was inputted by the user on 
        /// the client side. 
        /// </summary> 
        /// <param name="clientId"></param> 
        /// <param name="propertyName"></param> 
        /// <returns></returns> 
        private string GetExistingValue(string clientId, string propertyName)
        {
            string id = GetControlId(clientId, propertyName);
            if (_params[id] != null)
            {
                return _params[id] as string;
            }
            return string.Empty;
        }


        /// <summary> 
        /// Get the fully-qualified id of the textbox control used to store the value 
        /// of the property. 
        /// </summary> 
        /// <param name="clientId"></param> 
        /// <param name="propertyName"></param> 
        /// <returns></returns> 
        private string GetControlId(string clientId, string propertyName)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                clientId = clientId.Replace("_", "$");
            }
            return clientId + "$txt" + propertyName;
        }


        /// <summary>
        /// The id of the HTML control that will contain the value of the property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private string BuildControlId(PropertyInfo property)
        {
            return "txt" + property.Name;
        }


        /// <summary>
        /// Creates an HTML table row that contains controls to describe and add/edit the value of the property.
        /// e.g.
        /// FirstName:  TextBox(FirstName Value)    String
        /// </summary>
        /// <param name="property"></param>
        /// <param name="cliendId"></param>
        /// <param name="usePropertyValue"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        private TableRow CreatePropertyRow(PropertyInfo property, string cliendId, bool usePropertyValue, object propertyValue)
        {
            // Create the row. 
            TableRow row = new TableRow();

            // Create input control for the value. 
            TextBox txtProp = new TextBox();

            // Set the id of the textbox based on the propertyName.
            // txtFirstName
            txtProp.ID = BuildControlId(property);

            // Apply the existing value of the property to the textbox
            if (usePropertyValue)
            {
                txtProp.Text = Convert.ToString(propertyValue);
            }
            else
            {
                txtProp.Text = GetExistingValue(cliendId, property.Name);
            }

            // Store the input control into a tablecell. <Textbox>(Firstname)
            TableCell propValue = new TableCell();
            propValue.Controls.Add(txtProp);

            // Store the property name into a table cell. "FirstName"
            TableCell propName = new TableCell();
            propName.Text = property.Name;

            // Store the property type into a table cell. "String"
            TableCell propType = new TableCell();
            propType.Text = property.PropertyType.FullName;
            
            // Add the table cells to the row. 
            // FirstName:   <TextBoxBox>(Value)     String
            row.Cells.Add(propName);
            row.Cells.Add(propValue);
            row.Cells.Add(propType);
            
            return row;
        }
        #endregion
    }
}
