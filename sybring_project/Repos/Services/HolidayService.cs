using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sybring_project.Models.Db;
using sybring_project.Repos.Interfaces;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace sybring_project.Repos.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly HttpClient _httpClient;

        public HolidayService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("holiday");
        }



        //good work
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



        //get list of reddays from api
        public async Task<IEnumerable<Holiday>> GetRedDaysAsync()
        {
            var response = await _httpClient.GetAsync("http://sholiday.faboul.se/dagar/v2.1/2024");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(content);
            var redDays = new List<Holiday>();
          
            if (jsonResponse != null && jsonResponse["dagar"] != null)
            {
                foreach (var day in jsonResponse["dagar"])
                {
                    if (day["röd dag"] == "Ja")
                    {
                        redDays.Add(new Holiday
                        {
                            Datum = DateTime.Parse(day["datum"].ToString()),
                            Veckodag = day["veckodag"],
                            Röddag = day["röddag"]

                        });
                    }
                }
            }

            return redDays;
        }




        public HistoryHoliday ConvertToHistoricalHoliday(Holiday holiday)
        {
            return new HistoryHoliday
            {
                Röddag = holiday.Röddag,
                Vecka = holiday.Vecka,
                Veckodag = holiday.Veckodag,
                Datum = holiday.Datum,
                Arbetsfridag = holiday.Arbetsfridag,
                DagIVecka = holiday.DagIVecka,
                Helgdag = holiday.Helgdag,
                Namnsdag = holiday.Namnsdag,
                Flaggdag = holiday.Flaggdag,



            };
        }



















        //public bool IsWorkingDay(DateTime dateTime)
        //{
        //    try
        //    {
        //        var client = new HttpClient();
        //        var endpoint = $"http://sholiday.faboul.se/dagar/v2.1/{dateTime.Year}";
        //        var response = client.GetAsync(endpoint).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var holidaysJson = response.Content.ReadAsStringAsync().Result;
        //            var holidays = JsonConvert.DeserializeObject<IEnumerable<Holiday>>(holidaysJson);

        //            // Check if the provided date is not a holiday and is a working day
        //            return !holidays.Any(holiday => holiday.Datum.Date == dateTime.Date) && IsWorkingDayOfWeek(dateTime.DayOfWeek);
        //        }
        //        else
        //        {
        //            // Handle unsuccessful HTTP response
        //            Console.WriteLine($"Failed to fetch holidays data. Status code: {response.StatusCode}");
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //        return false;
        //    }
        //}




        //private bool IsWorkingDayOfWeek(DayOfWeek dayOfWeek)
        //{
        //    // Implement your logic to determine if the provided day of the week is a working day.
        //    // For example, you might want to exclude weekends (Saturday and Sunday).
        //    return dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday;
        //}



        //public async Task<List<Holiday>> GetRedDaysForYear(int year)
        //{
        //    var response = await _httpClient.GetAsync($"http://sholiday.faboul.se/dagar/v2.1/{year}");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        var holidays = JsonConvert.DeserializeObject<List<Holiday>>(content);

        //        // Filter red days
        //        var redDays = new List<Holiday>();
        //        foreach (var holiday in holidays)
        //        {
        //            if (holiday.Röddag)
        //            {
        //                redDays.Add(holiday);
        //            }
        //        }

        //        return redDays;
        //    }
        //    else
        //    {
        //        throw new Exception($"Failed to fetch holidays for year {year}. Status code: {response.StatusCode}");
        //    }
        //}














    }
}


