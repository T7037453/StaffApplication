using Microsoft.AspNetCore.Mvc;
using StaffApplication.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;

namespace StaffApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User != null)
            {

                List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role).ToList();
                var roles = new List<string>();

                foreach (var role in roleClaims)
                {
                    roles.Add(role.Value);
                }
            }
            


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}