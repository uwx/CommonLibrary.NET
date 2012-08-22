using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;


namespace ComLib.Automation
{
    /// <summary>
    /// Sample automation script command class.
    /// </summary>
    [Command(Name = "List", Description = "Lists all the automation commands available in the system")]
    public class CommandListCommands : Command
    {

        /// <summary>
        /// Get list of all the commands in the system.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override BoolMessageItem DoExecute(CommandContext context)
        {
            var filter = Get<string>("filter");
            string commandList = "";

            // No filter ? get all 
            if (string.IsNullOrEmpty(filter))
            {
                commandList = this.Script.Service.GetList(null, null);
                return new BoolMessageItem(commandList, true, string.Empty);
            }

            // Find by filter.
            commandList = this.Script.Service.GetList(null, (attrib) => attrib.Name.Contains(filter));
            return new BoolMessageItem(commandList, true, string.Empty);
        }
    }
}
