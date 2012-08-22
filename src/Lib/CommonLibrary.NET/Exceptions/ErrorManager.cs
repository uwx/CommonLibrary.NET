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
using ComLib.Locale;
using ComLib.ValidationSupport;


namespace ComLib.Exceptions
{
    /// <summary>
    /// Exception manager.
    /// </summary>
    public class ErrorManager
    {
        private static IErrorManager _provider;
        private static ILocalizedExceptionManager _localizedProvider;
        private static object _syncRoot = new object();
        private static IDictionary<string, IErrorManager> _namedHandlers;



        /// <summary>
        /// Initialize the defaults.
        /// </summary>
        static ErrorManager()
        {
            _localizedProvider = new ErrorManagerDefaultLocalized();
            _provider = _localizedProvider;
            _namedHandlers = new Dictionary<string, IErrorManager>();
            _namedHandlers[string.Empty] = _provider;
        }


        /// <summary>
        /// Initialize the provider.
        /// </summary>
        /// <param name="provider"></param>
        public static void Init(IErrorManager provider)
        {
            lock (_syncRoot)
            {
                _provider = provider;
            }
        }


        /// <summary>
        /// Initialize the provider.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isDefault"></param>
        /// <param name="provider"></param>
        public static void InitLocalizedManager(string name, bool isDefault, ILocalizedExceptionManager provider)
        {
            lock (_syncRoot)
            {
                _localizedProvider = provider;
                if (isDefault)                
                    _provider = _localizedProvider;                    
                
                if(!string.IsNullOrEmpty(name))
                    _namedHandlers[name] = _provider;
            }
        }


        /// <summary>
        /// Register an named exception handler.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isDefault"></param>
        /// <param name="handler"></param>
        public static void Register(string name, bool isDefault, IErrorManager handler)
        {
            lock (_syncRoot)
            {                
                if (isDefault)
                    _provider = handler;

                if (!string.IsNullOrEmpty(name))
                    _namedHandlers[name] = handler;
            }
        }


        #region IExceptionManager Members
        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        public static void Handle(object error, Exception exception)
        {
            InternalHandle(error, exception, null, null, null);
        }


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="handler">The arguments.</param>
        public static void Handle(object error, Exception exception, string handler)
        {
            InternalHandle(error, exception, handler, null, null);
        }


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Handle(object error, Exception exception, object[] arguments)
        {
            InternalHandle(error, exception, null, null, arguments);
        }


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="handler"></param>
        /// <param name="arguments">The arguments.</param>
        public static void Handle(object error, Exception exception, string handler, object[] arguments)
        {
            InternalHandle(error, exception, handler, null, arguments);
        }


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="errors">The error results.</param>
        public static void Handle(object error, Exception exception, IErrors errors)
        {
            InternalHandle(error, exception, null, errors, null);
        }


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="handler"></param>
        /// <param name="errors">The error results.</param>
        public static void Handle(object error, Exception exception, string handler, IErrors errors)
        {
            InternalHandle(error, exception, handler, errors, null);
        }


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="errors">The error results.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Handle(object error, Exception exception, IErrors errors, object[] arguments)
        {
            InternalHandle(error, exception, null, errors, arguments);
        }


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="handler"></param>
        /// <param name="errors">The error results.</param>
        /// <param name="arguments">The arguments.</param>
        public static void Handle(object error, Exception exception, string handler, IErrors errors, object[] arguments)
        {
            InternalHandle(error, exception, handler, errors, arguments);
        }


        /// <summary>
        /// Handles the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="handler"></param>
        /// <param name="errors">The error results.</param>
        /// <param name="arguments">The arguments.</param>
        private static void InternalHandle(object error, Exception exception, string handler, IErrors errors, object[] arguments)
        {
            if (handler == null)
            {
                _provider.Handle(error, exception, errors, arguments);
                return;
            }

            if (!_namedHandlers.ContainsKey(handler))
                throw new ArgumentException("Unknown exception handler : " + handler);

            IErrorManager exceptionManager = _namedHandlers[handler];
            exceptionManager.Handle(error, exception, errors, arguments);
        }


        /// <summary>
        /// Handles the exception by getting the error description from the <paramref name="resources"/> using
        /// the key specified by <paramref name="errorDescriptorKey"/>. Adds the error to <paramref name="errors"/>.
        /// </summary>
        /// <param name="errorDescriptorKey">The name of key to use to get the localized errors from resources. </param>
        /// <param name="resources">The localized resources that contains the error string.</param>
        /// <param name="errors">The list of errors to add to the error string to.</param>
        /// <param name="ex">The exception to handle.</param>
        public static void Handle(string errorDescriptorKey, ILocalizationResourceProvider resources, IErrors errors, Exception ex)
        {
            _localizedProvider.Handle(errorDescriptorKey, resources, errors, ex);
        }


        /// <summary>
        /// Handles the exception by getting the error description from the <paramref name="resources"/> using
        /// the key specified by <paramref name="errorDescriptorKey"/>. Adds the error to <paramref name="errors"/>.
        /// </summary>
        /// <param name="errorDescriptorKey">The name of key to use to get the localized errors from resources. </param>
        /// <param name="resources">The localized resources that contains the error string.</param>
        /// <param name="errors">The list of errors to add to the error string to.</param>
        /// <param name="ex">The exception to handle.</param>
        /// <param name="args">Array of strings to report in the error.</param>
        public static void Handle(string errorDescriptorKey, ILocalizationResourceProvider resources, IErrors errors, Exception ex, string[] args)
        {
            _localizedProvider.Handle(errorDescriptorKey, resources, errors, ex, args);
        }


        /// <summary>
        /// Handles the exception by getting the error description from the <paramref name="resources"/> using
        /// the key specified by <paramref name="errorDescriptorKey"/>. Converts all the <paramref name="args"/>
        /// to a string to put into the error.
        /// </summary>
        /// <param name="errorDescriptorKey">The name of key to use to get the localized errors from resources. </param>
        /// <param name="resources">The localized resources that contains the error string.</param>
        /// <param name="ex">The exception to handle.</param>
        /// <param name="args">Array of strings to report in the error.</param>
        public static void Handle(string errorDescriptorKey, ILocalizationResourceProvider resources, Exception ex, string[] args)
        {
            _localizedProvider.Handle(errorDescriptorKey, resources, ex, args);
        }
        #endregion
    }
}
