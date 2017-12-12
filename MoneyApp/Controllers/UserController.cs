using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using MoneyApp.Interfaces;
using MoneyApp.Models;
using MoneyApp.Repos;

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
            return new ObjectResult(_adapterRepo.GetAllUsers());
        }

        // GET api/user/dave
        [HttpGet("{username}")]
        public IActionResult GetUser(string username)
        {
            try
            {
                return new ObjectResult(_adapterRepo.GetUser(username));
            }
            catch (Exception)
            {
                return BadRequest();
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
                return BadRequest();
            }
        }

        // PUT api/user/username/accountname
        [HttpPost("{username}/{accountName}")]
        public IActionResult CreateMoneyAccount(string username, string accountName)
        {
            try
            {
                _adapterRepo.AddAccount(username, accountName);
                return RedirectToAction(nameof(GetMoneyAccount), new { username, accountName });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{username}/{accountName}")]
        public IActionResult GetMoneyAccount(string username, string accountName)
        {
            try
            {
                return new ObjectResult(_adapterRepo.GetAccount(username, accountName));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // api/user/username/accountname/delete?isDelete=true
        [HttpGet("{username}/{accountName}/{delete}")]
        public IActionResult DeleteMoneyAccount(string username, string accountName, [FromQuery] bool isDelete)
        {
            if (isDelete)
            {
                _adapterRepo.RemoveAccount(username, accountName);
                return RedirectToAction(nameof(GetUser), new { username });
            }
            return BadRequest();
        }

        //// PUT api/user/username/accountname
        //[HttpPut("{username}/{accountName}")]
        //public IActionResult Put(string username, string accountName)
        //{

        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
