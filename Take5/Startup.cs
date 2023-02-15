using Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MoreForYou.Services.Models.Utilities.Mapping;
using Repository.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take5.Areas.Identity.Data;
using Take5.Models.Models;
using Take5.Service.Implementation.Auth;
using Take5.Services;
using Take5.Services.Contracts;
using Take5.Services.Contracts.Auth;
using Take5.Services.Implementation;
using Take5.Services.Models.hub;

namespace Take5
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
            var connectionString = Configuration.GetConnectionString("SqlCon");

            services.AddControllersWithViews();
            services.AddDbContext<APPDBContext>(options =>
            options.UseSqlServer(connectionString));

            services.AddDbContext<AuthDBContext>(options =>
                        options.UseSqlServer(connectionString));

            services.AddDbContext<AuthDBContext>(options =>
            {
                options.UseSqlServer(connectionString,
            b => b.MigrationsAssembly(typeof(AuthDBContext).Assembly.FullName));
            }, ServiceLifetime.Transient);

            services.AddIdentity<AspNetUser, AspNetRole>(options =>
            {
                //options.SignIn.RequireConfirmedAccount = true;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
               

            })
            .AddEntityFrameworkStores<AuthDBContext>()
            .AddRoles<AspNetRole>()
            .AddDefaultUI().AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(AssembleType));

            services.AddScoped<DbContext, APPDBContext>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped<UserManager<AspNetUser>>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IDangerService, DangerService>();
            services.AddScoped<IDangerCategoryService, DangerCategoryService>();
            services.AddScoped<ITruckService, TruckService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IJobSiteService, JobSiteService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMeasureControlService, MeasureControlService>();
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserConnectionManager, UserConnectionManager>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IStepTwoRequestService, StepTwoRequestService>();
            services.AddScoped<ITripJobsiteService, TripJobsiteService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<ITripJobsiteWarningService, TripJobsiteWarningService>();
            services.AddScoped<ITripCancellationService, TripCancellationService>();
            services.AddScoped<ITripTake5TogetherService, TripTake5TogetherService>();
            services.AddScoped<IFirebaseNotificationService, FirebaseNotificationService>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMvc(option =>
            {
                option.CacheProfiles.Add("Default30",
                    new CacheProfile()
                    {
                        Duration = 30
                    });
            });
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddMvc();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("http://20.86.97.165/Take5")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddSignalR();
            services.ConfigAutoMapper();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Take5", Version = "v1" });
            });
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            services.AddMvc().AddMvcOptions(options =>
            {
                options.MaxModelValidationErrors = 999999;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Take5.API");
                c.EnableFilter();
            });
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{Take5}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<NotificationHub>("/NotificationHub");
                //endpoints.MapHub<DashBoardHub>("/DashBoardHub");


            });
        }
    }
}
