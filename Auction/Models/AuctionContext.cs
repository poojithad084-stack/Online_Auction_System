using Microsoft.EntityFrameworkCore;
namespace Auction.Models
{
    public class AuctionContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=.\sqlexpress; initial Catalog = AuctionDB; integrated Security = true; trustservercertificate=true;");
        }
        public DbSet<Auctions> Auctions { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}
