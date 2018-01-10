using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyApp.Authentication;
using MoneyApp.Dto;
using MoneyApp.Interfaces;

namespace MoneyApp.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private IAdapterRepo _adapterRepo;

        public UserController(IAdapterRepo adapterRepo)
        {
            _adapterRepo = adapterRepo;
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            try
            {
                var user = _adapterRepo.GetUser(this.GetUsername());
                return new JsonResult(user);
            }
            catch (Exception)
            {
                return BadRequest("Could Not Find User");
            }
        }

        [HttpPost("account/{accountName}")]
        public IActionResult CreateMoneyAccount(string accountName)
        {
            try
            {
                _adapterRepo.CreateMoneyAccountForUser(this.GetUsername(), accountName);
                return GetUser();
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
                return new JsonResult(_adapterRepo.GetMoneyAccount(accountGuid));
            }
            catch (Exception)
            {
                return BadRequest("Could Not Get Account");
            }
        }

        [HttpDelete("{accountGuid}")]
        public IActionResult RemoveMoneyAccountFromUser(Guid accountGuid)
        {
            try
            {
                _adapterRepo.RemoveAccount(this.GetUsername(), accountGuid);
                return GetUser();
            }
            catch (Exception)
            {
                return BadRequest("Could Not Delete Account");
            }
        }


        [HttpPost("account/moneySpentItem/{accountGuid}")]
        public IActionResult AddMoneySpentItem(Guid accountGuid, [FromBody] MoneySpentItemDto model)
        {
            try
            {
                _adapterRepo.CreateTransaction(accountGuid, model.ItemName, model.ItemCost, model.DateTime);
                return GetMoneyAccount(accountGuid);
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
                _adapterRepo.DeleteTransaction(accountGuid, moneyItemGuid);
                return GetMoneyAccount(accountGuid);
            }
            catch (Exception)
            {
                return BadRequest("Could Not Delete Money Item");
            }
        }

        private string GetUsername()
        {
            return HttpContext.User.Identity.Name.ToUpper();
        }
    }
}