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
    /// Interface for query.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuery<T> : IQuery
    {
        /// <summary>
        /// Adds a condition to the query.
        /// </summary>
        /// <param name="exp">Expression to extract field name.</param>
        /// <param name="comparison">Comparison expression.</param>
        /// <param name="val">Value to compare against.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> AddCondition(Expression<Func<T, object>> exp, ExpressionType comparison, object val);


        /// <summary>
        /// Adds columns to a select statement.
        /// </summary>
        /// <param name="colExpressions">Expressions to retrieve column names.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Select(params Expression<Func<T, object>>[] colExpressions);


        /// <summary>
        /// Adds a WHERE clause to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Where(Expression<Func<T, object>> exp);


        /// <summary>
        /// Adds an AND condition to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> And(Expression<Func<T, object>> exp);


        /// <summary>
        /// Adds an OR condition to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Or(Expression<Func<T, object>> exp);


        /// <summary>
        /// Adds an Order-By clause to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> OrderBy(Expression<Func<T, object>> exp);


        /// <summary>
        /// Adds a descending order clause to the query.
        /// </summary>
        /// <param name="exp">Expression to retrieve property name.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> OrderByDescending(Expression<Func<T, object>> exp);


        /// <summary>
        /// Adds columns to a select statement.
        /// </summary>
        /// <param name="cols">Array with columns to add.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Select(params string[] cols);


        /// <summary>
        /// Adds a column to a select statement.
        /// </summary>
        /// <param name="field">Name of field to add.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Select(string field);


        /// <summary>
        /// Adds an AS clause to the query.
        /// </summary>
        /// <param name="alias">Alias to use.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> As(string alias);


        /// <summary>
        /// Designates the table to operate on.
        /// </summary>
        /// <param name="tableName">Name of table.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> From(string tableName);


        /// <summary>
        /// Adds a WHERE condition to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Where(string field);


        /// <summary>
        /// Adds an AND operation to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> And(string field);


        /// <summary>
        /// Adds an OR operation to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Or(string field);


        /// <summary>
        /// Adds an IN condition to the query.
        /// </summary>
        /// <typeparam name="TParam">Type of parameter.</typeparam>
        /// <param name="vals">Array with values to check for.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> In<TParam>(params object[] vals);


        /// <summary>
        /// Adds a NOT-IN condition to the query.
        /// </summary>
        /// <typeparam name="TParam">Type of parameter.</typeparam>
        /// <param name="vals">Array with values to check for.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> NotIn<TParam>(params object[] vals);


        /// <summary>
        /// Adds a null comparison condition to the query.
        /// </summary>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Null();


        /// <summary>
        /// Adds a not-null comparison condition to the query.
        /// </summary>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> NotNull();


        /// <summary>
        /// Adds a LIKE condition to the query.
        /// </summary>
        /// <param name="val">Value to check for.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Like(string val);


        /// <summary>
        /// Adds a LIKE condition to the query.
        /// </summary>
        /// <param name="val">Value to check for.</param>
        /// <param name="addWildcardPrefix">True to add a wildcard prefix.</param>
        /// <param name="addWildcardSuffix">True to add a wildcard suffix.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Like(string val, bool addWildcardPrefix, bool addWildcardSuffix);


        /// <summary>
        /// Adds an equality condition to the query.
        /// </summary>
        /// <param name="val">Value to check for.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Is(object val);


        /// <summary>
        /// Adds an inequality condition to the query.
        /// </summary>
        /// <param name="val">Value to check for.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Not(object val);


        /// <summary>
        /// Adds a less-than comparison to the query.
        /// </summary>
        /// <param name="val">Value to check against.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> LessThan(object val);


        /// <summary>
        /// Adds a less-or-equal-than comparison to the query.
        /// </summary>
        /// <param name="val">Value to check against.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> LessEqual(object val);


        /// <summary>
        /// Adds a greater-than comparison to the query.
        /// </summary>
        /// <param name="val">Value to check against.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> MoreThan(object val);


        /// <summary>
        /// Adds a greater-or-equal comparison to the query.
        /// </summary>
        /// <param name="val">Value to check against.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> MoreEqual(object val);


        /// <summary>
        /// Adds an ascending order-by clause to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>this instance.</returns>
        IQuery<T> OrderBy(string field);


        /// <summary>
        /// Adds a descending order-by clause to the query.
        /// </summary>
        /// <param name="field">Name of field to use.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> OrderByDescending(string field);


        /// <summary>
        /// Limits the query result to a number of records.
        /// </summary>
        /// <param name="maxRecords">Number of records to limit.</param>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> Limit(int maxRecords);


        /// <summary>
        /// Completes the query.
        /// </summary>
        /// <returns>Instance of implementation class used.</returns>
        IQuery<T> End();
    }


    /// <summary>
    /// Interface to be implemented by query providers.
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Get/set the query information.
        /// </summary>
        QueryData Data { get; set; }


        /// <summary>
        /// Get/set the query builder.
        /// </summary>
        IQueryBuilder Builder { get; set; }


        /// <summary>
        /// Adds a new AND condition to the query.
        /// </summary>
        /// <param name="field">Conditional field.</param>
        /// <param name="comparison">Comparison expression.</param>
        /// <param name="val">Value to check against.</param>
        void AddCondition(string field, ExpressionType comparison, object val);


        /// <summary>
        /// Completes this condition construction.
        /// </summary>
        void Complete();
    }


    /// <summary>
    /// Interface to be implemented by query builder implementors.
    /// </summary>
    public interface IQueryBuilder
    {
        /// <summary>
        /// The criteria API.
        /// </summary>
        IQuery Criteria { get; set; }


        /// <summary>
        /// Builds the full markup for the criteria provided.
        /// </summary>
        /// <returns>String with created query.</returns>
        string Build();


        /// <summary>
        /// Builds the full markup fro the criteria provided using the from source supplied.
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns>String with created query.</returns>
        string Build(string from);


        /// <summary>
        /// Builds only the "select fields" markup.
        /// </summary>
        /// <returns>String with created query.</returns>
        string BuildSelect();


        /// <summary>
        /// Builds only the select fields marketup with conditional flag indicating whether or not
        /// to include the markup's select statement.
        /// </summary>
        /// <param name="includeSelect">if set to <c>true</c> [include select statement].</param>
        /// <returns>String with created query.</returns>
        string BuildSelect(bool includeSelect);


        /// <summary>
        /// Builds the limit.
        /// </summary>
        /// <returns>String with created query.</returns>
        string BuildLimit();


        /// <summary>
        /// Builds the conditions.
        /// </summary>
        /// <returns>String with created query.</returns>
        string BuildConditions();


        /// <summary>
        /// Builds the conditions.
        /// </summary>
        /// <param name="includeWhere">if set to <c>true</c> [include where].</param>
        /// <returns>String with created conditions.</returns>
        string BuildConditions(bool includeWhere);


        /// <summary>
        /// Builds the conditions.
        /// </summary>
        /// <param name="includeWhere">True to include WHERE clause.</param>
        /// <param name="includeOrderBy">True to include ORDER-BY clause.</param>
        /// <returns>String with created query.</returns>
        string BuildConditionsAndOrdering(bool includeWhere, bool includeOrderBy);


        /// <summary>
        /// Builds the order by.
        /// </summary>
        /// <returns>String with created order-by.</returns>
        string BuildOrderBy();


        /// <summary>
        /// Builds the order by.
        /// </summary>
        /// <param name="includeOrderBy">if set to <c>true</c> [include order by].</param>
        /// <returns>String with created order-by.</returns>
        string BuildOrderBy(bool includeOrderBy);
    }
}