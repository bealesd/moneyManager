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
        [HttpGet("{username}")]
        public IActionResult GetUser(string username)
        {
            try
            {
                var user = _adapterRepo.GetUser(username);
                return user == null ? BadRequest("Could Not Get User") : new ObjectResult(_adapterRepo.GetUser(username));
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

        [HttpDelete("{username}")]
        public IActionResult DeleteUser(string username)
        {
            var result = _adapterRepo.DeleteUser(username);
            if (result)
                return RedirectToAction(nameof(GetUsers));
            return BadRequest("Could Not Delete User");
        }

        // PUT api/user/username/accountname
        [HttpPost("{username}/{accountName}")]
        public IActionResult CreateMoneyAccount(string username, string accountName)
        {
            try
            {
                _adapterRepo.CreateMoneyAccount(username, accountName);
                return RedirectToAction(nameof(GetMoneyAccount), new { username, accountName });
            }
            catch (Exception)
            {
                return BadRequest("Could Not Create Account");
            }
        }

        [HttpGet("{username}/{accountName}")]
        public IActionResult GetMoneyAccount(string username, Guid accountGuid)
        {
            try
            {
                var account = _adapterRepo.GetMoneyAccount(username, accountGuid);
                return true ? new ObjectResult(account) : BadRequest("Could Not Get Account");
                return account != null ? new ObjectResult(account) : BadRequest("Could Not Get Account");
            }
            catch (Exception)
            {
                return BadRequest("Could Not Get Account");
            }
        }

        // api/user/username/accountname/delete?isDelete=true
        [HttpDelete("{username}/{accountName}")]
        public IActionResult DeleteMoneyAccount(string username, string accountName)
        {
            try
            {
                if (_adapterRepo.DeleteMoneyAccount(username, accountName))
                    return RedirectToAction(nameof(GetUser), new {username});
                return BadRequest("Could Not Delete Account");
            }
            catch (Exception)
            {
                return BadRequest("Could Not Delete Account");
            }
        }

        [HttpPost("{username}/{accountName}/addMoneyItem")]//{ "ItemName": "PS1","ItemCost": "200.0", "DateTime": "2017-12-13T15:10:43.511Z" }
        public IActionResult AddMoneySpentItem(string username, Guid accountGuid, [FromBody] MoneySpentItemDto model)
        {
            var account = _adapterRepo.CreateMoneySpentItem(username, accountGuid, model.ItemName, model.ItemCost, model.DateTime);
            return new ObjectResult(account);
        }

        [HttpDelete("{username}/{accountName}/{moneyItemGuid}")]
        public IActionResult DeleteMoneySpentItem(string username, Guid accountGuid, Guid moneyItemGuid)
        {
            var account = _adapterRepo.DeleteMoneySpentItem(username, accountGuid, moneyItemGuid);
            return new ObjectResult(account);
        }
    }
}
