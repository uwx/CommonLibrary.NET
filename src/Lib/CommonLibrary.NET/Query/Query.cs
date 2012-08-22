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
    /// This class implements a method to create queries.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Query<T> : Query, IQuery<T>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <returns>An empty string.</returns>
        public static new IQuery<T> New()
        {
            return New(string.Empty);
        }


        // Static factory
        /// <summary>
        /// Creates new Query object for the specified db provider name.
        /// </summary>
        /// <param name="dbProviderName">Name of the db provider.</param>
        /// <returns>A new instance of Query.</returns>
        public static IQuery<T> New(string dbProviderName)
        {
            if (string.IsNullOrEmpty(dbProviderName)) return new Query<T>();
            
            // No support for other databases yet.
            // Not sure it's necessay but just in case.
            return new Query<T>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Query&lt;T&gt;"/> class.
        /// </summary>
        public Query()
        {
            _builder = new QueryBuilderSql(this);
        }


        /// <summary>
        /// Adds a condition to the query.
        /// </summary>
        /// <param name="exp">Expression to extract field name.</param>
        /// <param name="comparison">Comparison expression.</param>
        /// <param name="val">Value to compare against.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> AddCondition(Expression<Func<T, object>> exp, ExpressionType comparison, object val)
        {
            string field = ExpressionHelper.GetPropertyName<T>(exp);
            AddCondition(field, comparison, val);
            return this;
        }


        /// <summary>
        /// Adds columns to a select statement.
        /// </summary>
        /// <param name="colExpressions">Expressions to retrieve column names.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Select(params Expression<Func<T, object>>[] colExpressions)
        {
            if (colExpressions == null || colExpressions.Length == 0)
                return this;

            string[] cols = new string[colExpressions.Length];
            for (int ndx = 0; ndx < colExpressions.Length; ndx++)
                cols[ndx] = ExpressionHelper.GetPropertyName<T>(colExpressions[ndx]);

            return Select(cols);
        }


        /// <summary>
        /// Adds a WHERE clause to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Where(Expression<Func<T, object>> exp)
        {
            string field = ExpressionHelper.GetPropertyName<T>(exp);
            return StartNewCondition(ConditionType.None, field) as IQuery<T>;
        }


        /// <summary>
        /// Adds an AND condition to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> And(Expression<Func<T, object>> exp)
        {
            string field = ExpressionHelper.GetPropertyName<T>(exp);
            return StartNewCondition(ConditionType.And, field) as IQuery<T>;
        }


        /// <summary>
        /// Adds an OR condition to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Or(Expression<Func<T, object>> exp)
        {
            string field = ExpressionHelper.GetPropertyName<T>(exp);
            return StartNewCondition(ConditionType.Or, field) as IQuery<T>;
        }


        /// <summary>
        /// Adds an Order-By clause to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> OrderBy(Expression<Func<T, object>> exp)
        {
            string field = ExpressionHelper.GetPropertyName<T>(exp);
            return OrderByInternal(field, OrderByType.Asc);
        }


        /// <summary>
        /// Adds a descending order clause to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> OrderByDescending(Expression<Func<T, object>> exp)
        {
            string field = ExpressionHelper.GetPropertyName<T>(exp);
            return OrderByInternal(field, OrderByType.Desc);
        }


        /// <summary>
        /// Adds columns to a select statement.
        /// </summary>
        /// <param name="cols">Array with columns to add.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Select(params string[] cols)
        {
            if( cols == null || cols.Length == 0)
                return this;
            
            foreach (string col in cols)
                _data.SelectFields.Add(new SelectField() { Field = col });

            return this;
        }


        /// <summary>
        /// Adds a column to a select statement.
        /// </summary>
        /// <param name="col">Name of column to add.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Select(string col)
        {
            _data.SelectFields.Add(new SelectField() { Field = col });
            return this;
        }


        /// <summary>
        /// Adds an AS clause to the query.
        /// </summary>
        /// <param name="columnAlias">Column alias to use.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> As(string columnAlias)
        {
            if (_data.SelectFields == null || _data.SelectFields.Count == 0)
                return this;

            _data.SelectFields[_data.SelectFields.Count - 1].Alias = columnAlias;
            return this;
        }


        /// <summary>
        /// Designates the table to operate on.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> From(string tableName)
        {
            _data.From = tableName;
            return this;
        }


        /// <summary>
        /// Adds a WHERE condition to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Where(string field)
        {
            return StartNewCondition(ConditionType.None, field) as IQuery<T>;
        }     


        /// <summary>
        /// Adds an AND operation to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> And(string field)
        {
            return StartNewCondition(ConditionType.And, field) as IQuery<T>;
        }


        /// <summary>
        /// Adds an OR operation to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Or(string field)
        {
            return StartNewCondition(ConditionType.Or, field) as IQuery<T>;
        }


        /// <summary>
        /// Adds an equality condition to the query.
        /// </summary>
        /// <param name="val">Value to check for.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Is(object val)
        {
            return BuildCondition("=", val);
        }


        /// <summary>
        /// Adds an inequality condition to the query.
        /// </summary>
        /// <param name="val">Value to check for.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Not(object val)
        {
            return BuildCondition("<>", val);
        }


        /// <summary>
        /// Adds a null comparison condition to the query.
        /// </summary>
        /// <returns>This instance.</returns>
        public IQuery<T> Null()
        {
            _lastCondition.Comparison = "is";
            _lastCondition.Value = "null";
            return this;
        }


        /// <summary>
        /// Adds a not-null comparison condition to the query.
        /// </summary>
        /// <returns>This instance.</returns>
        public IQuery<T> NotNull()
        {
            _lastCondition.Comparison = "is not";
            _lastCondition.Value = "null";
            return this;
        }


        /// <summary>
        /// Adds an IN condition to the query.
        /// </summary>
        /// <typeparam name="TParam">Type of parameter.</typeparam>
        /// <param name="vals">Array with values to check for.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> In<TParam>(params object[] vals)
        {
            return BuildCondition<TParam>("in", vals);
        }


        /// <summary>
        /// Adds a NOT-IN condition to the query.
        /// </summary>
        /// <typeparam name="TParam">Type of parameter.</typeparam>
        /// <param name="vals">Array with values to check for.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> NotIn<TParam>(params object[] vals)
        {
            return BuildCondition<TParam>("not in", vals);
        }


        /// <summary>
        /// Adds a LIKE condition to the query.
        /// </summary>
        /// <param name="val">Value to check for.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Like(string val)
        {
            return BuildCondition("like", val);
        }


        /// <summary>
        /// Adds a LIKE condition to the query.
        /// </summary>
        /// <param name="val">Value to check for.</param>
        /// <param name="addWildcardPrefix">True to add a wildcard prefix.</param>
        /// <param name="addWildcardSuffix">True to add a wildcard suffix.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Like(string val, bool addWildcardPrefix, bool addWildcardSuffix)
        {
            if (addWildcardPrefix) val = "%" + val;
            if (addWildcardSuffix) val = val + "%";
            return BuildCondition("like", val);
        }


        /// <summary>
        /// Adds a greater-than comparison to the query.
        /// </summary>
        /// <param name="val">Value to check against.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> MoreThan(object val)
        {
            return BuildCondition(">", val);
        }


        /// <summary>
        /// Adds a greater-or-equal comparison to the query.
        /// </summary>
        /// <param name="val">Value to check against.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> MoreEqual(object val)
        {
            return BuildCondition(">=", val);             
        }
        
        
        /// <summary>
        /// Adds a less-than comparison to the query.
        /// </summary>
        /// <param name="val">Value to check against.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> LessThan(object val)
        {
            return BuildCondition("<", val);
        }


        /// <summary>
        /// Adds a less-or-equal-than comparison to the query.
        /// </summary>
        /// <param name="val">Value to check against.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> LessEqual(object val)
        {
            return BuildCondition("<=", val);
        }


        /// <summary>
        /// Adds an ascending order-by clause to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>this instance.</returns>
        public IQuery<T> OrderBy(string field)
        {
            return OrderByInternal(field, OrderByType.Asc);
        }


        /// <summary>
        /// Adds a descending order-by clause to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> OrderByDescending(string field)
        {
            return OrderByInternal(field, OrderByType.Desc);
        }


        /// <summary>
        /// Limits the query result to a number of records.
        /// </summary>
        /// <param name="maxRecords">Number of records to limit.</param>
        /// <returns>This instance.</returns>
        public IQuery<T> Limit(int maxRecords)
        {
            _data.RecordLimit = maxRecords;
            _data.IsRecordLimitEnabled = true;
            return this;
        }


        /// <summary>
        /// Completes the query.
        /// </summary>
        /// <returns>This instance.</returns>
        public IQuery<T> End()
        {
            Complete();
            return this;
        }


        #region Private Helper methods
        /// <summary>
        /// Adds a condition to the query.
        /// </summary>
        /// <typeparam name="TParam">Type of parameter.</typeparam>
        /// <param name="comparison">Comparison operator.</param>
        /// <param name="vals">Values to use.</param>
        /// <returns>This instance.</returns>
        protected new IQuery<T> BuildCondition<TParam>(string comparison, params object[] vals)
        {
            var buffer = new StringBuilder();
            // String
            if (typeof(TParam) == typeof(string))
            {
                buffer.Append("'" + Encode((string)vals[0]) + "'");
                for (int ndx = 1; ndx < vals.Length; ndx++)
                    buffer.Append(", '" + Encode((string)vals[ndx]) + "'");
            }
            // DateTime
            else if (typeof(TParam) == typeof(DateTime))
            {
                buffer.Append("'" + Encode(((DateTime)vals[0]).ToShortDateString()) + "'");
                for (int ndx = 1; ndx < vals.Length; ndx++)
                    buffer.Append(", '" + Encode(((DateTime)vals[ndx]).ToShortDateString()) + "'");
            }
            // int, long, float, double.
            else if (TypeHelper.IsNumeric(typeof(TParam)))
            {
                buffer.Append(TypeHelper.Join(vals));
            }
            
            _lastCondition.Comparison = comparison;
            _lastCondition.Value =  "( " + buffer.ToString() + " )";
            return this;
        }


        /// <summary>
        /// Adds a condition to the query.
        /// </summary>
        /// <param name="comparison">Comparison operator.</param>
        /// <param name="val">Value to use.</param>
        /// <returns>This instance.</returns>
        protected new IQuery<T> BuildCondition(string comparison, object val)
        {
            var valToUse = ConvertVal(val);
            _lastCondition.Comparison = comparison;
            _lastCondition.Value = valToUse;
            return this;
        }


        /// <summary>
        /// Adds an order-by to the query.
        /// </summary>
        /// <param name="field">Name of field.</param>
        /// <param name="order">Type of ordering.</param>
        /// <returns>This instance.</returns>
        protected IQuery<T> OrderByInternal(string field, OrderByType order)
        {
            if (_data.LastOrderBy != null)
            {
                _data.Orderings.Add(_data.LastOrderBy);
            }
            _data.LastOrderBy = new OrderByClause() { Field = field, Ordering = order };
            return this;
        }
        #endregion
    }
    

    /// <summary>
    /// Base class for Query.
    /// </summary>
    public class Query : IQuery
    {
        /// <summary>
        /// Query information.
        /// </summary>
        protected QueryData _data = new QueryData();


        /// <summary>
        /// Instance of query builder.
        /// </summary>
        protected IQueryBuilder _builder;


        /// <summary>
        /// Last condition used when creating the query.
        /// </summary>
        protected Condition _lastCondition;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IQuery<object> New()
        {
            return Query<object>.New();
        }


        /// <summary>
        /// The data for the criteria, including select field, conditions, orderby etc.
        /// </summary>
        public QueryData Data
        {
            get { return _data; }
            set { _data = value; }
        }


        /// <summary>
        /// The data for the criteria, including select field, conditions, orderby etc.
        /// </summary>
        public IQueryBuilder Builder
        {
            get { return _builder; }
            set { _builder = value; }
        }


        /// <summary>
        /// Adds a new AND condition to the query.
        /// </summary>
        /// <param name="field">Conditional field.</param>
        /// <param name="comparison">Comparison expression.</param>
        /// <param name="val">Value to check against.</param>
        public void AddCondition(string field, ExpressionType comparison, object val)
        {
            StartNewCondition(ConditionType.And, field);
            string sign = RepositoryExpressionTypeHelper.GetText(comparison);
            BuildCondition(sign, val);
        }


        /// <summary>
        /// Completes this condition construction.
        /// </summary>
        public void Complete()
        {
            if (_lastCondition != null)
            {
                _data.Conditions.Add(_lastCondition);
                _lastCondition = null;
            }
            if (_data.LastOrderBy != null)
            {
                _data.Orderings.Add(_data.LastOrderBy);
                _data.LastOrderBy = null;
            }
        }


        /// <summary>
        /// Builds a comparison condition. 
        /// </summary>
        /// <typeparam name="TParam">Type of parameter.</typeparam>
        /// <param name="comparison">Comparison operator.</param>
        /// <param name="vals">Values to compare against.</param>
        /// <returns>This instance.</returns>
        protected virtual IQuery BuildCondition<TParam>(string comparison, params object[] vals)
        {
            var buffer = new StringBuilder();
            // String
            if (typeof(TParam) == typeof(string))
            {
                buffer.Append("'" + Encode((string)vals[0]) + "'");
                for (int ndx = 1; ndx < vals.Length; ndx++)
                    buffer.Append(", '" + Encode((string)vals[ndx]) + "'");
            }
            // DateTime
            else if (typeof(TParam) == typeof(DateTime))
            {
                buffer.Append("'" + Encode(((DateTime)vals[0]).ToShortDateString()) + "'");
                for (int ndx = 1; ndx < vals.Length; ndx++)
                    buffer.Append(", '" + Encode(((DateTime)vals[ndx]).ToShortDateString()) + "'");
            }
            // int, long, float, double.
            else if (TypeHelper.IsNumeric(typeof(TParam)))
            {
                buffer.Append(TypeHelper.Join(vals));
            }

            _lastCondition.Comparison = comparison;
            _lastCondition.Value = "( " + buffer.ToString() + " )";
            return this;
        }


        /// <summary>
        /// Builds a comparison condition using a string.
        /// </summary>
        /// <param name="comparison">Comparison operator.</param>
        /// <param name="val">String value to compare against.</param>
        /// <returns>This instance.</returns>
        protected virtual IQuery BuildCondition(string comparison, object val)
        {
            var valToUse = ConvertVal(val);
            _lastCondition.Comparison = comparison;
            _lastCondition.Value = valToUse;
            return this;
        }


        /// <summary>
        /// Converts an object to an equivalent string value.
        /// </summary>
        /// <param name="val">Object to convert to string.</param>
        /// <returns>String equivalent of object.</returns>
        protected string ConvertVal(object val)
        {
            string valToUse = "";

            // String
            if (val == null || (val.GetType() == typeof(string) && string.IsNullOrEmpty((string)val)))
                return "''";

            if (val.GetType() == typeof(string))
                valToUse = "'" + Encode((string)val) + "'";

            // Bool
            else if (val.GetType() == typeof(bool))
            {
                bool bval = (bool)val;
                valToUse = bval ? "1" : "0";
            }

            // DateTime
            else if (val.GetType() == typeof(DateTime))
            {
                DateTime date = (DateTime)val;
                valToUse = "'" + date.ToShortDateString() + "'";
            }

            // Int / Long / float / double
            else if (TypeHelper.IsNumeric(val))
                valToUse = val.ToString();

            return valToUse;
        }


        /// <summary>
        /// Starts a new query condition after
        /// saving any previous one.
        /// </summary>
        /// <param name="condition">Condition to start.</param>
        /// <param name="fieldName">Name of field to operate on.</param>
        /// <returns>This instance.</returns>
        protected IQuery StartNewCondition(ConditionType condition, string fieldName)
        {
            if (_lastCondition != null)
            {
                _data.Conditions.Add(_lastCondition);
            }
            _lastCondition = new Condition(condition);
            _lastCondition.Field = fieldName;
            return this;
        }


        /// <summary>
        /// Encode the text for single quotes.
        /// </summary>
        /// <param name="text">Text to encode.</param>
        /// <returns>Encoded text.</returns>
        public static string Encode(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return text.Replace("'", "''");
        }
    }
}
