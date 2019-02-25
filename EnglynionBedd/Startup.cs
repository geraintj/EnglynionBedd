using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EnglynionBedd.Gwasanaethau;
using EnglynionBedd.Gwasanaethau.Configuration;
using EnglynionBedd.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace EnglynionBedd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Gosodiadau>(options => Configuration.GetSection("Gosodiadau").Bind(options));
            services.Configure<GosodiadauAllweddgell>(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.ConfigureWarnings(b => b.Log(CoreEventId.ManyServiceProvidersCreatedWarning))
                    .UseSqlServer(Configuration.GetConnectionString("EnglynionBeddContextConnection")));

            services.AddMvc();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration["Gosodiadau:IdFacebook"];
                facebookOptions.AppSecret = Configuration["CyfrinachFacebook"];
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration["Gosodiadau:IdGoogle"];
                googleOptions.ClientSecret = Configuration["CyfrinachGoogle"];
            })
            .AddMicrosoftAccount(microsoftOptions =>
            {
                microsoftOptions.ClientId = Configuration["Gosodiadau:IdMicrosoft"];
                microsoftOptions.ClientSecret = Configuration["CyfrinachMicrosoft"];
            })
            .AddIdentityCookies(o => { });


            services.AddTransient<IGwasanaethauGwybodol, GwasanaethauGwybodol>();
            services.AddTransient<ICronfaEnglynion, CronfaEnglynion>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
