namespace Falcon.MtG.Models.Json
{
    using System.Collections.Generic;

    public class JsonCard
    {
        public JsonCard()
        {
            this.Names = new List<string>();
            this.Colors = new List<string>();
            this.Supertypes = new List<string>();
            this.Types = new List<string>();
            this.Subtypes = new List<string>();
            this.ConvertedManaCost = 0;
            this.LeadershipSkills = new JsonLeadership()
            {
                Brawl = false,
                Commander = false,
                Oathbreaker = false
            };
        }

        public string Artist { get; set; }

        public string BorderColor { get; set; }

        public List<string> ColorIdentity { get; set; }

        public List<string> Colors { get; set; }

        public float ConvertedManaCost { get; set; }

        public int EDHRECRank { get; set; }

        public string FlavorText { get; set; }

        public string FrameVersion { get; set; }

        public string Layout { get; set; }

        public JsonLeadership LeadershipSkills { get; set; }

        public JsonLegality Legalities { get; set; }

        public string Loyalty { get; set; }

        public string ManaCost { get; set; }

        public int? MultiverseId { get; set; }

        public string Name { get; set; }

        public List<string> Names { get; set; }

        public string Number { get; set; }

        public string Power { get; set; }

        public JsonPrices Prices { get; set; }

        public string Rarity { get; set; }

        public string Side { get; set; }

        public List<string> Subtypes { get; set; }

        public List<string> Supertypes { get; set; }

        public string Text { get; set; }

        public string Toughness { get; set; }

        public string Type { get; set; }

        public List<string> Types { get; set; }

        public string Watermark { get; set; }
    }
}