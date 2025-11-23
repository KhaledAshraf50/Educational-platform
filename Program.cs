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
            //_______________________________________________________
            builder.Services.AddScoped(typeof(I_BaseRepository<>), typeof(BaseRepository<>));

            builder.Services.AddScoped(typeof(I_BaseService<>), typeof(BaseService<>));


            builder.Services.AddScoped<I_homepage_serves,Homepage_Service>();
            builder.Services.AddScoped<I_instructor_services,instructor_services>();
            //builder.Services.AddScoped<I_instructor_repo,instructor_services>();
            builder.Services.AddScoped<I_instructor_repo,instructor_repo>();

            builder.Services.AddScoped<IstudentRepo, studentRepo>();
            builder.Services.AddScoped<IstudentService, studentService>();

            builder.Services.AddScoped<Icourses_repo, courses_repo>();
            builder.Services.AddScoped<Icourses_service, courses_service>();
            builder.Services.AddScoped<IExam_repo, Exam_repo>();
            builder.Services.AddScoped<IExam_service, Exam_service>();







            //_______________________________________________________


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
