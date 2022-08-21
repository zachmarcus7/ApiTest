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
    public class ArtistsController : ControllerBase
    {
        private readonly ArtLocalDataContext _dbContext;

        public ArtistsController(ArtLocalDataContext context)
        {
            _dbContext = context;
        }

        // GET: api/Artists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
          if (_dbContext.Artists == null)
          {
              return NotFound();
          }
            return await _dbContext.Artists.ToListAsync();
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetArtist(Guid id)
        {
          if (_dbContext.Artists == null)
          {
              return NotFound();
          }
            var artist = await _dbContext.Artists.FindAsync(id);

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }

        // POST: api/Artists
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Artist>> PostArtist(Artist artist)
        {
            // generate a new GUID for the artist
            artist.ArtistId = Guid.NewGuid();

            if (_dbContext.Artists == null)
            {
                return Problem("Entity set 'ArtLocalDataContext.Artists'  is null.");
            }
            _dbContext.Artists.Add(artist);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetArtist", new { id = artist.ArtistId }, artist);
        }

        // PUT: api/Artists/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutArtist(Guid id, Artist artist)
        {
            if (id != artist.ArtistId)
            {
                return BadRequest();
            }

            _dbContext.Entry(artist).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
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

        private bool ArtistExists(Guid id)
        {
            return (_dbContext.Artists?.Any(e => e.ArtistId == id)).GetValueOrDefault();
        }
    }
}
