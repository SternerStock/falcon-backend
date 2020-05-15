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

            Printing printing;
            int printings = sqlCard.Printings.Count;
            if (printings > 1)
            {
                int randomIndex = (new Random()).Next(0, printings);
                printing = sqlCard.Printings.ElementAt(randomIndex);
            }
            else
            {
                printing = sqlCard.Printings.First();
            }

            MultiverseId = printing.MultiverseId;
            FlavorText = printing.FlavorText;
            Artist = printing.Artist.Name;
            Watermark = printing.Watermark?.Name;
            Set = printing.Set.Name;
        }

        public int ID { get; set; }

        public int MultiverseId { get; set; }

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

        public string Artist { get; set; }

        public string Watermark { get; set; }

        public string Set { get; set; }
    }
}