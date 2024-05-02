using Microsoft.AspNetCore.Mvc;
using sybring_project.Repos.Interfaces;

namespace sybring_project.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryServices _countryServices;

        public CountryController(ICountryServices countryServices)
        {
            _countryServices = countryServices;
        }


        public async Task<IActionResult> Index()
        {
            var countries = await _countryServices.GetAllCountriesAsync();
            
                return View(countries);
                    
        }
    }
}
