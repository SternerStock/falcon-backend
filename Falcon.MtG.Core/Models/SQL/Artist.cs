namespace Falcon.MtG
{
    using System.Collections.Generic;

    public class Artist : ISimpleLookup
    {
        public Artist()
        {
            this.Printings = new HashSet<Printing>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Printing> Printings { get; set; }
    }
}