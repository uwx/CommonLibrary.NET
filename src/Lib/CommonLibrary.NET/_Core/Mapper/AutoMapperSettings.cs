using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib
{
    /// <summary>
    /// Configuration settings for mapper
    /// </summary>
    public class AutoMapperSettings
    {
        /// <summary>
        /// Default settings.
        /// </summary>
        public static readonly AutoMapperSettings Default = new AutoMapperSettings()        
        {
             CatchErrors = false,
             AutoCreateNestedObjects = true,
             MapNestedObjects = true,
             NameFilter = null,
             IsCaseSensitive = false,
             NestedObjectSeparator = ".",
             RemoveNameFilterWhenMapping = true
        };


        /// <summary>
        /// Default initialization
        /// </summary>
        public AutoMapperSettings() { }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="catchErrors">Whether or not to catch errors.</param>
        /// <param name="isCaseSensitive">Whether or not to consider case sensitivity in mapping properties</param>
        /// <param name="mapNestedObjects">Whether or not to map nested objects</param>
        /// <param name="namedFilter">A filter used as a prefix on the source properties to map. eg. p_age: p_ is the prefix.</param>
        /// <param name="removeNameFilterWhenMapping">Whether or not to remove the name filter from the destination property when mapping.
        ///     e.g. mapping from source "p_Name" with prefix "p_" and remove "p_" when mapping to destination "Name".
        /// </param>
        public AutoMapperSettings(bool catchErrors, bool isCaseSensitive, bool mapNestedObjects, string namedFilter, bool removeNameFilterWhenMapping)
        {
            CatchErrors = catchErrors;
            IsCaseSensitive = isCaseSensitive;
            MapNestedObjects = mapNestedObjects;
            NestedObjectSeparator = ".";
            NameFilter = namedFilter;
            RemoveNameFilterWhenMapping = removeNameFilterWhenMapping;
        }


        /// <summary>
        /// Whether or not to catch errors.
        /// </summary>
        public bool CatchErrors;


        /// <summary>
        /// Whether or not to consider case sensitivity in mapping properties
        /// </summary>
        public bool IsCaseSensitive;


        /// <summary>
        /// Filter for source properties ( used to only map source properties that have this in their name as a prefix )
        /// </summary>
        public string NameFilter;


        /// <summary>
        /// Whether or not to remove the name filter from the destination property when mapping.
        ///     e.g. mapping from source "p_Name" with prefix "p_" and remove "p_" when mapping to destination "Name".
        /// </summary>
        bool RemoveNameFilterWhenMapping;


        /// <summary>
        /// Whether or not to map nested objects
        /// </summary>
        public bool MapNestedObjects;


        /// <summary>
        /// The characther used to separted nested object names. e.g. "Address.City" "." is separator.
        /// </summary>
        public string NestedObjectSeparator;


        /// <summary>
        /// Whether or not to create nested objects if the types are null.
        /// </summary>
        public bool AutoCreateNestedObjects = true;
    }
}
