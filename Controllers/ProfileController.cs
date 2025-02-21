using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;
using pizzashop.ViewModel;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using PizzaShop.Services;
using Microsoft.EntityFrameworkCore;


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


    // get : userlist from database

    public async Task<IActionResult> UserList(string sortColumn , string sortOrder,int pageNumber = 1, int pagesize = 2, string searchKeyword = "")
    {
        searchKeyword=searchKeyword.ToLower();
        var users = _db.Userdetails.OrderBy(u => u.Createdby).ToList();
        var count = await _db.Userdetails.CountAsync();

        var query = from u in _db.Userdetails
                    join user in _db.Users on u.UserId equals user.Id
                    join role in _db.Roles on u.RoleId equals role.Roleid
                    select new UserListViewModel
                    {
                        Name = u.FirstName + " " + u.LastName,
                        Email = user.Email,
                        Role = role.Name,
                        Status = u.Status ? "Active" : "Inactive",
                        Phone = u.Phone
                    };

        if (!string.IsNullOrEmpty(searchKeyword))
        {
            // var search_query = from u in _db.Userdetails
            //                    join user in _db.Users on u.UserId equals user.Id
            //                    join role in _db.Roles on u.RoleId equals role.Roleid
            //                    where u.FirstName.Contains(searchKeyword) ||
            //                        u.LastName.Contains(searchKeyword) ||
            //                        user.Email.Contains(searchKeyword) ||
            //                        role.Name.ToLower().Contains(searchKeyword) ||
            //                        u.Phone.Contains(searchKeyword)
            //                    select new UserListViewModel
            //                    {
            //                        Name = u.FirstName + " " + u.LastName,
            //                        Email = user.Email,
            //                        Role = role.Name,
            //                        Status = u.Status ? "Active" : "Inactive",
            //                        Phone = u.Phone
            //                    };

            // var search_user = search_query.OrderBy(u => u.Name).ToList();

            // ViewBag.currentPage = pageNumber;
            // ViewBag.TotalPages = (int)Math.Ceiling((double)count / pagesize);
            // ViewBag.TotalCount = count;
            // ViewBag.startIndex = (pageNumber - 1) * pagesize + 1;
            // ViewBag.endIndex = (pageNumber - 1) * pagesize + pagesize;
            // ViewBag.pageSize = pagesize;
            // return View(search_user);

            query = query.Where(u => u.Name.ToLower().Contains(searchKeyword) ||
                                 u.Email.ToLower().Contains(searchKeyword) ||
                                 u.Role.ToLower().Contains(searchKeyword) ||
                                 u.Phone.Contains(searchKeyword));

            ViewBag.searchKeyword=searchKeyword;
        }

        // 🔹 Sorting Logic
        if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortOrder))
        {
            switch (sortColumn)
            {
                case "Name":
                    query = sortOrder == "asc" ? query.OrderBy(u => u.Name) : query.OrderByDescending(u => u.Name);
                    break;
                case "Role":
                    query = sortOrder == "asc" ? query.OrderBy(u => u.Role) : query.OrderByDescending(u => u.Role);
                    break;
            }
        }

        //  Query for combining three tables and get common data 
        // var query = from u in _db.Userdetails
        //             join user in _db.Users on u.UserId equals user.Id
        //             join role in _db.Roles on u.RoleId equals role.Roleid
        //             select new UserListViewModel
        //             {
        //                 Name = u.FirstName + " " + u.LastName,
        //                 Email = user.Email,
        //                 Role = role.Name,
        //                 Status = u.Status ? "Active" : "Inactive",
        //                 Phone = u.Phone
        //             };

        // counting total users in data base


        // pagination query
        // var userlist = query.OrderBy(u => u.Name).Skip((pageNumber - 1) * pagesize).Take(pagesize).ToList();

        // ViewBag.currentPage = pageNumber;
        // ViewBag.TotalPages = (int)Math.Ceiling((double)count / pagesize);
        // ViewBag.TotalCount = count;
        // ViewBag.startIndex = (pageNumber - 1) * pagesize + 1;
        // ViewBag.endIndex = (pageNumber - 1) * pagesize + pagesize;
        // ViewBag.pageSize = pagesize;
        // ViewBag.TotalPage=

        // 🔹 Pagination
        int totalCount = query.Count();
        var users_list = query.Skip((pageNumber - 1) * pagesize)
                         .Take(pagesize)
                         .ToList();   
        // 🔹 Pass values to ViewBag
        ViewBag.TotalCount = totalCount;
        ViewBag.PageSize = pagesize;
        ViewBag.CurrentPage = pageNumber;
        ViewBag.startIndex = (pageNumber - 1) * pagesize + 1;
        ViewBag.endIndex = (pageNumber - 1) * pagesize + pagesize;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pagesize);
        ViewBag.SortColumn = sortColumn;
        ViewBag.SortOrder = sortOrder;

        return View(users_list);
        // return View(userlist);
    }


}
