using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using RecruitmentWebFE.Models;

namespace RecruitmentWebFE.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int? id)
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult _CommonPartialView([FromBody] PageModel model)
    {
        if(!ModelState.IsValid)
        {
            return Json(new { success = "Call fail !!" });
        }    
        return Json(new { success = "Call successfully" });
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
