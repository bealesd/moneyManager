using System;
using Microsoft.AspNetCore.Mvc;
using MoneyApp.Dto;
using MoneyApp.Interfaces;

namespace MoneyApp.Controllers
{
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private IAdapterRepo _adapterRepo;
        private IUserLogin _userLogin;

        public UserController(IAdapterRepo adapterRepo, IUserLogin userLogin)
        {
            _adapterRepo = adapterRepo;
            _userLogin = userLogin;
        }

        //api/user
        [HttpGet]
        public IActionResult GetUsers()
        {
            return new ObjectResult(_adapterRepo.GetAllUsers());
        }

        [HttpGet("{username}/{password}")]
        public IActionResult GetUserGuid(string username, string password)
        {
            try
            {
                var userGuid = _userLogin.GetUserGuid(username, password);
                return userGuid == Guid.Empty ? BadRequest("Could Not Get User Guid") : new ObjectResult(userGuid);
            }
            catch (Exception)
            {
                return BadRequest("Could Not Get User Guid");
            }
        }

        //api/user/dave
        [HttpGet("{userGuid}")]
        public IActionResult GetUser(Guid userGuid)
        {
            try
            {
                var user = _adapterRepo.GetUser(userGuid);
                return user == null ? BadRequest("Could Not Get User") : new ObjectResult(user);
            }
            catch (Exception)
            {
                return BadRequest("Could Not Get User");
            }
        }

        //api/user/dave
        [HttpPost("{username}/{password}")]
        public IActionResult PostUser(string username, string password)
        {
            try
            {
                _userLogin.CreateUser(username, password);
                var userGuid = _userLogin.GetUserGuid(username, password);
                _adapterRepo.CreateUser(username, userGuid);
                return RedirectToAction(nameof(GetUser), new { userGuid });
            }
            catch (Exception)
            {
                return BadRequest("Could Not Create User");
            }
        }

        [HttpDelete("{userGuid}")]
        public IActionResult DeleteUser(Guid userGuid)
        {
            try
            {
                _userLogin.DeleteUser(userGuid);
                _adapterRepo.DeleteUser(userGuid);
                return RedirectToAction(nameof(GetUsers));
            }
            catch (Exception)
            {
                return BadRequest("Could Not Delete User");
            }
        }

        [HttpPost("account/{userGuid}/{accountName}")]
        public IActionResult CreateMoneyAccount(string accountName, Guid userGuid)
        {
            try
            {
                var user = _adapterRepo.GetUser(userGuid);
                _adapterRepo.CreateMoneyAccountForUser(user.UserGuid, accountName);
                return RedirectToAction(nameof(GetUser), new { userGuid });
            }
            catch (Exception)
            {
                return BadRequest("Could Not Create Account");
            }
        }

        [HttpGet("account/{accountGuid}")]
        public IActionResult GetMoneyAccount(Guid accountGuid)
        {
            try
            {
                return new ObjectResult(_adapterRepo.GetMoneyAccount(accountGuid));
            }
            catch (Exception)
            {
                return BadRequest("Could Not Get Account");
            }
        }

        // api/user/username/accountname/delete?isDelete=true
        [HttpDelete("{userGuid}/{accountGuid}")]
        public IActionResult RemoveMoneyAccountFromUser(Guid userGuid, Guid accountGuid)
        {
            try
            {
                _adapterRepo.RemoveMoneyAccountFromUser(userGuid, accountGuid);
                return RedirectToAction(nameof(GetUser), new { userGuid });
            }
            catch (Exception)
            {
                return BadRequest("Could Not Delete Account");
            }
        }


        [HttpPost("account/{accountGuid}")]//{ "ItemName": "PS1","ItemCost": "200.0", "DateTime": "2017-12-13T15:10:43.511Z" }
        public IActionResult AddMoneySpentItem(Guid accountGuid, [FromBody] MoneySpentItemDto model)
        {
            try
            {
                _adapterRepo.CreateMoneySpentItem(accountGuid, model.ItemName, model.ItemCost, model.DateTime);
                return RedirectToAction(nameof(GetMoneyAccount), accountGuid);
            }
            catch (Exception)
            {
                return BadRequest("Could Not Create Money Item");
            }
        }

        [HttpDelete("account/{accountGuid}/{moneyItemGuid}")]
        public IActionResult DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            try
            {
                _adapterRepo.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
                return RedirectToAction(nameof(GetMoneyAccount), accountGuid);
            }
            catch (Exception)
            {
                return BadRequest("Could Not Delete Money Item");
            }
        }
    }
}
