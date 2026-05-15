namespace BidService.Models
{
    public class Bid
    {
        public int BidId { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; } = DateTime.UtcNow;
        public int AuctionId { get; set; }
        public int BuyerId { get; set; }
    }
}
