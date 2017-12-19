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

        public UserManagementController(IUserApiService userApiService)
        {
            _apiPath = "http://localhost:37266/api";
            _client = new HttpClient();
            _userApiService = userApiService;
        }
        public IActionResult Login()
        {
            return View("LoginView");
        }

        public IActionResult RegisterUser(string username)//string password
        {
            try
            {
                _userApiService.CreateUser(username);
                return RedirectToAction(nameof(LoadUserAccountsView), new { username });
            }
            catch (Exception)
            {
                return View("LoginView");
            }
        }

        public IActionResult LoadUserAccountsView(string username)//string password
        {
            try
            {
                return View("AccountsOverview", _userApiService.GetUserDto(username));
            }
            catch (Exception)
            {
                return View("LoginView");
            }
        }

        public IActionResult LoadAccountView(Guid accountGuid)
        {
            var account = _userApiService.LoadAnAccount(accountGuid);
            return View("AccountView", account);
        }

        public IActionResult CreateMoneySpentItem(Guid accountGuid, float itemCost, string itemName)
        {
            try
            {
                var moneySpentItem = new MoneySpentItemDto() { ItemCost = itemCost, ItemName = itemName, DateTime = DateTime.Now };
                _userApiService.CreateMoneySpentItem(accountGuid, moneySpentItem);
                return RedirectToAction(nameof(LoadAccountView), new { accountGuid });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(LoadAccountView), new { accountGuid });
            }

        }

        public IActionResult DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            try
            {
                _userApiService.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
                return RedirectToAction(nameof(LoadAccountView), new { accountGuid });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(LoadAccountView), new { accountGuid });
            }

        }
    }
}
