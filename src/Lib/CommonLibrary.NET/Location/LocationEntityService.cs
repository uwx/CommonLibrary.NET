using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Entities;


namespace ComLib.LocationSupport
{
    /// <summary>
    /// Location entity class that sub-classes EntityServiceT and does some additional configuration on the city/state/country before saving
    /// to underlying repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LocationEntityService<T> : EntityService<T> where T : LocationBase, IEntity
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public LocationEntityService()
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="repo"></param>
        public LocationEntityService(IRepository<T> repo)
        {
            _repository = repo;
        }


        /// <summary>
        /// Create the entity.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="executor"></param>
        protected override void InternalCreate(IActionContext ctx, Action<IActionContext> executor)
        {
            IEntity entity = ctx.Item as IEntity;
            ConfigureLocationEntity(entity);
            base.InternalCreate(ctx, executor);
        }


        private void ConfigureLocationEntity(IEntity entity)
        {
            LocationBase location = entity as LocationBase;
            if (entity is City )
            {
                LocationHelper.ApplyCountryState(entity as City);
            }
            else if (entity is State)
            {
                LocationHelper.ApplyCountry(entity as State);
            }
            else if (entity is Country)
            {
                LocationHelper.ApplyCountry(entity as Country);
            }
        }
    }
}
