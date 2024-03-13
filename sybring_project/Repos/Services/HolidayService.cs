using Newtonsoft.Json;
using sybring_project.Models;
using sybring_project.Repos.Interfaces;
using System.Net.Http;

namespace sybring_project.Repos.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly HttpClient _httpClient;

        public HolidayService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("holiday");
        }



        public async Task<Holiday> GetHolidayReportAsync()
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
            return JsonConvert.DeserializeObject<Holiday>(content) ?? new Holiday();
        }

        public async Task<Holiday> GetHolidayDetails()
        {
            var response = await _httpClient.GetStringAsync("http://sholiday.faboul.se/dagar/v2.1/2024");

            return JsonConvert.DeserializeObject<Holiday>(response);
        }

       
    }
}


