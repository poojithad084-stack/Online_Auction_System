using BidService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class BidController : ControllerBase
{
    private readonly BidContext _context;
    public BidController(BidContext context)
    {
        _context = context;
    }

    private readonly IHttpClientFactory _httpClientFactory;

    public BidController(BidContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
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

    // POST: api/Bid
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Bid>> PostBid(BidDTO bidDTO)
    {
        var buyerId = int.Parse(User.FindFirst("userId").Value);

        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync(
            $"http://localhost:5001/api/auction/{bidDTO.AuctionId}");

        if (!response.IsSuccessStatusCode)
        {
            return BadRequest("Auction not found");
        }

        var auction = await response.Content.ReadFromJsonAsync<AuctionDTO>();

        if (auction.Status != "ACTIVE")
            return BadRequest("Auction is not active");

        if (auction.EndTime <= DateTime.UtcNow)
            return BadRequest("Auction has ended");

        if (bidDTO.Amount <= auction.CurrentBid)
            return BadRequest("Bid must be higher than current bid");

        var updateResponse = await client.PostAsJsonAsync(
            "http://localhost:5001/api/auction/update-bid",
            new
            {
                AuctionId = bidDTO.AuctionId,
                BidAmount = bidDTO.Amount,
                BuyerId = buyerId
            });

        if (!updateResponse.IsSuccessStatusCode)
        {
            return BadRequest("Someone already placed a higher bid");
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
