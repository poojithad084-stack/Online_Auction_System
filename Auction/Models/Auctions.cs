using System.ComponentModel.DataAnnotations;

namespace Auction.Models
{
    public class Auctions

    {
        [Key]
        public int AuctionID { get; set; }
        public string? AuctionName { get; set; }
        public int SellerID { get; set; }
        public string? Category { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal  Price { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now;
        public  ICollection<Image>? Images { get; set; }

    }
}
