using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using MoneyApp.Dto;
using MoneyApp.Interfaces;
using MoneyApp.Models;
using MoneyApp.Repos;
using Newtonsoft.Json.Linq;

namespace MoneyApp.Controllers
{
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private IAdapterRepo _adapterRepo;

        public UserController( IAdapterRepo adapterRepo)
        {
            _adapterRepo = adapterRepo;
        }

        //api/user
        [HttpGet]
        public IActionResult GetUsers()
        {
            Console.WriteLine(DateTime.Now);
            return new ObjectResult(_adapterRepo.GetAllUsers());
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
        [HttpPost("{username}")]
        public IActionResult PostUser(string username)
        {
            try
            {
                var result =_adapterRepo.CreateUser(username);
                if (result)
                    return RedirectToAction(nameof(GetUser), new { username });
                return BadRequest("Could Not Create User");
            }
            catch (Exception)
            {
                return BadRequest("Could Not Create User");
            }
        }

        [HttpDelete("{userGuid}")]
        public IActionResult DeleteUser(Guid userGuid)
        {
            var result = _adapterRepo.DeleteUser(userGuid);
            if (result)
                return RedirectToAction(nameof(GetUsers));
            return BadRequest("Could Not Delete User");
        }

        // PUT api/user/username/accountname
        [HttpPost("{userGuid}/{accountName}")]
        public IActionResult CreateMoneyAccount(Guid userGuid, string accountName)
        {
            try
            {
                if (_adapterRepo.CreateMoneyAccountForUser(userGuid, accountName))
                    return RedirectToAction(nameof(GetUser), new { userGuid });
                return BadRequest("Could Not Create Account");
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
                var account = _adapterRepo.GetMoneyAccount(accountGuid);
                return true ? new ObjectResult(account) : BadRequest("Could Not Get Account");
                return account != null ? new ObjectResult(account) : BadRequest("Could Not Get Account");
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
                if (_adapterRepo.RemoveMoneyAccountFromUser(userGuid, accountGuid))
                    return RedirectToAction(nameof(GetUser), new { userGuid});
                return BadRequest("Could Not Delete Account");
            }
            catch (Exception)
            {
                return BadRequest("Could Not Delete Account");
            }
        }

        [HttpPost("account/{accountGuid}")]//{ "ItemName": "PS1","ItemCost": "200.0", "DateTime": "2017-12-13T15:10:43.511Z" }
        public IActionResult AddMoneySpentItem(Guid accountGuid, [FromBody] MoneySpentItemDto model)
        {
            var account = _adapterRepo.CreateMoneySpentItem(accountGuid, model.ItemName, model.ItemCost, model.DateTime);
            return new ObjectResult(account);
        }

        [HttpDelete("account/{accountGuid}/{moneyItemGuid}")]
        public IActionResult DeleteMoneySpentItem(Guid accountGuid, Guid moneyItemGuid)
        {
            var account = _adapterRepo.DeleteMoneySpentItem(accountGuid, moneyItemGuid);
            return new ObjectResult(account);
        }
    }
}
