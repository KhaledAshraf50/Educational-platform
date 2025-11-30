using Luno_platform.Models;
using Luno_platform.Repository;
using Luno_platform.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Luno_platform.Service.PaymentService;

namespace Luno_platform
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =============================
            // Add services to the container
            // =============================
            builder.Services.AddControllersWithViews().AddViewOptions(options =>
            {
                options.HtmlHelperOptions.ClientValidationEnabled = true;
            });

            // -----------------------------
            // DbContext
            // -----------------------------
            builder.Services.AddDbContext<LunoDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // -----------------------------
            // Identity
            // -----------------------------
            builder.Services.AddIdentity<Users, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<LunoDBContext>()
            .AddDefaultTokenProviders();

            // -----------------------------
            // Cookie Authentication
            // -----------------------------
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            // -----------------------------
            // Scoped Services & Repositories
            // -----------------------------
            builder.Services.AddScoped(typeof(I_BaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped(typeof(I_BaseService<>), typeof(BaseService<>));

            builder.Services.AddScoped<I_homepage_serves, Homepage_Service>();
            builder.Services.AddScoped<I_instructor_services, instructor_services>();
            builder.Services.AddScoped<I_instructor_repo, instructor_repo>();

            builder.Services.AddScoped<IstudentRepo, studentRepo>();
            builder.Services.AddScoped<IstudentService, studentService>();

            builder.Services.AddScoped<Icourses_repo, courses_repo>();
            builder.Services.AddScoped<Icourses_service, courses_service>();

            builder.Services.AddScoped<IExam_repo, Exam_repo>();
            builder.Services.AddScoped<IExam_service, Exam_service>();
            builder.Services.AddScoped<ITask_repo, Task_repo>();
            builder.Services.AddScoped<ITask_service, Task_service>();

            builder.Services.AddScoped<IParentService, ParentService>();
            builder.Services.AddScoped<IParentRepo, ParentRepo>();
            builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
            builder.Services.AddScoped<studentRepo>();
            builder.Services.AddScoped<IstudentRepo, studentRepo>();

            builder.Services.AddScoped<IPaymentService, PaymentService>();

            
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();



            builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
            builder.Services.AddScoped<IReportsService, ReportsService>();
            builder.Services.AddScoped<SettingsService>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IClassRepo, ClassRepo>();


            // =============================
            // Build the app
            // =============================
            var app = builder.Build();

            // =============================
            // ✅ إنشاء Roles تلقائياً عند تشغيل التطبيق
            // =============================
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                    await EnsureRolesAsync(roleManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "حدث خطأ أثناء إنشاء الـ Roles");
                }
            }

            // =============================
            // Configure middleware
            // =============================
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // =============================
            // Route configuration
            // =============================
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Homepage}/{action=mainpage}/{id?}"
            );

            // =============================
            // Run app
            // =============================
            app.Run();
        }

        // =============================
        // ✅ Method لإنشاء الـ Roles
        // =============================
        private static async Task EnsureRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            string[] roleNames = { "student", "instructor", "parent", "admin" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }
        }
    }
}