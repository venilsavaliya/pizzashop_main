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

    public IActionResult Profile()
    {
        var username = Request.Cookies["UserName"];
    
    if (username != null)
    {
        // Fetch the user data from the database
        var user = _db.Userdetails.FirstOrDefault(u => u.UserName == username);
        
        if (user != null)
        {
            // Pass user data to the view
            return View(user);
        }
        else
        {
            // Redirect to login if user is not found
            return RedirectToAction("Login", "User");
        }
    }
    else
    {
        // Redirect to login if cookie is missing
        return RedirectToAction("Login", "User");
    }
    }

    

   
}
