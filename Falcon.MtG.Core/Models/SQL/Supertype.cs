namespace Falcon.MtG
{
    using System.Collections.Generic;

    public class Supertype : ISimpleLookup
    {
        public Supertype()
        {
            this.Cards = new HashSet<CardSupertype>();
            this.Types = new HashSet<CardTypeSupertype>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<CardSupertype> Cards { get; set; }

        public ICollection<CardTypeSupertype> Types { get; set; }
    }
}