using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using EntityDemoSite.DataAccess.Interfaces;
using EntityDemoSite.Domain.Entities;

namespace EntityDemoSite.DataAccess.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {

        //Note that by default Unity will create this concrete type even without a mapping defined.
        //if you switch to a unit of work pattern, you would ENSURE your DI container does not instantiate this
        //object and instead the UnitOfWork would pass this in and change this.RequiresContextDisposal=false since the UnitOfWork
        //object would handle context lifetime.
        //ENSURE this gets disposed!
         public OrderRepository(EntityContext context)
        {
            Context = context;
            RequiresContextDisposal = true;
        }

        public Order GetById(int id)
        {
            return Context.Orders.Single(o => o.OrderId == id);
        }

        public void Delete(Order order)
        {
            Context.Orders.Remove(order);
        }

        public IEnumerable<Order> GetAll()
        {
            return Context.Orders.Include(o=>o.Customer).Include(o=>o.ShipType).ToList();
        }

        public void Create(Order entity)
        {
            Context.Orders.Add(entity);
        }

        public void Update(Order order)
        {
            //In case the new guy on the team sends in a detached entity.
            if (this.Context.Entry(order).State == System.Data.EntityState.Detached)
            {
                this.Context.Entry(order).State = System.Data.EntityState.Modified;
            }
            else
            {

                //In MVC we're overwriting the object's properties with a timestamp.
                //The timestamp though is treated by EF as a computed field and only the original valyeds
                this.Context.Entry(order).Property(u => u.Timestamp).OriginalValue = order.Timestamp;
            }
        }
        
        public IEnumerable<Order> GetAllWithCustomerInformation()
        {
            //To include the items NOW and not rely on lazy loading
            return Context.Orders.Include(o => o.Customer).ToList();
        }

        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
