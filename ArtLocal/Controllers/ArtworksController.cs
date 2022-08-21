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
    public class ArtworksController : ControllerBase
    {
        private readonly ArtLocalDataContext _dbContext;

        public ArtworksController(ArtLocalDataContext context)
        {
            _dbContext = context;
        }

        // GET: api/Artworks
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Artwork>>> GetArtwork()
        {
          if (_dbContext.Artwork == null)
          {
              return NotFound();
          }
            return await _dbContext.Artwork.ToListAsync();
        }

        // GET: api/Artworks/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Artwork>> GetArtwork(Guid id)
        {
          if (_dbContext.Artwork == null)
          {
              return NotFound();
          }
            var artwork = await _dbContext.Artwork.FindAsync(id);

            if (artwork == null)
            {
                return NotFound();
            }

            return artwork;
        }

        // POST: api/Artworks
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Artwork>> PostArtwork(Artwork artwork)
        {
            // generate a new GUID for the artwork
            artwork.ArtworkId = Guid.NewGuid();

            if (_dbContext.Artwork == null)
            {
                return Problem("Entity set 'ArtLocalDataContext.Artwork'  is null.");
            }

            _dbContext.Artwork.Add(artwork);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetArtwork", new { id = artwork.ArtworkId }, artwork);
        }

        // PUT: api/Artworks/5
        // Here, the customer can update the artwork only when buying one
        [HttpPut("{id}")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> PutArtist(Guid id, Artwork artwork)
        {
            if (id != artwork.ArtworkId)
            {
                return BadRequest();
            }

            try
            {
                // only allow the sold property to be modified 
                _dbContext.Entry(artwork).Property(x => x.Sold).IsModified = true;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtworkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ArtworkExists(Guid id)
        {
            return (_dbContext.Artwork?.Any(e => e.ArtworkId == id)).GetValueOrDefault();
        }
    }
}
