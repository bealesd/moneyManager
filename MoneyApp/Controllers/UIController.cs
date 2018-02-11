using System;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using MoneyApp.Dto;
using MoneyApp.Interfaces;

// DeleteUser issue. Two logins required on first login. UserGuid error when cookie expires, Guid.Parse(Read("userGuid")), not in try/except;.
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
                Guid userGuid;
                Guid.TryParse(Read("userGuid"), out userGuid);
                if (userGuidLogin != Guid.Empty)
                   userGuid = userGuidLogin; 
                var userDto = _userApiService.GetUserDto(userGuid);
                ViewBag.errorMessage = errorMessage;
                ViewBag.user = userDto;
                ViewBag.accountsPosition = Read("accountsPosition");
                ViewBag.IsLoggedIn = "true";
                return View("ManageAccountsView");
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error: LoadOverview"));
            }
        }

        public IActionResult LoadTransactionView(string errorMessage)
        {
            try
            {
                Guid userGuid;
                Guid.TryParse(Read("userGuid"), out userGuid);
                if (userGuidLogin != Guid.Empty)
                    userGuid = userGuidLogin;
                var userDto = _userApiService.GetUserDto(userGuid);
                ViewBag.errorMessage = errorMessage;
                ViewBag.user = userDto;
                ViewBag.IsLoggedIn = "true";
                return View("TransactionsView");
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error: LoadOverview"));
            }
        }

        public IActionResult LoadAccountView(Guid accountGuid, string errorMessage)
        {
            Guid userGuid = Guid.Parse(Read("userGuid"));
            var userDto = _userApiService.GetUserDto(userGuid);
            try
            {
                var account = _userApiService.LoadMoneyAccount(accountGuid);
                ViewBag.errorMessage = errorMessage;
                ViewBag.account = account;
                ViewBag.user = userDto;
                Set("accountPosition", 0.ToString());
                ViewBag.accountPosition = Read("accountsPosition");
                ViewBag.IsLoggedIn = "true";
                return View("ManageAccountView");
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
                _userApiService.CreateUser(username, password);
                var userGuid = _userApiService.GetUserGuid(username, password);
                Set("userGuid", userGuid.ToString());
                Set("accountsPosition", 0.ToString());
                return RedirectToAction(nameof(LoadOverview));
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error"));
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
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, "Unknown Error: DeleteUser"));
            }
        }

        private Guid userGuidLogin;
        public IActionResult VerifyLogin(string username, string password)
        {
            try
            {
                var userGuid = _userApiService.GetUserGuid(username, password);
                userGuidLogin = userGuid;
                Set("userGuid", userGuid.ToString());
                Set("accountsPosition", 0.ToString());
                return LoadOverview(string.Empty);
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
                Guid userGuid = Guid.Parse(Read("userGuid"));

                var userDto = _userApiService.GetUserDto(userGuid);
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
            Guid userGuid = Guid.Parse(Read("userGuid"));
            var userDto = _userApiService.GetUserDto(userGuid);
            Set("accountsPosition", 0.ToString());
            try
            {
                _userApiService.CreateMoneyAccountForUser(accountName, userGuid);
                return LoadOverview(string.Empty);
            }
            catch (Exception e)
            {
                return LoadOverview(MessageHandler(e, "Unknown Error: CreateAccount"));
            }
        }

        public IActionResult DeleteAccount(Guid accountGuid)
        {
            Guid userGuid = Guid.Parse(Read("userGuid"));
            var userDto = _userApiService.GetUserDto(userGuid);
            Set("accountsPosition", 0.ToString());
            try
            {
                _userApiService.DeleteAccount(userDto.UserGuid, accountGuid);
                return LoadOverview(string.Empty);
            }
            catch (Exception e)
            {
                return LoadOverview(MessageHandler(e, "Unknown Error: DeleteAccount"));
            }
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
                return LoadAccountView(accountGuid, string.Empty);
            }
            catch (Exception e)
            {
                return LoadLoginView(MessageHandler(e, $"Unknown Error: {nameof(PaginateAccount)}"));
            }
        }

        public IActionResult CreateTransaction(Guid accountGuid, float itemCost, string itemName, DateTime dateTime)
        {
            // what to do if read fails
            Guid userGuid = Guid.Parse(Read("userGuid"));
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
            Guid userGuid = Guid.Parse(Read("userGuid"));
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