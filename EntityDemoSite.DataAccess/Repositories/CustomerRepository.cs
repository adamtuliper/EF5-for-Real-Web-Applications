using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Validation;
using System.Diagnostics;
using EntityDemoSite.Domain;
using System.Data.Objects;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EntityDemoSite.DataAccess.Interfaces;
using System.Data.SqlClient;
using EntityDemoSite.Domain.Entities;

namespace EntityDemoSite.DataAccess.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {

        /// <summary>
        /// The parameterless constructor is used by ObjectDataSource (Asp.Net Web Forms)
        /// The repository will need to handle its own context lifetime.
        /// </summary>
        public CustomerRepository()
        {
            this.Context = new EntityContext();
            this.RequiresContextDisposal = true;
        }

        /// <summary>
        /// Note that by default Unity will create this concrete type even without a mapping defined.
        /// If you switch to a unit of work pattern, you would ENSURE your DI container does not instantiate this
        /// object and instead the UnitOfWork would pass this in and change this.RequiresContextDisposal=false since the UnitOfWork
        /// object would handle context lifetime.
        /// ENSURE this gets disposed!
        /// </summary>
        /// <param name="context"></param>
        public CustomerRepository(EntityContext context)
        {
            this.Context = context;
            this.RequiresContextDisposal = true;
        }

        /// <summary>
        /// Loads a customer by id. This will add it to the context for tracking as well.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer GetById(int id)
        {
            return this.Context.Database.SqlQuery<Customer>("Proc_GetCustomer @customerID", new SqlParameter("@customerID", id)).Single();
            //return this.Context.Customers.Include("Orders").Single(o => o.CustomerId == id);
        }

        public void Delete(Customer entity)
        {
            //If the entity isn't attached, attach it otherwise it won't be deleted.
            if (this.Context.Entry(entity).State == System.Data.EntityState.Detached)
            {
                this.Context.Customers.Attach(entity);
            }

            this.Context.Customers.Remove(entity);
        }

        /// <summary>
        /// Get every customer. Convert to array (or ToList()) to bypass deferred execution
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAll()
        {
            return this.Context.Customers.ToArray();
        }

        public IEnumerable<Customer> GetAllPaged(int startingRecord, int pageSize)
        {
            return Context.Customers
                 .Skip(startingRecord)
                 .Take(pageSize)
                 .ToList();

        }

        /// <summary>
        /// Gives a customer to the context, does NOT save it to the db. Save() must be called to commit changes.
        /// </summary>
        /// <param name="customer"></param>
        public void Create(Customer customer)
        {
            this.Context.Customers.Add(customer);
        }

        #region extended information debugging properties - for informational purposes only, not part of the demo or required
        private bool GetDirtyProperties(DbContext ctx, object entity)
        {
            ObjectStateEntry entry;
            if (
                ((IObjectContextAdapter)ctx).ObjectContext.ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
            {
                IEnumerable<string> changedNames = entry.GetModifiedProperties();

                foreach (var name in changedNames)
                {
                    Debug.WriteLine(name);
                }
            }
            return false;
        }


        private bool IsDirtyProperty(ObjectContext ctx, object entity, string propertyName)
        {
            ObjectStateEntry entry;
            if (ctx.ObjectStateManager.TryGetObjectStateEntry(entity, out entry))
            {
                int propIndex = this.GetPropertyIndex(entry, propertyName);

                if (propIndex != -1)
                {
                    var oldValue = entry.OriginalValues[propIndex];
                    var newValue = entry.CurrentValues[propIndex];

                    return !Equals(oldValue, newValue);
                }
                else
                {
                    throw new ArgumentException(String.Format("Cannot find original value for property '{0}' in entity entry '{1}'",
                            propertyName,
                            entry.EntitySet.ElementType.FullName));
                }
            }

            return false;
        }


        private int GetPropertyIndex(ObjectStateEntry entry, string propertyName)
        {
            OriginalValueRecord record = entry.GetUpdatableOriginalValues();

            for (int i = 0; i != record.FieldCount; i++)
            {
                FieldMetadata metaData = record.DataRecordInfo.FieldMetadata[i];
                if (metaData.FieldType.Name == propertyName)
                {
                    return metaData.Ordinal;
                }
            }

            return -1;
        }
        #endregion

        /// <summary>
        /// Saves a customer. This also ensures it's part of the context (a requirement for saving)
        /// </summary>
        /// <param name="customer"></param>
        public void Update(Customer customer)
        {
            //In case the new guy on the team sends in a detached entity.
            if (this.Context.Entry(customer).State == System.Data.EntityState.Detached)
            {
                this.Context.Entry(customer).State = System.Data.EntityState.Modified;
            }
            else
            {

                //In MVC we're overwriting the object's properties with a timestamp.
                //The timestamp though is treated by EF as a computed field and only the original valyeds
                this.Context.Entry(customer).Property(u => u.Timestamp).OriginalValue = customer.Timestamp;
            }
            Context.SaveChanges();

        }

        /// <summary>
        /// Note this does not execute right away - it is deferred execution. It can be dangerous without understanding context lifetime.
        /// This is for demo purposes only.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAllEnumerable()
        {
            return this.Context.Customers.AsEnumerable();
        }

        /// <summary>
        /// Also potentially dangerous as you don't know what the callers are doing with it. 
        /// An 'evil' caller could do dangerous things. However there is some flexibility provided here without haviong 30 methods for querying a customer
        /// This is up to you to decide if you should return IQueryable or not.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Customer> GetAllCustomersQueryable()
        {
            return this.Context.Customers;
        }

        public void Save()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Web forms search method
        /// </summary>
        /// <param name="sortExpression"></param>
        /// <param name="nameSearchString"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomersByName(string sortExpression, string nameSearchString)
        {
            //return this.Context.Customers.Where(d => string.IsNullOrEmpty(nameSearchString) || d.FirstName.Contains(nameSearchString)).ToArray();
            //Fails on sql CE because of lack of internal isnull.
            //Error: The specified argument value for the function is not valid. [ Argument # = 1,Name of function(if known) = isnull ]
            //workaround is to separate these calls. NOTE this syntax works fine on sqlexpress/ full sql server
            //return this.Context.Customers.Where(d => string.IsNullOrEmpty(nameSearchString) || d.FirstName.Contains(nameSearchString)).ToArray();

            if (string.IsNullOrEmpty(nameSearchString))
            {
                return this.Context.Customers.ToArray();
            }
            else
            {
                return this.Context.Customers.Where(d => d.FirstName.Contains(nameSearchString)).ToArray();
            }

        }


    }
}
