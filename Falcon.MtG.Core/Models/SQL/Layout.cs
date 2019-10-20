namespace Falcon.MtG
{
    using System.Collections.Generic;

    public class Layout : ISimpleLookup
    {
        public Layout()
        {
            this.Cards = new HashSet<Card>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Card> Cards { get; set; }
    }
}