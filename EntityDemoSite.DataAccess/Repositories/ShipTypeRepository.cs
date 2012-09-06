using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityDemoSite.DataAccess.Interfaces;
using EntityDemoSite.Domain;
using EntityDemoSite.Domain.Entities;

namespace EntityDemoSite.DataAccess.Repositories
{
    public class ShipTypeRepository : BaseRepository, IShipTypeRepository, IDisposable
    {
        /// <summary>
        /// The parameterless constructor is used by ObjectDataSource (Asp.Net Web Forms)
        /// </summary>
        public ShipTypeRepository()
        {
            this.Context = new EntityContext();
            this.RequiresContextDisposal = true;
        }

        //Note that by default Unity will create this concrete type even without a mapping defined.
        public ShipTypeRepository(EntityContext context)
        {
            this.Context = context;
        }

        public ShipType GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(ShipType entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ShipType> GetAll()
        {
            return this.Context.ShipTypes.ToList();
        }

        public void Create(ShipType entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ShipType entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            this.Context.SaveChanges();
        }
    }
}
