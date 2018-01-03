using System;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
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
            dynamic errorModel = new ExpandoObject();
            errorModel.errorMessage = "";
            return View("LoginView", errorModel);
        }

        public IActionResult RegisterUser(string username, string password)
        {
            try
            {
                _userApiService.CreateUser(username, password);
                var userGuid = _userApiService.GetUserGuid(username, password);
                return RedirectToAction(nameof(UserAccountsPage), new { userGuid, postion = 0 });
                //return RedirectToAction(nameof(UserAccountsPage), new { username, password, postion = 0 });
            }
            catch (Exception)
            {
                dynamic errorModel = new ExpandoObject();
                errorModel.errorMessage = "";
                return View("LoginView", errorModel);
            }
        }

        public IActionResult RegisterUserPage()
        {
            return View("RegisterUserView");
        }

        public IActionResult DeleteUser(Guid userGuid)
        {
            try
            {
                _userApiService.DeleteUser(userGuid);
                return RedirectToAction(nameof(LoginPage));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(LoginPage));
            }
        }

        public IActionResult UserAccountsPageLogin(string username, string password)
        {
            try
            {
                var userGuid = _userApiService.GetUserGuid(username, password);
                return RedirectToAction(nameof(UserAccountsPage), new { userGuid, postion = 0 });
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(LoginPage));
            }
        }

        public IActionResult UserAccountsPage(Guid userGuid, int postion)
        {
            try
            {
                //var userGuid = _userApiService.GetUserGuid(username, password);
                var userDto = _userApiService.GetUserDto(userGuid);
                dynamic accoutsOverviewModel = new ExpandoObject();
                accoutsOverviewModel.user = userDto;
                //accoutsOverviewModel.password = password;
                accoutsOverviewModel.accountsIndex = postion;
                return View("AccountsOverview", accoutsOverviewModel);
            }
            catch (Exception)
            {
                dynamic errorModel = new ExpandoObject();
                errorModel.errorMessage = "Failed to login.";
                return View("LoginView", errorModel);
            }
        }

        public IActionResult UserAccountsIndex(Guid userGuid)
        {
            try
            {
                var userDto = _userApiService.GetUserDto(userGuid);
                int accountsPosition;

                accountsPosition = Convert.ToInt32(GetAccountsPositon("accountsPosition")) + 10;
                if (accountsPosition >= userDto.Accounts.Count)
                    accountsPosition = 0;

                SetAccountsPositon("accountsPosition", accountsPosition);
                return RedirectToAction(nameof(UserAccountsPage), new { userGuid, postion = accountsPosition });
            }
            catch (Exception)
            {
                dynamic errorModel = new ExpandoObject();
                errorModel.errorMessage = "Failed to login.";
                return View("LoginView", errorModel);
            }
        }

        public IActionResult AccountPage(Guid accountGuid, Guid userGuid, int position)
        {
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                var account = _userApiService.LoadMoneyAccount(accountGuid);
                dynamic accountModel = new ExpandoObject();
                accountModel.account = account;
                accountModel.position = position;
                accountModel.user = userDto;
                //accountModel.password = password;
                return View("AccountView", accountModel);
            }
            catch (Exception)
            {
                //return View("AccountsOverview", userDto);
                return RedirectToAction(nameof(UserAccountsIndex), userGuid);
            }
        }

        public IActionResult UserAccountIndex(Guid accountGuid, Guid userGuid)
        {
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                int accountPosition;

                accountPosition = Convert.ToInt32(GetAccountsPositon("accountPosition")) + 10;
                if (accountPosition >= userDto.Accounts.Count)
                    accountPosition = 0;

                SetAccountsPositon("accountPosition", accountPosition);
                return RedirectToAction(nameof(AccountPage), new { accountGuid, position = accountPosition, userGuid });
            }
            catch (Exception)
            {
                return View("LoginView");
            }
        }

        public IActionResult DeleteMoneyAccountFromUser(Guid accountGuid, Guid userGuid)
        {
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                _userApiService.DeleteMoneyAccountFromUser(userDto.UserGuid, accountGuid);
            }
            catch (Exception) { }
            return RedirectToAction(nameof(UserAccountsPage), new { postion = Convert.ToInt32(GetAccountsPositon("accountPosition")), userGuid });
        }

        public IActionResult CreateMoneyAccountForUser(string accountName, Guid userGuid)
        {
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                _userApiService.CreateMoneyAccountForUser(accountName, userGuid);
            }
            catch (Exception) { }
            return RedirectToAction(nameof(UserAccountsPage), new { userGuid, postion = 0 });
        }

        public IActionResult CreateMoneySpentItem(Guid accountGuid, float itemCost, string itemName, DateTime dateTime, Guid userGuid)
        {
            try
            {
                var moneySpentItem = new MoneySpentItemDto() { ItemCost = itemCost, ItemName = itemName, DateTime = dateTime };
                _userApiService.CreateMoneySpentItem(accountGuid, moneySpentItem);
                //return RedirectToAction(nameof(AccountPage), new { accountGuid });
            }
            catch (Exception)
            {
                //return RedirectToAction(nameof(AccountPage), new { accountGuid });
            }
            return RedirectToAction(nameof(AccountPage), new { accountGuid, userGuid, position = 0 });
        }

        public IActionResult DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid, Guid userGuid)
        {
            try
            {
                _userApiService.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
            }
            catch (Exception)
            {
                //return RedirectToAction(nameof(AccountPage), new { accountGuid });
            }
            return RedirectToAction(nameof(AccountPage), new { accountGuid, position = 0, userGuid });
        }

        public string GetAccountsPositon(string key)
        {
            return HttpContext.Session.GetString(key);
        }

        public void SetAccountsPositon(string key, int position)
        {
            HttpContext.Session.SetString(key, position.ToString());
        }

    }
}
