using Newtonsoft.Json.Linq;

namespace Falcon.MtG.MtgJsonModels
{
    public class JsonPrices
    {
        public JObject Paper { get; set; }
        public JObject PaperFoil { get; set; }
    }
}