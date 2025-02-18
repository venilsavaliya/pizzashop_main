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

    public UserController(PizzashopMainContext db, IEmailService emailService,JwtServices jwtService)
    {
        _db = db;
        _emailService = emailService;
        _jwtService =jwtService;
    }

    // GET: UserController/Login
    public IActionResult Login()
    {
        if (Request.Cookies["UserEmail"] != null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    // POST: UserController/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
  public async Task<IActionResult> Login(LoginViewModel user)  
    // if (ModelState.IsValid)
    {
        var existingUser = _db.Users.FirstOrDefault(u => u.Email == user.Email);


        if (existingUser != null)
        {
             
            if ( PasswordUtility.VerifyPassword(user.Password,existingUser.Password))
            {
                TempData["success"] = "Login Successful";
                 var token = _jwtService.GenerateJwtToken(existingUser.Email,"Admina");
                 Console.WriteLine(token);

                // HttpContext.Session.SetString("UserId", existingUser.Id.ToString());
                // ViewData["UserId"] = HttpContext.Session.GetString("UserId");
                // if (user.RememberMe == true)
                {
                    Response.Cookies.Append("jwt",token, new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(7),
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });
                }

                Console.WriteLine("ok done");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
            }
        }
        else
        {

            ModelState.AddModelError(string.Empty, "Invalid email or password.");

        }
        return View(user);
    }

    // GET: UserController/ForgotPassword
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // POST: UserController/ForgotPassword

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var existingUser = _db.Users.FirstOrDefault(u => u.Email == email);

        var urii = Url.Action("ResetPassword", "User", new { email = email }, Request.Scheme);

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
            return View();
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid email.");
        }

        return View();
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
        var User = _db.Users.FirstOrDefault(u => u.Email == Obj.Email);

        if (User == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email.");
            return View();
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
}
