using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyApp.Models;
using MoneyApp.Repos;

namespace MoneyApp.Controllers
{
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private IUserRepo _userRepo;
        private IAccountRepo _accountRepo;

        public UserController(IUserRepo userRepo, IAccountRepo accountRepo)
        {
            _userRepo = userRepo;
            _userRepo.Load();
            _accountRepo = accountRepo;
            _accountRepo.Load();
        }

        // GET api/user
        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(_userRepo.GetAllUsers());
        }

        // GET api/user/dave
        [HttpGet("{username}")]
        public IActionResult Get(string username)
        {
            if (username == String.Empty)
            {
                return BadRequest();
            }

            Guid userGuid = _userRepo.GetUser(username);
            if (userGuid == Guid.Empty)
                return BadRequest();

            return new ObjectResult(userGuid);
        }

        // POST api/user
        [HttpPost("{username}")]
        public IActionResult Post(string username)
        {
            if (username == null)
            {
                return BadRequest();
            }

            _userRepo.AddUser(username);
            return RedirectToAction("Get", new { username });
        }

        // PUT api/user/username/accountname
        [HttpPost("{username}/{accountName}")]
        public IActionResult CreateMoneyAccount(string username, string accountName)
        {
            Guid userGuid = _userRepo.GetUser(username);
            if (userGuid == Guid.Empty)
                return BadRequest();

            if (_accountRepo.GetAccount(accountName) != null)
            {
                return BadRequest();
            }

            _accountRepo.CreateAccount(accountName, userGuid);
            return RedirectToAction("GetMoneyAccount", new { username, accountName });
        }

        [HttpGet("{username}/{accountName}")]
        public IActionResult GetMoneyAccount(string username, string accountName)
        {
            Guid userGuid = _userRepo.GetUser(username);
            if (userGuid == Guid.Empty)
                return BadRequest();

            Account account = _accountRepo.GetAccount(accountName);
            if (account == null)
                return BadRequest();

            return new ObjectResult(account);
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
