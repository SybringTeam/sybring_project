namespace sybring_project.Models.Db
{
    public class CountriesSowAPI
    {
        public class Rootobject
        {
            public bool error { get; set; }
            public string msg { get; set; }
            public Datum[] data { get; set; }
        }

        public class Datum
        {
            public string iso2 { get; set; }
            public string iso3 { get; set; }
            public string country { get; set; }
            public string[] cities { get; set; }
        }
    }


  

}
