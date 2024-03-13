using Newtonsoft.Json;

namespace sybring_project.Models
{
    public class Hoilday
    {
        [JsonProperty("datum")]
        public DateTime Datum { get; set; }

        [JsonProperty("veckodag")]
        public DateTime Veckodag { get; set; }

        [JsonProperty("arbetsfridag")]
        public DateTime Arbetsfridag { get; set; }

        [JsonProperty("röddag")]
        public DateTime Röddag { get; set; }

        [JsonProperty("vecka")]
        public DateTime Vecka { get; set; }

        [JsonProperty("agivecka")]
        public string Dagivecka { get; set; }

        [JsonProperty("helgdag")]
        public string Helgdag { get; set; }

        [JsonProperty("namnsdag")]
        public object[] Namnsdag { get; set; }

        [JsonProperty("flaggdag")]
        public string Flaggdag { get; set; }


    }





}

