namespace Falcon.MtG
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class JsonCard
    {
        public JsonCard()
        {
            this.Names = new List<string>();
            this.Colors = new List<string>();
            this.Supertypes = new List<string>();
            this.Types = new List<string>();
            this.Subtypes = new List<string>();
            this.Legalities = new List<FormatLegality>();
            this.CMC = 0;
        }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("border")]
        public string Border { get; set; }

        [JsonProperty("cmc")]
        public float CMC { get; set; }

        [JsonProperty("colors")]
        public List<string> Colors { get; set; }

        [JsonProperty("flavor")]
        public string Flavor { get; set; }

        public bool IsCommanderLegal
        {
            get
            {
                foreach (var legality in this.Legalities)
                {
                    if (legality.Format == "Commander")
                    {
                        return legality.Legality == "Legal";
                    }
                }

                return false;
            }
        }
        
        public bool IsBrawlLegal
        {
            get
            {
                foreach (var legality in this.Legalities)
                {
                    if (legality.Format == "Standard")
                    {
                        return legality.Legality == "Legal";
                    }
                }

                return false;
            }
        }

        public bool IsPrimarySide
        {
            get
            {
                // One name in array is a single-face card, so it's always primary
                // Two names in array is a double-face card, flip card, or split card, where the first name is primary
                // Three names in array is a meld card, so first two names are primary and share the same "back"
                // Five names is right out
                return this.Names.Count <= 1 || this.Name != this.Names[this.Names.Count - 1];
            }
        }

        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("legalities")]
        public List<FormatLegality> Legalities { get; set; }

        [JsonProperty("loyalty")]
        public string Loyalty { get; set; }

        [JsonProperty("manaCost")]
        public string ManaCost { get; set; }

        [JsonProperty("multiverseid")]
        public int? MultiverseId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("names")]
        public List<string> Names { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("text")]
        public string OracleText { get; set; }

        [JsonProperty("originalText")]
        public string OriginalText { get; set; }

        [JsonProperty("originalType")]
        public string OriginalType { get; set; }

        [JsonProperty("power")]
        public string Power { get; set; }

        [JsonProperty("rarity")]
        public string Rarity { get; set; }

        [JsonProperty("releaseDate")]
        public string ReleaseDate { get; set; }

        [JsonProperty("reserved")]
        public bool? Reserved { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("subtypes")]
        public List<string> Subtypes { get; set; }

        [JsonProperty("supertypes")]
        public List<string> Supertypes { get; set; }

        [JsonProperty("toughness")]
        public string Toughness { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }

        [JsonProperty("watermark")]
        public string Watermark { get; set; }
    }
}