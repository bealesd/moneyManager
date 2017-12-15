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
        private string _apiPath;
        private HttpClient _client;

        public UserManagementController()
        {
            _apiPath = "http://localhost:37266/api";
            _client = new HttpClient();
        }
        public IActionResult Login()
        {
            return View("Login");
        }

        public IActionResult CreateUser(string username)
        {
            string path = $"{_apiPath}/user/{username}";
            var httpResponse = _client.PostAsync(path, null).Result;
            LoadAccountUserView()
            return View("AccountsOverview", httpResponse.Content.ReadAsAsync<Guid>().Result);
        }

        public IActionResult LoadAccountUserView(Guid userGuid)
        {
            var httpResponse =  _client.GetAsync($"{_apiPath}/user/{userGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                return View("Login", httpResponse.Content.ReadAsStringAsync().Result);

            var user = httpResponse.Content.ReadAsAsync<User>().Result;

            //foreach (var guid in user.AccountGuid)
            //{
            //    _client.GetAsync($"{_apiPath}/user/{username}/{guid}").Result;
            //}

            return View("AccountsOverview", httpResponse.Content.ReadAsAsync<User>().Result);

            // each user has list of account guid.
            // so once you have user, need to look up each account, then give this to the view.
            //GetMoneyAccount(string username, string accountName)
            
        }
    }
}
