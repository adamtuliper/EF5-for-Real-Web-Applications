using System.Web.Mvc;
using EntityDemoSite.Domain.Entities;
using EntityDemoSiteMVC.ViewModels.Customer;
using Gecko.Framework.Mvc.ActionFilters;
using EntityDemoSite.DataAccess.Interfaces;
using System.Linq;

namespace EntityDemoSiteMVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _repository;

        /// <summary>
        /// Note we don't have to manage the customer repository at all.
        /// </summary>
        /// <param name="repository"></param>
        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /Customer/
        public ViewResult Index()
        {
            var customers = _repository.GetAll();

            //Simple mapping to view models
            var viewModels = customers.Select(c => new CustomerIndexViewModel
                                                    {
                                                        CustomerId = c.CustomerId,
                                                        Address = c.Address,
                                                        City = c.City,
                                                        State = c.State,
                                                        FirstName = c.FirstName,
                                                        LastName = c.LastName
                                                    }).ToArray(); 
            return View(viewModels);

        }

        //
        // GET: /Customer/Details/5
        public ViewResult Details(int id)
        {
            return View(_repository.GetById(id));
        }

        //
        // GET: /Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerCreateViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer();

                customer.Address = customerViewModel.Address;
                customer.City = customerViewModel.City;
                customer.FirstName = customerViewModel.FirstName;
                customer.LastName = customerViewModel.LastName;
                customer.State = customerViewModel.State;
                customer.Zip = customerViewModel.Zip;

                _repository.Create(customer);
                _repository.Save();
                return RedirectToAction("Edit", new { id = customer.CustomerId });
            }
            return View(customerViewModel);
        }

        //
        // GET: /Customer/Edit/5
        [AutoMap(typeof(Customer), typeof(CustomerEditViewModel))]
        public ActionResult Edit(int id)
        {
            var customer = _repository.GetById(id);
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(CustomerEditViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                //Load our entity, copy ViewModel properties into it.
                Customer customer = _repository.GetById(customerViewModel.CustomerId);

                customer.Address = customerViewModel.Address;
                customer.City = customerViewModel.City;
                customer.CustomerId = customerViewModel.CustomerId;
                customer.FirstName = customerViewModel.FirstName;
                customer.LastName = customerViewModel.LastName;
                customer.State = customerViewModel.State;
                customer.Zip = customerViewModel.Zip;
                customer.Timestamp = customerViewModel.Timestamp;

                //Shebang
                _repository.Update(customer);
                _repository.Save();

                return RedirectToAction("Index", new { id = customer.CustomerId });
            }
            else
            {
                //some validation error. let the user correct.
                return View(customerViewModel);
            }
        }

        //
        // GET: /Customer/Delete/5
        public ActionResult Delete(int id)
        {
            //ICustomerRepository repository = new CustomerRepository(db);
            return View(_repository.GetById(id));
        }

        //
        // POST: /Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            //ICustomerRepository repository = new CustomerRepository(db);
            Customer customer = _repository.GetById(id);
            _repository.Delete(customer);
            _repository.Save();
            return RedirectToAction("Index");
        }
    }
}


