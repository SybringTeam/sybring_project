using Newtonsoft.Json;

namespace sybring_project.Models
{
    public class Hoilday
    {
        [JsonProperty("datum")]
        public string ? Datum { get; set; }

        [JsonProperty("veckodag")]
        public string Veckodag { get; set; }

        [JsonProperty("arbetsfridag")]
        public string Arbetsfridag { get; set; }

        [JsonProperty("röddag")]
        public string Röddag { get; set; }

        [JsonProperty("vecka")]
        public string Vecka { get; set; }

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

