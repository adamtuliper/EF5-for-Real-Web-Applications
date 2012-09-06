using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityDemoSite.Domain;
using EntityDemoSite.DataAccess;
using EntityDemoSite.Domain.Entities;
using EntityDemoSiteMVC.ViewModels.Order;
using EntityDemoSite.DataAccess.Repositories;
using EntityDemoSite.DataAccess.Interfaces;
using AutoMapper;


namespace EntityDemoSiteMVC.Controllers
{
    public class OrderController : Controller
    {

        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository repository)
        {
            _orderRepository = repository;
        }

        //
        // GET: /Order/

        public ViewResult Index()
        {
            //var orders = db.Orders.Include(o => o.Customer);
            //return View(orders.ToList());

            var model = _orderRepository.GetAll().Select(o => new OrderIndexViewModel()
                {
                    ActualShipDate = o.ActualShipDate,
                    EstimatedShipDate = o.EstimatedShipDate,
                    ShipName = o.ShipType.ShipName,
                    Name = string.Format("{0} {1}", o.Customer.FirstName, o.Customer.LastName),
                    OrderId = o.OrderId,
                    OrderTotal = o.OrderTotal
                }).ToList(); 

            return View(model);
        }


        // GET: /Order/Create

        public ActionResult Create()
        {

            OrderCreateViewModel model = new OrderCreateViewModel();
            model.Customers = GetAllCustomers();
            model.ShipTypes = GetAllShipTypes();
            model.EstimatedShipDate = DateTime.Now;

            //NO VIEWBAG: ViewBag.CustomerId = new SelectList(db.Customers, "CustomerId", "FirstName");
            return View(model);
        }

        private IEnumerable<SelectListItem> GetAllCustomers()
        {
            using (CustomerRepository repository = new CustomerRepository())
            {
                return repository.GetAll().Select(x => new SelectListItem
                {
                    Text = string.Format("{0} {1}", x.FirstName, x.LastName),
                    Value = x.CustomerId.ToString()
                }).ToList();
            }
        }

        private IEnumerable<SelectListItem> GetAllShipTypes()
        {
            using (var repository = new ShipTypeRepository())
            {
                return repository.GetAll().Select(x => new SelectListItem
                {
                    Text = x.ShipName,
                    Value = x.ShipTypeId.ToString()
                }).ToList();
            }
        }

        //
        // POST: /Order/Create

        [HttpPost]
        public ActionResult Create(OrderCreateViewModel order)
        {
            if (ModelState.IsValid)
            {
                //Get the order details to an Order Object.
                Order newOrder = Mapper.Map<OrderCreateViewModel, Order>(order);
                _orderRepository.Create(newOrder);
                _orderRepository.Save();
                return RedirectToAction("Index");
            }

            order.Customers = GetAllCustomers();
            order.ShipTypes = GetAllShipTypes();
            return View(order);
        }

        //
        // GET: /Order/Edit/5
        public ActionResult Edit(int id)
        {
            Order order = _orderRepository.GetById(id);
            OrderEditViewModel orderViewModel = Mapper.Map<Order, OrderEditViewModel>(order);
            orderViewModel.ShipTypes = GetAllShipTypes();

            return View(orderViewModel);
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        public ActionResult Edit(OrderEditViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                Order order = _orderRepository.GetById(orderViewModel.OrderId);

                Mapper.Map<OrderEditViewModel, Order>(orderViewModel, order);
                _orderRepository.Update(order);
                _orderRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                //need to rebind ship types (no viewstate!)
                orderViewModel.ShipTypes = GetAllShipTypes();

                return View(orderViewModel);
            }
        }

        //
        // GET: /Order/Delete/5
        public ActionResult Delete(int id)
        {
            Order order = _orderRepository.GetById(id);
            OrderEditViewModel orderViewModel = Mapper.Map<Order, OrderEditViewModel>(order);

            return View(orderViewModel);
        }

        //
        // POST: /Order/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _orderRepository.Delete(_orderRepository.GetById(id));
            _orderRepository.Save();
            return RedirectToAction("Index");
        }

    }
}