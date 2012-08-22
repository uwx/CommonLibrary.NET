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
using System.Reflection;


namespace ComLib
{
    /// <summary>
    /// This class provides helper expression methods.
    /// </summary>
    public class ExpressionHelper
    {        

        /// <summary>
        /// Get the property name from the expression.
        /// e.g. GetPropertyName(Person)( p => p.FirstName);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T, object>> exp)
        {
            MemberExpression memberExpression = null;

            // Get memberexpression.
            if (exp.Body is MemberExpression)
                memberExpression = exp.Body as MemberExpression;

            if (exp.Body is UnaryExpression)
            {
                var unaryExpression = exp.Body as UnaryExpression;
                if (unaryExpression.Operand is MemberExpression)
                    memberExpression = unaryExpression.Operand as MemberExpression;
            }

            if (memberExpression == null)
                throw new InvalidOperationException("Not a member access.");

            var info = memberExpression.Member as PropertyInfo;
            return info.Name;
        }


        /// <summary>
        /// Get the property name from the expression.
        /// e.g. GetPropertyName(Person)( p => p.FirstName);
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static string GetPropertyName(Expression<Func<object>> exp)
        {
            var memberExpression = exp.Body as MemberExpression;
            if (memberExpression == null) throw new InvalidOperationException("Not a member access.");

            var info = memberExpression.Member as PropertyInfo;
            return info.Name;
        }


        /// <summary>
        /// Get the property name from the expression.
        /// e.g. GetPropertyName(Person)( p => p.FirstName);
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static object GetPropertyNameAndValue(Expression<Func<object>> exp, ref string propName)
        {
            var memberExpression = exp.Body as MemberExpression;
            PropertyInfo propInfo = null;
            if (exp.Body is MemberExpression)
            {
                propInfo = ((MemberExpression)exp.Body).Member as PropertyInfo;
            }
            else if (exp.Body is UnaryExpression)
            {
                Expression op = ((UnaryExpression)exp.Body).Operand;
                propInfo = ((MemberExpression)op).Member as PropertyInfo;
            }

            object val = exp.Compile().DynamicInvoke();
            propName = propInfo.Name;
            return val;
        }


        /// <summary>
        /// Delete using the expression.
        /// e.g. entity.LogLevel == 1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        static void Build<T>(Expression<Func<T, bool>> predicate)
        {
            string format = "{0} {1} {2}";
            BinaryExpression exp = (BinaryExpression)predicate.Body;
            MemberExpression left = (MemberExpression)exp.Left;

            string fieldName = left.Member.Name;
            string operand = RepositoryExpressionTypeHelper.GetText(exp.NodeType);
            string fieldVal = "";
            object rval = null;

            if (exp.Right is ConstantExpression)
            {
                ConstantExpression right = (ConstantExpression)exp.Right;
                fieldVal = RepositoryExpressionValueHelper.GetVal(right.Value);
            }
            else if (exp.Right is MemberExpression)
            {
                MemberExpression right = (MemberExpression)exp.Right;
                rval = Expression.Lambda(exp.Right).Compile().DynamicInvoke();
            }

            // Check for data types.
            PropertyInfo propInfo = left.Member as PropertyInfo;

            // String ? Encode single quotes. ' = ''
            if (propInfo.PropertyType == typeof(string))
            {
                fieldVal = string.Format("'{0}'", fieldVal.Replace("'", "''"));
            }
            else if (propInfo.PropertyType == typeof(DateTime))
            {
                fieldVal = "'" + ((DateTime)rval).ToShortDateString() + "'";
            }

            string val = string.Format(format, fieldName, operand, fieldVal);
            Console.WriteLine(val);
        }
    }


    class RepositoryExpressionValueHelper
    {
        public static string GetVal(object val)
        {
            if (val.GetType() == typeof(bool) || val.GetType() == typeof(Boolean))
            {
                bool boolVal = (bool)val;
                return boolVal == true ? "1" : "0";
            }
            return val.ToString();
        }
    }


    class RepositoryExpressionTypeHelper
    {
        private static IDictionary<ExpressionType, string> _map = new Dictionary<ExpressionType, string>();

        static RepositoryExpressionTypeHelper()
        {
            _map[ExpressionType.Equal] = "=";
            _map[ExpressionType.NotEqual] = "<>";
            _map[ExpressionType.GreaterThanOrEqual] = ">=";
            _map[ExpressionType.LessThanOrEqual] = "<=";
            _map[ExpressionType.LessThan] = "<";
            _map[ExpressionType.GreaterThan] = ">";
        }


        /// <summary>
        /// Get the sql text equivalent of the expression type.
        /// </summary>
        /// <param name="expType"></param>
        /// <returns></returns>
        public static string GetText(ExpressionType expType)
        {
            if (!_map.ContainsKey(expType))
                throw new ArgumentException("expresion type :" + expType.ToString() + "not supported.");

            return _map[expType];
        }
    }
}
