using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace ComLib
{
    /// <summary>
    /// Action result.
    /// </summary>
    public interface IStatusResult
    {
        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        string Message { get; }


        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        string Key { get; }


        /// <summary>
        /// Gets the target. The object for which this status applies to.
        /// </summary>
        /// <value>The target.</value>
        object Target { get; }
    }



    /// <summary>
    /// Interface for action results
    /// </summary>
    public interface IStatusResults : IEnumerable<StatusResult>
    {
        /// <summary>
        /// Add single status / error message.
        /// </summary>
        /// <param name="message"></param>
        void Add(string message);


        /// <summary>
        /// Add single action result to represent status/error.
        /// </summary>
        /// <param name="result"></param>
        void Add(StatusResult result);


        /// <summary>
        /// Add single action result.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="target"></param>
        void Add(string key, string message, object target);


        /// <summary>
        /// Add a set of actionresults.
        /// </summary>
        /// <param name="sourceResults"></param>
        void AddAll(IEnumerable<StatusResult> sourceResults);


        /// <summary>
        /// Get the number of action results in this list. 
        /// </summary>
        int Count { get; }


        /// <summary>
        /// Determine whether or not they are valid.
        /// </summary>
        bool IsValid { get; }
    }



    /// <summary>
    /// A single Validation result
    /// </summary>
    public class StatusResult : IStatusResult
    {
        private string _key;
        private string _message;
        private object _target;


        /// <summary>
        /// Initalized all the read-only
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <param name="isValid"></param>
        public StatusResult(string key, string message, object target)
        {
            SetInfo(key, message, target);
        }


        /// <summary>
        /// Initalize
        /// </summary>
        /// <param name="error"></param>
        /// <param name="isValid"></param>
        public StatusResult(string message, object target)
        {
            SetInfo(null, message, target);
        }


        /// <summary>
        /// Set the info.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        /// <param name="message"></param>
        /// <param name="isValid"></param>
        public void SetInfo(string key, string message, object target)
        {
            _key = key;
            _message = message;
            _target = target;
        }


        /// <summary>
        /// Message or error
        /// </summary>
        public string Message
        {
            get { return _message; }
        }


        /// <summary>
        /// Key which can represent a specific field/property to serve
        /// as contextual information for an action result.
        /// </summary>
        public string Key
        {
            get { return _key; }
        }


        /// <summary>
        /// The object associated with this action.
        /// </summary>
        public object Target
        {
            get { return _target; }
        }


        /// <summary>
        /// String representation of action result.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";
            bool hasKey = (_key != null && _key != "");

            if (hasKey)
                result = _key + " : " + _message;
            else
                result = _message;

            return result;
        }
    }



    /// <summary> 
    /// Stores validation results. 
    /// </summary> 
    /// <remarks>NOTE : Errors could be a read-only collection. 
    /// </remarks> 
    public class StatusResults : IStatusResults
    {
        protected List<StatusResult> _results;


        /// <summary>
        /// Null object.
        /// </summary>
        public static readonly StatusResults Empty = new StatusResults();



        /// <summary>
        /// Default constructor.
        /// </summary>
        public StatusResults()
        {
            _results = new List<StatusResult>();
        }


        /// <summary> 
        /// Construtor. 
        /// </summary> 
        /// <param name="isValid"></param> 
        public StatusResults(List<StatusResult> results)
        {
        }


        /// <summary>
        /// Returns the number of items in the results.
        /// </summary>
        public int Count
        {
            get
            {
                if (_results == null)
                    return 0;

                return _results.Count;
            }
        }


        /// <summary>
        /// <para>Adds a <see cref="ValidationResult"/>.</para>
        /// </summary>
        /// <param name="validationResult">The validation result to add.</param>
        public void Add(StatusResult result)
        {
            _results.Add(result);
        }


        /// <summary>
        /// Add a new validation result.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <param name="target"></param>
        public virtual void Add(string key, string message, object target)
        {
            Add(new StatusResult(key, message, target));
        }


        /// <summary>
        /// Add a new validation result with the supplied message
        /// </summary>
        /// <param name="message"></param>
        public virtual void Add(string message)
        {
            Add(new StatusResult(string.Empty, message, null));
        }


        /// <summary>
        /// <para>Adds all the <see cref="results"/> instances from <paramref name="sourceResults"/>.</para>
        /// </summary>
        /// <param name="sourceResults">The source results to add.</param>
        public void AddAll(IEnumerable<StatusResult> sourceResults)
        {
            _results.AddRange(sourceResults);
        }


        /// <summary> 
        /// For this base class, isValid doesn't mean anything.
        /// Specifically if this is used to store status messages.
        /// So return true here and allow derived classes to override
        /// the implementation.
        /// </summary> 
        public virtual bool IsValid
        {
            get
            {
                return true;
            }
        }


        /// <summary>
        /// This is exposed to allow results to be built into a single message
        /// in the ValidationUtils class.
        /// </summary>
        internal List<StatusResult> Results
        {
            get { return _results; }
        }


        /// <summary>
        /// Get the typed enumerator for the results.
        /// </summary>
        /// <returns></returns>
        IEnumerator<StatusResult> IEnumerable<StatusResult>.GetEnumerator()
        {
            return _results.GetEnumerator();
        }


        /// <summary>
        /// Get the enumerator for the results.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _results.GetEnumerator();
        }
    }
}
