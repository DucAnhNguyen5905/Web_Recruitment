using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RecruitmentWebFE.Models;
using RecruitmentWebFE.Services;

namespace RecruitmentWebFE.Controllers
{
    public class LoginController : BaseController
    {
        private readonly EmployerService _service;

        public LoginController(EmployerService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var isLoggedIn = HasCookie("AccessToken");
            if (isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.Login(model.Email, model.Password);

            if (result == null || string.IsNullOrWhiteSpace(result.token))
            {
                ModelState.AddModelError(string.Empty, "Đăng nhập thất bại.");
                return View(model);
            }

            Response.Cookies.Append("AccessToken", result.token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false, 
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.employer_ID.ToString()),
                new Claim(ClaimTypes.Email, result.Email ?? ""),
                new Claim(ClaimTypes.Name, result.Email ?? "")
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });

            return RedirectToAction("Index", "Home");
        }


    }
}