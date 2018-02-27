using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Services;
using WebMarkupMin.AspNetCore2;
using WebMarkupMin.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BlogCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //public static string ConnectionString { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // For Use with Database

            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //ConnectionString = Configuration.GetConnectionString("DefaultConnection");

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders();


            services
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/login/";
                   options.LogoutPath = "/logout/";
               });


            // Add application services.
            //services.AddTransient<IEmailSender, EmailSender>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDal, MemoryCache>();
           

            services
               .AddWebMarkupMin(options =>
               {
                   options.AllowMinificationInDevelopmentEnvironment = false;
                   options.DisablePoweredByHttpHeaders = true;
               })
               .AddHtmlMinification(options =>
               {
                   options.MinificationSettings.RemoveOptionalEndTags = false;
                   options.MinificationSettings.WhitespaceMinificationMode = WhitespaceMinificationMode.Safe;
               });

            services.AddMvc();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/error/{0}");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseWebMarkupMin();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
