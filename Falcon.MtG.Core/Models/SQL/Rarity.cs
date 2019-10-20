namespace Falcon.MtG
{
    using System.Collections.Generic;

    public class Rarity : ISimpleLookup
    {
        public Rarity()
        {
            this.Printings = new HashSet<Printing>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Printing> Printings { get; set; }
    }
}