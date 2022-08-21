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
    public class InvoicesController : ControllerBase
    {
        private readonly ArtLocalDataContext _dbContext;

        public InvoicesController(ArtLocalDataContext context)
        {
            _dbContext = context;
        }

        // GET: api/Invoices
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoice()
        {
            if (_dbContext.Invoice == null)
            {
                return NotFound();
            }

            return await _dbContext.Invoice.ToListAsync();
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            if (_dbContext.Invoice == null)
            {
                return NotFound();
            }

            var invoice = await _dbContext.Invoice.FindAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // POST: api/Invoices
        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            // generate a new GUID for the invoice
            invoice.InvoiceId = Guid.NewGuid();

            if (_dbContext.Invoice == null)
            {
                return Problem("Entity set 'ArtLocalDataContext.Invoice'  is null.");
            }

            _dbContext.Invoice.Add(invoice);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.InvoiceId }, invoice);
        }
        private bool InvoiceExists(Guid id)
        {
            return (_dbContext.Invoice?.Any(e => e.InvoiceId == id)).GetValueOrDefault();
        }
    }
}
