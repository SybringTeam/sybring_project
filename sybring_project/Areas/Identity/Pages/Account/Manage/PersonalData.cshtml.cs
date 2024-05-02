// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sybring_project.Data;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using System.Net.Http;
using sybring_project.Models;
using static sybring_project.Models.Db.CountriesSowAPI;

namespace sybring_project.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly IUserServices _userServices;
        private readonly ApplicationDbContext _db;
        private readonly ICountryServices _countryServices;


        public PersonalDataModel(UserManager<User> userManager,
            ILogger<PersonalDataModel> logger, IUserServices userServices, ApplicationDbContext db,
            ICountryServices countryServices)
        {
            _userManager = userManager;
            _logger = logger;
            _userServices = userServices;
            _db = db;
            _countryServices = countryServices;

        }
        [BindProperty]
        public User UserData { get; set; }
        public List<CountriesSowAPI.Datum> ApiCountryNames { get; set; }




        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Fetch list of countries from API
            var apiCountries = await _countryServices.GetAllCountriesAsync();
            ApiCountryNames = new List<CountriesSowAPI.Datum>(apiCountries.data);

            return Page();
        }




        public async Task<IActionResult> OnPost(string iceContactName, string iceContactNumber,
            string selectedCountry, string address, string phone)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserAsync(User)}'.");
            }
         
            user.PhoneNumber = phone;
            user.Address = address;
            user.ICEContactName = iceContactName;
            user.UserICE = iceContactNumber;
            user.CitizenMembership = selectedCountry;


            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation("User updated their ICE contact information successfully.");
                return RedirectToPage();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }
    }
}