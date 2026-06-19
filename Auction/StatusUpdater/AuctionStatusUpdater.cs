using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Auction.Models;

public class AuctionStatusUpdater : BackgroundService
{

    private readonly IServiceProvider _serviceProvider;


    public AuctionStatusUpdater(IServiceProvider serviceProvider)

    {

        _serviceProvider = serviceProvider;

    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)

    {

        while (!stoppingToken.IsCancellationRequested)

        {

            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AuctionContext>();

            var now = DateTime.UtcNow;


            var auctions = await context.Auctions

                                        .Where(a => a.Status != "deleted" && a.Status != "completed")

                                        .ToListAsync(stoppingToken);


            foreach (var auction in auctions)

            {

                if (now < auction.StartDate)

                    auction.Status = "upcoming";

                else if (now >= auction.StartDate && now <= auction.EndDate)

                    auction.Status = "active";

                else if (now > auction.EndDate)

                    auction.Status = "completed";

            }


            await context.SaveChangesAsync(stoppingToken);



            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        }

    }

}