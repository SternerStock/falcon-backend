namespace Falcon.MtG.Models.Sql
{
    public class CardSubtype
    {
        public int CardID { get; set; }
        public Card Card { get; set; }
        public int SubtypeID { get; set; }
        public Subtype Subtype { get; set; }
    }
}