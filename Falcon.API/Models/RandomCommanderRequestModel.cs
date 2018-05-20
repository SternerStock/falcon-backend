namespace Falcon.API.Models
{
    using Falcon.MtG;
    using Newtonsoft.Json;

    public class RandomCommanderRequestModel
    {
        [JsonProperty("colors")]
        public string[] Colors { get; set; }

        [JsonProperty("format")]
        public EdhFormat Format { get; set; }

        [JsonProperty("matchAll")]
        public bool MatchAll { get; set; }
    }
}