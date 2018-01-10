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

        public IActionResult LoadLoginView(string errorMessage)
        {
            _userApiService.Logout();
            ViewBag.IsLoggedIn = "false";
            ViewBag.errorMessage = errorMessage;
            return View("LoginView");
        }

        public IActionResult LoadRegisterUserView()
        {
            ViewBag.IsLoggedIn = "false";
            return View("RegisterUserView");
        }

        public IActionResult LoadOverview(string errorMessage)
        {
            try
            {
                var user = _userApiService.GetUserDto();
                ViewBag.errorMessage = errorMessage;
                ViewBag.user = user;
                ViewBag.accountsPosition = Read("accountsPosition");
                ViewBag.IsLoggedIn = "true";
                return View("Overview");
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error: LoadOverview"));
            }
        }

        public IActionResult LoadAccountView(Guid accountGuid, string errorMessage)
        {
            var userDto = _userApiService.GetUserDto();

            try
            {
                var account = _userApiService.LoadMoneyAccount(accountGuid);
                ViewBag.errorMessage = errorMessage;
                ViewBag.account = account;
                ViewBag.user = userDto;
                Set("accountPosition", 0.ToString());
                ViewBag.accountPosition = Read("accountsPosition");
                ViewBag.IsLoggedIn = "true";
                return View("AccountView");
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error: LoadAccountView"));
            }
        }

        public IActionResult CreateUser(string username, string password)
        {
            try
            {
                _userApiService.Register(username, password);
                Set("accountsPosition", 0.ToString());
                return LoadOverview("");
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error"));
            }
        }

        public IActionResult DeleteUser()
        {
            try
            {
                _userApiService.DeleteUser();
                return LoadLoginView("");
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error: DeleteUser"));
            }
        }

        public IActionResult Login(string username, string password)
        {
            try
            {
                Set("accountsPosition", 0.ToString());
                _userApiService.Login(username, password);
                return LoadOverview("");
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error: VerifyLogin"));
            }
        }

        public IActionResult PaginateOverview()
        {
            try
            {
                var userDto = _userApiService.GetUserDto();
                int accountsPosition;

                accountsPosition = Convert.ToInt32(Read("accountsPosition")) + 10;
                if (accountsPosition >= userDto.Accounts.Count)
                    accountsPosition = 0;

                Set("accountsPosition", accountsPosition.ToString());
                return LoadOverview(string.Empty);
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error: PaginateOverview"));
            }
        }

        public IActionResult CreateAccount(string accountName)
        {
            Set("accountsPosition", 0.ToString());
            try
            {
                _userApiService.CreateMoneyAccountForUser(accountName);
                return LoadOverview(string.Empty);
            }
            catch (Exception e)
            {
                return LoadOverview(MessageHandler(e, "Unknown Error: CreateAccount"));
            }
        }

        public IActionResult DeleteAccount(Guid accountGuid)
        {
            Set("accountsPosition", 0.ToString());
            try
            {
                _userApiService.DeleteAccount(accountGuid);
                return LoadOverview(string.Empty);
            }
            catch (Exception e)
            {
                return LoadOverview(MessageHandler(e, "Unknown Error: DeleteAccount"));
            }
        }

        public IActionResult PaginateAccount(Guid accountGuid)
        {
            var userDto = _userApiService.GetUserDto();
            try
            {
                int accountPosition;
                accountPosition = Convert.ToInt32(Read("accountPosition")) + 10;
                if (accountPosition >= userDto.Accounts.Count)
                    accountPosition = 0;
                Set("accountPosition", accountPosition.ToString());
                return LoadAccountView(accountGuid, string.Empty);
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, $"Unknown Error: {nameof(PaginateAccount)}"));
            }
        }

        public IActionResult CreateTransaction(Guid accountGuid, float itemCost, string itemName, DateTime dateTime)
        {
            Set("accountPosition", 0.ToString());
            try
            {
                var moneySpentItem = new MoneySpentItemDto() { ItemCost = itemCost, ItemName = itemName, DateTime = dateTime };
                _userApiService.CreateMoneySpentItem(accountGuid, moneySpentItem);
                return LoadAccountView(accountGuid, String.Empty);
            }
            catch (Exception e)
            {
                return LoadAccountView(accountGuid, e.Message);
            }
        }

        public IActionResult DeleteTransaction(Guid accountGuid, Guid moneyItemGuid)
        {
            Set("accountPosition", 0.ToString());
            try
            {
                _userApiService.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
                return LoadAccountView(accountGuid, String.Empty);
            }
            catch (Exception e)
            {
                return LoadAccountView(accountGuid, e.Message);
            }
        }

        public void Set(string key, string value)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(10);
            Response.Cookies.Append(key, value, option);
        }

        public string Read(string key)
        {
            try
            {
                var cookieValueFromReq = Request.Cookies[key];
                return cookieValueFromReq;
            }
            catch (Exception)
            {
                throw new Exception("Session Timeout");
            }
        }

        public string MessageHandler(Exception e, string customMessage)
        {
            var message = customMessage;
            if (e.Message != string.Empty)
                message = e.Message;
            return message;
        }
    }
}