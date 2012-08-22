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


namespace ComLib
{
    /// <summary>
    /// Combines a boolean succes/fail flag with a error/status message.
    /// </summary>
    public class BoolMessage
    {
        /// <summary>
        /// True message.
        /// </summary>
        public static readonly BoolMessage True = new BoolMessage(true, string.Empty);


        /// <summary>
        /// False message.
        /// </summary>
        public static readonly BoolMessage False = new BoolMessage(false, string.Empty);
        

        /// <summary>
        /// Success / failure ?
        /// </summary>
        public readonly bool Success;

        /// <summary>
        /// Error message for failure, status message for success.
        /// </summary>
        public readonly string Message;


        /// <summary>
        /// Set the readonly fields.
        /// </summary>
        /// <param name="success">Flag for message to set.</param>
        /// <param name="message">Message to set for flag.</param>
        public BoolMessage(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }



    /// <summary>
    /// Combines a boolean succes/fail flag with a error/status message and an object.
    /// </summary>
    public class BoolMessageItem : BoolMessage
    {
        /// <summary>
        /// Item associated with boolean message.
        /// </summary>
        private object _item;


        /// <summary>
        /// True message.
        /// </summary>
        public static new readonly BoolMessageItem True = new BoolMessageItem(null, true, string.Empty);


        /// <summary>
        /// False message.
        /// </summary>
        public static new readonly BoolMessageItem False = new BoolMessageItem(null, false, string.Empty);
        

        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        public BoolMessageItem(object item, bool success, string message)
            : base(success, message)
        {
            _item = item;
        }


        /// <summary>
        /// Return readonly item.
        /// </summary>
        public object Item
        {
            get { return _item; }
        }
    }


    /// <summary>
    /// Tuple to store boolean, string message, and Exception object.
    /// </summary>
    public class BoolMessageEx : BoolMessage
    {
        private Exception _ex;


        /// <summary>
        /// True message.
        /// </summary>
        public static new readonly BoolMessageEx True = new BoolMessageEx(true, null, string.Empty);


        /// <summary>
        /// False message.
        /// </summary>
        public static new readonly BoolMessageEx False = new BoolMessageEx(false, null, string.Empty);


        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="ex">The exception.</param>
        /// <param name="message">The message.</param>
        public BoolMessageEx(bool success, Exception ex, string message)
            : base(success, message)
        {
            _ex = ex;
        }


        /// <summary>
        /// Return readonly item.
        /// </summary>
        public Exception Ex
        {
            get { return _ex; }
        }
    } 



    /// <summary>
    /// Combines a boolean succes/fail flag with a error/status message and an object.
    /// </summary>
    /// <typeparam name="T">Type of object combined with a boolean flag.</typeparam>
	public class BoolMessageItem<T> : BoolMessageItem
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
		public BoolMessageItem(T item, bool success, string message) : base(item, success, message )
		{
		}


        /// <summary>
        /// Return item as correct type.
        /// </summary>
        public new T Item
        {
            get { return (T)base.Item; }
        }
	}

    
    /// <summary>
    /// Combines a boolean succes/fail flag with a error/status message, 
    /// an object and validation results.
    /// </summary>
    /// <typeparam name="T">Type of object combined with a boolean flag.</typeparam>
    public class BoolResult<T> : BoolMessageItem<T>
    {
        private IValidationResults _errors;


        /// <summary>
        /// Empty false result.
        /// </summary>
        public static new readonly BoolResult<T> False = new BoolResult<T>(default(T), false, string.Empty, ValidationResults.Empty);
        
        
        /// <summary>
        /// Empty True result.
        /// </summary>
        public static new readonly BoolResult<T> True = new BoolResult<T>(default(T), true, string.Empty, ValidationResults.Empty);


        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        /// <param name="errors">List of validation errors messages.</param>
        public BoolResult(T item, bool success, string message, IValidationResults errors)
            : base(item, success, message)
		{
            _errors = errors;
		}


        /// <summary>
        /// List of errors from performing some action.
        /// </summary>
        public IValidationResults Errors
        {
            get { return _errors; }
        }
    }


    /// <summary>
    /// Class to store success/fail message, error collection and some result object(T).
    /// </summary>
    public class BoolErrorsItem : BoolMessageItem
    {
        /// <summary>
        /// List of errors from performing some action.
        /// </summary>
        protected IErrors _errors;


        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors</param>
        public BoolErrorsItem(object item, bool success, string message, IErrors errors)
            : base(item, success, message)
        {
            _errors = errors;
        }


        /// <summary>
        /// List of errors from performing some action.
        /// </summary>
        public IErrors Errors
        {
            get { return _errors; }
        }
    }



    /// <summary>
    /// Bool result with error collection and resulting item with typed access.
    /// </summary>
    /// <typeparam name="T">Type of item to use.</typeparam>
    public class BoolErrorsItem<T> : BoolErrorsItem
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolMessageItem&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        /// <param name="errors">List of validation errors.</param>
        public BoolErrorsItem(T item, bool success, string message, IValidationResults errors)
            : base(item, success, message, errors)
		{
            _errors = errors;
		}


        /// <summary>
        /// Return item as correct type.
        /// </summary>
        public new T Item
        {
            get { return (T)base.Item; }
        } 
    }



    /// <summary>
    /// Extensions to the boolmessage item.
    /// </summary>
    public static class BoolMessageExtensions
    {
        /// <summary>
        /// Convert the result to an exit code.
        /// </summary>
        /// <param name="result">Result to convert to an exit code.</param>
        /// <returns>0 for a successful exit, 1 otherwise.</returns>
        public static int AsExitCode(this BoolMessage result)
        {
            return result.Success ? 0 : 1;
        }
    }

}
