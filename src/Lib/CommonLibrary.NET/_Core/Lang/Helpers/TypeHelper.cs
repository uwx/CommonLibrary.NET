using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using ComLib.Lang.Types;
using ComLib.Lang.Core;


namespace ComLib.Lang.Helpers
{
    /// <summary>
    /// Helper class for datatypes.
    /// </summary>
    public class LangTypeHelper
    {
        public static LNumber ConverToLangDayOfWeekNumber(LObject obj)
        {
            if (obj.Type == LTypes.Date)
            {
                var day = (int)((LDate) obj).Value.DayOfWeek;
                return new LNumber(day);
            }
            
            if (obj.Type == LTypes.DayOfWeek)
            {
                var day = (int)((LDayOfWeek)obj).Value;
                return new LNumber(day);
            }
            return (LNumber)obj;
        }


        /// <summary>
        /// Converts from c# datatypes to fluentscript datatypes inside
        /// </summary>
        /// <param name="val"></param>
        public static LObject ConvertToLangValue(object val)
        {
            if (val == null) return LObjects.Null;            

            var type = val.GetType();
            
            if (type == typeof(int))       
                return new LNumber(Convert.ToDouble(val));
            
            if (type == typeof(double))
                return new LNumber((double)val);
            
            if (type == typeof(string))
                return new LString((string)val);

            if (type == typeof(DateTime))
                return new LDate((DateTime)val);

            if (type == typeof(TimeSpan))
                return new LTime((TimeSpan)val);

            if (type == typeof(DayOfWeek))
                return new LDayOfWeek((DayOfWeek) val);

            if (type == typeof(bool))
                return new LBool((bool)val);

            var isGenType = type.IsGenericType;
            if (isGenType)
            {
                var gentype = type.GetGenericTypeDefinition();
                if (type == typeof(List<object>) || gentype == typeof (List<>) || gentype == typeof (IList<>))
                    return new LArray((IList) val);

                if (type == typeof (Dictionary<string, object>))
                    return new LMap((Dictionary<string, object>) val);
            }
            // object
            return new LClass(val);
        }

        
        /// <summary>
        /// Converts a Type object from the host language to a fluentscript type.
        /// </summary>
        /// <param name="hostLangType"></param>
        /// <returns></returns>
        public static LType ConvertToLangType(Type hostLangType)
        {
            if (hostLangType == typeof(bool)) return LTypes.Bool;
            if (hostLangType == typeof(DateTime)) return LTypes.Date;
            if (hostLangType == typeof(int)) return LTypes.Number;
            if (hostLangType == typeof(double)) return LTypes.Number;
            if (hostLangType == typeof(string)) return LTypes.String;
            if (hostLangType == typeof(TimeSpan)) return LTypes.Time;
            if (hostLangType == typeof(Nullable)) return LTypes.Null;
            if (hostLangType == typeof(IList)) return LTypes.Array;
            if (hostLangType == typeof(IDictionary)) return LTypes.Map;
            
            return LTypes.Object;
        }


        /// <summary>
        /// Converts to host language type to a fluentscript type.
        /// </summary>
        /// <param name="hostLangType"></param>
        /// <returns></returns>
        public static LType ConvertToLangTypeClass(Type hostLangType)
        {
            var type = new LObjectType();
            type.Name = hostLangType.Name;
            type.FullName = hostLangType.FullName;
            type.TypeVal = TypeConstants.LClass;
            return type;
        }


        /// <summary>
        /// Get the type in the host language that represents the same type in fluentscript.
        /// </summary>
        /// <param name="ltype">The LType in fluentscript.</param>
        /// <returns></returns>
        public static Type ConvertToHostLangType(LType ltype)
        {
            if (ltype == LTypes.Bool) return typeof(bool);
            if (ltype == LTypes.Date) return typeof(DateTime);
            if (ltype == LTypes.Number) return typeof(int);
            if (ltype == LTypes.Number) return typeof(double);
            if (ltype == LTypes.String) return typeof(string);
            if (ltype == LTypes.Time) return typeof(TimeSpan);
            if (ltype == LTypes.Array) return typeof(IList);
            if (ltype == LTypes.Map) return typeof(IDictionary);
            if (ltype == LTypes.Null) return typeof(Nullable);
            
            return typeof (object);
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
                
                args[ndx] = ConvertToLangValue(val);
            }
        }


        /// <summary>
        /// Converts from c# datatypes to fluentscript datatypes inside
        /// </summary>
        /// <param name="args"></param>
        public static object[] ConvertToArrayOfHostLangValues(object[] args)
        {
            if (args == null || args.Length == 0)
                return args;

            // Convert types from c# types fluentscript compatible types.
            var convertedItems = new object[args.Length];
            for (int ndx = 0; ndx < args.Length; ndx++)
            {
                var val = args[ndx];
                if (val is LObject)
                    convertedItems[ndx] = ((LObject)val).GetValue();
                else
                    convertedItems[ndx] = val;
            }
            return convertedItems;
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
            var l = targetList as System.Collections.IList;
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
        public static object[] ConvertArgs(List<object> args, MethodInfo method)
        {
            var hostLangArgs = new List<object>();
            var parameters = method.GetParameters();
            if (parameters.Length == 0) return hostLangArgs.ToArray();

            // For each param
            for (int ndx = 0; ndx < parameters.Length; ndx++)
            {
                var param = parameters[ndx];
                var sourceArg = args[ndx] as LObject;

                // Null
                if (sourceArg == LObjects.Null)
                    hostLangArgs.Add(LangTypeHelper.GetDefaultValue(param.ParameterType));
                
                // Array or Map
                else if (sourceArg.Type.IsBuiltInType())
                    hostLangArgs.Add(sourceArg.GetValue());

                // 4. LArrayType
                else if (sourceArg.Type == LTypes.Array && param.ParameterType.IsGenericType)
                {
                    var gentype = param.ParameterType.GetGenericTypeDefinition();
                    if (gentype == typeof(List<>) || gentype == typeof(IList<>))
                    {
                        args[ndx] = ConvertToTypedList((List<object>)sourceArg.GetValue(), param.ParameterType);
                    }
                }
            }
            return hostLangArgs.ToArray();
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
    }
}
