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

namespace ComLib.Collections
{
    /// <summary>
    /// Simple class to store and parse the propertyKeys.
    /// propertyKey = configObjectInstanceName.Key.
    /// e.g.
    /// 1. "profileOptions.IsEnabled"
    /// 2. "profileOptions.user1.PageSize"
    /// 
    /// </summary>
    public class PropertyKey
    {
        private static readonly PropertyKey _empty;


        /// <summary>
        /// Static constructor to create the null object.
        /// </summary>
        static PropertyKey()
        {
            _empty = new PropertyKey(string.Empty, string.Empty, string.Empty);
        }


        /// <summary>
        /// Gets the null object.
        /// </summary>
        public static PropertyKey Empty
        {
            get { return _empty; }
        }


        /// <summary>
        /// The first property e.g. A as in "A.B.C"
        /// </summary>
        public readonly string Group;


        /// <summary>
        /// The second property e.g. B as in "A.B.C"
        /// </summary>
        public readonly string SubGroup;


        /// <summary>
        /// The last property. e.g Either B if 2 properties as in "A.B" or C if 3 properties as in "A.B.C"
        /// </summary>
        public readonly string Key;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="subGroup"></param>
        /// <param name="key"></param>
        public PropertyKey(string group, string subGroup, string key)
        {
            Group = group;
            SubGroup = subGroup;
            Key = key;
        }


        /// <summary>
        /// Whether or not this has a subgroup.
        /// </summary>
        public bool HasSubGroup
        {
            get { return !string.IsNullOrEmpty(this.SubGroup); }
        }


        /// <summary>
        /// Builds the path by only including the Group and SubGroup if applicable,
        /// without using the Key.
        /// </summary>
        /// <returns></returns>
        public string BuildWithoutKey()
        {
            string fullkey = Group;
            if (!string.IsNullOrEmpty(SubGroup))
                fullkey += "." + SubGroup;
            return fullkey;
        }


        /// <summary>
        /// Return the key in "Group.SubGroup.Key".
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return PropertyKey.BuildKey(Group, SubGroup, Key);
        }


        #region Static Methods
        /// <summary>
        /// Builds the property key which is the combination of the group and the key.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="subGroup"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BuildKey(string group, string subGroup, string key)
        {
            string fullkey = group;
            if (!string.IsNullOrEmpty(subGroup))
                fullkey += "." + subGroup;

            if (!string.IsNullOrEmpty(key))
                fullkey += "." + key;

            return fullkey;
        }


        /// <summary>
        /// Builds the object key which is the combination of the group and the key.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BuildKey(string group, string key)
        {
            if (!string.IsNullOrEmpty(group))
                group = group.Trim();

            key = key.Trim();

            return group + "." + key;
        }


        /// <summary>
        /// Parses the propertyKey string "name.Property" and returns a
        /// PropertyKey object with the name and property separate.
        /// </summary>
        /// <param name="propertyKey"></param>
        /// <returns></returns>
        public static PropertyKey Parse(string propertyKey)
        {
            if (string.IsNullOrEmpty(propertyKey))
                return PropertyKey.Empty;

            string[] tokens = propertyKey.Split('.');

            // If only 2 tokens. Convert to "group.key"
            if (tokens.Length == 2)
                return new PropertyKey(tokens[0], string.Empty, tokens[1]);

            if (tokens.Length == 1)
                return new PropertyKey(tokens[0], string.Empty, string.Empty);

            // Convert to "group.subGroup.key"
            return new PropertyKey(tokens[0], tokens[1], tokens[2]);
        }
        #endregion
    }
}
