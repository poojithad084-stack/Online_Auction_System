using Microsoft.EntityFrameworkCore;

namespace CategoryService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Models.CategoryContext>((options) => options.UseSqlServer(builder.Configuration.GetConnectionString("CategoryDb")));

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
