using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Entities
{

    /// <summary>
    /// Enum to represent an action being performed on an entity.
    /// This action is used and passed on to validators and massagers
    /// so they can run appropriately.
    /// </summary>
    public enum EntityAction
    {
        /// <summary>
        /// Represents creation action on entity.
        /// </summary>        
        Create,


        /// <summary>
        /// Represents retrieval action on entity.
        /// </summary>
        Retrieve,


        /// <summary>
        /// Represents update action on entity.
        /// </summary>
        Update,


        /// <summary>
        /// Represents deletion action on entity.
        /// </summary>
        Delete,


        /// <summary>
        /// Copy a entity.
        /// </summary>
        Copy,


        /// <summary>
        /// Import entity(s)
        /// </summary>
        Import,


        /// <summary>
        /// Export entity(s)
        /// </summary>
        Export,


        /// <summary>
        /// Backup the entities.
        /// </summary>
        Backup,


        /// <summary>
        /// Save the entity.
        /// </summary>
        Save
    }
}
