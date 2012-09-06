using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityDemoSite.Domain;
using EntityDemoSite.Domain.Entities;

namespace EntityDemoSite.DataAccess.Interfaces
{
    public interface IShipTypeRepository : IRepository<ShipType>
    {
        //Additional methods here to extend the interface. We add them here so we can fake them if need be.
    }
}
