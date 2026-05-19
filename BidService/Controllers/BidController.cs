using BidService.Models;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BidController : ControllerBase
{
    private readonly BidContext _context;
    //public BidController(BidContext context)
    //{
    //    _context = context;
    //}

    private readonly IHttpClientFactory _httpClientFactory;

    private readonly IConfiguration _configuration;

    public BidController(BidContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    // GET: api/Bid
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bid>>> GetBid()
    {
        return await _context.Bids.ToListAsync();
    }

    // GET: api/Bid/5
    [HttpGet("{bidid}")]
    public async Task<ActionResult<Bid>> GetBid(int bidid)
    {
        var bid = await _context.Bids.FindAsync(bidid);

        if (bid == null)
        {
            return NotFound();
        }

        return bid;
    }

    // PUT: api/Bid/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{bidid}")]
    public async Task<IActionResult> PutBid(int? bidid, Bid bid)
    {
        if (bidid != bid.BidId)
        {
            return BadRequest();
        }

        _context.Entry(bid).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BidExists(bidid))
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

    //this code is for testing with out jwt
    //[HttpPost]
    //[AllowAnonymous]
    //public async Task<ActionResult<Bid>> PostBid(int buyerId,BidDTO bidDTO)
    //{
    //    //var buyerId = int.Parse(User.FindFirst("userId").Value);
    //    int rows = 0;
    //    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
    //    {
    //        await connection.OpenAsync();
    //        rows = await connection.ExecuteAsync(
    //            "UpdateAuctionItem",
    //            new
    //            {
    //                AuctionID = bidDTO.AuctionId,
    //                CurrentBid = bidDTO.Amount,
    //                BidderId = buyerId
    //            },
    //            commandType: CommandType.StoredProcedure
    //            );
    //    }

    //    if (rows == 0)
    //    {
    //        return BadRequest("Bid too low or auction ended");
    //    }

    //    var bid = new Bid
    //    {
    //        Amount = bidDTO.Amount,
    //        BidTime = DateTime.UtcNow,
    //        AuctionId = bidDTO.AuctionId,
    //        BuyerId = buyerId
    //    };

    //    _context.Bids.Add(bid);
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction("GetBid", new { id = bid.BidId }, bid);
    //}

    //this is actual code to update the bid
    // POST: api/Bid
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Bid>> PostBid(BidDTO bidDTO)
    {
        var buyerId = int.Parse(User.FindFirst("userId").Value);
        int rows = 0;
        using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            await connection.OpenAsync();
            rows = await connection.ExecuteAsync(
                "UpdateAuctionItem",
                new
                {
                    AuctionID = bidDTO.AuctionId,
                    CurrentBid = bidDTO.Amount,
                    BidderId = buyerId
                },
                commandType: CommandType.StoredProcedure
                );
        }

        if (rows == 0)
        {
            return BadRequest("Bid too low or auction ended");
        }

        var bid = new Bid
        {
            Amount = bidDTO.Amount,
            BidTime = DateTime.UtcNow,
            AuctionId = bidDTO.AuctionId,
            BuyerId = buyerId
        };

        _context.Bids.Add(bid);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetBid", new { id = bid.BidId }, bid);
    }

    // DELETE: api/Bid/5
    [HttpDelete("{bidid}")]
    public async Task<IActionResult> DeleteBid(int? bidid)
    {
        var bid = await _context.Bids.FindAsync(bidid);
        if (bid == null)
        {
            return NotFound();
        }

        _context.Bids.Remove(bid);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool BidExists(int? bidid)
    {
        return _context.Bids.Any(e => e.BidId == bidid);
    }
}
