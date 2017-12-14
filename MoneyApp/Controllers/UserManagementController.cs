using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using MoneyApp.Models;

namespace MoneyApp.Controllers
{
    public class UserManagementController : Controller
    {

        public UserManagementController()
        {
        }

        public IActionResult Index()
        {
            return View("Index", null);
        }

        public async Task<IActionResult> LoadAccountUserView(string username)
        {
            var client = new HttpClient();
            var httpResponse = await client.GetAsync($"http://localhost:37266/api/user/{username}");
            if (httpResponse.IsSuccessStatusCode)
                return View("AccountsOverview", await httpResponse.Content.ReadAsAsync<User>());

            return View("Index", httpResponse.IsSuccessStatusCode);
        }
    }
}
