using Microsoft.Identity.Client;

namespace BidService.Models
{
    public class AuctionDTO
    {
        public string Status { get; set; }
        public DateTime EndTime { get; set; }
        public decimal CurrentBid { get; set; }
    }
}
