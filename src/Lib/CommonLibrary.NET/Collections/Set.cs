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
    /// Interface for a set of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISet<T> : ICollection<T>
    {
        /// <summary>
        /// Unions the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        ISet<T> Union(ISet<T> other);


        /// <summary>
        /// Returns set with elements common to both.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        ISet<T> Intersect(ISet<T> other);


        /// <summary>
        /// Either or.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        ISet<T> ExclusiveOr(ISet<T> other);


        /// <summary>
        /// Gets the items in the set not contained in the set supplied.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        ISet<T> Minus(ISet<T> other);
    }



    /// <summary>
    /// Helper class for sets.
    /// </summary>
    public class SetHelper<T>
    {
        /// <summary>
        /// Gets all the unique elements from both sets.
        /// Whats in first OR second.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static ISet<T> Union(ISet<T> first, ISet<T> second)
        {
            ISet<T> union = new DictionarySet<T>();

            // Add all of the first ones.
            IEnumerator<T> items = first.GetEnumerator();
            while (items.MoveNext())
            {
                if (!union.Contains(items.Current))
                    union.Add(items.Current);
            }

            items = second.GetEnumerator();
            while (items.MoveNext())
            {
                if (!union.Contains(items.Current))
                    union.Add(items.Current);
            }
            return union;
        }


        /// <summary>
        /// Finds the intersection of the elements in first and second.
        /// Whats in both first AND second.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static ISet<T> Intersect(ISet<T> first, ISet<T> second)
        {
            ISet<T> intersect = new DictionarySet<T>();

            // Determine which ones to check.
            ISet<T> setToIterate = first;
            ISet<T> setToCheck = second;
            if (second.Count > first.Count)
            {
                setToIterate = second;
                setToCheck = first;
            }

            // Add all of the first ones.
            IEnumerator<T> items = setToIterate.GetEnumerator();
            while (items.MoveNext())
            {
                if (setToCheck.Contains(items.Current))
                    intersect.Add(items.Current);
            }
            return intersect;
        }


        /// <summary>
        /// Exclusives the or.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static ISet<T> ExclusiveOr(ISet<T> first, ISet<T> second)
        {
            ISet<T> union = new DictionarySet<T>();

            // Add all of the first ones.
            IEnumerator<T> items = first.GetEnumerator();
            while (items.MoveNext())
            {
                if (!second.Contains(items.Current))
                    union.Add(items.Current);
            }

            items = second.GetEnumerator();
            while (items.MoveNext())
            {
                if (!first.Contains(items.Current))
                    union.Add(items.Current);
            }
            return union;
        }


        /// <summary>
        /// Minuses the specified other.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static ISet<T> Minus(ISet<T> first, ISet<T> second)
        {
            ISet<T> minus = new DictionarySet<T>();

            // Add all of the first ones.
            IEnumerator<T> items = first.GetEnumerator();
            while (items.MoveNext())
            {
                if (!second.Contains(items.Current))
                    minus.Add(items.Current);
            }
            return minus;
        }
    }
}
