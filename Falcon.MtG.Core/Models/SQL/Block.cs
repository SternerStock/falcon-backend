namespace Falcon.MtG
{
    using System.Collections.Generic;

    public class Block : ISimpleLookup
    {
        public Block()
        {
            this.Sets = new HashSet<Set>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Set> Sets { get; set; }
    }
}