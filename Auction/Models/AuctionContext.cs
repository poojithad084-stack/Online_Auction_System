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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auctions>()
                .HasMany(a => a.Images)
                .WithOne(i => i.Auction)
                .HasForeignKey(i => i.AuctionID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Auctions>()
                .HasKey(a => a.AuctionID);

            modelBuilder.Entity<Auctions>()
                .Property(a => a.BasePrice)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Auctions>()
               .Property(a => a.currentBid)
               .HasColumnType("decimal(10,2)");
        }
    }
    }


