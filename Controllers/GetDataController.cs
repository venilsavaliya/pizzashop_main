using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;

namespace pizzashop.Controllers;

// [ApiController]
// [Route("api/[controller]")]
public class GetDataController : Controller
{

    private readonly PizzashopMainContext _db;

    public GetDataController(PizzashopMainContext db)
    {
        _db = db;
    }

    // Get: GetData/GetCountries
    [HttpGet]
    public IActionResult GetCountries()
    {
        var countries = _db.Countries.ToList();
        return Json(countries);
    }
    

    // Get: GetData/GetStates
    [HttpGet]
    public IActionResult GetStates(int countryId)
    {
        var states = _db.States.Where(s => s.CountryId == countryId).ToList();
        return Json(states);
    }
    
    // Get: GetData/GetCities
    [HttpGet]
    public IActionResult GetCities(int stateId)
    {
        var cities = _db.Cities.Where(c => c.StateId == stateId).ToList();
        return Json(cities);
    }

   
}
