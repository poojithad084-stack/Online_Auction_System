namespace Auction.Models
{
    public class AuctionDTO
    {
        public int AuctionID { get; set; }
        public string? AuctionName { get; set; }
        public string? Category { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
    }
}
