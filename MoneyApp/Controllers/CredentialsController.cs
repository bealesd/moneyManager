using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyApp.Authentication;
using MoneyApp.Interfaces;

namespace MoneyApp.Controllers
{
    [Route("api/[Controller]")]
    public class CredentialsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IAdapterRepo _adapterRepo;

        public CredentialsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAdapterRepo adapterRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _adapterRepo = adapterRepo;
        }

        [HttpPost()]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            var user = new ApplicationUser { UserName = model.Username };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                _adapterRepo.CreateUser(user.NormalizedUserName);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            return BadRequest("Couldn't create account.");
        }

        [HttpGet()]
        public async Task<IActionResult> LoginUser(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == model.Username);
            if (result.Succeeded)
                return new JsonResult(_adapterRepo.GetUser(user.NormalizedUserName));

            return BadRequest("Couldn't log in.");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> LogoutUser()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()//delete user threw up an error message!
        {
            var username =_signInManager.Context.User.Identity.Name;
            var user = _userManager.Users.FirstOrDefault(u => u.UserName == username);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _adapterRepo.DeleteUser(user.NormalizedUserName);
                return Ok();
            }
            return BadRequest("Couldn't delete user.");
        }
    }
}