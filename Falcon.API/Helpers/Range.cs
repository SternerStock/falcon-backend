namespace Falcon.API.Helpers
{
    using Newtonsoft.Json;

    public class Range
    {
        [JsonProperty("max")]
        public int Max { get; set; }

        [JsonProperty("min")]
        public int Min { get; set; }
    }
}