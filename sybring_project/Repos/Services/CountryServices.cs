using Newtonsoft.Json;
using sybring_project.Models;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using System.Text.Json;
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



        public async Task<Rootobject> GetAllCountriesAsync() 
        {
            var response = await _httpClient.GetStringAsync("countries");
            return JsonConvert.DeserializeObject<Rootobject>(response)!;
        }
    }
}

