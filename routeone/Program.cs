using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using routeone.Helpers;
using routeone.Mapping_Profile;
using routeone.Settings;
using System.Data;

namespace routeone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. 
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DataBaseEntites>(option =>
                 {

                    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });


            //  builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();
            // builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAutoMapper(m => m.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new UserProfile()));
            builder.Services.AddAutoMapper(m => m.AddProfile(new RoleProfile()));


            builder.Services.AddIdentity<AppliactionUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true; 
            }).AddEntityFrameworkStores<DataBaseEntites>().AddDefaultTokenProviders(); 
            

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => 
            {
                 options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "Home/Error";
		});

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
           builder.Services.AddTransient<IEmailSettings, EmailSettings>();  

            builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
           builder.Services.AddTransient<ISmsServices, SmsServices>();

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

			}).AddGoogle(o => 
            {
                IConfiguration GoogleAuthSection = builder.Configuration.GetSection("Authentication:Google");
                o.ClientId = GoogleAuthSection["ClientID"];
                o.ClientSecret = GoogleAuthSection["ClientSecret"];

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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
