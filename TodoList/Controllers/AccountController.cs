using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Data;

namespace TodoList.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserListContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(UserListContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == username && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim("Id", user.Id.ToString())
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync("Cookies", principal).Wait();
                ViewBag.Email = "Fff";
                ViewData["Email"] = "fff";
                return RedirectToAction("Index", "TodoItems", new { email = username });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password!");
                return View();
            }
        }

        private string GenerateJwtToken(string username, string id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("Id", id)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("token");

            HttpContext.SignOutAsync("Cookies").Wait();

            return RedirectToAction("Login", "Account");
        }
    }
}
