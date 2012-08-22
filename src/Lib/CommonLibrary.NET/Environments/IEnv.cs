using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ComLib.Environments
{
    /// <summary>
    /// Represents functionality for an Environment ( dev, qa, uat, prod etc )
    /// </summary>
    public interface IEnv
    {
        /// <summary>
        /// Name of the currently selected environment.
        /// </summary>
        string Name { get; set; }


        /// <summary>
        /// Environment type of the current selected env.
        /// </summary>
        EnvType EnvType { get; }


        /// <summary>
        /// Currently selected environment.
        /// </summary>
        EnvItem Selected { get; }
        

        /// <summary>
        /// Is dev
        /// </summary>
        bool IsDev { get; }


        /// <summary>
        /// Is prod.
        /// </summary>
        bool IsProd { get; }


        /// <summary>
        /// Is Qa
        /// </summary>
        bool IsQa { get; }


        /// <summary>
        /// Is uat environment.
        /// </summary>
        bool IsUat { get; }
                       

        /// <summary>
        /// All available environments names.
        /// e.g. "ny.prod,london.prod,uat,qa,dev"
        /// </summary>
        List<string> Available { get; }


        /// <summary>
        /// List of environments that this inherits from.
        /// </summary>
        List<EnvItem> Inheritance { get; }
        
        
        /// <summary>
        /// Get the inheritance path, e.g. prod;qa;dev.
        /// </summary>
        string Inherits { get; }


        /// <summary>
        /// Reference paths. e.g. config files.
        /// </summary>
        string RefPath { get; }


        /// <summary>
        /// Get a readonly collection of all the environments.
        /// </summary>
        /// <returns></returns>
        ReadOnlyCollection<EnvItem> GetAll();


        /// <summary>
        /// Get the environment item associated with the name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        EnvItem Get(string name);


        /// <summary>
        /// Get the environment item at the specified index.
        /// </summary>
        /// <param name="ndx"></param>
        /// <returns></returns>
        EnvItem Get(int ndx);


        /// <summary>
        /// Get the number of various available environments. 
        /// </summary>
        int Count { get; }


        /// <summary>
        /// Change to the new environment.
        /// </summary>
        /// <param name="envName"></param>
        void Change(string envName);


        /// <summary>
        /// Handler for environment change notifications.
        /// </summary>
        event EventHandler OnEnvironmentChange;         
    }
}
