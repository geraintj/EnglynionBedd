using System;
using EnglynionBedd.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(EnglynionBedd.Areas.Identity.IdentityHostingStartup))]
namespace EnglynionBedd.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<EnglynionBeddContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("EnglynionBeddContextConnection")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<EnglynionBeddContext>();
            });
        }
    }
}