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

        // GET api/user
        [HttpGet]
        public IActionResult GetUsers()
        {
            Console.WriteLine(DateTime.Now);
            return new ObjectResult(_adapterRepo.GetAllUsers());
        }

        // GET api/user/dave
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

        // POST api/user
        [HttpPost("{username}")]
        public IActionResult PostUser(string username)
        {
            try
            {
                _adapterRepo.AddUser(username);
                return RedirectToAction(nameof(GetUser), new { username });
            }
            catch (Exception)
            {
                return BadRequest("Could Not Create User");
            }
        }

        // PUT api/user/username/accountname
        [HttpPost("{username}/{accountName}")]
        public IActionResult CreateMoneyAccount(string username, string accountName)
        {
            try
            {
                _adapterRepo.AddNewAccount(username, accountName);
                return RedirectToAction(nameof(GetMoneyAccount), new { username, accountName });
            }
            catch (Exception)
            {
                return BadRequest("Could Not Create Account");
            }
        }

        [HttpGet("{username}/{accountName}")]
        public IActionResult GetMoneyAccount(string username, string accountName)
        {
            try
            {
                var account = _adapterRepo.GetAccount(username, accountName);
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
                if (_adapterRepo.RemoveAccount(username, accountName))
                    return RedirectToAction(nameof(GetUser), new {username});
                return BadRequest("Could Not Delete Account");
            }
            catch (Exception)
            {
                return BadRequest("Could Not Delete Account");
            }
        }

        [HttpPost("{username}/{accountName}/addMoneyItem")]//{ "ItemName": "PS1","ItemCost": "200.0", "DateTime": "2017-12-13T15:10:43.511Z" }
        public IActionResult AddMoneySpentItem(string username, string accountName, [FromBody] MoneySpentItemDto model)
        {
            var account = _adapterRepo.AddMoneySpentItem(username, accountName, model.ItemName, model.ItemCost, model.DateTime);
            return new ObjectResult(account);
        }

        [HttpDelete("{username}/{accountName}/{moneyItemGuid}")]
        public IActionResult RemoveMoneySpentItem(string username, string accountName, Guid moneyItemGuid)
        {
            var account = _adapterRepo.RemoveMoneySpentItem(username, accountName, moneyItemGuid);
            return new ObjectResult(account);
        }
    }
}
