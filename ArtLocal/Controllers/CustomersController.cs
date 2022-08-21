using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtLocal.Data;
using ArtLocal.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArtLocal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ArtLocalDataContext _dbContext;

        public CustomersController(ArtLocalDataContext context)
        {
            _dbContext = context;
        }

        // Get all Customers
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            if (_dbContext.Customers == null)
            {
                return NotFound();
            }
            return await _dbContext.Customers.ToListAsync();
        }

        // Get a specific Customer
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid id)
        {
            if (_dbContext.Customers == null)
            {
                return NotFound();
            }
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // Create a new Customer
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            // generate a new GUID for the customer
            customer.CustomerId = Guid.NewGuid();

            if (_dbContext.Customers == null)
            {
                return Problem("Entity set 'ArtLocalDataContext.Customers'  is null.");
            }

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        private bool CustomerExists(Guid id)
        {
            return (_dbContext.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
