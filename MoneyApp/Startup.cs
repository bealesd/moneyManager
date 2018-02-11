using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        public void ConfigureServices(IServiceCollection services)
        {
            var testMode = Configuration.GetSection("TestMode").Get<bool>(); ;

            services.AddMvc();

            services.AddSession();

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "User Api", Version = "v1" }); });

            IFilePaths filePaths;
            if (testMode)
            {
                filePaths = new TestFilePaths();
                filePaths.WipeFiles();
            }
                
            else
                filePaths = new StandardFilePaths();

            services.AddSingleton<IAdapterRepo>(new AdapterRepo
                                                    (
                                                        new UserRepo(new JsonReaderWriter(), filePaths.UserPath()),
                                                        new AccountRepo(new JsonReaderWriter(), filePaths.AccountPath())
                                                    ));
            services.AddSingleton<IUserLogin>(new UserLoginRepo(new JsonReaderWriter(), filePaths.CredentialsPath()));
            services.AddSingleton<IUserApiService>(new UserApiService());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller}/{action}",
                //    defaults: new { controller = "UI", action = "LoadLoginView" });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Api"); });
        }
    }
}