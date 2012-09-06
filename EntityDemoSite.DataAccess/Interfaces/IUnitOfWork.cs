using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityDemoSite.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository CustomerRepository { get; }
        IOrderRepository OrderRepository { get; }
        void Save();
    }
}
