namespace Falcon.MtG
{
    using System.Collections.Generic;

    public class Subtype : ISimpleLookup
    {
        public Subtype()
        {
            this.Cards = new HashSet<CardSupertype>();
            this.Types = new HashSet<CardTypeSubtype>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<CardSupertype> Cards { get; set; }

        public ICollection<CardTypeSubtype> Types { get; set; }
    }
}