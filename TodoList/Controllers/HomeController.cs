using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoList.Models;
using Users.Data;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserListContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(UserListContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "TodoItems");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
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
