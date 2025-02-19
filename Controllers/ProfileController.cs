using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;
using Microsoft.AspNetCore.Http;
using pizzashop.ViewModel;
using System.Threading.Tasks;
using PizzaShop.Services;


namespace pizzashop.Controllers;
public class ProfileController : Controller
{
    private readonly PizzashopMainContext _db;

    public ProfileController(PizzashopMainContext db)
    {
        _db = db;
      
    }


    // get: Profile/Profile

    public IActionResult Profile()
    {
        return View();
    }

    // post: Profile/Profile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(Userdetail user)
    {
        if (ModelState.IsValid)
        {
          
            return RedirectToAction("Profile", "Profile");
        }
        else{
            return RedirectToAction("Profile", "Profile");
        }
    }

   
}
