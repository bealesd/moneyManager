using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MoneyApp.Controllers
{
    public class UserManagementController : Controller
    {
        public UserManagementController()
        {
            
        }

        public IActionResult Index()
        {
            return View("Index", null);
        }
    }
}
