using Microsoft.EntityFrameworkCore;
namespace BidService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //var connectionString = builder.Configuration.GetConnectionString("BidContext") ?? throw new InvalidOperationException("Connection string 'BidContext' not found.");

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();

            builder.Services.AddDbContext<Models.BidContext>(options =>
                            options.UseSqlServer(
                                builder.Configuration.GetConnectionString("DefaultConnection")
                            )
                        );

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
