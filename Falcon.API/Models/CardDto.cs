namespace Falcon.API.Models
{
    using Falcon.MtG;
    using System.Linq;

    public class CardDto
    {
        public CardDto(Card c)
        {
            var lastPrinting = c.Printings.OrderByDescending(p => p.Set.Date).First();

            this.Id = c.ID;
            this.MultiverseId = lastPrinting.MultiverseId;
            this.Name = c.Name;
            this.ManaCost = c.ManaCost;
            this.TypeLine = c.TypeLine;
            this.OracleText = c.OracleText;
            this.FlavorText = lastPrinting.FlavorText;
            this.Power = c.Power;
            this.Toughness = c.Toughness;
            this.ColorIdentity = c.ColorIdentity.Select(ci => ci.Symbol).ToArray();
        }

        public string[] ColorIdentity { get; set; }

        public string FlavorText { get; set; }

        public int Id { get; set; }

        public string ManaCost { get; set; }

        public int? MultiverseId { get; set; }

        public string Name { get; set; }

        public string OracleText { get; set; }

        public string Power { get; set; }

        public string Toughness { get; set; }

        public string TypeLine { get; set; }
    }
}