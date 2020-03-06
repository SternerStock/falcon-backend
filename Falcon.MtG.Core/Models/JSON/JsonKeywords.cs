namespace Falcon.MtG.Models.Json
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class JsonKeywords
    {
        [JsonProperty("abilityWords")]
        public List<string> AbilityWords { get; set; }

        [JsonProperty("keywordAbilities")]
        public List<string> KeywordAbilities { get; set; }

        [JsonProperty("keywordActions")]
        public List<string> KeywordActions { get; set; }

        [JsonProperty("meta")]
        public JsonVersion Version { get; set; }
    }
}