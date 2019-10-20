namespace Falcon.MtG
{
    using System.Collections.Generic;

    public class CardType : ISimpleLookup
    {
        public CardType()
        {
            this.Cards = new HashSet<CardCardType>();
            this.Subtypes = new HashSet<CardTypeSubtype>();
            this.Supertypes = new HashSet<CardTypeSupertype>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<CardCardType> Cards { get; set; }

        public ICollection<CardTypeSubtype> Subtypes { get; set; }

        public ICollection<CardTypeSupertype> Supertypes { get; set; }
    }
}