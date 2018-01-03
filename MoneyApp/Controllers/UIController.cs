using System;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using MoneyApp.Dto;
using MoneyApp.Interfaces;

// look at cookies for user GUID storage.
namespace MoneyApp.Controllers
{
    public class UIController : Controller
    {
        private string _apiPath;
        private HttpClient _client;
        private IUserApiService _userApiService;

        public UIController(IUserApiService userApiService)
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
                HttpContext.Session.SetString("userGuid", userGuid.ToString());
                return RedirectToAction(nameof(UserAccountsPage), new { userGuid, postion = 0 });
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

        public IActionResult DeleteUser()
        {
            Guid userGuid = Guid.Parse(HttpContext.Session.GetString("userGuid"));
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
                HttpContext.Session.SetString("userGuid", userGuid.ToString());
                return UserAccountsPage(0);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(LoginPage));
            }
        }

        public IActionResult UserAccountsPage(int postion)
        {
            try
            {
                Guid userGuid = Guid.Parse(HttpContext.Session.GetString("userGuid"));
                var userDto = _userApiService.GetUserDto(userGuid);
                dynamic accoutsOverviewModel = new ExpandoObject();
                accoutsOverviewModel.user = userDto;
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

        public IActionResult UserAccountsIndex()
        {
            try
            {
                Guid userGuid = Guid.Parse(HttpContext.Session.GetString("userGuid"));
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

        public IActionResult AccountPage(Guid accountGuid, int position)
        {
            //HttpContext.Session.SetString("userGuid", userGuid.ToString());

            Guid userGuid = Guid.Parse(HttpContext.Session.GetString("userGuid"));
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                var account = _userApiService.LoadMoneyAccount(accountGuid);
                dynamic accountModel = new ExpandoObject();
                accountModel.account = account;
                accountModel.position = position;
                accountModel.user = userDto;
                return View("AccountView", accountModel);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(UserAccountsIndex), userGuid);
            }
        }

        public IActionResult UserAccountIndex(Guid accountGuid)
        {
            Guid userGuid = Guid.Parse(HttpContext.Session.GetString("userGuid"));
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

        public IActionResult DeleteMoneyAccountFromUser(Guid accountGuid)
        {
            Guid userGuid = Guid.Parse(HttpContext.Session.GetString("userGuid"));
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                _userApiService.DeleteMoneyAccountFromUser(userDto.UserGuid, accountGuid);
            }
            catch (Exception) { }
            return RedirectToAction(nameof(UserAccountsPage), new { postion = Convert.ToInt32(GetAccountsPositon("accountPosition")), userGuid });
        }

        public IActionResult CreateMoneyAccountForUser(string accountName)
        {
            Guid userGuid = Guid.Parse(HttpContext.Session.GetString("userGuid"));
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                _userApiService.CreateMoneyAccountForUser(accountName, userGuid);
            }
            catch (Exception) { }
            return RedirectToAction(nameof(UserAccountsPage), new { userGuid, postion = 0 });
        }

        public IActionResult CreateMoneySpentItem(Guid accountGuid, float itemCost, string itemName, DateTime dateTime)
        {
            Guid userGuid = Guid.Parse(HttpContext.Session.GetString("userGuid"));
            try
            {
                var moneySpentItem = new MoneySpentItemDto() { ItemCost = itemCost, ItemName = itemName, DateTime = dateTime };
                _userApiService.CreateMoneySpentItem(accountGuid, moneySpentItem);
            }
            catch (Exception)
            {
            }
            return RedirectToAction(nameof(AccountPage), new { accountGuid, userGuid, position = 0 });
        }

        public IActionResult DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            Guid userGuid = Guid.Parse(HttpContext.Session.GetString("userGuid"));
            try
            {
                _userApiService.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
            }
            catch (Exception)
            {
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