using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using EntityDemoSite.Domain;
using EntityDemoSite.Domain.Entities;

namespace EntityDemoSite.Domain.Interfaces
{
    /// <summary>
    /// This allows the final implementation (ie web project) use a context 
    /// without a specific reference to the entity framework. 
    /// </summary>
    public interface IContext : IDisposable
    {
        DbEntityEntry Entry<TEntity>(TEntity entity) where TEntity : class;
        IDbSet<Customer> Customers { get; set; }
        IDbSet<Order> Orders { get; set; }
    }
}
