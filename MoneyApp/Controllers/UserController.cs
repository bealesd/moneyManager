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

        public UserController(IAdapterRepo adapterRepo)
        {
            _adapterRepo = adapterRepo;
        }

        //api/user
        [HttpGet]
        public IActionResult GetUsers()
        {
            return new ObjectResult(_adapterRepo.GetAllUsers());
        }

        //api/user/dave
        [HttpGet("{username}")]
        public IActionResult GetUser(string username)
        {
            try
            {
                var user = _adapterRepo.UserLogin(username);
                return user == null ? BadRequest("Could Not Get User") : new ObjectResult(user);
            }
            catch (Exception)
            {
                return BadRequest("Could Not Get User");
            }
        }

        //api/user/dave
        [HttpPost("{username}")]
        public IActionResult PostUser(string username)
        {
            try
            {
                _adapterRepo.CreateUser(username);
                return RedirectToAction(nameof(GetUser), new { username });
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
                _adapterRepo.DeleteUser(userGuid);
                return RedirectToAction(nameof(GetUsers));
            }
            catch (Exception)
            {
                return BadRequest("Could Not Delete User");
            }
        }

        // PUT api/user/username/accountname
        [HttpPost("{userGuid}/{accountName}")]
        public IActionResult CreateMoneyAccount(Guid userGuid, string accountName)
        {
            try
            {
                _adapterRepo.CreateMoneyAccountForUser(userGuid, accountName);
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
            if (!ModelState.IsValid)
            {
                return BadRequest("Could Not Create Money Item");
            }
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
