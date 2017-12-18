using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using MoneyApp.Dto;
using MoneyApp.Interfaces;
using MoneyApp.Models;

namespace MoneyApp.Controllers
{
    public class UserManagementController : Controller
    {
        private string _apiPath;
        private HttpClient _client;
        private IUserApiService _userApiService;

        //private ISessionHandler _sessionHandler;

        public UserManagementController(IUserApiService userApiService)//ISessionHandler sessionHandler)
        {
            _apiPath = "http://localhost:37266/api";
            _client = new HttpClient();
            _userApiService = userApiService;
            //_sessionHandler = sessionHandler;
        }
        public IActionResult Login()
        {
            return View("Login");
        }

        public IActionResult RegisterUser(string username)//string password
        {
            try
            {
                _userApiService.CreateUser(username);
                return RedirectToAction(nameof(LoadAccountUserView), new { username });
            }
            catch (Exception)
            {
                return View("Login");
            }
        }

        public IActionResult LoadAccountUserView(string username)//string password
        {
            //var userDto = UserApiService.GetUser(username)
            //if error: return View("Login", httpResponse.Content.ReadAsStringAsync().Result);
            //else: return View("AccountsOverview", userDto);

            var httpResponse =  _client.GetAsync($"{_apiPath}/user/{username}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                return View("Login", httpResponse.Content.ReadAsStringAsync().Result);

            var user = httpResponse.Content.ReadAsAsync<User>().Result;
            var userDto = CreateUserDto(user);

            //_sessionHandler.UpdateSessionHandler(ControllerContext.HttpContext);
            //_sessionHandler.SetSessionString("userGuid", user.UserGuid.ToString());

            return View("AccountsOverview", userDto);
        }

        public IActionResult LoadAccountView(Guid accountGuid)
        {
            //var userGuid = Guid.Parse(_sessionHandler.GetSessionString("userGuid"));
            var account = LoadAnAccount(accountGuid);
            return View("AccountView", account);
        }

        public UserDto CreateUserDto(User user)
        {
            var userDto = new UserDto() {Username = user.Username, UserGuid = user.UserGuid, Accounts = new List<Account>()};
            user.AccountGuid.ForEach(guid => userDto.Accounts.Add(LoadAnAccount(guid)));
            return userDto;
        }

        private Account LoadAnAccount(Guid accountGuid)
        {
            var httpResponse = _client.GetAsync($"{_apiPath}/user/account/{accountGuid}").Result;
            if (!httpResponse.IsSuccessStatusCode)
                return null;
            var account = httpResponse.Content.ReadAsAsync<Account>().Result;
            return account;
        }
    }
}
