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
using System.Text;

using ComLib;


namespace ComLib.Entities
{
    /// <summary>
    /// The action context to pass to ModelService to perform any action on the model.
    /// This is used to for encapsulation to avoid changing the method signature
    /// of a ModelService if additional arguments need to be passed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionContext<T> : ActionContext
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public new T Item 
        {
            get { return (T)_item; }
            set { _item = value; }
        }


        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>The item.</value>
        public new List<T> Items
        {
            get { return _items as List<T>; }
            set { _items = value; }
        }


        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        public ActionContext() : base()
        {
        }


        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        /// <param name="item"></param>
        public ActionContext(T item) : base(item)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContext&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="combineErrors">if set to <c>true</c> [combine errors].</param>
        public ActionContext(T entity, bool combineErrors) : base(entity, combineErrors)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ActionContext&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="combineErrors">if set to <c>true</c> [combine errors].</param>
        public ActionContext(bool combineErrors, int id) : base(combineErrors, id)
        {
        }


        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        /// <param name="errors">Error collection</param>
        public ActionContext(IValidationResults errors) : base(errors)
        {
        }


        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="errors">Error collection</param>
        public ActionContext(T item, IValidationResults errors) : base(item, errors)
        {
        }


        /// <summary>
        /// Create the model action context using existing errors or message collection.
        /// If empty, a default instance will be created.
        /// </summary>
        /// <param name="errors">Error collection</param>
        /// <param name="id"></param>
        public ActionContext(IValidationResults errors, int id) : base(errors, id)
        {
        }
    }
}
