
using GymManagement.BLL.Services.Classes;
using GymManagementSystem.BLL;
using GymManagementSystem.BLL.Services.Attachment;
using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL;
using GymManagementSystem.DAL.DataSeeding;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IMemberService, MemberService>(); // Allow DI For MemberService
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // Allow DI For GenericRepository With Open Generic Type
            builder.Services.AddScoped<ISessionRepository, SessionRepository>(); // Allow DI For SessionRepository

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));


            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow DI For GymDbContext With Options

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<GymDbContext>();





            var app = builder.Build();


            using var scop = app.Services.CreateScope();
            var _context = scop.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scop.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var UserManager = scop.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var RoleManager = scop.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var folderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");

            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                await _context.Database.MigrateAsync();
            }

            try
            {
                await GymDataSeeding.SeedAsync(_context, folderPath, logger);

                await IdentityDataSeeding.SeedIdentityDataAsync(UserManager, RoleManager, logger);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during data seeding.");
            }



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}


















