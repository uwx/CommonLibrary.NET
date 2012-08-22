using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CodePlexDocGenerator
{
    /// <summary>
    /// This class is used to represent information
    /// of a ComLib component as that is defined
    /// in the _Examples.xml file.
    /// </summary>
    public class Component
    {
        /// <summary>
        /// Get/set the short description.
        /// </summary>
        public string ShortDesc { get; set; }


        /// <summary>
        /// Get/set the summary.
        /// </summary>
        public string Summary { get; set; }


        /// <summary>
        /// Get/set the component name.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Get/set the component id.
        /// </summary>
        public int component_id { get; set; }


        /// <summary>
        /// Get/set the file reference.
        /// </summary>
        public string FileReference { get; set; }


        /// <summary>
        /// Get/set whether wiki output is enabled.
        /// </summary>
        public bool WikiEnabled { get; set; }


        /// <summary>
        /// Get/set the wiki style.
        /// </summary>
        public WikiStyle Style { get; set; }


        /// <summary>
        /// Get/set the example file.
        /// </summary>
        public string ExampleFile { get; set; }


        /// <summary>
        /// Get/set the file id.
        /// </summary>
        public string FileId { get; set; }


        /// <summary>
        /// Default class constructor.
        /// </summary>
        /// <param name="dr">Datarow with main information.</param>
        public Component(DataRow dr)
        {
            component_id = Convert.ToInt32(dr["component_id"]);
            ShortDesc = Convert.ToString(dr["shortdesc"]);
            Summary = Convert.ToString(dr["summary"]);
            Name = Convert.ToString(dr["name"]);
        }


        /// <summary>
        /// Return the description for this component.
        /// </summary>
        /// <returns></returns>
        public string GetDescription()
        {
            if (!string.IsNullOrEmpty(ShortDesc))
                return ShortDesc;
            else
                return Summary.Replace("@shortdesc", ShortDesc);
        }
    }
}
