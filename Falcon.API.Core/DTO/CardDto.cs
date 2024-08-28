namespace Falcon.API.DTO
{
    using System;
    using System.Linq;
    using Falcon.MtG.Models.Sql;

    public class CardDto
    {
        public CardDto(Card sqlCard)
        {
            ID = sqlCard.ID;
            Name = sqlCard.Name;
            ManaCost = sqlCard.ManaCost;
            TypeLine = sqlCard.TypeLine;
            OracleText = sqlCard.OracleText;
            Power = sqlCard.Power;
            Toughness = sqlCard.Toughness;
            Loyalty = sqlCard.Loyalty;
            Color = sqlCard.Colors.Select(c => c.Color.Symbol).ToArray();
            ColorIdentity = sqlCard.ColorIdentity.Select(c => c.Color.Symbol).ToArray();

            var shuffledPrintings = sqlCard.Printings.OrderBy(p => Guid.NewGuid());

            MultiverseId = shuffledPrintings.Select(p => p.MultiverseId).ToArray();
            FlavorText = shuffledPrintings.Where(p => !string.IsNullOrEmpty(p.FlavorText)).FirstOrDefault()?.FlavorText;
        }

        public int ID { get; set; }

        public int[] MultiverseId { get; set; }

        public string Name { get; set; }

        public string ManaCost { get; set; }

        public string TypeLine { get; set; }

        public string OracleText { get; set; }

        public string FlavorText { get; set; }

        public string Power { get; set; }

        public string Toughness { get; set; }

        public string Loyalty { get; set; }

        public string[] Color { get; set; }

        public string[] ColorIdentity { get; set; }
    }
}