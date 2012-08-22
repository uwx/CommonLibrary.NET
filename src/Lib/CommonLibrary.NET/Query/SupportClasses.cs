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
using System.Linq.Expressions;



namespace ComLib.Data
{
    /// <summary>
    /// Represents a field in a select clause.
    /// </summary>
    public class SelectField
    {
        /// <summary>
        /// Represented field.
        /// </summary>
        public string Field;


        /// <summary>
        /// Field alias.
        /// </summary>
        public string Alias;


        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return ToString(false, "[", "]");
        }


        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <param name="surround">True to surround alias with text.</param>
        /// <param name="left">Left surround text.</param>
        /// <param name="right">Right surround text.</param>
        /// <returns>String representation.</returns>
        public virtual string ToString(bool surround, string left, string right)
        {
            string aliasText = !string.IsNullOrEmpty(Alias) ? " as " + Alias : string.Empty;
            if (!surround)
                return Field + aliasText;

            return left + Field + right + aliasText;
        }
    }  


    /// <summary>
    /// Represents one entry to order by.
    /// </summary>
    public class OrderByClause
    {
        /// <summary>
        /// Field name.
        /// </summary>
        public string Field;


        /// <summary>
        /// Ordering information.
        /// </summary>
        public OrderByType Ordering;


        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return ToString(false, "[", "]");
        }


        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <param name="surround">True to surround alias with text.</param>
        /// <param name="left">Left surround text.</param>
        /// <param name="right">Right surround text.</param>
        /// <returns>String representation.</returns>
        public virtual string ToString(bool surround, string left, string right)
        {
            if (!surround)
                return Field + " " + Ordering.ToString();

            return left + Field + right + " " + Ordering.ToString();
        }
    }
    

    /// <summary>
    /// Represents a condition entry.
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// And | Or
        /// </summary>
        public ConditionType ConditionType;


        /// <summary>
        /// Condition field.
        /// </summary>
        public string Field;


        /// <summary>
        /// Type of comparison.
        /// </summary>
        public string Comparison;


        /// <summary>
        /// Value to compare to.
        /// </summary>
        public string Value;


        /// <summary>
        /// Default class constructor.
        /// </summary>
        public Condition()
        {
        }


        /// <summary>
        /// Creates an instance of this class using a condition type.
        /// </summary>
        /// <param name="condition">Condition type to use.</param>
        public Condition(ConditionType condition)
        {
            ConditionType = condition;
        }


        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="condition">Condition type to use.</param>
        /// <param name="key">Field name.</param>
        /// <param name="assignment">Comparison type.</param>
        /// <param name="val">Value to compare to.</param>
        public Condition(ConditionType condition, string key, string assignment, string val)
        {
            ConditionType = condition;
            Field = key;
            Comparison = assignment;
            Value = val;
        }


        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <returns>String representation.</returns>
        public override string ToString()
        {
            return ToString(false, "[", "]");
        }


        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <param name="surround">True to surround alias with text.</param>
        /// <param name="left">Left surround text.</param>
        /// <param name="right">Right surround text.</param>
        /// <returns>String representation.</returns>
        public virtual string ToString(bool surround, string left, string right)
        {
            string col = surround ? left + Field + right : Field;
            string val = string.IsNullOrEmpty(Value) ? "''" : Value;

            return string.Format("{0} {1} {2}", col, Comparison, val);
        }
    }


    /// <summary>
    /// Enumeration to represent condition types
    /// used when constructing statements.
    /// </summary>
    public enum ConditionType { 
        /// <summary>
        /// AND condition.
        /// </summary>
        And, 
        

        /// <summary>
        /// OR condition.
        /// </summary>
        Or, 
        
        
        /// <summary>
        /// No condition.
        /// </summary>
        None };


    /// <summary>
    /// Enumeration that represents type of ordering.
    /// </summary>
    public enum OrderByType { 
        /// <summary>
        /// Ascending ordering.
        /// </summary>
        Asc, 
        
        
        /// <summary>
        /// Descending ordering.
        /// </summary>
        Desc };


    /// <summary>
    /// Enumeration representing the comparison operator.
    /// </summary>
    public enum Eq { 
        /// <summary>
        /// Less-than operator.
        /// </summary>
        Less, 
        
        
        /// <summary>
        /// Less-or-equal operator.
        /// </summary>
        LessEqual, 
        
        
        /// <summary>
        /// Equals operator.
        /// </summary>
        Equals, 
        
        
        /// <summary>
        /// More-than operator.
        /// </summary>
        More, 
        
        
        /// <summary>
        /// More-or-equal-than operator.
        /// </summary>
        MoreEqual, 
        
        
        /// <summary>
        /// Not-equal operator.
        /// </summary>
        NotEqual, 
        
        
        /// <summary>
        /// In operator.
        /// </summary>
        In, 
        
        
        /// <summary>
        /// Not-in operator.
        /// </summary>
        NotIn };
}
