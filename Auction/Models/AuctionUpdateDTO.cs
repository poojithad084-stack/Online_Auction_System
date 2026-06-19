using System.ComponentModel.DataAnnotations;

namespace Auction.Models
{
    public class AuctionUpdateDTO
    {
        public decimal currentBid { get; set; }

        public required string BuyerEmail { get; set; }
    }
}
