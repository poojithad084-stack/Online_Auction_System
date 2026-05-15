using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auction.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly AuctionContext _context;

        public AuctionsController(AuctionContext context)
        {
            _context = context;
        }

        // GET: api/Auctions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Auctions>>> GetAuctions()
        {
            return await _context.Auctions.ToListAsync();
        }

        // GET: api/Auctions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Auctions>> GetAuction(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);

            if (auction == null)
            {
                return NotFound();
            }

            return auction;
        }

        // PUT: api/Auctions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuction(int id, Auctions auction)
        {
            if (id != auction.AuctionID)
            {
                return BadRequest();
            }

            _context.Entry(auction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuctionExists(id))
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

        // POST: api/Auctions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Auctions>> PostAuction(Auctions auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuction", new { id = auction.AuctionID }, auction);
        }

        // DELETE: api/Auctions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuctionExists(int id)
        {
            return _context.Auctions.Any(e => e.AuctionID == id);
        }

        

    }
}
