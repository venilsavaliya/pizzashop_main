using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;

namespace pizzashop.Controllers;

// [ApiController]
// [Route("api/[controller]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PizzashopMainContext _db;

    public HomeController(ILogger<HomeController> logger,PizzashopMainContext db)
    {
        _logger = logger;
        _db = db;
    }

    // [HttpGet("Index")]
      [Authorize]
    public IActionResult Index()
    {
       string userEmail = Request.Cookies["UserEmail"];

        // if (string.IsNullOrEmpty(userEmail))
        // {
        //     return RedirectToAction("Login", "User");
        // }
        // ViewBag.Email = userEmail;
        return View();
    }
   
    public IActionResult Privacy()
    {
        return View();
    }

    
    

   
}
