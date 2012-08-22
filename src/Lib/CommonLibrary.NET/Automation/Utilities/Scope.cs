using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Automation
{
    /// <summary>
    /// Used to store local variables.
    /// </summary>
    public class Scope 
    {        
        private List<OrderedDictionary> _stack;
        private int _currentStackIndex = 0;


        /// <summary>
        /// Initialize
        /// </summary>
        public Scope()
        {
            _stack = new List<OrderedDictionary>();
            _stack.Add(new OrderedDictionary());
        }


        /// <summary>
        /// Whether or not the scope contains the supplied variable name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            int stackIndex = Find(name);
            return stackIndex > -1;
        }


        /// <summary>
        /// Get the variable from the current scope.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                return Get<object>(name);
            }
        }


        /// <summary>
        /// Get the variable value associated with name from the scope
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Name of the varibale to get</param>
        /// <returns></returns>
        public T Get<T>(string name)
        {
            int stackIndex = Find(name);
            // Not found?
            if (stackIndex == -1) throw new KeyNotFoundException("variable : " + name + " was not found");

            // Convert to correct type.
            return (T)Convert.ChangeType(_stack[stackIndex][name], typeof(T));
        }


        /// <summary>
        /// Sets a value into the current scope.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void SetValue(string name, object val)
        {
            _stack[_currentStackIndex][name] = val;
        }


        /// <summary>
        /// Push another scope into the script.
        /// </summary>
        internal void Push()
        {
            _stack.Add(new OrderedDictionary());
            _currentStackIndex++;
        }


        /// <summary>
        /// Remove the current scope from the script.
        /// </summary>
        internal void Pop()
        {
            if (_stack.Count == 1) return;

            _stack.RemoveAt(_stack.Count - 1);
            _currentStackIndex--;
        }



        private int Find(string name)
        {
            int stackIndex = -1;
            for (var ndx = _stack.Count - 1; ndx >= 0; ndx--)
            {
                if (_stack[ndx].Contains(name))
                {
                    stackIndex = ndx;
                    break;
                }
            }
            return stackIndex;
        }
    }
}
