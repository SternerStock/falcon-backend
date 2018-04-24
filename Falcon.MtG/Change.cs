namespace Falcon.MtG
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Change
    {
        [JsonProperty("newSetFiles")]
        public List<string> NewSetFiles { get; set; }

        [JsonProperty("removedSetFiles")]
        public List<string> RemovedSetFiles { get; set; }

        [JsonProperty("updatedSetFiles")]
        public List<string> UpdatedSetFiles { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("when")]
        public string When { get; set; }
    }
}