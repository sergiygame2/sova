using System;
using System.Linq;
using System.Reflection;
using GI.Models.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportApp;
using SportApp.Data;
using SportApp.Lookups;
using SportApp.Models;
using SportApp.Repositories;
using SportApp.Services;

namespace Tests
{
    public class TestStartup : Startup
    {
        private IAppPermissionsLookup _permissions;

        public TestStartup(IHostingEnvironment env) : base(env)
        {
            _permissions = new AppPermissionsLookup();
        }
        
        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Console.WriteLine("teststartup conf start");

            loggerFactory.AddConsole(Configuration.GetSection("LoggingTest"));
            loggerFactory.AddDebug();

            app.UseIdentity();

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();
            app.UseCors("AllowAll");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "admin",
                    template: "admin/{*url}",
                    defaults: new { controller = "Admin", action = "Index" }
                );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}"
                );
            });
            InitializeDatabase(app);
            Console.WriteLine("teststartup conf end");
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            services.AddOptions();

            services.Configure<UploadedFilesSettings>(Configuration.GetSection("UploadedFilesSettings"));
            var uploadedFilesPath = Configuration.GetSection("UploadedFilesSettings").GetValue<string>("PhysicalPath").Replace("\\", "/");
            //uploadedFilesPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), uploadedFilesPath));

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

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ViewUsers", policy => policy.RequireClaim("permission", _permissions.ViewUsers));
                options.AddPolicy("CreateUsers", policy => policy.RequireClaim("permission", _permissions.CreateUsers));
                options.AddPolicy("UpdateUsers", policy => policy.RequireClaim("permission", _permissions.UpdateUsers));
                options.AddPolicy("RemoveUsers", policy => policy.RequireClaim("permission", _permissions.RemoveUsers));

                options.AddPolicy("ViewRoles", policy => policy.RequireClaim("permission", _permissions.ViewRoles));
                options.AddPolicy("CreateRoles", policy => policy.RequireClaim("permission", _permissions.CreateRoles));
                options.AddPolicy("UpdateRoles", policy => policy.RequireClaim("permission", _permissions.UpdateRoles));
                options.AddPolicy("RemoveRoles", policy => policy.RequireClaim("permission", _permissions.RemoveRoles));

                options.AddPolicy("ViewGyms", policy => policy.RequireClaim("permission", _permissions.ViewGyms));
                options.AddPolicy("CreateGyms", policy => policy.RequireClaim("permission", _permissions.CreateGyms));
                options.AddPolicy("UpdateGyms", policy => policy.RequireClaim("permission", _permissions.UpdateGyms));
                options.AddPolicy("RemoveGyms", policy => policy.RequireClaim("permission", _permissions.RemoveGyms));

                options.AddPolicy("ViewComments", policy => policy.RequireClaim("permission", _permissions.ViewComments));
                options.AddPolicy("CreateComments", policy => policy.RequireClaim("permission", _permissions.CreateComments));
                options.AddPolicy("UpdateComments", policy => policy.RequireClaim("permission", _permissions.UpdateComments));
                options.AddPolicy("RemoveComments", policy => policy.RequireClaim("permission", _permissions.RemoveComments));
            });

            services.AddSingleton<IAppPermissionsLookup, AppPermissionsLookup>();

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddScoped<IGymRepository, GymRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddSingleton<IPaginationUtilities, PaginationUtilities>();
            

            services.Configure((RazorViewEngineOptions razorOptions) =>
            {
                var previous = razorOptions.CompilationCallback;
                razorOptions.CompilationCallback = (context) =>
                {
                    previous?.Invoke(context);

                    var assembly = typeof(Startup).GetTypeInfo().Assembly;
                    var assemblies = assembly.GetReferencedAssemblies()
                        .Select(x => MetadataReference.CreateFromFile(Assembly.Load(x).Location))
                        .ToList();
                    assemblies.Add(
                        MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("System.Private.Corelib"))
                        .Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("Microsoft.AspNetCore.Razor"))
                        .Location));
                    assemblies.Add(
                        MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("mscorlib")).Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("System.Private.Corelib"))
                        .Location));
                    assemblies.Add(
                        MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Linq")).Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("System.Threading.Tasks"))
                        .Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("System.Runtime"))
                        .Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("System.Dynamic.Runtime"))
                        .Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("Microsoft.AspNetCore.Razor.Runtime"))
                        .Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("Microsoft.AspNetCore.Mvc"))
                        .Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("Microsoft.AspNetCore.Razor"))
                        .Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("Microsoft.AspNetCore.Mvc.Razor"))
                        .Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("Microsoft.AspNetCore.Html.Abstractions"))
                        .Location));
                    assemblies.Add(MetadataReference.CreateFromFile(Assembly
                        .Load(new AssemblyName("System.Text.Encodings.Web"))
                        .Location));

                    context.Compilation = context.Compilation.AddReferences(assemblies);
                };
            });

            services.AddTransient<IDbInitializer, DbInitializer>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("MySqlConnectionString")));
        }
    }
}