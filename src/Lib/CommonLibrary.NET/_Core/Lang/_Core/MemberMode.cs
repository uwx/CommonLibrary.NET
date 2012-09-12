﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Lang.Core
{
    /// <summary>
    /// Represents the member access mode
    /// </summary>
    public enum MemberMode
    {
        /// <summary>
        /// External function
        /// </summary>
        FunctionExternal,


        /// <summary>
        /// Internal function
        /// </summary>
        FunctionScript,


        /// <summary>
        /// Instance method on class
        /// </summary>
        CustObjMethodInstance,


        /// <summary>
        /// Static method on class
        /// </summary>
        CustObjMethodStatic
    }
}
