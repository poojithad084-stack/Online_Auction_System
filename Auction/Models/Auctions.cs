using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace Auction.Models
{
    public class Auctions

    {
        [Key]
        public int AuctionID { get; set; }
        public string? SellerID { get; set; }

        [Required]
        [Column(TypeName = "Varchar(40)")]
        public string? Category { get; set; }

        [Required]
        [Column(TypeName = "Varchar(40)")]
        public string? ProductName { get; set; }

        [Required]
        [Column(TypeName = "Varchar(100)")]
        public string? Description { get; set; }

        public decimal  BasePrice { get; set; }

       public decimal?currentBid { get; set; }

       [Column(TypeName = "Varchar(50)")]
        public string? ImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "Varchar(20)")]
        public string? Status { get; set; } 
        public  ICollection<Image>? Images { get; set; }

    }
}
