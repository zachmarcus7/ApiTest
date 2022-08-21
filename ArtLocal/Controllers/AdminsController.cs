using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtLocal.Data;
using ArtLocal.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace ArtLocal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ArtLocalDataContext _dbContext;

        public AdminsController(ArtLocalDataContext context)
        {
            _dbContext = context;
        }

        // GET: api/Admins
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmin()
        {
          if (_dbContext.Admins == null)
          {
              return NotFound();
          }
            return await _dbContext.Admins.ToListAsync();
        }

        // GET: api/Admins/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Admin>> GetAdmin(Guid id)
        {
          if (_dbContext.Admins == null)
          {
              return NotFound();
          }
            var admin = await _dbContext.Admins.FindAsync(id);

            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // POST: api/Admins
        // Note: This is left anonymous for testing purposes
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
            // generate a new GUID for the admin
            admin.AdminId = Guid.NewGuid();

            if (_dbContext.Admins == null)
            {
                return Problem("Entity set 'ArtLocalDataContext.Customers'  is null.");
            }

            _dbContext.Admins.Add(admin);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { id = admin.AdminId }, admin);
        }

    }
}
