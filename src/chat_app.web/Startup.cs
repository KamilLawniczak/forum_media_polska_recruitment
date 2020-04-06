using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat_app.domain;
using chat_app.domain.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace chat_app.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ChatContext> (options => {
                options.UseSqlServer (Configuration.GetConnectionString ("ChatDb"));
            });

            services.AddSingleton<ISecurePasswordService, SecurePasswordService> ();
            services.AddTransient<Func<ChatContext>> (x => () => x.GetService<ChatContext> ());
            services.AddTransient<ChatUserService> ();

            services.AddAuthentication (CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie (options => {
                        options.Cookie.HttpOnly = false;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes (5);

                        options.LoginPath = "/User/LogIn";
                        options.SlidingExpiration = true;
                    });

            services.AddControllersWithViews ();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment ())
            {
                app.UseDeveloperExceptionPage ();
                app.UseBrowserLink ();
            } else
            {
                app.UseExceptionHandler ("/Home/Error");
                app.UseHsts ();
            }
            app.UseHttpsRedirection ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllerRoute (
                    name: "default",
                    pattern: "{controller=Chat}/{action=Index}/{id?}");
            });
        }
    }
}
