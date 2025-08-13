namespace Falcon.MtG.Models.Sql
{
    using System;
    using System.Collections.Generic;

    public class Card
    {
        public Card()
        {
            this.Colors = new HashSet<CardColor>();

            this.ColorIdentity = new HashSet<CardColorIdentity>();

            this.Supertypes = new HashSet<CardSupertype>();

            this.Types = new HashSet<CardCardType>();

            this.Subtypes = new HashSet<CardSubtype>();

            this.Printings = new HashSet<Printing>();

            this.Keywords = new HashSet<CardKeyword>();

            this.OtherSides = new HashSet<Card>();

            this.Legalities = new HashSet<Legality>();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public int ID { get; set; }

        public Guid ScryfallOracleId { get; set; }

        public string Name { get; set; }

        public string CockatriceName { get; set; }

        public string ManaCost { get; set; }

        public double CMC { get; set; }

        public string TypeLine { get; set; }

        public string OracleText { get; set; }

        public string Power { get; set; }

        public string Toughness { get; set; }

        public string Loyalty { get; set; }

        public int LayoutID { get; set; }

        public int? MainSideID { get; set; }

        public string Side { get; set; }

        public int? EDHRECRank { get; set; }

        public float? EDHRECSalt { get; set; }

        public ICollection<CardColor> Colors { get; set; }

        public ICollection<CardColorIdentity> ColorIdentity { get; set; }

        public ICollection<CardSupertype> Supertypes { get; set; }

        public ICollection<CardCardType> Types { get; set; }

        public ICollection<CardSubtype> Subtypes { get; set; }

        public ICollection<Printing> Printings { get; set; }

        public ICollection<CardKeyword> Keywords { get; set; }

        public Layout Layout { get; set; }

        public ICollection<Card> OtherSides { get; set; }

        public Card MainSide { get; set; }

        public ICollection<Legality> Legalities { get; set; }
    }
}