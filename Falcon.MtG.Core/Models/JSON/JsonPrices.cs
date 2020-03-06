namespace Falcon.MtG.Models.Json
{
    using Newtonsoft.Json.Linq;

    public class JsonPrices
    {
        public JObject Paper { get; set; }
        public JObject PaperFoil { get; set; }
    }
}