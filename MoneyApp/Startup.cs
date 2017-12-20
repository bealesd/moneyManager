using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyApp.Helper;
using MoneyApp.IO;
using MoneyApp.Repos;
using MoneyApp.Interfaces;
using MoneyApp.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace MoneyApp
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
            services.AddMvc();
                //.AddSessionStateTempDataProvider();

            //services.AddSession();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "User Api", Version = "v1" }); });

            services.AddSingleton<IAdapterRepo>(new AdapterRepo
                                                    (
                                                        new UserRepo(new JsonReaderWriter(), new PathHelper().TempPath("users.txt")), 
                                                        new AccountRepo(new JsonReaderWriter(), new PathHelper().TempPath("account.txt")),
                                                        new UserLoginRepo(new JsonReaderWriter(), new PathHelper().TempPath("userCredentials.txt"))
                                                    ));
            //services.AddSingleton<ISessionHandler>();
            services.AddSingleton<IUserApiService>(new UserApiService());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseStaticFiles();
            //app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=UserManagementController}/{action=LoginPage}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Api"); });
        }
    }
}