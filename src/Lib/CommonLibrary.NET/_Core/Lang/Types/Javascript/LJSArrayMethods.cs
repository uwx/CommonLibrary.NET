using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Lang.Parsing;
using ComLib.Lang.Helpers;

namespace ComLib.Lang.Types
{
    /// <summary>
    /// Array datatype
    /// </summary>
    public class LJSArrayMethods : LTypeMethods
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public override void Init()
        {
            // Create the methods associated with this type.
            this.AddMethod("concat",       "Concat",       typeof(string),  "Joins two or more strings, and returns a copy of the joined strings" );
            this.AddMethod("indexOf",      "IndexOf",      typeof(double),  "Returns the position of the first found occurrence of a specified value in a string" );
            this.AddMethod("join",         "join",         typeof(double),  "Returns the position of the last found occurrence of a specified value in a string");
            this.AddMethod("pop",          "CharAt",       typeof(string),  "Searches for a match between a regular expression and a string, and returns the matches" );
            this.AddMethod("push",         "Replace",      typeof(string),  "Searches for a match between a substring (or regular expression) and a string, and replaces the matched substring with a new substring" );
            this.AddMethod("reverse",      "Search",       typeof(string),  "Searches for a match between a regular expression and a string, and returns the position of the match" );
            this.AddMethod("shift",        "CharAt",       typeof(string),  "Extracts a part of a string and returns a new string" );
            this.AddMethod("slice",        "CharAt",       typeof(string),  "Splits a string into an array of substrings" );
            this.AddMethod("sort",         "Substr",       typeof(string),  "Extracts the characters from a string, beginning at a specified start position, and through the specified number of character" );
            this.AddMethod("splice",       "Substring",    typeof(string),  "Extracts the characters from a string, between two specified indices" );
            this.AddMethod("unshift",      "ToLowerCase",  typeof(string),  "Converts a string to lowercase letters" );
            this.AddMethod("valueOf",      "ToString",     typeof(string),  "Returns the primitive value of a String object" );
            this.AddProperty("length",     "Length",       typeof(double),  "Returns the length of the string");

            // Associate the arguments for each declared function.
            this.AddArg("charAt", 		"index",       "number", true,  "", 0,    "0 | 4",        "An integer representing the index of the character you want to return");
            this.AddArg("concat", 		"items",       "list",   true,  "", null, "'abc', 'def'", "The strings to be joined");
            this.AddArg("indexOf", 		"pattern",     "string", true,  "", null, "abc",          "The string pattern to search for");
            this.AddArg("indexOf", 		"start",       "number", false, "", 0,    "0 | 5",        "The starting position of the search");
            this.AddArg("lastIndexOf", 	"searchvalue", "string", true,  "", null, "abc",          "The string to search for");
            this.AddArg("lastIndexOf", 	"start",       "number", false, "", 0,    "0 | 4",        "The position where to start the search. If omitted, the default value is the length of the string");
            this.AddArg("replace", 		"searchvalue", "string", true,  "", "",   "abc",          "The value, or regular expression, that will be replaced by the new value");
            this.AddArg("replace", 		"newvalue",    "string", true,  "", "",   "bbb",          "The value to replace the searchvalue with");
            this.AddArg("search", 		"searchvalue", "string", true,  "", "",   "abc",          "The value, or regular expression, to search for.");
            this.AddArg("substr", 		"start",	   "number", true,  "", 0,    "0 | 4",        "The postition where to start the extraction. First character is at index 0");
            this.AddArg("substr", 		"length",      "number", false, "", "",   "5 | 10",       "The number of characters to extract. If omitted, it extracts the rest of the string" );
            this.AddArg("substring", 	"from",	       "number", true,  "", 0,    "0 | 4",        "The index where to start the extraction. First character is at index 0");
            this.AddArg("substring", 	"to",          "number", false, "", "",   "5 | 10",       "The index where to stop the extraction. If omitted, it extracts the rest of the string");
        }


        #region Javascript API methods
        /// <summary>
        /// Adds all the items from the send array into the first array(target)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        public object AddRange(IList target, IList array)
        {
            if (array == null) return target;
            if (array.Count == 0) return target;
            foreach (var item in array)
                target.Add(item);

            return target;
        }


        /// <summary>
        /// Removes items from the array starting at the position supplied and removes the number of items supplied
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="start">The starting index position in the list to remove items from.</param>
        /// <param name="howmany">How many items to remove</param>
        /// <returns></returns>
        public object RemoveRange(IList target, int start, int howmany)
        {
            if (target == null) return target;
            if (target.Count == 0) return target;
            var totalRemoved = 0;
            while (totalRemoved < howmany)
            {
                target.RemoveAt(start);
                totalRemoved++;
            }

            return target;
        }

        
        /// <summary>
        /// Lenght of the array.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        public int Length(IList target)
        {
            return target.Count;
        }


        /// <summary>
        /// Get / set value by index.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public object IndexerGet(IList target, int index)
        {
            if (target == null || target.Count == 0) return null;
            if (index < 0 || index >= target.Count) throw new IndexOutOfRangeException("Index : " + index);
            return target[index];
        }


        /// <summary>
        /// Get / set value by index.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="index">The index position to set the value at</param>
        /// <param name="value">The vlaue to set</param>
        /// <returns></returns>
        public void IndexerSet(IList target, int index, object value)
        {
            if (target == null || target.Count == 0) 
                return;

            if (index < 0 || index >= target.Count) throw new IndexOutOfRangeException("Index : " + index);
            target[index] = value;
         
        }


        /// <summary>
        /// Joins two or more arrays, and returns a copy of the joined arrays
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="arrays">Array of arrays to add</param>
        /// <returns>A copy of joined array</returns>
        public object Concat(IList target, params object[] arrays)
        {
            if (arrays == null || arrays.Length == 0) return target;

            for (int ndx = 0; ndx < arrays.Length; ndx++)
            {
                object item = arrays[ndx];
                LArray array = (item is LArray) ? (LArray)item : ConvertToLArray(item);
                AddRange(target, array.Raw);
            }
            return this;
        }


        /// <summary>
        /// Joins all elements of an array into a string
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="separator">The separator to use for joining the elements.</param>
        /// <returns></returns>
        public object Join(IList target, string separator = ",")
        {
            if (target == null || target.Count == 0) return string.Empty;

            var buffer = new StringBuilder();

            buffer.Append(target[0].ToString());
            if (target.Count > 1)
            {
                for (int ndx = 1; ndx < target.Count; ndx++)
                {
                    buffer.Append(separator + target[ndx].ToString());
                }
            }
            string result = buffer.ToString();
            return result;
        }


        /// <summary>
        /// Removes the last element of an array, and returns that element
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <returns>The removed element</returns>
        public object Pop(IList target)
        {
            object toRemove = target[target.Count - 1];
            target.RemoveAt(target.Count - 1);
            return toRemove;
        }


        /// <summary>
        /// Adds new elements to the end of an array, and returns the new length
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="elements">The elements to add</param>
        /// <returns>The new length</returns>
        public object Push(IList target, params object[] elements)
        {
            if (elements == null || elements.Length == 0) return null;

            // Add
            foreach (object elem in elements)
                target.Add(elem);

            return target.Count;
        }


        /// <summary>
        /// Reverses the order of the elements in an array
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <returns></returns>
        public object Reverse(IList target)
        {
            int length = target.Count;
            if (length == 0 || length == 1) return null;

            // 2 or more.
            int highIndex = length - 1;
            int stopIndex = length / 2;
            if (length % 2 == 0)
                stopIndex--;
            for (int lowIndex = 0; lowIndex <= stopIndex; lowIndex++)
            {
                object tmp = target[lowIndex];
                target[lowIndex] = target[highIndex];
                target[highIndex] = tmp;
                highIndex--;
            }
            return this;
        }


        /// <summary>
        /// Removes the first element of an array, and returns that element
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <returns>The first element</returns>
        public object Shift(IList target)
        {
            if (target.Count == 0) return null;
            object item = target[0];
            target.RemoveAt(0);
            return item;
        }


        /// <summary>
        /// Selects a part of an array, and returns the new array
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="start">The start of the selection</param>
        /// <param name="end">The end of the selection, if not supplied, selects all elements from start to end of the array</param>
        /// <returns>A new array</returns>
        public object Slice(IList target, int start, int end)
        {
            List<object> items = new List<object>();
            for (var ndx = start; ndx <= end; ndx++)
                items.Add(target[ndx]);
            return new LArray(null, items);
        }


        /// <summary>
        /// Adds/Removes elements from an array
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="index">The index position to add/remove</param>
        /// <param name="howmany">How many elements to remove, if 0 no elements are removed</param>
        /// <param name="elements">Optional: The elements to add</param>
        /// <returns></returns>
        public object Splice(IList target, int index, int howmany, params object[] elements)
        {
            List<object> removed = null;
            if (howmany > 0)
            {
                removed = new List<object>();
                for (int ndxRemove = index; ndxRemove < (index + howmany); ndxRemove++)
                {
                    removed.Add(target[ndxRemove]);
                }
                RemoveRange(target, index, howmany);
            }
            if (elements != null && elements.Length > 0 )
            {
                int lastIndex = index;
                for (int ndx = 0; ndx < elements.Length; ndx++)
                {
                    object objToAdd = elements[ndx];
                    target.Insert(lastIndex, objToAdd);
                    lastIndex++;
                }
            }
            return new LArray(removed);
        }
        

        /// <summary>
        /// Adds new elements to the beginning of an array, and returns the new length
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="elements">The elements to add.</param>
        /// <returns>The new length</returns>
        public object UnShift(IList target, params object[] elements)
        {
            if (elements == null || elements.Length == 0) return null;
            for (int ndx = 0; ndx < elements.Length; ndx++)
            {
                object val = elements[ndx];
                target.Insert(0, val);
            }
            return target.Count;
        }


        /// <summary>
        /// Gets the element at the specified position
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="ndx">The index position to get</param>
        /// <returns>Element at position</returns>
        public object GetByIndex(IList target, int ndx)
        {
            if (ndx < 0 || ndx >= target.Count)
                throw new IndexOutOfRangeException("Index out of bounds " + ndx);

            // TO_DO:
            return target[ndx];
        }


        /// <summary>
        /// Sets the element at the specified position
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="ndx">The index position to set</param>
        /// <param name="val">The value to set</param>
        /// <returns>The object being set</returns>
        public object SetByIndex(IList target, int ndx, object val)
        {
            target[ndx] = val;
            return val;
        }
        #endregion


        private LArray ConvertToLArray(object list)
        {
            return list as LArray;
        }
    }
}
