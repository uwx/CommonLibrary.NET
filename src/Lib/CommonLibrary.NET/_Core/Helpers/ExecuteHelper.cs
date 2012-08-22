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
using System.Runtime.InteropServices;
using System.Reflection;


namespace ComLib
{
    /// <summary>
    /// Wrapper class to simplify lines of code around Try/Catch blocks with various customized behaviour.
    /// </summary>
    public class Try
    {
        private static LamdaLogger _logger = new LamdaLogger();


        /// <summary>
        /// Initialize logging lamda.
        /// </summary>
        public static LamdaLogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }


        /// <summary>
        /// Calls the action and logs any exception that occurrs
        /// </summary>
        /// <param name="action">Action to execute inside a try/catch.</param>
        public static void CatchLog(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex, null);
            }
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="errorMessage">Error message to log.</param>
        /// <param name="action">The function to call.</param>
        public static void CatchLog(string errorMessage, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _logger.Error(errorMessage, ex, null);
            }
        }


        /// <summary>
        /// Calls the action and logs any exception that occurrs and rethrows the exception.
        /// <param name="action">Action to execute in a try/catch.</param>
        /// </summary>
        public static void CatchLogRethrow(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex, null);
                throw ex;
            }
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="errorMessage">Error message to log.</param>
        /// <param name="action">The function to call.</param>
        /// <param name="logger">The logger to use</param>
        public static void CatchLog(string errorMessage, Action action, Action<object, Exception, object[]> logger)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger(errorMessage, ex, null);
            }
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="action">The function to call.</param>
        /// <param name="exceptionHandler">The action to use for handling the exception</param>
        public static void Catch(Action action, Action<Exception> exceptionHandler = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                    exceptionHandler(ex);
            }
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="errorMessage">Error message to log.</param>
        /// <param name="action">The function to call.</param>
        /// <param name="exceptionHandler">The action to use for handling the exception</param>
        /// <param name="finallyHandler">The action to use in the finally block</param>
        public static void CatchHandle(string errorMessage, Action action, Action<Exception> exceptionHandler, Action finallyHandler)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (exceptionHandler != null)
                    exceptionHandler(ex);
            }
            finally
            {
                if (finallyHandler != null)
                {
                    finallyHandler();
                }
            }
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="errorMessage">Error message to log.</param>
        /// <param name="action">Action to execute.</param>
        public static T CatchLogGet<T>(string errorMessage, Func<T> action)
        {
            return CatchLogGet<T>(errorMessage, false, action, null);
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <typeparam name="T">Type of return item.</typeparam>
        /// <param name="errorMessage">Error message to log.</param>
        /// <param name="rethrow">Whether or not to rethrow</param>
        /// <param name="action">The function to call.</param>
        /// <param name="logger">The logger to use</param>
        /// <returns>Returned result.</returns>
        public static T CatchLogGet<T>(string errorMessage, bool rethrow, Func<T> action, Action<object, Exception, object[]> logger)
        {
            T result = default(T);
            try
            {
                result = action();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger(errorMessage, ex, null);
                else if (_logger != null)
                    _logger.Error(errorMessage, ex, null);

                if (rethrow) throw ex;
            }
            return result;
        }


        /// <summary>
        /// Get a boolmessage item after running the action.
        /// </summary>
        /// <param name="errorMessage">Error message to log.</param>
        /// <param name="action">The function to call.</param>
        /// <returns>Returned result.</returns>
        public static BoolMessageItem<T> CatchLogGetBoolResult<T>(string errorMessage, Func<T> action)
        {
            return CatchLogGetBoolResult<T>(errorMessage, action, null);
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <typeparam name="T">Type of return item.</typeparam>
        /// <param name="errorMessage">Error message to log.</param>
        /// <param name="action">The function to call.</param>
        /// <param name="logger">Logger object.</param>
        /// <returns>Return item.</returns>
        public static BoolMessageItem<T> CatchLogGetBoolResult<T>(string errorMessage, Func<T> action, Action<object, Exception, object[]> logger)
        {
            T result = default(T);
            bool success = false;
            string message = string.Empty;
            try
            {
                result = action();
                success = true;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger(errorMessage, ex, null);
                else if (_logger != null)
                    _logger.Error(errorMessage, ex, null);

                message = ex.Message;
            }
            return new BoolMessageItem<T>(result, success, message);
        }


        /// <summary>
        /// Executes an action inside a try catch and logs any exceptions.
        /// </summary>
        /// <param name="errorMessage">Error message to log.</param>
        /// <param name="action">The function to call.</param>
        /// <param name="logger">Logger object.</param>
        /// <returns>Return item.</returns>
        public static BoolMessageItem TryCatchLogGetBoolMessageItem(string errorMessage, Func<BoolMessageItem> action, Action<object, Exception, object[]> logger)
        {
            BoolMessageItem result = BoolMessageItem.False;
            string fullMessage = string.Empty;
            try
            {
                result = action();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger(errorMessage, ex, null);
                else if (_logger != null)
                    _logger.Error(errorMessage, ex, null);

                fullMessage = errorMessage + Environment.NewLine
                        + ex.Message + Environment.NewLine
                        + ex.Source + Environment.NewLine
                        + ex.StackTrace;
                result = new BoolMessageItem(null, false, fullMessage);
            }
            return result;
        }


        /// <summary>
        /// Handle the highest level application exception.
        /// </summary>
        /// <param name="ex">Exception to handle.</param>
        public static void HandleException(Exception ex)
        {
            string message = string.Format("{0}, {1} \r\n {2}", ex.Message, ex.Source, ex.StackTrace);
            try
            {
                if (_logger != null)
                    _logger.Error(message, ex, null);
            }
            catch { }
        }


        /// <summary>
        /// Runs the action, logs an error and returns boolMessageEx that wraps up all there values.
        /// Bool flag for successful call without excpetion, errorMessage if there was an exception, 
        /// and the exception itself if it occurred.
        /// </summary>
        /// <param name="message">Success message</param>
        /// <param name="errorMessage">Failure message</param>
        /// <param name="action">Action to run</param>
        /// <returns>Error message.</returns>
        public static BoolMessageEx CatchLogGetBoolResultEx(string message, string errorMessage, Action action)
        {
            string finalMessage = message;
            bool success = false;
            Exception ex = null;
            try
            {
                action();
                success = true;
            }
            catch (Exception exception)
            {
                success = false;
                finalMessage = errorMessage;
                ex = exception;
                _logger.Error(ex, null, null);
            }
            return new BoolMessageEx(success, ex, finalMessage);
        }


        /// <summary>
        /// Runs the action, logs an error and returns boolMessageEx that wraps up all there values.
        /// Bool flag for successful call without excpetion, errorMessage if there was an exception, 
        /// and the exception itself if it occurred.
        /// </summary>
        /// <param name="message">Success message</param>
        /// <param name="errorMessage">Failure message</param>
        /// <param name="action">Action to run</param>
        /// <returns>Error message.</returns>
        public static BoolMessageEx CatchLogReturnBoolResultEx(string message, string errorMessage, Func<BoolMessageEx> action)
        {
            string finalMessage = message;
            bool success = false;
            BoolMessageEx result = null;
            Exception ex = null;
            try
            {
                result = action();
                ex = result.Ex;                
                success = result.Success;
                finalMessage = result.Message;
            }
            catch (Exception exception)
            {
                success = false;
                ex = exception;
                finalMessage = errorMessage;
                _logger.Error(exception, null, null);
            }
            return new BoolMessageEx(success, ex, finalMessage);
        }


        /// <summary>
        /// Runs the action, logs an error and returns boolMessageEx that wraps up all there values.
        /// Bool flag for successful call without excpetion, errorMessage if there was an exception, 
        /// and the exception itself if it occurred.
        /// </summary>
        /// <param name="message">Success message</param>
        /// <param name="errorMessage">Failure message</param>
        /// <param name="action">Action to run</param>
        /// <returns>Error message.</returns>
        public static BoolMessageEx GetBoolResultEx(string message, string errorMessage, Func<BoolMessageEx> action)
        {
            BoolMessageEx result = action();
            string finalmessage = message;
            if (!result.Success)
                finalmessage = string.IsNullOrEmpty(errorMessage) ? result.Message : errorMessage;

            return new BoolMessageEx(result.Success, result.Ex, finalmessage);
        }
    }
}
