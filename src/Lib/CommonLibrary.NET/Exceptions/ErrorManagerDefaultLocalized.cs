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
using ComLib.Logging;
using ComLib.Locale;

namespace ComLib.Exceptions
{
    /// <summary>
    /// Localized error manager.
    /// </summary>
    public class ErrorManagerDefaultLocalized : ErrorManagerDefault, ILocalizedExceptionManager
    {
        /// <summary>
        /// Handles the error by added it it the validation errors, and logging it.
        /// </summary>
        /// <param name="errorDescriptor"></param>
        /// <param name="resources"></param>
        /// <param name="errors"></param>
        /// <param name="ex"></param>
        public void Handle(string errorDescriptor, ILocalizationResourceProvider resources, IErrors errors, Exception ex)
        {
            string error = resources.GetString(errorDescriptor);
            
            // Add to error list.
            errors.Add(error);

            // Add to log.
            if (ex == null)
                Logger.Error(error);
            else
                Logger.Error(error, ex);
        }


        /// <summary>
        /// Handle the error by formatting the error message first and then adding it
        /// to the validation errors. Then add it to the log.
        /// </summary>
        /// <param name="errorDescriptor"></param>
        /// <param name="resources"></param>
        /// <param name="errors"></param>
        /// <param name="ex"></param>
        /// <param name="args"></param>
        public void Handle(string errorDescriptor, ILocalizationResourceProvider resources, IErrors errors, Exception ex, string[] args)
        {
            string error = resources.GetString(errorDescriptor);
            string errorDetails = error;

            if (args != null && args.Length > 0)
            {
                foreach (string arg in args)
                {
                    errorDetails += arg + " ";
                }
            }

            // Add to validation results.
            errors.Add(errorDetails);

            // Add to log.
            if (ex == null)
                Logger.Error(errorDetails);
            else
                Logger.Error(errorDetails, ex);
        }


        /// <summary>
        /// Handles the error by added it it the validation errors, and logging it.
        /// </summary>
        /// <param name="errorDescriptor"></param>
        /// <param name="resources"></param>
        /// <param name="ex"></param>
        /// <param name="args"></param>
        public void Handle(string errorDescriptor, ILocalizationResourceProvider resources, Exception ex, string[] args)
        {
            string error = resources.GetString(errorDescriptor);
            string errorDetails = error;

            if (args != null && args.Length > 0)
            {
                foreach (string arg in args)
                {
                    errorDetails += arg + " ";
                }
            }

            // Add to log.
            if (ex == null)
                Logger.Error(errorDetails);
            else
                Logger.Error(errorDetails, ex);
        }
    }
}
