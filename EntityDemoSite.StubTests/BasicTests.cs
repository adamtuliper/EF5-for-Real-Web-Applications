using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EntityDemoSite.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityDemoSite.DataAccess;
using EntityDemoSite.DataAccess.Repositories;
using System.Diagnostics;
using System.Data.Entity;
using System.Data.SqlClient;
using EntityDemoSite.Domain;
using EntityDemoSite.DataAccess.Interfaces;
using System.Data.Entity.Validation;
using EntityDemoSite.Domain.Interfaces;
using EntityDemoSite.Domain.Validation;


namespace EntityDemoSite.StubTests
{
    [TestClass]
        public class BasicTests
    {
        EntityContext _dbContext;
        private ICustomerValidator _validator;
        [TestCleanup]
        public void CleanUp()
        {
            _dbContext.Database.Delete();
            _dbContext.Dispose();
        }

        [TestInitialize]
        public void Initialize()
        {

            _dbContext = new EntityContext();
            _validator= new CustomerValidator();
            //AlwaysRecreateDatabase 
            //CreateDatabaseOnlyIfNotExists 
            //RecreateDatabaseIfModelChanges 
            Database.SetInitializer(new CreateDatabaseIfNotExists<EntityContext>());
        }


        [TestMethod]
        public void RawSqlTests()
        {
            //Switch to the sql express connect string for this

            //return an entity - these WILL be tracked by the context!!
            var customers = _dbContext.Customers.SqlQuery("Select * From Customer");
            foreach (var bi in customers)
            {
                Debug.WriteLine(bi.CustomerId);
            }
            var customerIds = _dbContext.Database.SqlQuery<int>("Select CustomerId From dbo.Customer").ToList();

            //The same applies to stored procedures
            var customerFromProc1 = _dbContext.Database.SqlQuery<Customer>("Proc_GetCustomer @param1", new SqlParameter("param1", 1));
            Debug.WriteLine(customerFromProc1.Single().CustomerId);

            var customerFromProc2 = _dbContext.Customers.SqlQuery("Proc_GetCustomer @param1", new SqlParameter("param1", 1));
            Debug.WriteLine(customerFromProc2.Single().CustomerId);

        }

        [TestMethod]
        public void ConcurrencyTests()
        {
            //Get the same record but loaded from two separate contexts so the tracking won't overlap.

            ICustomerRepository repository = new CustomerRepository(_dbContext);
            var secondContext = new EntityContext();

            //add new customer to db.
            var newCustomer = CreateCustomer(repository);

            //load this customer from context 1
            var firstCustomer1 = repository.GetById(newCustomer.CustomerId);
            firstCustomer1.City = "newCity3";

            //load the same customer from context 2
            ICustomerRepository secondRepository = new CustomerRepository(secondContext);
            var firstCustomer2 = secondRepository.GetById(firstCustomer1.CustomerId);

            //Check the states
            Debug.WriteLine(_dbContext.Entry(newCustomer).State);
            Debug.WriteLine(_dbContext.Entry(firstCustomer1).State);
            Debug.WriteLine(secondContext.Entry(firstCustomer2).State);

            //Modify the second customer. We have the same TimeStamp as the original record
            //however the timestamp in the database would've been updated already.
            firstCustomer2.Address = "222 main st" + DateTime.Now.ToString();
            Debug.WriteLine(secondContext.Entry(firstCustomer2).State);

            //This should update that customer.
            repository.Update(firstCustomer1);
            repository.Save();

            //This should throw an exception.
            secondRepository.Update(firstCustomer2);
            secondRepository.Save();

            secondContext.Dispose();
        }

        private Customer CreateCustomer(ICustomerRepository repository)
        {
            Customer customer = new Customer(_validator) { FirstName = "Test", LastName = "User", City = "Bethlehem", State = "PA", Zip = "18018", Address = "555 main st" };
            repository.Create(customer);
            repository.Save();
            return customer;
        }


        [TestMethod]
        //[ExpectedException(
        public void TestNoKey()
        {
            ICustomerRepository repository = new CustomerRepository(_dbContext);
            var bip = new Customer(_validator);
            bip.CustomerId = 10;
            bip.CustomerId = 20;
            var customer = CreateCustomer(repository);
            _dbContext.Customers.Attach(bip);
            bip.CustomerId = 30;
            Debug.WriteLine(_dbContext.Entry(bip).State);
            var newContext = new EntityContext();
            var repository2 = new CustomerRepository(newContext);
            Debug.WriteLine(newContext.Entry(customer).State);
            customer.Address = "555";
            Debug.WriteLine(newContext.Entry(customer).State);
            //newContext.Entry(customer).State = System.Data.EntityState.Modified;
            Debug.WriteLine(newContext.Entry(customer).State);

            newContext.Customers.Attach(customer);
            customer.CustomerId = 0;
            newContext.SaveChanges();

            repository2.Update(customer);
            repository2.Save();
            customer.Address = "556";
            Debug.WriteLine(newContext.Entry(customer).State);
        }


        [TestMethod]
        public void ShowSmartFieldUpating()
        {
            ICustomerRepository repository = new CustomerRepository(_dbContext);
            var customers = repository.GetAll();
            Debug.Assert(customers.Count() == 0);

            Customer customer = new Customer(_validator) { FirstName = "Test", City = "bethlehem" };
            repository.Create(customer);
            repository.Save();
            customer.Address = "1 main st" + DateTime.Now.ToString();
            customer.City = customer.City;
            repository.Update(customer);
            repository.Save();


        }
        [TestMethod]
        public void TestCustomerLoading()
        {

            var customer = _dbContext.Customers.Where(o => o.CustomerId == 1);


            ICustomerRepository repository = new CustomerRepository(_dbContext);
            Debug.WriteLine(customer.Single().FirstName);

            var customers = repository.GetAll();
            //Debug.Assert(customers.Count == 0);
            Customer customer2 = new Customer(_validator) { FirstName = "Test", City = "bethlehem" };
            repository.Create(customer2);
            repository.Save();
        }


        [TestMethod]
        public void TestEnumerableOutsideContext()
        {
            IEnumerable<Customer> customers;
            using (EntityContext context = new EntityContext())
            {
                var repository = new CustomerRepository(context);
                customers = repository.GetAllEnumerable();
            }
            //note the The operation cannot be completed because the DbContext has been disposed.
            Debug.WriteLine(string.Format("Customer count:{0}", customers.Count()));
        }

        [TestMethod]
        public void TestEagerLoading()
        {
            using (EntityContext context = new EntityContext())
            {
                var repository = new OrderRepository(context);
                var orders = repository.GetAll();
                Debug.WriteLine(string.Format("Order count:{0}", orders.Count()));
                var firstOrder = orders.First();
                //firstOr
                //context.SaveChanges();
            }
        }

        /// <summary>
        /// This is indeed a dual operation test, as it ensures our load code/context works correctly 
        /// for saving and then loading. True, we could initialize the data in our database
        /// but for this small integration test I threw both in at once.
        /// </summary>
        [TestMethod]
        public void InsertAndLoadCustomer()
        {

            //Add to context just to show its state prior to save
            Customer customerNew = new Customer(_validator);
            customerNew.FirstName = "Mary";
            customerNew.LastName = "Doe";
            _dbContext.Customers.Attach(customerNew);
            //State is 'unchanged' no errors raised by simply adding it.
            Debug.WriteLine(_dbContext.Entry(customerNew).State);
            
            //Insert via repository
            ICustomerRepository repository = new CustomerRepository(_dbContext);
            //creates and saves it
            var customer = CreateCustomer(repository);
            //Get its state from the context - should be unchanged (as its saved)
            Debug.WriteLine(_dbContext.Entry(customer).State);
            //Saving again does nothing to the database (see profiler)
            repository.Update(customer);
            repository.Save();

            //update the first name.
            customer.FirstName = "John";
            //Get its state from the context
            Debug.WriteLine(_dbContext.Entry(customer).State);
            //saving again notice only the first name is updated.
            repository.Update(customer);
            repository.Save();


            Customer insertCustomer = new Customer(_validator);
            insertCustomer.FirstName = "Jane";
            insertCustomer.LastName = "Doe";
            insertCustomer.Address = "555 main st";
            insertCustomer.City = "Orlando";
            insertCustomer.Zip = "33400";
            insertCustomer.State = "FL";
            Debug.WriteLine(insertCustomer.CustomerId);
            _dbContext.Customers.Add(insertCustomer);
            Debug.WriteLine(_dbContext.Entry(insertCustomer).State);
            _dbContext.SaveChanges();
            //Note the primary key is automatically updated.
            Debug.WriteLine(insertCustomer.CustomerId);
            Debug.WriteLine(_dbContext.Entry(insertCustomer).State);
            
        }

        [TestMethod]
        public void InsertAndUpdateCustomer()
        {
            ICustomerRepository repository = new CustomerRepository(_dbContext);
            var customer = CreateCustomer(repository);

            Debug.WriteLine(_dbContext.Entry(customer).State);
            customer.City = "City " + DateTime.Now.ToString();
            Debug.WriteLine(_dbContext.Entry(customer).State);

            repository.Update(customer);
            repository.Save();
            Debug.WriteLine(_dbContext.Entry(customer).State);
        }

        [TestMethod]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void TestAddCustomerMissingRequiredFields()
        {

            using (EntityContext context = new EntityContext())
            {
                Customer customer = new Customer(_validator);

                ICustomerRepository repository = new CustomerRepository(context);
                repository.Create(customer);
                repository.Save();
            }
        }

    }
}
