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
using System.Collections;


namespace ComLib
{

    /// <summary>
    /// Helper class to get a service object out of the 
    /// service locator.
    /// </summary>
    public class Ioc
    {
        private static IIoc _container;
        private static object _syncRoot = new object();


        /// <summary>
        /// Sets the object container.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void Init(IIoc container)
        {
            lock (_syncRoot)
            {
                _container = container;
            }
        }


        /// <summary>
        /// Adds the object to the container.
        /// </summary>
        /// <param name="objName">Name of the obj.</param>
        /// <param name="obj">The obj.</param>
        public static void AddObject(string objName, object obj)
        {
            _container.AddObject(objName, obj);
        }


        /// <summary>
        /// Get the service and automatically converts to the appropriate type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObject<T>(string objName) 
        {
            return _container.GetObject<T>(objName);
        }


        /// <summary>
        /// Get the object using just the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObject<T>()
        {
            return _container.GetObject<T>();
        }


        /// <summary>
        /// Determine if the container contains the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Contains<T>()
        {
            return _container.Contains<T>();
        }
    }
}
