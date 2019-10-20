namespace Falcon.MtG.MtgJsonModels
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

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