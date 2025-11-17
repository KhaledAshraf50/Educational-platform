using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace Luno_platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<LunoDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

);
            builder.Services.AddScoped(typeof(I_BaseRepository<>), typeof(BaseRepository<>));

            builder.Services.AddScoped(typeof(I_BaseService<>), typeof(BaseService<>));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Homepage}/{action=mainpage}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
