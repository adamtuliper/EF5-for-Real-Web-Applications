using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityDemoSite.DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        T GetById(int id);

        //the following CRUD operations DO NOT send the updates to the database. Save() must be called when done.
        
        void Delete(T entity);
        IEnumerable<T> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Save();

    }
}
