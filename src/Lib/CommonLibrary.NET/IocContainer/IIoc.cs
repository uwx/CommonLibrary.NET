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
    /// Service locator interface used for getting any service instance.
    /// </summary>
    public interface IIoc
    {
        /// <summary>
        /// Get a named service  associated with the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectName"></param>
        /// <returns></returns>
        T GetObject<T>(string objectName);


        /// <summary>
        /// Get object using just the type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetObject<T>();


        /// <summary>
        /// Determine if the container contains the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool Contains<T>();


        /// <summary>
        /// Add a named service.
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="obj"></param>
        void AddObject(string objectName, object obj);
    }
}
