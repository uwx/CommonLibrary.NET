using System;
using System.Collections.Generic;
using System.Reflection;
using ComLib.Lang.Types;

namespace ComLib.Lang.Helpers
{
    /// <summary>
    /// Helper class for datatypes.
    /// </summary>
    public class TypeHelper
    {
        /// <summary>
        /// Converts a Type object from the host language to fluentscript type.
        /// </summary>
        /// <param name="hostLangType"></param>
        /// <returns></returns>
        public static LType ConvertToLangType(Type hostLangType)
        {
            if (hostLangType == typeof(bool))     return LTypesLookup.BoolType;
            if (hostLangType == typeof(int))      return LTypesLookup.NumberType;
            if (hostLangType == typeof(double))   return LTypesLookup.NumberType;
            if (hostLangType == typeof(string))   return LTypesLookup.StringType;
            if (hostLangType == typeof(DateTime)) return LTypesLookup.DateType;
            if (hostLangType == typeof(TimeSpan)) return LTypesLookup.TimeType;
            if (hostLangType == typeof(Nullable)) return LTypesLookup.NullType;
            return LTypesLookup.ObjectType;
        }


        /// <summary>
        /// Converts from c# datatypes to fluentscript datatypes inside
        /// </summary>
        /// <param name="args"></param>
        public static void ConvertToLangTypeValues(List<object> args)
        {
            if (args == null || args.Count == 0)
                return;

            // Convert types from c# types fluentscript compatible types.
            for (int ndx = 0; ndx < args.Count; ndx++)
            {
                var val = args[ndx];
                if (val == null)
                    args[ndx] = LNull.NullResult;
                else if (val.GetType() == typeof(int))
                    args[ndx] = new LTypeValue(Convert.ToDouble(val), LTypesLookup.NumberType);
                else if (val.GetType() == typeof(double))
                    args[ndx] = new LTypeValue(val, LTypesLookup.NumberType);
                else if (val.GetType() == typeof(string))
                    args[ndx] = new LTypeValue(val, LTypesLookup.StringType);
                else if (val.GetType() == typeof(DateTime))
                    args[ndx] = new LTypeValue(val, LTypesLookup.DateType);
                else if (val.GetType() == typeof(TimeSpan))
                    args[ndx] = new LTypeValue(val, LTypesLookup.TimeType);
                else if (val.GetType() == typeof(List<object>))
                    args[ndx] = new LTypeValue(val, LTypesLookup.ArrayType);
                else if (val.GetType() == typeof(Dictionary<string, object>))
                    args[ndx] = new LTypeValue(val, LTypesLookup.MapType);

                // TODO: Need to handle other types such as List<T>, Dictionary<string, T> etc.
            }
        }


        /// <summary>
        /// Converts the source to the target list type by creating a new instance of the list and populating it.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetListType"></param>
        /// <returns></returns>
        public static object ConvertToTypedList(IList<object> source, Type targetListType)
        {
            var t = targetListType; // targetListType.GetType();
            var dt = targetListType.GetGenericTypeDefinition();
            var targetType = dt.MakeGenericType(t.GetGenericArguments()[0]);
            var targetList = Activator.CreateInstance(targetType);
            System.Collections.IList l = targetList as System.Collections.IList;
            foreach (var item in source) l.Add(item);
            return targetList;
        }


        /// <summary>
        /// Converts the source to the target list type by creating a new instance of the list and populating it.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetListType"></param>
        /// <returns></returns>
        public static object ConvertToTypedDictionary(IDictionary<string, object> source, Type targetListType)
        {
            var t = targetListType; // targetListType.GetType();
            var dt = targetListType.GetGenericTypeDefinition();
            var targetType = dt.MakeGenericType(t.GetGenericArguments()[0], t.GetGenericArguments()[1]);
            var targetDict = Activator.CreateInstance(targetType);
            System.Collections.IDictionary l = targetDict as System.Collections.IDictionary;
            foreach (var item in source) l.Add(item.Key, item.Value);
            return targetDict;
        }


        /// <summary>
        /// Converts arguments from one type to another type that is required by the method call.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="method">The method for which the parameters need to be converted</param>
        public static void ConvertArgs(List<object> args, MethodInfo method)
        {
            var parameters = method.GetParameters();
            if (parameters == null || parameters.Length == 0) return;

            // For each param
            for (int ndx = 0; ndx < parameters.Length; ndx++)
            {
                var param = parameters[ndx];
                object sourceArg = args[ndx];

                // types match ? continue to next one.
                if (sourceArg.GetType() == param.ParameterType)
                    continue;

                // 1. Double to Int32
                if (sourceArg.GetType() == typeof(double) && param.ParameterType == typeof(int))
                    args[ndx] = Convert.ToInt32(sourceArg);

                // 2. Double to Int32
                if (sourceArg.GetType() == typeof(double) && param.ParameterType == typeof(long))
                    args[ndx] = Convert.ToInt64(sourceArg);

                // 3. LDate to datetime
                else if (sourceArg is LDate)
                    args[ndx] = ((LDate)sourceArg).Raw;

                // 4. Null
                else if (sourceArg == LNull.Instance)
                    args[ndx] = TypeHelper.GetDefaultValue(param.ParameterType);

                // 5. LArray
                else if ((sourceArg is LArray || sourceArg is List<object>) && param.ParameterType.IsGenericType)
                {
                    if (sourceArg is LArray) sourceArg = ((LArray)sourceArg).Raw;
                    var gentype = param.ParameterType.GetGenericTypeDefinition();
                    if (gentype == typeof(List<>) || gentype == typeof(IList<>))
                    {
                        args[ndx] = ConvertToTypedList((List<object>)sourceArg, param.ParameterType);
                    }
                }
            }
        } 


        /// <summary>
        /// Gets the default value for the supplied type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(Type type)
        {
            if (type == typeof(int)) return 0;
            if (type == typeof(bool)) return false;
            if (type == typeof(double)) return 0.0;
            if (type == typeof(DateTime)) return DateTime.MinValue;
            if (type == typeof(TimeSpan)) return TimeSpan.MinValue;
            return null;
        }


        /// <summary>
        /// Whether or not the object supplied is a basic fluentscript/languge type that extends from LObject
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsBasicLangType(object obj)
        {
            return IsBasicTypeCSharpType(obj);
        }


        /// <summary>
        /// Whether or not the type supplied is a basic type.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsBasicTypeCSharpType(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            Type type = obj.GetType();
            if (type == typeof(int)) return true;
            if (type == typeof(long)) return true;
            if (type == typeof(double)) return true;
            if (type == typeof(bool)) return true;
            if (type == typeof(string)) return true;
            if (type == typeof(DateTime)) return true;
            if (type == typeof(TimeSpan)) return true;

            return false;
        }


        /// <summary>
        /// Converts each item in the parameters object array to an integer.
        /// </summary>
        /// <param name="parameters"></param>
        public static int[] ConvertToInts(object[] parameters)
        {
            // Convert all parameters to int            
            int[] args = new int[parameters.Length];
            for (int ndx = 0; ndx < parameters.Length; ndx++)
            {
                args[ndx] = Convert.ToInt32(parameters[ndx]);
            }
            return args;
        }


        /// <summary>
        /// Converts a list of items to a dictionary with the items.
        /// </summary>
        /// <typeparam name="T">Type of items to use.</typeparam>
        /// <param name="items">List of items.</param>
        /// <returns>Converted list as dictionary.</returns>
        public static IDictionary<T, T> ToDictionary<T>(IList<T> items)
        {
            IDictionary<T, T> dict = new Dictionary<T, T>();
            foreach (T item in items)
            {
                dict[item] = item;
            }
            return dict;
        }
    }
}
