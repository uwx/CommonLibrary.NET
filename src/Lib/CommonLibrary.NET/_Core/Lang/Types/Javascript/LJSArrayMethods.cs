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
            this.AddMethod("concat", 		"Concat", 		typeof(LArray),		"Joins two or more arrays, and returns a copy of the joined arrays" );
            this.AddMethod("indexOf", 		"IndexOf", 		typeof(double),		"Search the array for an element and returns it's position" );
            this.AddMethod("join", 			"Join", 		typeof(LArray),		"Joins all elements of an array into a string" );
            this.AddMethod("lastIndexOf", 	"LastIndexOf",	typeof(double),		"Search the array for an element, starting at the end, and returns it's position" );
            this.AddMethod("pop", 			"Pop", 			typeof(object),		"Removes the last element of an array, and returns that element" );
            this.AddMethod("push", 			"Push", 		typeof(double),		"Adds new elements to the end of an array, and returns the new length" );
            this.AddMethod("reverse", 		"Reverse", 		typeof(LArray),		"Reverses the order of the elements in an array" );
            this.AddMethod("shift", 		"Shift", 		typeof(object),		"Removes the first element of an array, and returns that element" );
            this.AddMethod("slice", 		"Slice", 		typeof(LArray),		"Selects a part of an array, and returns the new array" );
            this.AddMethod("sort", 			"Sort", 		typeof(LArray),		"Sorts the elements of an array" );
            this.AddMethod("splice", 		"Splice", 		typeof(LArray),		"Adds/Removes elements from an array" );
            this.AddMethod("toString", 		"ToString", 	typeof(string),		"Converts an array to a string, and returns the result" );
            this.AddMethod("unshift", 		"Unshift", 		typeof(double),		"Adds new elements to the beginning of an array, and returns the new length" );
            this.AddMethod("valueOf", 		"ValueOf", 		typeof(object),		"Returns the primitive value of an array" );
            this.AddProperty(true, true,    "length",      "Length",       typeof(double),     "Sets or returns the number of elements in an array");

            // Associate the arguments for each declared function.
            //          Method name,    Param name,    Type,     Required   Alias,  Default,    Example         Description
            this.AddArg("concat", 		"items",       "list",     true,      "",     null,       "'abc', 'def'", "The arrays to be joined");
            this.AddArg("indexOf", 		"item",        "object",   true,      "",     null,       "abc",          "The item to search for");
            this.AddArg("indexOf",      "start",       "number",   false,     "",     0,          "0 | 5",        "Where to start the search. Negative values will start at the given position counting from the end, and search to the end");
            this.AddArg("join",         "separator",   "string",   false,     "",     ",",        "abc",          "The separator to be used. If omitted, the elements are separated with a comma");
            this.AddArg("lastIndexOf",  "item",        "object",   true,      "",     null,       "abc",          "The item to search for");
            this.AddArg("lastIndexOf", 	"start",       "number",   false,     "",     0,          "0 | 4",        "Where to start the search. Negative values will start at the given position counting from the end, and search to the beginning");
            this.AddArg("push",         "items",       "list",     true,      "",     null,       "abc",          "The items(s) to add to the array");
            this.AddArg("slice",        "start",       "number",   true,      "",     null,       "0",            "An integer that specifies where to start the selection (The first element has an index of 0). Use negative numbers to select from the end of an array");
            this.AddArg("slice",        "end",         "number",   false,     "",     null,       "1",            "An integer that specifies where to end the selection. If omitted, all elements from the start position and to the end of the array will be selected. Use negative numbers to select from the end of an array");
            this.AddArg("sort",         "sortfunction","function", false,     "",     "",         "",             "The function that defines the sort order");
            this.AddArg("splice",       "index",       "number",   true,      "",     null,       "1",            "An integer that specifies at what position to add/remove items, Use negative values to specify the position from the end of the array");
            this.AddArg("splice",       "howmany",     "number",   true,      "",     null,       "2",            "The number of items to be removed. If set to 0, no items will be removed");
            this.AddArg("splice",       "items",       "list",     false,     "",     null,       "2,3,4",        "The new item(s) to be added to the array");            
            this.AddArg("unshift", 		"items",       "list",     true,      "",     null,       "'abc', 'def'", "The item(s) to add to the beginning of the array");
            
        }


        #region Javascript API methods
        /// <summary>
        /// Lenght of the array.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        public int Length(LArray target)
        {
            return target.Raw.Count;
        }


        /// <summary>
        /// Joins two or more arrays, and returns a copy of the joined arrays
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="arrays">Array of arrays to add</param>
        /// <returns>A copy of joined array</returns>
        public LArray Concat(LArray target, params object[] arrays)
        {
            if (arrays == null || arrays.Length == 0) return target;

            var copy = new List<object>();
            AddRange(copy, target.Raw);
            for (int ndx = 0; ndx < arrays.Length; ndx++)
            {
                object item = arrays[ndx];
                IList array = (item is LArray) ? ((LArray)item).Raw : (IList)item;
                AddRange(copy, array);
            }
            return new LArray(copy);
        }


        /// <summary>
        /// Joins two or more arrays, and returns a copy of the joined arrays
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="item">The item to search for</param>
        /// <param name="start">The starting position of  the search.</param>
        /// <returns>A copy of joined array</returns>
        public int IndexOf(LArray target, object item, int start)
        {
            var foundPos = -1;
            var list = target.Raw;
            var total = list.Count;
            for(var ndx = start; ndx < total; ndx++)
            {
                var itemAt = list[ndx];
                if (itemAt == item)
                {
                    foundPos = ndx;
                    break;
                }
            }
            return foundPos;
        }


        /// <summary>
        /// Joins all elements of an array into a string
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="separator">The separator to use for joining the elements.</param>
        /// <returns></returns>
        public object Join(LArray target, string separator = ",")
        {
            if (target == null || target.Raw.Count == 0) return string.Empty;

            var buffer = new StringBuilder();

            var list = target.Raw;
            var total = list.Count;
            
            buffer.Append(target[0].ToString());
            if (total > 1)
            {
                for (int ndx = 1; ndx < list.Count; ndx++)
                {
                    buffer.Append(separator + list[ndx].ToString());
                }
            }
            string result = buffer.ToString();
            return result;
        }


        /// <summary>
        /// Joins two or more arrays, and returns a copy of the joined arrays
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="item">The item to search for</param>
        /// <param name="start">The starting position of  the search.</param>
        /// <returns>A copy of joined array</returns>
        public int LastIndexOf(LArray target, object item, int start)
        {
            var foundPos = -1;
            var list = target.Raw;
            var total = list.Count;
            for(var ndx = start; ndx < total; ndx++)
            {
                var itemAt = list[ndx];
                if (itemAt == item)
                {
                    foundPos = ndx;
                }
            }
            return foundPos;
        }


        /// <summary>
        /// Removes the last element of an array, and returns that element
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <returns>The removed element</returns>
        public object Pop(LArray target)
        {
            var index = target.Raw.Count - 1;
            object toRemove = target.Raw[index];
            target.Raw.RemoveAt(index);
            return toRemove;
        }


        /// <summary>
        /// Adds new elements to the end of an array, and returns the new length
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="elements">The elements to add</param>
        /// <returns>The new length</returns>
        public int Push(LArray target, params object[] elements)
        {
            if (elements == null || elements.Length == 0) return 0;

            // Add
            var list = target.Raw;
            foreach (object elem in elements)
                list.Add(elem);

            return list.Count;
        }


        /// <summary>
        /// Reverses the order of the elements in an array
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <returns></returns>
        public LArray Reverse(LArray target)
        {
            var list = target.Raw;
            int length = list.Count;
            if (length == 0 || length == 1) return null;

            // 2 or more.
            int highIndex = length - 1;
            int stopIndex = length / 2;
            if (length % 2 == 0)
                stopIndex--;
            for (int lowIndex = 0; lowIndex <= stopIndex; lowIndex++)
            {
                object tmp = list[lowIndex];
                list[lowIndex] = list[highIndex];
                list[highIndex] = tmp;
                highIndex--;
            }
            return target;
        }


        /// <summary>
        /// Removes the first element of an array, and returns that element
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <returns>The first element</returns>
        public object Shift(LArray target)
        {
            if (target.Raw.Count == 0) return null;
            object item = target[0];
            target.Raw.RemoveAt(0);
            return item;
        }


        /// <summary>
        /// Selects a part of an array, and returns the new array
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="start">The start of the selection</param>
        /// <param name="end">The end of the selection, if not supplied, selects all elements from start to end of the array</param>
        /// <returns>A new array</returns>
        public LArray Slice(LArray target, int start, int end)
        {
            List<object> items = new List<object>();
            if (end == -1)
                end = target.Raw.Count;
            for (var ndx = start; ndx < end; ndx++)
                items.Add(target.Raw[ndx]);
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
        public LArray Splice(LArray target, int index, int howmany, params object[] elements)
        {
            List<object> removed = null;
            if (howmany > 0)
            {
                removed = new List<object>();
                for (int ndxRemove = index; ndxRemove < (index + howmany); ndxRemove++)
                {
                    removed.Add(target[ndxRemove]);
                }
                RemoveRange(target.Raw, index, howmany);
            }
            if (elements != null && elements.Length > 0 )
            {
                int lastIndex = index;
                for (int ndx = 0; ndx < elements.Length; ndx++)
                {
                    object objToAdd = elements[ndx];
                    target.Raw.Insert(lastIndex, objToAdd);
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
        public int UnShift(LArray target, params object[] elements)
        {
            if (target.Raw == null) return 0;
            if (elements == null || elements.Length == 0) return target.Raw.Count;
            for (int ndx = 0; ndx < elements.Length; ndx++)
            {
                object val = elements[ndx];
                target.Raw.Insert(0, val);
            }
            return target.Raw.Count;
        }
        #endregion



        #region Helpers
        /// <summary>
        /// Adds all the items from the send array into the first array(target)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        protected object AddRange(IList target, IList array)
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
        protected object RemoveRange(IList target, int start, int howmany)
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
        /// Get / set value by index.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="index"></param>
        /// <returns></returns>
        public object Indexer_Get(LArray target, int index)
        {
            if (target == null || target.Raw == null || target.Raw.Count == 0) return LNull.Instance;
            var list = target.Raw;
            if (index < 0 || index >= list.Count) throw new IndexOutOfRangeException("Index : " + index);
            return list[index];
        }


        /// <summary>
        /// Get / set value by index.
        /// </summary>
        /// <param name="target">The target list to apply this method on.</param>
        /// <param name="index">The index position to set the value at</param>
        /// <param name="value">The vlaue to set</param>
        /// <returns></returns>
        public void Indexer_Set(LArray target, int index, object value)
        {
            if (target == null || target.Raw == null || target.Raw.Count == 0) return;
            var list = target.Raw;
            if (index < 0 || index >= list.Count) throw new IndexOutOfRangeException("Index : " + index);
            list[index] = value;
        }
        #endregion
    }
}
