using Newtonsoft.Json;

namespace sybring_project.Models.Db
{
    public class HistoryHoliday
    {
        [JsonProperty("datum")]
        public DateTime Datum { get; set; } = DateTime.Now;

        //[JsonProperty("veckodag")]
        //public DateTime Veckodag { get; set; } = DateTime.Today;

        [JsonProperty("veckodag")]
        public string Veckodag { get; set; } = string.Empty;

        [JsonProperty("arbetsfridag")]
        public string Arbetsfridag { get; set; } = string.Empty;

        [JsonProperty("röddag")]
        public string Röddag { get; set; } = string.Empty;

        [JsonProperty("vecka")]
        public string Vecka { get; set; } = string.Empty;


        [JsonProperty("dag i vecka")]
        public int DagIVecka { get; set; }

        [JsonProperty("helgdag")]
        public string Helgdag { get; set; } = string.Empty;

        [JsonProperty("namnsdag")]
        //public object[] Namnsdag { get; set; }

        public List<object> Namnsdag { get; set; }

        [JsonProperty("flaggdag")]
        public string Flaggdag { get; set; }


        // Custom property to determine if it's a red day
        public bool IsRedDay => Röddag.ToLower() == "ja";

    }
}
