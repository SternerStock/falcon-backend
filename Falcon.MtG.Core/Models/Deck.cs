namespace Falcon.MtG.Models
{
    using System.Collections.Generic;
    using Falcon.MtG.Models.Sql;

    public class Deck
    {
        public IEnumerable<Card> Cards { get; set; }
    }
}