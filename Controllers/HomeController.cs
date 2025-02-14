using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;

namespace pizzashop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
       string userEmail = Request.Cookies["UserEmail"];

        // if (string.IsNullOrEmpty(userEmail))
        // {
        //     return RedirectToAction("Login", "User");
        // }

        ViewBag.Email = userEmail;
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
