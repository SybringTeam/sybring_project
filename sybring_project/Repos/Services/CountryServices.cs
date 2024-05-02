using Newtonsoft.Json;
using sybring_project.Models;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using System.Text.Json;
using static sybring_project.Models.CountryAPI;
using static sybring_project.Models.Db.CountriesSowAPI;

namespace sybring_project.Repos.Services
{
    public class CountryServices : ICountryServices
    {
        private readonly HttpClient _httpClient;

        public CountryServices(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("countries");
            _httpClient.BaseAddress = new Uri("https://countriesnow.space/api/v0.1/");
        }


        //public async Task<Class1[]> GetAllCountriesAsync()
        //{
        //    var countryResponse = await _httpClient.GetStringAsync("all");
        //    return JsonConvert.DeserializeObject<Class1[]>(countryResponse)!;


        //}


        public async Task<CountriesSowAPI.Rootobject> GetAllCountriesAsync() 
        {
            var response = await _httpClient.GetStringAsync("countries");
            return JsonConvert.DeserializeObject<CountriesSowAPI.Rootobject>(response);
        }
    }
}

