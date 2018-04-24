namespace Falcon.MtG
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class JsonSet
    {
        [JsonProperty("block")]
        public string Block { get; set; }

        [JsonProperty("booster")]
        public List<object> Booster { get; set; }

        [JsonProperty("border")]
        public string Border { get; set; }

        [JsonProperty("cards")]
        public List<JsonCard> Cards { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("LastUpdated")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("magicCardsInfoCode")]
        public string MagicCardsInfoCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("releaseDate")]
        public string ReleaseDate { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}