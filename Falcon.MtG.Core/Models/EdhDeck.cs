namespace Falcon.MtG.Models
{
    using System.Text;
    using Falcon.MtG.Models.Sql;

    public class EdhDeck : Deck
    {
        public Card Commander { get; set; }
        public Card Partner { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(base.ToString());

            stringBuilder.AppendLine($"SB: 1 {this.Commander.CockatriceName}");

            if (this.Partner != null)
            {
                stringBuilder.AppendLine($"SB: 1 {this.Partner.CockatriceName}");
            }

            return stringBuilder.ToString();
        }
    }
}