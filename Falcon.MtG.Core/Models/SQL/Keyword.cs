namespace Falcon.MtG.Models.Sql
{
    using System.Collections.Generic;

    public class Keyword : ISimpleLookup
    {
        public Keyword()
        {
            this.Cards = new HashSet<CardKeyword>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public ICollection<CardKeyword> Cards { get; set; }
    }
}