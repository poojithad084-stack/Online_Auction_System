using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Migrations;
using Auction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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

            var auctions = await _context.Auctions

                                         .Where(a => a.Status != "deleted" && a.Status != "completed")

                                         .Include(a => a.Images)
                                         .ToListAsync();


            return auctions;

        }


        // GET: api/Auctions/5
        [HttpGet("{id}")]

        public async Task<ActionResult<Auctions>> GetAuctionById(int id)

        {

            var auction = await _context.Auctions.Include(a => a.Images)

                            .FirstOrDefaultAsync(a => a.AuctionID == id && a.Status != "deleted" && a.Status != "completed");


            if (auction == null)

            {

                return NotFound();

            }


            return Ok(auction);

        }

        [HttpGet("expired")]

        public async Task<ActionResult<IEnumerable<Auctions>>> GetAuctionsExpired()

        {

            var auctions = await _context.Auctions

                                        .Where(a => a.Status == "completed")

                                       .Select(a => new AuctionDTO

                                       {

                                           AuctionID = a.AuctionID,

                                           SellerID = a.SellerID

                                       })

                                        .ToListAsync();

            return Ok(auctions);

        }

       


        // GET: api/Auctions/5
        [HttpGet("BasePrice/{BasePrice}")]


        public async Task<ActionResult<IEnumerable<Auctions>>> GetAuctionbyPrice(decimal price)

        {

            var auction = await _context.Auctions.Include(a => a.Images).

                Where(a => a.BasePrice >= price && a.Status != "deleted" && a.Status != "completed").ToListAsync();


            if (auction == null)

            {

                return NotFound();

            }


            return Ok(auction);

        }


        // GET: api/Auctions/5
        [HttpGet("ProductName/{name}")]

        public async Task<ActionResult<IEnumerable<Auctions>>> GetAuctionByProduct(string name)

        {

            var auction = await _context.Auctions.Include(a => a.Images).

                Where(a => a.ProductName == name && a.Status != "deleted" && a.Status != "completed").ToListAsync();


            if (auction == null)

            {

                return NotFound();

            }


            return Ok(auction);

        }


        // GET: api/Auctions/5
        [HttpGet("Category/{Category}")]

        public async Task<ActionResult<IEnumerable<Auctions>>> GetAuctionByCategory(string category)

        {

            var auction = await _context.Auctions.Include(a => a.Images).

                Where(a => a.Category == category && a.Status != "deleted" && a.Status != "completed").ToListAsync();


            if (auction == null)

            {

                return NotFound();

            }


            return Ok(auction);

        }
        [HttpGet("email/{SellerId}")]

        public ActionResult<IEnumerable<Auctions>> GetAuctionBySellerEmail(string SellerId)

        {

            var auction =  _context.Auctions.Include(a => a.Images).

                Where(a => a.SellerID.ToLower() == SellerId.ToLower()).ToList();


            return Ok(auction);

        }
        // PUT: api/Auctions/5// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]

        public async Task<ActionResult> PutAuction(int id, AuctionDTO auctionDto)

        {

            if (id != auctionDto.AuctionID)

            {

                return BadRequest();

            }


            try
            {

                var auction = await _context.Auctions.FindAsync(id);

                if (auction == null) return NotFound();


                auction.Category = auctionDto.Category;

                auction.ProductName = auctionDto.ProductName;

                auction.Description = auctionDto.Description;

                auction.BasePrice = auctionDto.BasePrice;

                auction.ImageUrl = auctionDto.ImageUrl;

                auction.StartDate = auctionDto.StartDate;

                auction.EndDate = auctionDto.EndDate;


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


            return Ok();

        }


        // POST: api/Auctions// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754       
        
       [HttpPost]

        public async Task<ActionResult<Auctions>> PostAuction(AuctionDTO auctionDto)

        {

            var emailClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;


            if (string.IsNullOrEmpty(emailClaim))

            {

                return Unauthorized(new { message = "Invalid token" });

            }

            var auction = new Auctions
            {

                SellerID = emailClaim,

                //SellerID = auctionDto.SellerID,
               Category = auctionDto.Category,

                ProductName = auctionDto.ProductName,

                Description = auctionDto.Description,

                BasePrice = auctionDto.BasePrice,

                ImageUrl = auctionDto.ImageUrl,

                StartDate = auctionDto.StartDate,

                EndDate = auctionDto.EndDate,

            };

            await _context.Auctions.AddAsync(auction);

            await _context.SaveChangesAsync();



            return CreatedAtAction(nameof(GetAuctionById), new { id = auction.AuctionID }, auction);

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


            auction.Status = "deleted";

            await _context.SaveChangesAsync();


            return Ok();

        }



        private bool AuctionExists(int id)

        {

            return _context.Auctions.Any(e => e.AuctionID == id);

        }


        [HttpPatch("{id}")]

        public async Task<IActionResult> PatchAuction(int id, [FromBody] AuctionUpdateDTO auctionUpdateDTO)

        {

            var auction = _context.Auctions.FirstOrDefault(e => e.AuctionID == id);


            if (auction == null)

            {

                return NotFound();

            }

            if (auction.EndDate < DateTime.Now || auction.Status == "completed" || auction.Status == "deleted")

            {

                return BadRequest("Auction Expired");

            }

            if (auction.BasePrice > auctionUpdateDTO.currentBid)

            {

                return BadRequest("Price is Too Low");

            }

            if (auction.SellerID == auctionUpdateDTO.BuyerEmail)

            {

                return BadRequest("Seller cannot Bid his Auction");

            }


            auction.currentBid = auctionUpdateDTO.currentBid;


            await _context.SaveChangesAsync();


            return Ok(auction);

        }



    }

}