namespace Falcon.MtG.Models.Sql
{
    using System.Collections.Generic;

    public class Subtype : ISimpleLookup
    {
        public Subtype()
        {
            this.Cards = new HashSet<CardSubtype>();
            this.Types = new HashSet<CardTypeSubtype>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<CardSubtype> Cards { get; set; }

        public ICollection<CardTypeSubtype> Types { get; set; }
    }
}