using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ComLib.Reflection;


namespace ComLib.Information
{
    /// <summary>
    /// Information Service
    /// </summary>
    public class InformationService : ExtensionService<InfoAttribute, IInformation>,  IInformationService
    {
        /// <summary>
        /// Gets the information task after validating that the user has access to it.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="authenticate"></param>
        /// <param name="authenticator"></param>
        /// <returns></returns>
        public BoolMessageItem<KeyValue<InfoAttribute, IInformation>> GetInfoTask(string name, bool authenticate, Func<string, bool> authenticator)
        {
            var metadata = Lookup[name];
            var roles = ((InfoAttribute)metadata.Attribute).Roles;
            if (authenticate && !authenticator(roles))
                return new BoolMessageItem<KeyValue<InfoAttribute, IInformation>>(null, false, "Not authorized");

            var info = Create(name);    
            var pair = new KeyValue<InfoAttribute, IInformation>((InfoAttribute)metadata.Attribute, info);
            return new BoolMessageItem<KeyValue<InfoAttribute, IInformation>>(pair, true, "");
        }
    }
}
