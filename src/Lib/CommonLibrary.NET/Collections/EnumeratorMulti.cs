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
    /// Extension to enumerator with extended methods to indicate if last or first item
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumeratorMulti<T> : IEnumerator<T>
    {
        private IEnumerator<T> _currentEnumerator;
        private IList<IEnumerator<T>> _allEnumerators;
        private int _currentEnumeratorIndex;


        /// <summary>
        /// Initialize the list.
        /// </summary>
        /// <param name="allEnumerators"></param>
        public EnumeratorMulti(IList<IEnumerator<T>> allEnumerators)
        {
            _allEnumerators = allEnumerators;
            _currentEnumeratorIndex = -1;
        }


        #region IEnumerator<T> Members
        /// <summary>
        /// Get the current item.
        /// </summary>
        public T Current
        {
            get 
            { 
                if( IsWithinBounds() )
                    return _allEnumerators[_currentEnumeratorIndex].Current;

                return default(T);
            }
        }

        #endregion


        #region IDisposable Members
        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {   
            // Nothing to dispose.
        }

        #endregion


        #region IEnumerator Members
        /// <summary>
        /// Get the current item in list.
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get 
            {
                if (IsWithinBounds())
                    return _allEnumerators[_currentEnumeratorIndex].Current;
                return null;
            }
        }


        /// <summary>
        /// Move to the next item.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {            
            // Check for case where moving to first one.
            if (_currentEnumeratorIndex == -1)
                MoveToNextEnumerator();

            // Can move on the current enumerator?
            if (_currentEnumerator.MoveNext())
                return true;

            // Last enumerator - can not move anymore.
            if (_currentEnumeratorIndex == _allEnumerators.Count - 1)
                return false;

            // Ok. move to the next enumerator.
            MoveToNextEnumerator();

            // If past
            // Now move next on the next enumerator.
            return _currentEnumerator.MoveNext();
        }


        /// <summary>
        /// Reset the iterator to first item enumerator.
        /// </summary>
        public void Reset()
        {
            _currentEnumeratorIndex = -1;
            _currentEnumerator = null;
        }
        #endregion


        private void MoveToNextEnumerator()
        {
            _currentEnumeratorIndex++;
            _currentEnumerator = _allEnumerators[_currentEnumeratorIndex];
        }


        /// <summary>
        /// Check if current index referencing the enumerator in the list 
        /// is within bounds.
        /// </summary>
        /// <returns></returns>
        private bool IsWithinBounds()
        {
            return _currentEnumeratorIndex > -1 && _currentEnumeratorIndex < _allEnumerators.Count;
        }
    }
}
