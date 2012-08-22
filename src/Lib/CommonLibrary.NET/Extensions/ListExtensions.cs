using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace ComLib.Extensions
{
    /// <summary>
    /// Extension methods for dictionaries.
    /// </summary>
    public static class ListExtensions
    {        
        /// <summary>
        /// AddRange of items of same type to IList 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="itemsToAdd"></param>
        public static IList<T> AddRange<T>(this IList<T> items, IList<T> itemsToAdd)
        {
            if (items == null || itemsToAdd == null)
                return items;

            foreach (T item in itemsToAdd)
                items.Add(item);

            return items;
        }


        /// <summary>
        /// Is empty collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        [Obsolete("Method moved to Utilities.EnumerableExtensions.IsNullOrEmpty()")]
        public static bool IsNullOrEmpty<T>(IList<T> items)
        {
            return items.IsNullOrEmpty();
        }
    }
}
