using System.ComponentModel.DataAnnotations.Schema;

namespace Auction.Models
{
    public class AuctionDTO
    {
        public int AuctionID { get; set; }
        public string? Category { get; set; } 
        public string? SellerID { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
    }
}
