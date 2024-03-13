using Newtonsoft.Json;
using sybring_project.Models;
using sybring_project.Repos.Interfaces;
using System.Net.Http;

namespace sybring_project.Repos.Services
{
    public class HoildayService : IHoildayService
    {
        private readonly HttpClient _httpClient;
        public HoildayService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("hoilday");
        }


        public async Task<Hoilday> GetHoildayReport()
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://sholiday.faboul.se/dagar/v2.1/2024")
            };
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Hoilday>(content) ?? new Hoilday() { Datum = "No data available" };
        }



    }
}


