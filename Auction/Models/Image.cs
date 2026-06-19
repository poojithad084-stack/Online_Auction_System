using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Auction.Models
{
    public class Image
    {
        [Key]
        public int ImageID { get; set; }

        [Required]
        [Column(TypeName="Varchar(40)")]
        public string? ImageName { get; set; }

        [ForeignKey("Auction")]
        public int AuctionID { get; set; }

        [JsonIgnore]
        public Auctions? Auction { get; set; }

    }
}
