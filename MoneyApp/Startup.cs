using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyApp.Authentication;
using MoneyApp.Helper;
using MoneyApp.IO;
using MoneyApp.Repos;
using MoneyApp.Interfaces;
using MoneyApp.Services;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Identity;

namespace MoneyApp
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
            //opt.UseInMemoryDatabase(Guid.NewGuid().ToString())
            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddMvc();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "User Api", Version = "v1" }); });

            services.AddSingleton<IAdapterRepo>(new AdapterRepo
                                                    (
                                                        new UserRepo(new JsonReaderWriter(), new PathHelper().TempPath("users.txt")),
                                                        new AccountRepo(new JsonReaderWriter(), new PathHelper().TempPath("account.txt"))
                                                    ));

            services.AddSingleton<IUserApiService>(new UserApiService());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}",
                    defaults: new { controller = "UI", action = "LoadLoginView" });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Api"); });
        }
    }
}