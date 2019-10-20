namespace Falcon.MtG
{
    using System.Collections.Generic;

    public class Frame : ISimpleLookup
    {
        public Frame()
        {
            this.Printings = new HashSet<Printing>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Printing> Printings { get; set; }
    }
}