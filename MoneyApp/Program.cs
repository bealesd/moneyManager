using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoneyApp.Repos;

namespace MoneyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            UserRepo userRepo = new UserRepo();
            userRepo.AddUser("David Beales");

            System.Threading.Thread.Sleep(2000);

            //BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
