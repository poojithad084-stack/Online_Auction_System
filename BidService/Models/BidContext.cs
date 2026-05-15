using Microsoft.EntityFrameworkCore;

namespace BidService.Models
{
    public class BidContext:DbContext
    {
        public DbSet<Bid> Bids { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=.\sqlexpress;initial catalog=Auction;integrated security=true;trustservercertificate=true");
        }
    }
}
