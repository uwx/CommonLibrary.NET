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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;



namespace ComLib
{
    /// <summary>
    /// Interface for action context.
    /// </summary>
    /// <remarks>This interface is NOT generic in order to
    /// use reflection for the EntityManager to be able to easily 
    /// create an instance where it does NOT know the type of entity.</remarks>
    public interface IActionContext
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        int Id { get; set; }


        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <remarks>This is NOT genericly type. See class remarks.</remarks>
        /// <value>The item.</value>
        object Item { get; set; }


        /// <summary>
        /// List of models to perform action on.
        /// </summary>
        object Items { get; set; }


        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        IValidationResults Errors { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [combine message errors].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [combine message errors]; otherwise, <c>false</c>.
        /// </value>
        bool CombineMessageErrors { get; set; }


        /// <summary>
        /// The name of the user used for authentication.
        /// </summary>
        string UserName { get; set; }


        /// <summary>
        /// Additional arguments to supply to context.
        /// </summary>
        IDictionary Args { get; }
    }



    /// <summary>
    /// The action context to pass to ModelService to perform any action on the model.
    /// This is used to for encapsulation to avoid changing the method signature
    /// of a ModelService if additional arguments need to be passed.
    /// </summary>
    public class ActionContext : IActionContext
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }


        /// <summary>
        /// The item.
        /// </summary>
        /// <value>The item.</value>
        protected object _item;


        /// <summary>
        /// Get/set the item.
        /// </summary>
        public object Item
        {
            get { return _item; }
            set { _item = value; }
        }


        /// <summary>
        /// The items.
        /// </summary>
        protected object _items;


        /// <summary>
        /// Get/set the items.
        /// </summary>
        public object Items
        {
            get { return _items; }
            set { _items = value; }
        }


        /// <summary>
        /// Dictionary with arguments.
        /// </summary>
        protected IDictionary _args = new Hashtable();


        /// <summary>
        /// Get/set the arguments.
        /// </summary>
        public IDictionary Args
        {
            get { return _args; }
            set { _args = value; }
        }


        /// <summary>
        /// The name of user used for authentication purposes.
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public IValidationResults Errors { get; set; }


        /// <summary>
        /// Combines the messages and errors into a single line.
        /// </summary>
        protected bool _combineMessagesErrors = false;


        /// <summary>
        /// Get/set whether to combine message errors.
        /// </summary>
        public bool CombineMessageErrors
        {
            get { return _combineMessagesErrors; }
            set { _combineMessagesErrors = value; }
        }
        #endregion


        #region Constructors
        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        public ActionContext()
        {
            Errors = new ValidationResults();
        }


        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        /// <param name="item">Item to use.</param>
        public ActionContext(object item)
        {
            Item = item;
            Errors = new ValidationResults();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContext"/> class.
        /// </summary>
        /// <param name="entity">Item to use.</param>
        /// <param name="combineErrors">if set to <c>true</c> [combine errors].</param>
        public ActionContext(object entity, bool combineErrors)
        {
            Item = entity;
            CombineMessageErrors = combineErrors;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContext"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="combineErrors">if set to <c>true</c> [combine errors].</param>
        public ActionContext(bool combineErrors, int id)
        {
            Id = id;
            CombineMessageErrors = combineErrors;
        }


        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        /// <param name="errors">Error collection</param>
        public ActionContext(IValidationResults errors)
        {
            Errors = errors == null ? new ValidationResults() : errors;
        }


        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        /// <param name="item">Item to use.</param>
        /// <param name="errors">Error collection</param>
        public ActionContext(object item, IValidationResults errors)
            : this(errors)
        {
            Item = item;
            Errors = errors == null ? new ValidationResults() : errors;
        }


        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        /// <param name="errors">Error collection</param>
        /// <param name="id">Item id.</param>
        public ActionContext(IValidationResults errors, int id)
            : this(errors)
        {
            Id = id;
        }
        #endregion
    }
}
