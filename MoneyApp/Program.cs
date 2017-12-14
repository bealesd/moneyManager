using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoneyApp.IO;
using MoneyApp.Repos;

namespace MoneyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //private string usersPath =
            //        Path.GetDirectoryName(
            //            Path.GetDirectoryName(
            //                Path.GetDirectoryName(
            //                    System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)));

            //UserRepo userRepo = new UserRepo(new JsonReaderWriter(),
            //                                     @"C:\Users\dave\Desktop\Users.txt");

            //userRepo.AddAccountToUser("David Beales");

            //System.Threading.Thread.Sleep(2000);

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
