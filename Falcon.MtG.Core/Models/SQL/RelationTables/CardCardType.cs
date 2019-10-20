namespace Falcon.MtG
{
    public class CardCardType
    {
        public int CardID { get; set; }
        public Card Card { get; set; }
        public int CardTypeID { get; set; }
        public CardType CardType { get; set; }
    }
}