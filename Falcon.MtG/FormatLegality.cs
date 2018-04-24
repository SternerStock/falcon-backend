namespace Falcon.MtG
{
    using Newtonsoft.Json;

    public class FormatLegality
    {
        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("legality")]
        public string Legality { get; set; }
    }
}