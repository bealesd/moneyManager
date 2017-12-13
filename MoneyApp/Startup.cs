using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoneyApp.IO;
using MoneyApp.Repos;
using MoneyApp.Helper;
using MoneyApp.Interfaces;
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

            //services.AddSingleton<IUserRepo>(new UserRepo(new JsonReaderWriter(),
            //                                     new Helper.Helper().TempPath("users.txt")));

            //services.AddSingleton<IAccountRepo>(new AccountRepo(new JsonReaderWriter(),
            //                                         new Helper.Helper().TempPath("account.txt")));

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "User Api", Version = "v1" }); });


            services.AddSingleton<IAdapterRepo>(new AdapterRepo
                                                    (
                                                        new UserRepo(new JsonReaderWriter(), new Helper.Helper().TempPath("users.txt")), 
                                                        new AccountRepo(new JsonReaderWriter(), new Helper.Helper().TempPath("account.txt"))
                                                    ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Api"); });
        }
    }
}
