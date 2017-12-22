using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using MoneyApp.Dto;
using MoneyApp.Interfaces;

namespace MoneyApp.Controllers
{
    public class UserManagementController : Controller
    {
        private string _apiPath;
        private HttpClient _client;
        private IUserApiService _userApiService;

        public UserManagementController(IUserApiService userApiService)
        {
            _apiPath = "http://localhost:37266/api";
            _client = new HttpClient();
            _userApiService = userApiService;
        }
        public IActionResult LoginPage()
        {
            return View("LoginView");
        }

        public IActionResult RegisterUser(string username)//string password
        {
            try
            {
                _userApiService.CreateUser(username);
                return RedirectToAction(nameof(UserAccountsPage), new { username });
            }
            catch (Exception)
            {
                return View("LoginView");
            }
        }

        public IActionResult RegisterUserPage()//string password
        {
            return View("RegisterUserView");
        }

        public IActionResult DeleteUser(string username)
        {
            try
            {
                _userApiService.DeleteUser(username);
                return RedirectToAction(nameof(LoginPage));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(LoginPage));
            }
        }

        public IActionResult UserAccountsPage(string username)//string password
        {
            try
            {
                var userDto = _userApiService.GetUserDto(username);
                return View("AccountsOverview", userDto);
            }
            catch (Exception)
            {
                return View("LoginView");
            }
        }

        public IActionResult AccountPage(Guid accountGuid, UserDto userDto)
        {
            try
            {
                var account = _userApiService.LoadMoneyAccount(accountGuid);
                return View("AccountView", account);
            }
            catch (Exception)
            {
                return View("AccountsOverview", userDto);
            }
        }

        public IActionResult DeleteMoneyAccountFromUser(Guid accountGuid, string username)
        {
            try
            {
                var userDto = _userApiService.GetUserDto(username);
                _userApiService.DeleteMoneyAccountFromUser(userDto.UserGuid, accountGuid);
                return View("AccountsOverview", userDto);
            }
            catch (Exception)
            {
                var userDto = _userApiService.GetUserDto(username);
                return View("AccountsOverview", userDto);
            }
        }

        public IActionResult CreateMoneyAccountForUser(string username, string accountName)
        {
            try
            {
                var userDto = _userApiService.GetUserDto(username);
                _userApiService.CreateMoneyAccountForUser(userDto.UserGuid, accountName);
                return View("AccountsOverview", userDto);
            }
            catch (Exception)
            {
                var userDto = _userApiService.GetUserDto(username);
                return View("AccountsOverview", userDto);
            }
        }

        public IActionResult CreateMoneySpentItem(Guid accountGuid, float itemCost, string itemName, DateTime dateTime)
        {
            try
            {
                var moneySpentItem = new MoneySpentItemDto() { ItemCost = itemCost, ItemName = itemName, DateTime = dateTime };
                _userApiService.CreateMoneySpentItem(accountGuid, moneySpentItem);
                return RedirectToAction(nameof(AccountPage), new { accountGuid });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(AccountPage), new { accountGuid });
            }
        }

        public IActionResult DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            try
            {
                _userApiService.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
                return RedirectToAction(nameof(AccountPage), new { accountGuid });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(AccountPage), new { accountGuid });
            }
        }
    }
}
