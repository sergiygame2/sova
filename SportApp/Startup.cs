using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportApp.Data;
using SportApp.Models;
using SportApp.Repositories;
using SportApp.Services;

namespace SportApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

          
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            services.AddOptions();
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 0;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddScoped<IGymRepository, GymRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            
            services.AddTransient<IDbInitializer, DbInitializer>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("MySqlConnectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseDefaultFiles();
            app.UseIdentity();

            app.UseStaticFiles();

            app.UseCors("AllowAll");

            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = "235726570155665",
                AppSecret = "5a874129278465d113c11d600de2d270"
            });
            app.UseGoogleAuthentication(new GoogleOptions
            {
                ClientId = "890692129897-89eb0oclt6covikm1b1qrfp9qps1nqgr.apps.googleusercontent.com",
                ClientSecret = "W26j3ubFzzMy337WCi_WGUBL"
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            InitializeDatabase(app);
        }

        public virtual void InitializeDatabase(IApplicationBuilder app)
        {
            IDbInitializer databaseInitializer = app.ApplicationServices.GetService<IDbInitializer>();
            databaseInitializer.SeedData();
        }
    }
}
