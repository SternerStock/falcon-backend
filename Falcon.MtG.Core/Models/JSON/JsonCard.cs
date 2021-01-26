namespace Falcon.MtG.Models.Json
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class JsonCard
    {
        public JsonCard()
        {
            this.OtherFaceIds = new List<Guid>();
            this.Colors = new List<string>();
            this.Supertypes = new List<string>();
            this.Types = new List<string>();
            this.Subtypes = new List<string>();
            this.Keywords = new List<string>();
            this.Printings = new List<string>();
            this.Variations = new List<Guid>();
            this.ConvertedManaCost = 0;
            this.LeadershipSkills = new JsonLeadership()
            {
                Brawl = false,
                Commander = false,
                Oathbreaker = false
            };
        }

        public Guid UUID { get; set; }

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

        public JsonIdentifiers Identifiers { get; set; }

        public string Name { get; set; }

        public string FaceName { get; set; }

        public List<Guid> OtherFaceIds { get; set; }

        public string Number { get; set; }

        public string Power { get; set; }

        public JsonPrices Prices { get; set; }

        public string Rarity { get; set; }

        public string Side { get; set; }

        public List<string> Subtypes { get; set; }

        public List<string> Supertypes { get; set; }

        public List<string> Keywords { get; set; }

        public string Text { get; set; }

        public string Toughness { get; set; }

        public string Type { get; set; }

        public List<string> Types { get; set; }

        public List<string> Printings { get; set; }

        public List<Guid> Variations { get; set; }

        public string Watermark { get; set; }

        public string CockatriceName
        {
            get
            {
                string name = FaceName ?? Name;
                if (Printings.Contains("UST") && !Supertypes.Contains("Basic") && Variations.Count > 0)
                {
                    Match match = Regex.Match(Number, @"\d+([b-z])");
                    if (match.Success)
                    {
                        return $"{name} ({match.Groups[1]})";
                    }
                }

                return name;
            }
        }
    }
}