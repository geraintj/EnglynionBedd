using System;
using AspNetCore.Identity.DocumentDb;
using EnglynionBedd.Gwasanaethau;
using EnglynionBedd.Gwasanaethau.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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

            services.AddAuthentication()
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
                    });

            services.AddSingleton<IDocumentClient>(new DocumentClient(
                new Uri(Configuration["Gosodiadau:CyfeiriadBasDdata"]), Configuration["BasDdataCosmos"]));

            services.AddIdentity<DocumentDbIdentityUser, DocumentDbIdentityRole>()
                .AddDocumentDbStores(options =>
                {
                    options.Database = Configuration["Gosodiadau:EnwBasDdata"];
                    options.UserStoreDocumentCollection = Configuration["Gosodiadau:CasgliadDefnyddwyr"];
                });


            services.AddTransient<IGwasanaethauGwybodol, GwasanaethauGwybodol>();
            services.AddTransient<ICronfaEnglynion, CronfaEnglynion>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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
