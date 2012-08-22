using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Entities
{
    /// <summary>
    /// Data massager for an entity.
    /// </summary>
    public class EntityMassager : IEntityMassager
    {
        #region IEntityMassager Members
        /// <summary>
        /// Massage the entity data given the entity action.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        public virtual void Massage(object entity, EntityAction action)
        {            
        }

        #endregion
    }
}
