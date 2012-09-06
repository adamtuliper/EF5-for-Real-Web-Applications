using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityDemoSite.DataAccess.Interfaces;
using EntityDemoSite.DataAccess.Repositories;

namespace EntityDemoSite.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        //private IContext _context;
        private readonly EntityContext _context;
        private ICustomerRepository _customerRepository;
        private IOrderRepository _orderRepository;
        private bool _disposed = false;

        //For next stage of unit of work implementation using IContext
        //public UnitOfWork(IContext context)
        //{
        //    _context = context;
        //}

        public UnitOfWork(EntityContext context)
        {
            _context = context;
        }
                
        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (this._customerRepository == null)
                {
                    this._customerRepository = new CustomerRepository(_context);
                }
                return _customerRepository;
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                if (this._orderRepository == null)
                {
                    this._orderRepository = new OrderRepository(_context);
                }
                return _orderRepository;
            }
        }

        public void Save()
        {
            //This allows us to not have to expose the Save method on our Context so users
            //can't circumvent the UnitOfWork's SaveChanges
            ((EntityContext)_context).SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    ((IDisposable)_context).Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
