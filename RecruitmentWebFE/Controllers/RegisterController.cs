using Microsoft.AspNetCore.Mvc;
using RecruitmentWebFE.Models;

namespace RecruitmentWebFE.Controllers
{
    public class RegisterController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            return RedirectToAction("Index", "Login");
        }



    }
}
