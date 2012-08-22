using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Information
{
    /// <summary>
    /// This interface should be implemented
    /// by an information service.
    /// </summary>
    public interface IInformation
    {
        /// <summary>
        /// Supported formats for the information.
        /// </summary>
        string SupportedFormats { get; }


        /// <summary>
        /// The format to get the info in.
        /// </summary>
        string Format { get; set; }


        /// <summary>
        /// Get the information using the default format.
        /// </summary>
        /// <returns></returns>
        string GetInfo();
    }
}
