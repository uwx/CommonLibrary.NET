using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ComLib;

namespace ComLib.Web.Helpers
{
    /// <summary>
    /// Helper class for Json related functionality.
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// Escapes text to handle double quotes.
        /// </summary>
        /// <param name="text">Text to escape.</param>
        /// <param name="encloseInQuotes">True to enclose in quotes.</param>
        /// <returns>Escaped text.</returns>
        public static string EscapeString(string text, bool encloseInQuotes = false)
        {
            if (string.IsNullOrEmpty(text))
                return encloseInQuotes ? "\"\"" : string.Empty;

            text = text.Replace("\\", "\\\\");
            text = text.Replace("\"", "\\\"");
            return encloseInQuotes ? "\"" + text + "\"" : text;
        }


        /// <summary>
        /// Converts the paged items to list of json objects using the property metadata provided.
        /// </summary>
        /// <typeparam name="T">Type of paged items.</typeparam>
        /// <param name="result">Paged list.</param>
        /// <param name="_columnProps">Metadata.</param>
        /// <param name="convertBoolToYesNo">True to convert booleans to yes/no.</param>
        /// <returns>Json string.</returns>
        public static string ConvertToJsonString<T>(PagedList<T> result, IList<PropertyInfo> _columnProps, bool convertBoolToYesNo = false)
        {
            StringBuilder buffer = new StringBuilder();

            // Build up all the records.
            for (int ndx = 0; ndx < result.Count; ndx++)
            {
                T item = result[ndx];
                string propname = string.Empty;
                object objval = null;
                string val = string.Empty;

                // Start the record 
                buffer.Append("{ ");

                // Build up all the properties in each record.
                for (int propNdx = 0; propNdx < _columnProps.Count; propNdx++)
                {
                    PropertyInfo prop = _columnProps[propNdx];
                    objval = _columnProps[propNdx].GetValue(item, null);
                    Type type = prop.PropertyType;
                    bool encloseInQuotes = false;

                    // String.
                    if (type == typeof(string))
                    {
                        val = objval == null ? string.Empty : objval.ToString();
                        val = val.Replace("\\", "\\\\");
                        val = val.Replace("\"", "\\\"");
                        encloseInQuotes = true;
                    }
                    // bool
                    else if (type == typeof(bool))
                    {
                        val = objval == null ? "false" : objval.ToString().ToLower();
                        if(convertBoolToYesNo)
                            val = val == "true" ? "\"yes\"" : "\"no\"";
                    }
                    // DateTime
                    else if (type == typeof(DateTime))
                    {
                        if (objval == null)
                            val = "0";
                        else
                        {
                            DateTime date = (DateTime)objval;
                            val = date.ToShortDateString();
                            encloseInQuotes = true;
                        }
                    }
                    else if (type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double))
                    {
                        val = ComLib.Converter.ConvertTo<double>(objval).ToString();
                    }
                    if (encloseInQuotes)
                        buffer.Append("\"" + _columnProps[propNdx].Name + "\": " + "\"" + val + "\"");
                    else
                        buffer.Append("\"" + _columnProps[propNdx].Name + "\": " + val);
                    if (propNdx != _columnProps.Count - 1)
                        buffer.Append(", ");
                }

                // End the record.
                buffer.Append(" }");
                if (ndx != result.Count - 1) buffer.Append(",");
            }
            return buffer.ToString();
        }
    }
}
