namespace Falcon.MtG.Models
{
    using System.Text;
    using Falcon.MtG.Models.Sql;

    public class OathbreakerDeck : Deck
    {
        public Card Oathbreaker { get; set; }
        public Card SignatureSpell { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.ToString());

            stringBuilder.AppendLine($"SB: 1 {this.Oathbreaker.CockatriceName}");
            stringBuilder.AppendLine($"SB: 1 {this.SignatureSpell.CockatriceName}");

            return stringBuilder.ToString();
        }
    }
}