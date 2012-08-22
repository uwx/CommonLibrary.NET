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
using System.Linq;
using System.Text;

using ComLib.Entities;
using ComLib.LocationSupport;
using ComLib.ValidationSupport;



namespace ComLib.Tests.Products
{
    /// <summary>
    /// Product entity.
    /// </summary>
    public partial class Product : ActiveRecordBaseEntity<Product>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Product New()
        {
            Product entity = new Product(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set AppId
		/// </summary>
		public int AppId { get; set; }


		/// <summary>
		/// Get/Set Make
		/// </summary>
		public string Make { get; set; }


		/// <summary>
		/// Get/Set Model
		/// </summary>
		public string Model { get; set; }


		/// <summary>
		/// Get/Set Cost
		/// </summary>
		public double Cost { get; set; }


		/// <summary>
		/// Get/Set IsInStock
		/// </summary>
		public bool IsInStock { get; set; }


		/// <summary>
		/// Get/Set StockDate
		/// </summary>
		public DateTime StockDate { get; set; }





        protected virtual IEntityValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;            
                Product entity = (Product)validationEvent.Target;
				Validation.IsNumericWithinRange(entity.AppId, false, false, -1, -1, results, "AppId");
				Validation.IsStringLengthMatch(entity.Make, false, true, true, 1, 50, results, "Make" );
				Validation.IsStringLengthMatch(entity.Model, true, false, true, -1, 50, results, "Model" );
				Validation.IsDateWithinRange(entity.StockDate, false, false, DateTime.MinValue, DateTime.MaxValue, results, "StockDate" );

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }
    }
}
