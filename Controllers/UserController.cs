using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;
using Microsoft.AspNetCore.Http;

namespace pizzashop.Controllers;
public class UserController : Controller
{

    private readonly PizzashopMainContext _db;

    public UserController(PizzashopMainContext db)
    {
        _db = db;
    }

    // GET: HomeController/Login
    public IActionResult Login()
    {

        return View();
    }

    // POST: HomeController/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(User user)

    {

        if (ModelState.IsValid)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Email == user.Email );

            if(existingUser != null)
            {
                if(existingUser.Password == user.Password)
                {   TempData["success"] = "Login Successful";
                    Console.WriteLine(existingUser.Id.ToString());
                    HttpContext.Session.SetString("UserId", existingUser.Id.ToString());
                    ViewData["UserId"] = HttpContext.Session.GetString("UserId");
                    return RedirectToAction("Index", "Home");
                }
                else{
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                }
            }
            else{
            
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                
            }
        }

        return View(user);
        
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
