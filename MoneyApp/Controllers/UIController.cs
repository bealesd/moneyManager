using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using MoneyApp.Dto;
using MoneyApp.Interfaces;

#region expando
//dynamic accoutsOverviewModel = new ExpandoObject();
//accoutsOverviewModel.user = userDto;
//accoutsOverviewModel.accountsIndex = postion;
//@model dynamic
#endregion
// DeleteUser issue. Check error handling, should messages bubble up through the program. Possible two logins required on first login.
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

        public IActionResult LoadLoginView()
        {
            ViewBag.IsLoggedIn = "false";
            ViewBag.errorMessage = "";
            return View("LoginView");
        }

        public IActionResult LoadRegisterUserView()
        {
            ViewBag.IsLoggedIn = "false";
            return View("RegisterUserView");
        }

        public IActionResult LoadOverview()
        {
            try
            {
                Guid userGuid = Guid.Parse(Read("userGuid"));
                var userDto = _userApiService.GetUserDto(userGuid);
                ViewBag.user = userDto;
                ViewBag.accountsPosition = Read("accountsPosition");
                ViewBag.IsLoggedIn = "true";
                return View("Overview");
            }
            catch (Exception)
            {
                ViewBag.errorMessage = "Failed to login.";
                return LoadLoginView();
            }
        }

        public IActionResult LoadAccountView(Guid accountGuid, int position)
        {
            Guid userGuid = Guid.Parse(Read("userGuid"));

            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                var account = _userApiService.LoadMoneyAccount(accountGuid);
                ViewBag.account = account;
                ViewBag.user = userDto;
                Set("accountPosition", 0.ToString());
                ViewBag.accountPosition = Read("accountsPosition");
                ViewBag.IsLoggedIn = "true";
                return View("AccountView");
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(PaginateOverview));
            }
        }

        public IActionResult CreateUser(string username, string password)
        {
            try
            {
                _userApiService.CreateUser(username, password);

                var userGuid = _userApiService.GetUserGuid(username, password);
                Set("userGuid", userGuid.ToString());
                Set("accountsPosition", 0.ToString());
                return RedirectToAction(nameof(LoadOverview));
            }
            catch (Exception)
            {
                return LoadLoginView();
            }
        }
        
        public IActionResult DeleteUser()
        {
            Guid userGuid = Guid.Parse(Read("userGuid"));
            try
            {
                _userApiService.DeleteUser(userGuid);
                return RedirectToAction(nameof(LoadLoginView));
            }
            catch (Exception)
            {
                return LoadLoginView();
            }
        }

        public IActionResult VerifyLogin(string username, string password)
        {
            try
            {
                var userGuid = _userApiService.GetUserGuid(username, password);
                Set("userGuid", userGuid.ToString());
                Set("accountsPosition", 0.ToString());
                return LoadOverview();
            }
            catch (Exception)
            {
                return LoadLoginView();
            }
        }
        
        public IActionResult PaginateOverview()
        {
            try
            {
                Guid userGuid = Guid.Parse(Read("userGuid"));

                var userDto = _userApiService.GetUserDto(userGuid);
                int accountsPosition;

                accountsPosition = Convert.ToInt32(Read("accountsPosition")) + 10;
                if (accountsPosition >= userDto.Accounts.Count)
                    accountsPosition = 0;

                Set("accountsPosition", accountsPosition.ToString());
                return RedirectToAction(nameof(LoadOverview));
            }
            catch (Exception)
            {
                ViewBag.errorMessage = "Failed to login.";
                return LoadLoginView();
            }
        }

        public IActionResult CreateAccount(string accountName)
                {
                    Guid userGuid = Guid.Parse(Read("userGuid"));
                    var userDto = _userApiService.GetUserDto(userGuid);
                    try
                    {
                        _userApiService.CreateMoneyAccountForUser(accountName, userGuid);
                    }
                    catch (Exception) { }

                    Set("accountsPosition", 0.ToString());
                    return RedirectToAction(nameof(LoadOverview));
                }

        public IActionResult DeleteAccount(Guid accountGuid)
        {
            Guid userGuid = Guid.Parse(Read("userGuid"));
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                _userApiService.DeleteAccount(userDto.UserGuid, accountGuid);
            }
            catch (Exception) { }

            Set("accountsPosition", 0.ToString());
            return RedirectToAction(nameof(LoadOverview));
        }

        public IActionResult PaginateAccount(Guid accountGuid)
        {
            Guid userGuid = Guid.Parse(Read("userGuid"));
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                int accountPosition;
                accountPosition = Convert.ToInt32(Read("accountPosition")) + 10;
                if (accountPosition >= userDto.Accounts.Count)
                    accountPosition = 0;

                Set("accountPosition", accountPosition.ToString());
                return RedirectToAction(nameof(LoadAccountView), new { accountGuid });
            }
            catch (Exception)
            {
                return LoadLoginView();
            }
        }

        public IActionResult CreateTransaction(Guid accountGuid, float itemCost, string itemName, DateTime dateTime)
        {
            Guid userGuid = Guid.Parse(Read("userGuid"));
            try
            {
                var moneySpentItem = new MoneySpentItemDto() { ItemCost = itemCost, ItemName = itemName, DateTime = dateTime };
                _userApiService.CreateMoneySpentItem(accountGuid, moneySpentItem);
            }
            catch (Exception)
            {
            }
            Set("accountPosition", 0.ToString());
            return RedirectToAction(nameof(LoadAccountView), new { accountGuid });
        }

        public IActionResult DeleteTransaction(Guid accountGuid, Guid moneyItemGuid)
        {
            Guid userGuid = Guid.Parse(Read("userGuid"));
            try
            {
                _userApiService.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
            }
            catch (Exception)
            {
            }
            Set("accountPosition", 0.ToString());
            return RedirectToAction(nameof(LoadAccountView), new { accountGuid });
        }

        public void Set(string key, string value)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(1);
            Response.Cookies.Append(key, value, option);
        }

        public string Read(string key)
        {
            var cookieValueFromReq = Request.Cookies[key];
            return cookieValueFromReq;
        }
    }
}