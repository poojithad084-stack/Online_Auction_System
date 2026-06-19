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
    public class ImagesController : ControllerBase
    {
        private readonly AuctionContext _context;

        public ImagesController(AuctionContext context)
        {
            _context = context;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetImages()
        {
            return await _context.Images.ToListAsync();
        }

        // GET: api/Images/5
        [HttpGet("{auctionID}")]
        public async Task<ActionResult<Image>> GetImage(int auctionID)
        {
            var images =  await _context.Images.Where(i=>i.AuctionID == auctionID).ToListAsync();

            if (images == null)
            {
                return NotFound();
            }

            return Ok(images);
        }

        // PUT: api/Images/{auctionId}/{imageId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{auctionId}/{imageId}")]
        public async Task<IActionResult> UpdateImage(int auctionId, int imageId, Image updatedImage)
        {
            if (imageId != updatedImage.ImageID || auctionId != updatedImage.AuctionID)
            {
                return BadRequest("AuctionId or ImageId mismatch.");
            }

            var image = await _context.Images.FirstOrDefaultAsync(i => i.ImageID == imageId && i.AuctionID == auctionId);
            if (image == null)
            {
                return NotFound("Image not found for this auction.");
            }

            image.ImageName = updatedImage.ImageName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Images.AnyAsync(i => i.ImageID == imageId && i.AuctionID == auctionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Images/{auctionId}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{auctionID}")]
        public async Task<ActionResult<Image>> PostImage(int auctionID, Image image)
        {
            var auction = await _context.Auctions.FindAsync(auctionID);
            if(auction == null)
            
                return NotFound("Auction not found");
            image.AuctionID = auction.AuctionID;
            await _context.Images.AddAsync(image);
            await  _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImage), new { auctionID }, image);
        }

        // DELETE: api/Images/{auctionId}/{imageId}
        [HttpDelete("{auctionID}/{imageID}")]
        public async Task<IActionResult> DeleteImage(int auctionID, int imageID)
        {
            var image = await _context.Images.FirstOrDefaultAsync(i => i.ImageID == imageID && i.AuctionID == auctionID);           
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
             await _context.SaveChangesAsync();

            return Ok();
        }


        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.ImageID == id);
        }


    }
}
