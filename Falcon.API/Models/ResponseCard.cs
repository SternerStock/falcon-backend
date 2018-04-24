namespace Falcon.API.Models
{
    using System.Linq;
    using Falcon.MtG;
    using Newtonsoft.Json;

    public class ResponseCard
    {
        public ResponseCard(Card c)
        {
            this.Id = c.ID;
            this.MultiverseId = c.MultiverseId;
            this.Name = c.Name;
            this.ManaCost = c.ManaCost;
            this.TypeLine = c.TypeLine;
            this.OracleText = c.OracleText;
            this.FlavorText = c.FlavorText;
            this.Power = c.Power;
            this.Toughness = c.Toughness;
            this.ColorIdentity = c.ColorIdentity.Select(ci => ci.Symbol).ToArray();
        }

        [JsonProperty("colorIdentity")]
        public string[] ColorIdentity { get; set; }

        [JsonProperty("flavorText")]
        public string FlavorText { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("manaCost")]
        public string ManaCost { get; set; }

        [JsonProperty("multiverseId")]
        public int? MultiverseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("oracleText")]
        public string OracleText { get; set; }

        [JsonProperty("power")]
        public int? Power { get; set; }

        [JsonProperty("toughness")]
        public int? Toughness { get; set; }

        [JsonProperty("typeLine")]
        public string TypeLine { get; set; }
    }
}