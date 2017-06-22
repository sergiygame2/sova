using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportApp.Models;

namespace SportApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId =  HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                ViewData["LoggedIn"] = true;
                ViewData["CurrentUserName"] = user.FullName;
                ViewData["CurrentUserLogin"] = user.UserName;
            }
            else
            {
                ViewData["LoggedIn"] = false;
                ViewData["CurrentUserName"] = ""; 
                ViewData["CurrentUserLogin"] = "";
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
