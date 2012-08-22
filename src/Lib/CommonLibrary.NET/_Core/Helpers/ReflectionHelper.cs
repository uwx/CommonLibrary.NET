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
using System.Reflection;


namespace ComLib
{
    /// <summary>
    /// This class provides basic reflection helper methods.
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// Set the property value using the string value.
        /// </summary>
        /// <param name="obj">Object whose property is to be set.</param>
        /// <param name="prop">Property name.</param>
        /// <param name="propVal">Value to set.</param>
        public static void SetProperty(object obj, PropertyInfo prop, string propVal)
        {
            Guard.IsNotNull(obj, "Object containing properties to set is null");
            Guard.IsNotNull(prop, "Property not supplied.");

            // Correct property with write access 
            if (prop != null && prop.CanWrite)
            {
                // Check same Type
                if (Converter.CanConvertToCorrectType(prop, propVal))
                {
                    object convertedVal = Converter.ConvertToSameType(prop, propVal);
                    prop.SetValue(obj, convertedVal, null);
                }
            }
        }


        /// <summary>
        /// Gets the property value safely, without throwing an exception.
        /// If an exception is caught, null is returned.
        /// </summary>
        /// <param name="obj">Object whose property is to be retrieved.</param>
        /// <param name="propInfo">Property name.</param>
        /// <returns></returns>
        public static object GetPropertyValueSafely(object obj, PropertyInfo propInfo)
        {
            Guard.IsNotNull(obj, "Must provide object to get it's property.");
            if (propInfo == null) return null;

            object result = null;
            try
            {
                result = propInfo.GetValue(obj, null);
            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
    }
}
