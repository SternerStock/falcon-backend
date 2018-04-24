namespace Falcon.API.Helpers
{
    using Newtonsoft.Json;

    public class Category
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}