using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MedicalAppointmentsSystem.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using MedicalAppointmentsSystem.Configurations;
using MedicalAppointmentsSystem.Services.MailService;
using MailKit;
using MedicalAppointmentsSystem.Services.ImageService;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace MedicalAppointmentsSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<IdentityUser,  IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            builder.Services.AddRazorPages();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));


            /*============================== SERVICES ==============================*/
            builder.Services.AddTransient<IEmailSender, EmailSender>();    
            
            builder.Services.AddTransient<IEmailService, EmailService>();

            builder.Services.AddScoped <IFileService, FileService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login"; // Or your actual login path
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();

            app.MapControllerRoute(
            name: "default",
            pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");



            app.Run();
        }
    }
}
