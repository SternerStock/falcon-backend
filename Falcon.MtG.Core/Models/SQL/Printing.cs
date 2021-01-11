namespace Falcon.MtG.Models.Sql
{
    using System;
    using System.Collections.Generic;

    public class Printing
    {
        public Printing()
        {
            this.Pricings = new HashSet<Pricing>();
        }

        public int ID { get; set; }

        public Guid UUID { get; set; }

        public int MultiverseId { get; set; }

        public string FlavorText { get; set; }

        public string CollectorNumber { get; set; }

        public string Side { get; set; }

        public int? ArtistID { get; set; }

        public int? WatermarkID { get; set; }

        public int FrameID { get; set; }

        public int RarityID { get; set; }

        public int BorderID { get; set; }

        public int SetID { get; set; }

        public int CardID { get; set; }

        public Artist Artist { get; set; }

        public Watermark Watermark { get; set; }

        public Frame Frame { get; set; }

        public Rarity Rarity { get; set; }

        public Border Border { get; set; }

        public Set Set { get; set; }

        public Card Card { get; set; }

        public ICollection<Pricing> Pricings { get; set; }
    }
}