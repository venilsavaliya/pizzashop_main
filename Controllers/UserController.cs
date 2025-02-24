using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;
using Microsoft.AspNetCore.Http;
using pizzashop.ViewModel;
using System.Threading.Tasks;
using PizzaShop.Services;


namespace pizzashop.Controllers;
public class UserController : Controller
{
    private readonly PizzashopMainContext _db;
    private readonly IEmailService _emailService;

    private readonly JwtServices _jwtService;

    public UserController(PizzashopMainContext db, IEmailService emailService, JwtServices jwtService)
    {
        _db = db;
        _emailService = emailService;
        _jwtService = jwtService;
    }

    // GET: UserController/Login
    public IActionResult Login()
    {
        if (Request.Cookies["jwt"] != null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    // POST: UserController/Login ********************************************************************************
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel user)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _db.Users.FirstOrDefault(u => u.Email == user.Email);

            if (existingUser != null)
            {
                if (PasswordUtility.VerifyPassword(user.Password, existingUser.Password))
                {
                    TempData["success"] = "Login Successful";

                    // Getting Role of the user from the database
                    var roleid = _db.Userdetails.FirstOrDefault(u => u.UserId == existingUser.Id);
                    string role = _db.Roles.FirstOrDefault(u => u.Roleid == roleid.RoleId)?.Name ?? "User";

                    // setting jwt token which have email and role of the user
                    var token = _jwtService.GenerateJwtToken(existingUser.Email, role);

                    // if remember me is checked then set the cookie for 10 days
                    if (user.RememberMe)
                    {
                        Response.Cookies.Append("jwt", token, new CookieOptions
                        {
                            Expires = DateTime.UtcNow.AddDays(10),
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        });
                    }
                    else
                    {

                        // if remember me is not checked then set the cookie for the current browser session 
                        Response.Cookies.Append("jwt", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict
                        });
                    }

                    var _user = _db.Userdetails.FirstOrDefault(u => u.UserId == existingUser.Id);
                    string username = _user?.UserName ?? "User";
                    string url = _user?.Profile ?? "User";




                    Response.Cookies.Append("UserName", username, new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                    Response.Cookies.Append("ProfileUrl", url, new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                    Response.Cookies.Append("email", existingUser.Email, new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("InvalidCredentials", "Invalid email or password.");
                }
            }
            else
            {

                ModelState.AddModelError("InvalidCredentials", "Invalid email or password.");

            }
            return View(user);
        }
        else
        {
            return View(user);
        }
    }


    // GET: UserController/ForgotPassword
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // POST: UserController/ForgotPassword********************************************************************************

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {

            var existingUser = _db.Users.FirstOrDefault(u => u.Email == model.Email);

            var urii = Url.Action("ResetPassword", "User", new { email = model.Email }, Request.Scheme);

            if (existingUser != null)
            {
                await _emailService.SendEmailAsync("tatva.pce175@outlook.com", "Reset Password", @$"<div class='main-container' style='background-color: rgb(223, 223, 223); font-family: Arial, sans-serif; padding-bottom: 20px;'>

        <div class='head-section' >
            <h1 style='background-color: rgb(72, 108, 185); color: white; text-align: center;padding: 2% 0%;'>PIZZASHOP</h1>
        </div>
        <div style='margin:10px 5px;'>Pizza Shop,</div>
        <div style='margin:15px 5px;'>Please Click <a href={urii}>here</a> to reset your account password.</div>
        <div style='margin:15px 5px;'>If you have any issue or have any question, please do not hesitate to contact our support team.</div>
        <div style='margin:15px 5px;margin-bottom: 20px;'> <span style='color: rgb(207, 207, 62);'>Important Note:</span> For security Reason this link will expires in next 24 hours. if you dit not request a password reset than ignore this or contact our support Team immediatly. </div>

    </div>");
                // TempData["success"] = "Password reset link has been sent to your email.";
                ViewBag.Message = "Password reset link has been sent to your email.";
                
            }
            else
            {
                ModelState.AddModelError("InvalidEmail", "Invalid email.");
            }

            return View(model);
        }
        else
        {
            ModelState.AddModelError("InvalidEmail", "Invalid email.");
            return View(model);
        }

    }

    // Get: UserController/ResetPassword
    public IActionResult ResetPassword(string email)
    {
        ViewData["email"] = email;
        return View();
    }

    // POST: UserController/ResetPassword
    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordviewModel Obj)
    {
        if(!ModelState.IsValid)
        {
            return View(Obj);
        }
        var User = _db.Users.FirstOrDefault(u => u.Email == Obj.Email);

        if (User == null)
        {
            return View();
        }

        if(Obj.Password != Obj.ConfirmPassword)
        {
            ModelState.AddModelError("CustomeError", "Password and Confirm Password does not match.");
            return View(Obj);
        }
        string hashedPassword = PasswordUtility.HashPassword(Obj.Password);
        User.Password = hashedPassword;
        _db.Users.Update(User);
        _db.SaveChanges();
        ViewBag.Message = "Password has been reset successfully.";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // get: UserController/Logout
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        Response.Cookies.Delete("email");
        Response.Cookies.Delete("UserName");
        Response.Cookies.Delete("ProfileUrl");
        
        return RedirectToAction("Login", "User");
    }
}
