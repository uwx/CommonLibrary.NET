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
    public struct Tuple2<T1, T2>
    {
        private readonly T1 _first;
        /// <summary>
        /// The first item of the tuple
        /// </summary>
        public T1 First
        {
            get { return _first; }
        }


        private readonly T2 _second;
        /// <summary>
        /// The second item of the tuple
        /// </summary>
        public T2 Second
        {
            get { return _second; }
        }


        /// <summary>
        /// Initialize the tuple items.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public Tuple2(T1 first, T2 second)
        {
            _first = first;
            _second = second;
        }
    }



    public struct Tuple3<T1, T2, T3> 
    {
        private readonly T1 _first;
        /// <summary>
        /// The first item of the tuple
        /// </summary>
        public T1 First 
        { 
            get { return _first; } 
        }


        private readonly T2 _second;
        /// <summary>
        /// The second item of the tuple
        /// </summary>
        public T2 Second
        {
            get { return _second; }
        }


        private readonly T3 _third;
        /// <summary>
        /// The second item of the tuple
        /// </summary>
        public T3 Third
        {
            get { return _third; }
        }


        /// <summary>
        /// Initialize the tuple items.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public Tuple3(T1 first, T2 second, T3 third)
        {
            _first = first;
            _second = second;
            _third = third;
        }
    }
}
