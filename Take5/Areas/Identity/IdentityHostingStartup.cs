using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Take5.Areas.Identity.Data;

//[assembly: HostingStartup(typeof(WebApplication22.Areas.Identity.IdentityHostingStartup))]
namespace WebApplication22.Areas.Identity
{
    public class IdentityHostingStartup     //: IHostingStartup
    {
        //public void Configure(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices((context, services) => {
        //        services.AddDbContext<AuthDBContext>(options =>
        //            options.UseSqlServer(
        //                context.Configuration.GetConnectionString("AuthDBContextConnection")));

        //        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        //            .AddEntityFrameworkStores<AuthDBContext>();
        //    });
        //}
    }
}