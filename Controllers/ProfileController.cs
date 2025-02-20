using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;
using pizzashop.ViewModel;
using Microsoft.AspNetCore.Http;
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


    // post: Profile/Profile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUserProfile(Userdetail user)
    {
            var statename = _db.States.FirstOrDefault(s => s.StateId.ToString() == user.State);
            var countryname = _db.Countries.FirstOrDefault(c => c.CountryId.ToString() == user.Country); 
            var cityname = _db.Cities.FirstOrDefault(c => c.CityId.ToString() == user.City);

            user.State = statename?.StateName;
            user.Country = countryname?.CountryName;
            user.City = cityname?.CityName;

            _db.Userdetails.Update(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("Profile", "Profile");
   
    }


    // get: Profile/ChangePassword

    public IActionResult ChangePassword()
    {
        return View();
    }

    // post: Profile/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var email = Request.Cookies["email"];
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                if (PasswordUtility.VerifyPassword(model.OldPassword, user.Password))
                {   
                  
                    user.Password = PasswordUtility.HashPassword(model.NewPassword);
                    _db.Users.Update(user);
                    _db.SaveChanges();
                    TempData["success"] = "Password changed successfully";
                }
                else
                {
                    TempData["error"] = "Old password is incorrect";
                }
            }
            else
            {
                TempData["error"] = "User not found";
            }
        }
        return View();
    }


}
