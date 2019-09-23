namespace Falcon.MtG.MtgJsonModels
{
    using Newtonsoft.Json;

    public class JsonTypesFile
    {
        public JsonTypesFile Types { get; set; }

        [JsonProperty("meta")]
        public JsonVersion Version { get; set; }
    }
}