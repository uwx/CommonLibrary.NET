using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using ComLib;
using ComLib.Entities;
using ComLib.ValidationSupport;


namespace ComLib.NamedQueries
{
    /// <summary>
    /// Extend the validation with validation of the query's parameters if there are any.
    /// </summary>
    public partial class NamedQueryValidator : EntityValidator
    {
        /// <summary>
        /// Validate the query parameters if there are any.
        /// </summary>
        /// <param name="validationEvent">Validation event to use.</param>
        /// <returns></returns>
        public override bool Validate(ValidationEvent validationEvent)
        {
            object target = validationEvent.Target;
            IValidationResults results = validationEvent.Results; 

            bool valid = ValidateInternal(validationEvent);
            if (!valid) return valid;

            // Has paremeters?
            NamedQuery query = (NamedQuery)target;
            if (string.IsNullOrEmpty(query.Parameters)) return true;

            IList<NamedQueryParam> queryParams = NamedQueryParamsParser.ParseParams(query.Parameters);
            NamedQueryParamsValidator validator = new NamedQueryParamsValidator(queryParams);
            bool isValid = validator.Validate(results).IsValid;
            return isValid;
        }   
    }



    /// <summary>
    /// Extend the named query by adding parameters using a structure rather than string.
    /// </summary>
    public partial class NamedQuery : ActiveRecordBaseEntity<NamedQuery>
    {
        /// <summary>
        /// Add query parameters.
        /// </summary>
        /// <param name="queryParams">List of query parameters.</param>
        public void AddParameters(List<NamedQueryParam> queryParams)
        {
            string allParams = string.Empty;
            foreach (NamedQueryParam param in queryParams)
            {
                allParams += param.ToString() + ",";
            }
            if (allParams[allParams.Length - 1] == ',')
                allParams = allParams.Substring(0, allParams.Length - 1);

            this.Parameters = allParams;
        }


        /// <summary>
        /// Add query parameters.
        /// </summary>
        /// <param name="queryParams">List of query parameters.</param>
        public void AddParameters(List<string> queryParams)
        {
            string allParams = string.Empty;
            foreach (string queryParam in queryParams)
            {
                NamedQueryParam param = NamedQueryParamsParser.ParseParams(queryParam)[0];
                allParams += param.ToString() + ",";
            }
            if (allParams[allParams.Length - 1] == ',')
                allParams = allParams.Substring(0, allParams.Length - 1);

            this.Parameters = allParams;
        }
    }



    /// <summary>
    /// Class to represent an indivdual parameter that is part
    /// of a named query.
    /// </summary>
    /// <example>
    /// "@id:int:1:'';@businessDate:date:1:${today};@username:string:1:''".
    /// </example>
    public class NamedQueryParam
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        public string Name;


        /// <summary>
        /// The type of the parameter.
        /// </summary>
        public string Typ;


        /// <summary>
        /// Indicates if the parameter is requried.
        /// </summary>
        public bool IsRequired = true;


        /// <summary>
        /// The default value of the parameter.
        /// </summary>
        public string DefaultValue;


        /// <summary>
        /// The value to use for the parameter.
        /// </summary>
        public string Val;


        /// <summary>
        /// Return string representation.
        /// </summary>
        /// <returns>String representation of this instance.</returns>
        public override string ToString()
        {
            //@businessDate:date:1:${today};@username:string:1:''
            string required = IsRequired ? "1" : "0";
            string val = this.Name + ":" + this.Typ + ":" + required + ":'" + DefaultValue + "'";
            return val;
        }
        
    }



    /// <summary>
    /// Parser for parsing parameter definition strings in the format 
    /// "@id:int:1:'';@businessDate:date:1:${today};@username:string:1:''".
    /// </summary>
    public class NamedQueryParamsParser
    {
        /// <summary>
        /// Regular expression for the parameter definition.
        /// </summary>
        // (?<name>@[\w_]+):(?<type>[\w]+):(?<required>[01]{1}):(?<defaultValue>\$?\{?[\w_'\"]+\}?);*
        public const string ParamDefinitionRegEx = @"(?<name>@[\w_]+):(?<type>[\w]+):(?<required>[01]{1}):(?<defaultValue>\$?\{?[\w_'" + "\"" + @"]+\}?);*";



        /// <summary>
        /// Parses the parameters definition string and returns a List of NamedQueryParam objects.
        /// </summary>
        /// <param name="paramDefinitions">E.g. "@id:int:1:'';@businessDate:date:1:${today};@username:string:1:''"</param>
        /// <returns>List of NamedQueryParam objects.</returns>
        public static IList<NamedQueryParam> ParseParams(string paramDefinitions)
        {
            MatchCollection parameterMatches = Regex.Matches(paramDefinitions, ParamDefinitionRegEx);
            IList<NamedQueryParam> namedQueryParams = new List<NamedQueryParam>();

            // Get each parameter.
            foreach (Match parameterMatch in parameterMatches)
            {
                NamedQueryParam param = new NamedQueryParam();

                Group group = parameterMatch.Groups["name"];
                if (group != null)
                    param.Name = group.Value;

                group = parameterMatch.Groups["type"];
                if (group != null)
                    param.Typ = group.Value;

                group = parameterMatch.Groups["required"];
                if (group != null)
                    param.IsRequired = group.Value == "1" ? true : false;

                group = parameterMatch.Groups["defaultValue"];
                if (group != null)
                    param.DefaultValue = group.Value;

                namedQueryParams.Add(param);
            }
            return namedQueryParams;
        }
    }



    /// <summary>
    /// Validator class for the named query parameters.
    /// </summary>
    public class NamedQueryParamsValidator : EntityValidator
    {
        private static IDictionary<string, Type> _validTypes;
        private IList<NamedQueryParam> _namedQueryParams;


        /// <summary>
        /// Keep track of the various types of the parameters.
        /// </summary>
        static NamedQueryParamsValidator()
        {
            _validTypes = new Dictionary<string, Type>();
            _validTypes.Add(typeof(string).Name, typeof(string));
            _validTypes.Add(typeof(int).Name, typeof(int));
            _validTypes.Add(typeof(double).Name, typeof(double));
            _validTypes.Add(typeof(DateTime).Name, typeof(DateTime));
            _validTypes.Add(typeof(bool).Name, typeof(bool));
        }
        

        /// <summary>
        /// initialize list of parameters.
        /// </summary>
        /// <param name="namedQueryParams">List of parameters.</param>
        public NamedQueryParamsValidator(IList<NamedQueryParam> namedQueryParams)
        {
            _namedQueryParams = namedQueryParams;
        }


        /// <summary>
        /// Validate all the params and collect all the errors.
        /// </summary>
        /// <returns>True if the validation passed.</returns>
        protected override bool ValidateInternal(ValidationEvent validationEvent)
        {
            object target = validationEvent.Target;
            IValidationResults results = validationEvent.Results; 

            string message = string.Empty;
            bool success = true;

            foreach (NamedQueryParam param in _namedQueryParams)
            {
                BoolMessage result = ValidateParam(param);
                if (!result.Success)
                {
                    success = false;
                    message += Environment.NewLine + result.Message;
                }
            }

            // Don't create instance, return reaon-only singleton.
            if (success) return BoolMessage.True.Success;

            return new BoolMessage(false, message).Success;
        }


        /// <summary>
        /// Validate a single parameter by 
        /// 1. checking it's type against
        /// 2. Check the supplied value against the type.
        /// 3. supplied value is not null.
        /// </summary>
        /// <param name="param">Query parameter.</param>
        /// <returns>Result of validation.</returns>
        protected BoolMessage ValidateParam(NamedQueryParam param)
        {
            // Check type.
            if (!_validTypes.ContainsKey(param.Typ))
                return new BoolMessage(false, "Unknown type : " + param.Typ);

            // Check if required param.
            if (param.IsRequired)
            {
                if (param.Val == null)
                    return new BoolMessage(false, "Value not provided for : " + param.Name);
            }
            return BoolMessage.True;
        }
    }
}
