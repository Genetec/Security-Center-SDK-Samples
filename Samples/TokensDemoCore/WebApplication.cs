using System;
using Genetec.Sdk;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SdkHelpers.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WebApplicationCore
{
    public class WebApplication
    {
        /// <summary>
        /// The SDK Engine used by this sample. Created once.
        /// </summary>
        public static Engine Engine { get; set; }

        /// <summary>
        /// Manage if the Engine was created or not.
        /// </summary>
        public static bool IsSdkEngineCreated { get; set; }

        public static void Main(string[] args)
        {
            SdkAssemblyLoader.Start();
            IsSdkEngineCreated = false;

            var builder = CreateWebHostBuilder(args).Build();

            builder.Run();
        } 

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseKestrel().UseIISIntegration()
                .UseStartup<Startup>();
    }
    
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                // You might want to only set the application cookies over a secure connection:
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddMvc();

            services.AddControllers();  
        }
    }
}