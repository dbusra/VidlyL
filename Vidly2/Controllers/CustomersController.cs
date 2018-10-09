﻿using System;
using System.Collections.Generic;
using System.Data.Entity; // Include method's library.
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly2.Models;
using Vidly2.ViewModels;

namespace Vidly2.Controllers
{
    public class CustomersController : Controller
    {

        private ApplicationDbContext _context;

        public CustomersController()
        {
                _context = new ApplicationDbContext(); 
        }
        //DbContext object is a disposable object, so we need to sispose it properly 
        
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();

        }
        public ActionResult New()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new NewCustomerViewModel
            {
                MembershipTypes = membershipTypes
            };
            return View(viewModel);  // future we implement editing a customer, so we need to pass a customer object to this view in that time because of this we create view model 
        }

        // GET: Customers
        public ViewResult Index()
         {
             /* when executed in below statement entity framework will not query the database. this is called DEFERRED EXECTION.
              Queries executed when we iterate over this customers(var customers) object. We can immediately execute this query by calling the ToList() method */
             var customers = _context.Customers.Include(c => c.MembershipType).ToList(); // this Customers property is a DbSet defined in our DBContext, we can get all customers in the database.
             return View(customers);

        }
        //public IEnumerable<Customer> GetCustomers()
        //{
        //    var customers = new List<Customer>
        //    {
        //        new Customer {Id = 1,
        //            Name = "John Smith" },

        //        new Customer{Id = 2,
        //            Name = "Mary Williams" }
        //    };
        //    return customers;
        //} 


        //[Route("customers/details/{id}")]

        public ActionResult Details(int id)  
        {
            /* SingleOrDefault() returns record if there is some record otherwise throws exception
             * FirstOrDefault()  returns record if there is some record otherwise returns null. */
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);//Take membership types and in with these id's compare customers table membershipId when find them equal return just that MembershipType.

            if (customer == null)
            {
                return HttpNotFound(); // 404 error
            }
            return View(customer);
            
        }
      
    }
}

// when we add some records to tables directly, -because we wouldnt use migration, only migrations deployed- records won't be deployed to target database