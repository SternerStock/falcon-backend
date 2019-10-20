namespace Falcon.MtG
{
    public class CardTypeSubtype
    {
        public int CardTypeID { get; set; }
        public CardType CardType { get; set; }
        public int SubtypeID { get; set; }
        public Subtype Subtype { get; set; }
    }
}